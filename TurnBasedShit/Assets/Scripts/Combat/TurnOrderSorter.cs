using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderSorter : MonoBehaviour {
    List<GameObject> unitsInPlay = new List<GameObject>();

    public GameObject playingUnit;

    Coroutine turnWaiter = null;


    private void Start() {
        FindObjectOfType<MenuCanvas>().addNewRunOnClose(updatePlayerUnits);
    }

    private void Update() {
    }

    public void resetList() {
        unitsInPlay.Clear();
        playingUnit = null;

        foreach(var i in FindObjectsOfType<UnitClass>()) {
            if(i.stats.item != null && !i.stats.item.isEmpty()) {
                for(int s = 0; s < i.stats.item.getPassiveMod(StatModifier.passiveModifierType.addExtraTurn, i.stats, false); s++)
                    unitsInPlay.Add(i.gameObject);
            }
            unitsInPlay.Add(i.gameObject);
        }
        setNextInTurnOrder();
    }

    public void clearList() {
        unitsInPlay.Clear();
    }

    public int listCount() {
        return unitsInPlay.Count;
    }

    public void updatePlayerUnits() {
        for(int i = 0; i < unitsInPlay.Count; i++) {
            if(!unitsInPlay[i].GetComponent<UnitClass>().combatStats.isPlayerUnit)
                continue;

            unitsInPlay[i] = FindObjectOfType<PartyObject>().getInstantiatedMember(unitsInPlay[i].GetComponent<UnitClass>().stats);
        }

        FindObjectOfType<TurnOrderCanvas>().updateInfomation();
    }

    public List<GameObject> getNextPlayers(int num) {
        var pool = new List<GameObject>();
        foreach(var i in unitsInPlay)
            pool.Add(i.gameObject);

        var sorted = new List<GameObject>();


        while(num > sorted.Count) {   //  adds the certain number of units to fill in the required num
            while(pool.Count > 0) {
                var temp = getNextPlayer(pool);
                pool.Remove(temp);
                sorted.Add(temp);
            }

            foreach(var i in FindObjectsOfType<UnitClass>())
                pool.Add(i.gameObject);
        }

        return sorted;

        /*
        var sorted = new List<GameObject>();

        List<GameObject> pool = new List<GameObject>();
        foreach(var i in unitsInPlay)
            pool.Add(i.gameObject);


        while(num > 0) {
            while(pool.Count > 0) {
                var nextSpeed = pool[0].GetComponent<UnitClass>().getSpeed();
                var nextIndex = 0;

                for(int i = 1; i < pool.Count; i++) {
                    if(pool[i].GetComponent<UnitClass>().getSpeed() > nextSpeed) {
                        nextSpeed = pool[i].GetComponent<UnitClass>().getSpeed();
                        nextIndex = i;
                    }
                }

                //  add extra turns for the unit
                if(pool[nextIndex].GetComponent<UnitClass>().stats.item != null && !pool[nextIndex].GetComponent<UnitClass>().stats.item.isEmpty() && pool[nextIndex] != playingUnit) {
                    for(int i = 0; i < pool[nextIndex].GetComponent<UnitClass>().stats.item.getPassiveMod(StatModifier.passiveModifierType.addExtraTurn, pool[nextIndex].GetComponent<UnitClass>().stats, false); i++) {
                        sorted.Add(pool[nextIndex]);
                        num--;

                        if(num == 0)
                            return sorted;
                    }
                }

                sorted.Add(pool[nextIndex]);
                pool.RemoveAt(nextIndex);
                num--;
            }
            foreach(var i in FindObjectsOfType<UnitClass>())
                pool.Add(i.gameObject);
        }

        return sorted;
        */
    }

    public GameObject getNextPlayer(List<GameObject> pool) {
        if(pool.Count == 0)
            return null;

        GameObject next = null;
        bool hasFirsters = false;
        bool nextIsLaster = false;
        foreach(var i in pool) {
            if(i == null || !unitViableForTurn(i))
                continue;

            bool laster = false;
            foreach(var p in i.GetComponent<UnitClass>().stats.getAllPassiveMods()) {     //  check for units with traits that make them take their turn first
                if(p.type == StatModifier.passiveModifierType.takeFirstTurn) {
                    hasFirsters = true;
                    if(next == null || next.GetComponent<UnitClass>().stats.u_speed < i.GetComponent<UnitClass>().stats.u_speed) {
                        next = i.gameObject;
                        nextIsLaster = false;
                    }
                }
                if(p.type == StatModifier.passiveModifierType.takeLastTurn) {
                    laster = true;
                    if(pool.Count == 1)
                        return i.gameObject;
                }
            }

            if(!hasFirsters) {
                if(next == null || (i.GetComponent<UnitClass>().getSpeed() > next.GetComponent<UnitClass>().getSpeed()) || !laster && nextIsLaster) {
                    next = i.gameObject;
                }
            }
        }
        return next;
    }

    bool unitViableForTurn(GameObject unit) {
        if(unit.GetComponent<UnitClass>().combatMods != null && unit.GetComponent<UnitClass>().combatMods.Length > 0) {
            //  checks if unit is viable to play
            foreach(var m in unit.GetComponent<UnitClass>().combatMods) {
                if(m == UnitClass.combatModifier.onlyAttackOnEvenRounds && FindObjectOfType<RoundCounterCanvas>().roundCount % 2 != 0) {    //  can only attack on even rounds
                    return false;
                }
            }
        }
        return true;
    }

    public void addUnitToList(GameObject unit) {
        unitsInPlay.Add(unit);
    }

    public void removeUnitFromList(GameObject unit) {
        unitsInPlay.Remove(unit);
        foreach(var i in unitsInPlay) {
            if(i == playingUnit)
                return;
        }
        setNextInTurnOrder();
    }

    public void setNextInTurnOrder() {
        if(turnWaiter != null)
            return;
        turnWaiter = StartCoroutine(waitForMenuToClose());
    }

    IEnumerator waitForMenuToClose() {
        while(FindObjectOfType<MenuCanvas>().isOpen())
            yield return new WaitForEndOfFrame();


        //  cleans up flair
        foreach(var i in FindObjectsOfType<CombatCards>())
            i.hideCards();

        //  removes current unit from list
        if(playingUnit != null) {
            //  trigger items
            if(playingUnit.GetComponent<UnitClass>().stats.item != null && !playingUnit.GetComponent<UnitClass>().stats.item.isEmpty()) {
                playingUnit.GetComponent<UnitClass>().stats.item.triggerUseTime(playingUnit.GetComponent<UnitClass>(), StatModifier.useTimeType.afterTurn);
                playingUnit.GetComponent<UnitClass>().stats.item.triggerUseTime(playingUnit.GetComponent<UnitClass>(), StatModifier.useTimeType.afterEveryTurn);
            }

            //  resets unit after turn, and removes it from the list of playing units
            playingUnit.GetComponent<UnitClass>().combatStats.attackingTarget = null;
            unitsInPlay.Remove(playingUnit);
            FindObjectOfType<PartyObject>().saveParty();
        }


        //  removes units that dont exist
        foreach(GameObject i in unitsInPlay.ToArray()) {
            if(i != null) {
                if(i.GetComponent<UnitClass>().stats.canLevelUp()) {
                    FindObjectOfType<LevelUpCanvas>().levelUpUnit(i);
                }
            }
            if(i == null)
                unitsInPlay.Remove(i);
        }


        //  resets round if needed
        if(unitsInPlay.Count == 0) {
            FindObjectOfType<UnitBattleMech>().nextBattleRound();

            //  trigger items
            foreach(var i in unitsInPlay) {
                if(i.GetComponent<UnitClass>().stats.item != null && !i.GetComponent<UnitClass>().stats.item.isEmpty()) {
                    i.GetComponent<UnitClass>().stats.item.triggerUseTime(i.GetComponent<UnitClass>(), StatModifier.useTimeType.afterRound);
                }
            }
            yield return 0;
        }

        //  sets next to unit with most speed
        GameObject next = getNextPlayer(unitsInPlay);


        if(next == null) {
            FindObjectOfType<UnitBattleMech>().nextBattleRound();
        }
        else {
            //  sets playing unit to next
            playingUnit = next;
            playingUnit.GetComponent<UnitClass>().prepareUnitForNextRound();
            FindObjectOfType<BattleOptionsCanvas>().runCombatOptions();

            //  flair
            foreach(var i in FindObjectsOfType<CombatSpot>())
                i.setColor();
            FindObjectOfType<UnitCombatHighlighter>().updateHighlights();

            //  triggers
            if(playingUnit.GetComponent<UnitClass>().stats.item != null && !playingUnit.GetComponent<UnitClass>().stats.item.isEmpty()) {
                playingUnit.GetComponent<UnitClass>().stats.item.triggerUseTime(playingUnit.GetComponent<UnitClass>(), StatModifier.useTimeType.beforeTurn);
                playingUnit.GetComponent<UnitClass>().stats.item.triggerUseTime(playingUnit.GetComponent<UnitClass>(), StatModifier.useTimeType.beforeEveryTurn);
            }

            FindObjectOfType<BattleOptionsCanvas>().battleState = 0;
            FindObjectOfType<TurnOrderCanvas>().updateInfomation(true);
            FindObjectOfType<BattleOptionsCanvas>().updateButtonInteractability();

            turnWaiter = null;
        }
    }
}

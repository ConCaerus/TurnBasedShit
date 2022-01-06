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

    public void resetList() {
        unitsInPlay.Clear();
        playingUnit = null;

        foreach(var i in FindObjectsOfType<UnitClass>()) {
            if(i.stats.item != null && !i.stats.item.isEmpty()) {
                for(int s = 0; s < i.stats.item.getPassiveMod(Item.passiveEffectTypes.extraTurn); s++)
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
            if(!unitsInPlay[i].GetComponent<UnitClass>().isPlayerUnit)
                continue;

            unitsInPlay[i] = FindObjectOfType<PartyObject>().getInstantiatedMember(unitsInPlay[i].GetComponent<UnitClass>().stats);
        }

        FindObjectOfType<TurnOrderCanvas>().updateInfomation();
    }

    public List<GameObject> getNextPlayers(int num) {
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
                    for(int i = 0; i < pool[nextIndex].GetComponent<UnitClass>().stats.item.getPassiveMod(Item.passiveEffectTypes.extraTurn); i++) {
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


        //  removes current unit from list
        if(playingUnit != null) {
            //  trigger items
            if(playingUnit.GetComponent<UnitClass>().stats.item != null && !playingUnit.GetComponent<UnitClass>().stats.item.isEmpty()) {
                playingUnit.GetComponent<UnitClass>().stats.item.triggerUseTime(playingUnit.GetComponent<UnitClass>(), Item.useTimes.afterTurn);
                playingUnit.GetComponent<UnitClass>().stats.item.triggerUseTime(playingUnit.GetComponent<UnitClass>(), Item.useTimes.afterEachTurn);
            }

            //  resets unit after turn, and removes it from the list of playing units
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
            FindObjectOfType<UnitBattleMech>().resetBattleRound();

            //  trigger items
            foreach(var i in unitsInPlay) {
                if(i.GetComponent<UnitClass>().stats.item != null && !i.GetComponent<UnitClass>().stats.item.isEmpty()) {
                    i.GetComponent<UnitClass>().stats.item.triggerUseTime(i.GetComponent<UnitClass>(), Item.useTimes.afterRound);
                }
            }
            yield return 0;
        }

        //  sets next to unit with most speed
        var next = unitsInPlay[0];
        foreach(var i in unitsInPlay) {
            if(i != null) {
                if(i.GetComponent<UnitClass>().getSpeed() > next.GetComponent<UnitClass>().getSpeed())
                    next = i.gameObject;
            }
        }

        //  sets playing unit to next
        playingUnit = next;
        playingUnit.GetComponent<UnitClass>().prepareUnitForNextRound();
        FindObjectOfType<BattleOptionsCanvas>().runCombatOptions();

        //  flair
        foreach(var i in FindObjectsOfType<CombatSpot>())
            i.setColor();
        FindObjectOfType<UnitCombatHighlighter>().updateHighlights();
        FindObjectOfType<CombatCameraController>().resetLookingAtObj();

        //  triggers
        if(playingUnit.GetComponent<UnitClass>().stats.item != null && !playingUnit.GetComponent<UnitClass>().stats.item.isEmpty()) {
            playingUnit.GetComponent<UnitClass>().stats.item.triggerUseTime(playingUnit.GetComponent<UnitClass>(), Item.useTimes.beforeTurn);
            playingUnit.GetComponent<UnitClass>().stats.item.triggerUseTime(playingUnit.GetComponent<UnitClass>(), Item.useTimes.beforeEachTurn);
        }

        FindObjectOfType<BattleOptionsCanvas>().battleState = 0;
        FindObjectOfType<TurnOrderCanvas>().updateInfomation(true);
        FindObjectOfType<BattleOptionsCanvas>().updateButtonInteractability();

        turnWaiter = null;
    }
}

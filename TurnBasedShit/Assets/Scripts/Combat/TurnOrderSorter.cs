using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderSorter : MonoBehaviour {
    List<GameObject> unitsInPlay = new List<GameObject>();

    public GameObject playingUnit;


    public void resetList() {
        unitsInPlay.Clear();
        playingUnit = null;

        foreach(var i in FindObjectsOfType<UnitClass>()) {
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

    public void addUnitToList(GameObject unit) {
        unitsInPlay.Add(unit);
    }

    public void removeUnitFromList(GameObject unit) {
        unitsInPlay.Remove(unit);
    }

    public GameObject setNextInTurnOrder() {
        //  removes current unit from list
        if(playingUnit != null) {
            //  triggers
            FindObjectOfType<ItemUser>().triggerTime(Item.useTimes.afterTurn, playingUnit.GetComponent<UnitClass>(), true);
            FindObjectOfType<ItemUser>().triggerTime(Item.useTimes.afterEachTurn, playingUnit.GetComponent<UnitClass>(), false);

            //  resets unit after turn, and removes it from the list of playing units
            playingUnit.GetComponent<UnitClass>().stunned = false;
            unitsInPlay.Remove(playingUnit);
            FindObjectOfType<PartyObject>().saveParty();
        }

        //  removes units that dont exist
        foreach(GameObject i in unitsInPlay.ToArray()) {
            if(i != null)
                i.GetComponent<UnitClass>().checkIfDead();
            if(i == null)
                unitsInPlay.Remove(i);
        }
        FindObjectOfType<ItemUser>().resetInplayItems();

        //  resets round if needed
        if(unitsInPlay.Count == 0) {
            FindObjectOfType<UnitBattleMech>().resetBattleRound();
            FindObjectOfType<ItemUser>().triggerTime(Item.useTimes.afterRound, playingUnit.GetComponent<UnitClass>(), false);
            return null;
        }

        //  sets next to unit with most speed
        var next = unitsInPlay[0];
        foreach(var i in unitsInPlay) {
            if(i != null) {
                if(i.GetComponent<UnitClass>().stats.getModifiedSpeed() > next.GetComponent<UnitClass>().stats.getModifiedSpeed())
                    next = i.gameObject;
            }
        }

        //  sets playing unit to next
        playingUnit = next;
        playingUnit.GetComponent<UnitClass>().prepareUnitForNextRound();
        FindObjectOfType<BattleOptionsCanvas>().runCombatOptions();

        //  flair
        FindObjectOfType<CombatCameraController>().moveToPlayingUnit();

        //  triggers
        FindObjectOfType<ItemUser>().triggerTime(Item.useTimes.beforeTurn, playingUnit.GetComponent<UnitClass>(), true);
        FindObjectOfType<ItemUser>().triggerTime(Item.useTimes.beforeEachTurn, playingUnit.GetComponent<UnitClass>(), false);

        return playingUnit;
    }
}

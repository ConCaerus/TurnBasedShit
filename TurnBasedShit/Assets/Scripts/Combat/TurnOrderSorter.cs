using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderSorter : MonoBehaviour {
    List<GameObject> unitsInPlay = new List<GameObject>();

    public GameObject playingUnit;

    Coroutine attackWaiter = null;


    private void LateUpdate() {
        if(playingUnit != null) {
            if(playingUnit.GetComponent<UnitClass>().defending)
                setNextInTurnOrder();
            else if(playingUnit.GetComponent<UnitClass>().attacking && attackWaiter == null)
                attackWaiter = StartCoroutine(waitToFinishAttacking());
        }
        else
            setNextInTurnOrder();
    }


    public void resetList() {
        unitsInPlay.Clear();
        playingUnit = null;

        foreach(var i in FindObjectsOfType<UnitClass>()) {
            unitsInPlay.Add(i.gameObject);
        }
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
            FindObjectOfType<UnitTurnOrderHighlighting>().dehighlightUnit(playingUnit);
            unitsInPlay.Remove(playingUnit);
            FindObjectOfType<Party>().saveParty();
        }

        //  resets round if needed
        if(unitsInPlay.Count == 0)
            FindObjectOfType<UnitBattleMech>().resetBattleRound();

        //  sets next to unit with most speed
        var next = unitsInPlay[0];
        foreach(var i in unitsInPlay) {
            if(i != null) {
                if(i.GetComponent<UnitClass>().stats.u_speed > next.GetComponent<UnitClass>().stats.u_speed)
                    next = i.gameObject;
            }
        }

        //  sets playing unit to next
        playingUnit = next;
        FindObjectOfType<UnitTurnOrderHighlighting>().highlightUnit(playingUnit);
        playingUnit.GetComponent<UnitClass>().prepareUnitForNextRound();
        FindObjectOfType<BattleOptionsCanvas>().runCombatLogic();
        return playingUnit;
    }


    IEnumerator waitToFinishAttacking() {
        yield return new WaitForEndOfFrame();

        if(playingUnit.GetComponent<UnitClass>().attacking)
            attackWaiter = StartCoroutine(waitToFinishAttacking());
        else {
            setNextInTurnOrder();
            attackWaiter = null;
        }
    }
}

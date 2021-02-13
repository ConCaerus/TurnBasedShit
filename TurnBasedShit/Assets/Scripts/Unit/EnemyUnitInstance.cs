using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitInstance : UnitClass {


    Coroutine combatTurnWaitor = null;

    private void Awake() {
        isPlayerUnit = false;
    }


    private void Update() {
        if((attacking || defending) && combatTurnWaitor == null && FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject) {
            combatTurnWaitor = StartCoroutine(combatTurn());
        }
    }


    IEnumerator combatTurn() {
        yield return new WaitForSeconds(0.5f);

        if(attacking && attackingTarget == null) {
            setRandomAttackingTarget();
        }

        yield return new WaitForSeconds(0.5f);

        if(attacking)
            attackTargetUnit();
        combatTurnWaitor = null;
    }
}

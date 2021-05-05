using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitInstance : UnitClass {
    public GameInfo.diffLvl enemyDiff = 0;
    public GameInfo.element weakTo, strongTo;


    private void Awake() {
        isPlayerUnit = false;
    }


    public IEnumerator combatTurn() {
        yield return new WaitForSeconds(0.5f);
        if(this != null && FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject && attackingTarget == null) {
            setRandomAttackingTarget();

            yield return new WaitForSeconds(0.5f);

            if(this != null && FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject) {
                attack(attackingTarget);
            }
        }
    }
}

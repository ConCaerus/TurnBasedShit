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
        drawAttackingLine();
    }


    void drawAttackingLine() {
        var line = GetComponent<LineRenderer>();

        if(attacking && attackingTarget != null) {
            line.enabled = true;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, attackingTarget.transform.position);
            line.startColor = Color.white / 2.0f;
            line.endColor = Color.white / 4.0f;
        }

        else if(!attacking) {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position);
            line.enabled = false;
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

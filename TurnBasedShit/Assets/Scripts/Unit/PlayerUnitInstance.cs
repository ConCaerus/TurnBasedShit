using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerUnitInstance : UnitClass {

    private void Awake() {
        isPlayerUnit = true;
    }


    private void Update() {
        attackingLogic();
    }


    void attackingLogic() {
        drawAttackingLine();

        if(attacking && attackingTarget == null) {
            setAttackingTarget();
        }

        else if(attacking && attackingTarget != null && FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject) {
            attackTargetUnit();
        }
    }


    void drawAttackingLine() {
        var line = GetComponent<LineRenderer>();

        if(attacking) {
            line.enabled = true;
            line.SetPosition(0, transform.position);
            if(attackingTarget == null) {
                line.SetPosition(1, getMousePos());
                line.startColor = Color.white;
                line.endColor = Color.white / 2.0f;
            }
            else if(attackingTarget != null) {
                line.SetPosition(1, attackingTarget.transform.position);
                line.startColor = Color.white / 2.0f;
                line.endColor = Color.white / 4.0f;
            }
        }

        else {
            line.enabled = false;
        }
    }

    void setAttackingTarget() {
        if(Input.GetMouseButtonDown(0)) {
            foreach(var i in FindObjectsOfType<UnitClass>()) {
                if(i.isMouseOverUnit) {
                    attackingTarget = i.gameObject;
                    return;
                }
            }
        }
    }
}

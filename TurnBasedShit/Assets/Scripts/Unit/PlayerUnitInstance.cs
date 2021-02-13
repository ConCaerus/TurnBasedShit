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
        if(attacking && attackingTarget == null) {
            setAttackingTarget();
        }

        else if(attacking && attackingTarget != null && FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject) {
            attackTargetUnit();
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

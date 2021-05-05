using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerUnitInstance : UnitClass {
    public GameObject heldWeapon;

    private void Awake() {
        isPlayerUnit = true;
    }

    private void Start() {
        updateHeldWeapon();
    }


    private void Update() {
        attackingLogic();
    }


    void attackingLogic() {
        if(FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject && attackingTarget == null) {
            setAttackingTarget();
        }

        else if(attackingTarget != null && FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject) {
            attack(attackingTarget);
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

    public void updateHeldWeapon() {
        var w = stats.equippedWeapon;
        if(w == null || w.isEmpty())
            return;

        heldWeapon.transform.localPosition = new Vector2(w.heldX, w.heldY);
        heldWeapon.transform.localScale = new Vector3(-w.heldSize, w.heldSize, 0.0f);
        heldWeapon.transform.rotation = Quaternion.Euler(0.0f, 0.0f, w.heldRot);
        heldWeapon.GetComponent<SpriteRenderer>().sprite = w.w_sprite.getSprite();
    }
}

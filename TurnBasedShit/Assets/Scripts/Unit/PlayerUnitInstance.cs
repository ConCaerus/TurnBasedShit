﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerUnitInstance : UnitClass {
    private void Awake() {
        isPlayerUnit = true;
    }

    private void Start() {
        updateSprites();
    }


    private void Update() {
        attackingLogic();
        if(Input.GetKeyDown(KeyCode.G))
            GetComponentInChildren<UnitSpriteHandler>().setWeapon(stats.equippedWeapon);
    }


    public override void setAttackingAnim() {
        if(GetComponentInChildren<UnitSpriteHandler>() != null) {
            GetComponentInChildren<UnitSpriteHandler>().setAnimSpeed(1.0f);
            GetComponentInChildren<UnitSpriteHandler>().setAnimState(2);
        }
    }
    public override void setDefendingAnim() {
        if(GetComponentInChildren<UnitSpriteHandler>() != null) {
            GetComponentInChildren<UnitSpriteHandler>().setAnimSpeed(1.0f);
            GetComponentInChildren<UnitSpriteHandler>().setAnimState(1);
        }
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

    public void updateSprites() {
        GetComponentInChildren<UnitSpriteHandler>().setEverything(stats.u_sprite, stats.equippedWeapon, stats.equippedArmor);
    }
}

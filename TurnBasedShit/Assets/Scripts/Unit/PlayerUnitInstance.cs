using System.Collections;
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
        if(FindObjectOfType<TurnOrderSorter>().playingUnit != gameObject)
            attackAnim = null;
        if(FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject && attackingTarget == null) {
            setAttackingTarget();
        }

        else if(attackingTarget != null && attackingTarget != gameObject && attackAnim == null && FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject && FindObjectOfType<BattleOptionsCanvas>().battleState == 1) {
            attack(attackingTarget);
        }

        if(attackAnim == null && FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject && FindObjectOfType<BattleOptionsCanvas>().battleState == 3) {
            useWeaponSpecialUse();
        }
    }

    void setAttackingTarget() {
        if(Input.GetMouseButtonDown(0) && (FindObjectOfType<BattleOptionsCanvas>().battleState == 1 || FindObjectOfType<BattleOptionsCanvas>().battleState == 3)) {
            foreach(var i in FindObjectsOfType<UnitClass>()) {
                if(i.isMouseOverUnit) {
                    attackingTarget = i.gameObject;
                    return;
                }
            }
        }
    }


    void useWeaponSpecialUse() {
        //  wants to heal but no target
        if(stats.equippedWeapon.w_specialUsage == Weapon.specialUsage.healing && attackingTarget == null)
            return;
        if(stats.equippedWeapon.w_specialUsage == Weapon.specialUsage.healing && attackingTarget != null) {
            stats.equippedWeapon.applySpecailUsage(attackingTarget);
            FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
        }

        //  summon
        if(stats.equippedWeapon.w_specialUsage == Weapon.specialUsage.summoning) {
            var obj = Instantiate(stats.equippedWeapon.w_summonedUnit.gameObject);
            FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
        }
    }


    public void addWeaponTypeExpOnKill(GameInfo.diffLvl diff) {
        float exp = 2.5f * (1 + (int)diff);
        if(stats.equippedWeapon.w_attackType == Weapon.attackType.blunt)
            stats.u_bluntExp += exp;
        else if(stats.equippedWeapon.w_attackType == Weapon.attackType.edged)
            stats.u_edgedExp += exp;
        else if(stats.equippedWeapon.w_attackType == Weapon.attackType.summoned)
            stats.u_summonedExp += exp;
    }

    public void updateSprites() {
        GetComponentInChildren<UnitSpriteHandler>().setEverything(stats.u_sprite, stats.equippedWeapon, stats.equippedArmor);
        foreach(var i in FindObjectsOfType<CombatSpot>()) {
            if(i.unit == gameObject)
                i.setColor();
        }
    }
}

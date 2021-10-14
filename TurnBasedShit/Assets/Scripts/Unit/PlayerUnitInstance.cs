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
            stats.equippedWeapon.applySpecailUsage(stats, attackingTarget.GetComponent<UnitClass>());
            FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
        }

        //  summon
        if(stats.equippedWeapon.w_specialUsage == Weapon.specialUsage.summoning && roomToSummon()) {
            var obj = Instantiate(FindObjectOfType<PresetLibrary>().getSummonForWeapon(stats.equippedWeapon).gameObject);
            obj.GetComponent<SummonedUnitInstance>().summoner = stats;
            FindObjectOfType<SummonSpotSpawner>().getCombatSpotAtIndexForUnit(gameObject, getSummonCount() - 1).GetComponentInChildren<CombatSpot>().unit = obj.gameObject;
            obj.transform.position = FindObjectOfType<SummonSpotSpawner>().getCombatSpotAtIndexForUnit(gameObject, getSummonCount() - 1).transform.GetChild(0).transform.position + new Vector3(0.0f, obj.GetComponent<UnitClass>().spotOffset);
            obj.GetComponent<UnitClass>().setup();

            //  apply item modifiers to summon
            if(stats.equippedItem != null && !stats.equippedItem.isEmpty()) {
                obj.GetComponent<UnitClass>().tempPowerMod += stats.equippedItem.getPassiveMod(Item.passiveEffectTypes.modSummonDamageGiven);
            }

            FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
        }
    }


    bool roomToSummon() {
        int max = stats.getSummonedLevel();
        int count = 0;
        foreach(var i in FindObjectsOfType<SummonedUnitInstance>()) {
            if(i.summoner.isEqualTo(stats))
                count++;
        }
        return count < max;
    }

    int getSummonCount() {
        int count = 0;
        foreach(var i in FindObjectsOfType<SummonedUnitInstance>()) {
            if(i.summoner.isEqualTo(stats))
                count++;
        }
        return count;
    }

    //  returns true if the level increased
    public bool addWeaponTypeExpOnKill(float ex) {
        if(stats.equippedWeapon.w_attackType == Weapon.attackType.blunt) {
            int temp = stats.getBluntLevel();
            stats.u_bluntExp += ex;
            return temp != stats.getBluntLevel();
        }
        else if(stats.equippedWeapon.w_attackType == Weapon.attackType.edged) {
            int temp = stats.getEdgedLevel();
            stats.u_edgedExp += ex;
            return temp != stats.getEdgedLevel();
        }
        return false;
    }

    public void updateSprites() {
        GetComponentInChildren<UnitSpriteHandler>().setEverything(stats.u_sprite, stats.equippedWeapon, stats.equippedArmor);
        foreach(var i in FindObjectsOfType<CombatSpot>()) {
            if(i.unit == gameObject) {
                i.setColor();
                foreach(var j in i.GetComponentsInChildren<CombatSpot>())
                    j.setColor();
            }
        }

        //  if not summoning, kill all summoned shit
        if(stats.equippedWeapon == null || stats.equippedWeapon.isEmpty() || stats.equippedWeapon.w_specialUsage != Weapon.specialUsage.summoning) {
            foreach(var i in FindObjectsOfType<SummonedUnitInstance>()) {
                if(i.summoner.isEqualTo(stats))
                    i.die(DeathInfo.killCause.murdered);
            }
        }

        FindObjectOfType<SummonSpotSpawner>().updateSpots();
    }
}

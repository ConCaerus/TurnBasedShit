using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerUnitInstance : UnitClass {
    private void Awake() {
        combatStats.isPlayerUnit = true;
    }

    private void Start() {
        updateSprites();
        FindObjectOfType<MenuCanvas>().addNewRunOnClose(updateSprites);
    }


    private void Update() {
        attackingLogic();

        if((Vector2)transform.localPosition != combatStats.normalPos)
            transform.localPosition = combatStats.normalPos;

        if(Input.GetKeyDown(KeyCode.Space))
            Debug.Log(combatStats.normalPos + "       " + (Vector2)transform.localPosition);
    }


    public override void setAttackingAnim() {
        if(GetComponentInChildren<UnitSpriteHandler>() != null) {
            GetComponentInChildren<UnitSpriteHandler>().setAnimSpeed(1.0f);
            GetComponentInChildren<UnitSpriteHandler>().triggerAttackAnim();
        }
    }
    public override void setDefendingAnim() {
        if(GetComponentInChildren<UnitSpriteHandler>() != null) {
            GetComponentInChildren<UnitSpriteHandler>().setAnimSpeed(1.0f);
            GetComponentInChildren<UnitSpriteHandler>().triggerDefendAnim();
        }
    }

    void attackingLogic() {
        if(FindObjectOfType<TurnOrderSorter>().playingUnit != gameObject)
            attackAnim = null;
        if(FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject && combatStats.attackingTarget == null) {
            setAttackingTarget();
        }

        else if(combatStats.attackingTarget != null && combatStats.attackingTarget != gameObject && attackAnim == null && FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject && FindObjectOfType<BattleOptionsCanvas>().battleState == 1) {
            attack(combatStats.attackingTarget);
        }

        if(attackAnim == null && FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject && FindObjectOfType<BattleOptionsCanvas>().battleState == 3) {
            useWeaponSpecialUse();
        }
    }

    void setAttackingTarget() {
        if(Input.GetMouseButtonDown(0) && (FindObjectOfType<BattleOptionsCanvas>().battleState == 1 || FindObjectOfType<BattleOptionsCanvas>().battleState == 3)) {
            foreach(var i in FindObjectsOfType<UnitClass>()) {
                if(i.isMouseOverUnit) {
                    combatStats.attackingTarget = i.gameObject;
                    return;
                }
            }
        }
    }


    void useWeaponSpecialUse() {
        //  wants to heal but no target
        if(stats.weapon.sUsage == Weapon.specialUsage.healing && combatStats.attackingTarget == null)
            return;
        if(stats.weapon.sUsage == Weapon.specialUsage.healing && combatStats.attackingTarget != null) {
            stats.weapon.applySpecailUsage(stats, combatStats.attackingTarget.GetComponent<UnitClass>());
            FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
        }

        //  summon
        if(stats.weapon.sUsage == Weapon.specialUsage.summoning && roomToSummon()) {
            var obj = Instantiate(FindObjectOfType<PresetLibrary>().getSummonForWeapon(stats.weapon).gameObject);
            obj.GetComponent<SummonedUnitInstance>().summoner = stats;
            getNextSummonSpotForUnit().GetComponent<CombatSpot>().unit = obj.gameObject;
            obj.transform.position = FindObjectOfType<SummonSpotSpawner>().getCombatSpotAtIndexForUnit(gameObject, getSummonCount() - 1).transform.position + new Vector3(0.0f, obj.GetComponent<UnitClass>().combatStats.spotOffset);
            obj.GetComponent<UnitClass>().setup();

            //  apply item modifiers to summon
            if(stats.item != null && !stats.item.isEmpty()) {
                obj.GetComponent<UnitClass>().combatStats.tempPowerMod += stats.item.getPassiveMod(Item.passiveEffectTypes.modSummonDamageGiven);
            }

            FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
        }
    }


    public bool roomToSummon() {
        int max = stats.getSummonedLevel();
        int count = 0;
        foreach(var i in FindObjectsOfType<SummonedUnitInstance>()) {
            if(i.summoner.isTheSameInstanceAs(stats))
                count++;
        }
        return count < max;
    }

    public GameObject getNextSummonSpotForUnit() {
        Debug.Log(getSummonCount() - 1);
        return FindObjectOfType<SummonSpotSpawner>().getCombatSpotAtIndexForUnit(gameObject, getSummonCount() - 1);
    }

    int getSummonCount() {
        int count = 0;
        foreach(var i in FindObjectsOfType<SummonedUnitInstance>()) {
            if(i.summoner.isTheSameInstanceAs(stats))
                count++;
        }
        return count;
    }

    //  returns true if the level increased
    public bool addWeaponTypeExpOnKill(float ex) {
        if(stats.weapon.aType == Weapon.attackType.blunt) {
            int temp = stats.getBluntLevel();
            stats.u_bluntExp += ex;
            return temp != stats.getBluntLevel();
        }
        else if(stats.weapon.aType == Weapon.attackType.edged) {
            int temp = stats.getEdgedLevel();
            stats.u_edgedExp += ex;
            return temp != stats.getEdgedLevel();
        }
        return false;
    }

    public void updateSprites() {
        GetComponentInChildren<UnitSpriteHandler>().setReference(stats, true);
        GetComponentInChildren<UnitSpriteHandler>().updateVisuals();
        foreach(var i in FindObjectsOfType<CombatSpot>()) {
            if(i.unit == gameObject) {
                i.setColor();
                foreach(var j in i.GetComponentsInChildren<CombatSpot>())
                    j.setColor();
            }
        }

        //  if not summoning, kill all summoned shit
        if((stats.weapon == null || stats.weapon.isEmpty() || stats.weapon.sUsage != Weapon.specialUsage.summoning) && (stats.item == null || stats.item.isEmpty() || stats.item.getTimedMod(Item.timedEffectTypes.chanceEnemyTurnsIntoSummon) == 0.0f)) {
            foreach(var i in FindObjectsOfType<SummonedUnitInstance>()) {
                if(i.summoner.isTheSameInstanceAs(stats))
                    i.die(DeathInfo.killCause.murdered);
            }
        }

        FindObjectOfType<SummonSpotSpawner>().updateSpots();
        combatStats.normalPos = GetComponentInChildren<UnitSpriteHandler>().getCombatNormalPos();
        transform.localPosition = combatStats.normalPos;
        transform.localScale = combatStats.normalSize;
    }
}

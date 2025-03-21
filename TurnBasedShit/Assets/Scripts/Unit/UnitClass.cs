﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class UnitClass : MonoBehaviour {
    public UnitCombatStats combatStats;

    public UnitStats stats;

    [System.Serializable]
    public enum combatModifier {
        onlyAttackOnEvenRounds
    }

    public combatModifier[] combatMods;

    public bool defending = false;
    public bool stunned = false;
    public bool charging = false;

    public bool isMouseOverUnit { get; protected set; } = false;
    public Coroutine attackAnim, defendAnim;



    private void OnMouseEnter() {
        if(FindObjectOfType<MenuCanvas>().isOpen() || FindObjectOfType<BattleResultsCanvas>() != null) {
            isMouseOverUnit = false;
            FindObjectOfType<UnitCombatHighlighter>().updateHighlights();
            return;
        }

        if(FindObjectOfType<BattleOptionsCanvas>().battleState == 3 && stats.weapon.sUsage == Weapon.specialUsage.healing) {
            GetComponent<CombatUnitUI>().showingWouldBeHealedValue = true;
            GetComponent<CombatUnitUI>().moveLightHealthSliderToValue(stats.u_health + FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().stats.weapon.sUsageAmount);
        }

        isMouseOverUnit = true;
        FindObjectOfType<UnitCombatHighlighter>().updateHighlights();
    }

    private void OnMouseOver() {
        GetComponent<InfoBearer>().setInfo(InfoTextCreator.createForUnitClass(this));
        if(FindObjectOfType<BattleOptionsCanvas>().battleState == 3) {
            GetComponent<CombatUnitUI>().showingWouldBeHealedValue = true;
            GetComponent<CombatUnitUI>().moveLightHealthSliderToValue(stats.u_health + FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().stats.weapon.sUsageAmount);
        }
    }

    private void OnMouseExit() {
        if(GetComponent<CombatUnitUI>().showingWouldBeHealedValue) {
            GetComponent<CombatUnitUI>().showingWouldBeHealedValue = false;
            GetComponent<CombatUnitUI>().updateUIInfo();
        }

        isMouseOverUnit = false;
        FindObjectOfType<UnitCombatHighlighter>().updateHighlights();
    }

    public void setup() {
        if(combatStats.isPlayerUnit && GetComponentInChildren<UnitSpriteHandler>() != null) {
            GetComponentInChildren<UnitSpriteHandler>().setReference(stats, true);
        }
        GetComponent<InfoBearer>().setInfo(InfoTextCreator.createForUnitClass(this));

        if(string.IsNullOrEmpty(stats.u_name)) {
            name = NameLibrary.getRandomUsablePlayerName();
        }
        else
            name = stats.u_name;

        foreach(var i in FindObjectsOfType<CombatSpot>()) {
            if(i.unit == gameObject) {
                transform.parent = i.transform;
                break;
            }
        }

        if(GetComponent<BossUnitInstance>() == null) {
            combatStats.normalSize = new Vector3(1.0f, 1.0f);
            combatStats.normalPos = new Vector3(0.0f, combatStats.spotOffset);
        }

        if(GetComponentInChildren<UnitSpriteHandler>() != null)
            combatStats.normalPos = GetComponentInChildren<UnitSpriteHandler>().getCombatNormalPos();

        transform.localPosition = combatStats.normalPos;
        transform.localScale = combatStats.normalSize;

        if(combatStats.isPlayerUnit) {
            FindObjectOfType<PartyObject>().resaveInstantiatedUnit(stats);
        }
    }


    public float getSpeed() {
        return stats.getModifiedSpeed(combatStats.tempSpeedMod);
    }


    public void takeBleedDamage() {
        if(stats.u_bleedCount > 0) {
            float temp = (stats.getModifiedMaxHealth() / 100.0f) * stats.u_bleedCount;
            stats.u_health -= temp;

            FindObjectOfType<DamageTextCanvas>().showDamageTextForUnit(gameObject, temp, DamageTextCanvas.damageType.bleed);
            if(GameVariables.chanceCureBleed()) //  currently at 100% but you never know
                stats.u_bleedCount--;

            checkIfDead(DeathInfo.killCause.bleed);
        }
    }

    public bool checkIfDead(DeathInfo.killCause cause, GameObject killer = null) {
        if(stats.u_health <= 0.0f) {
            die(cause, killer);
            return true;
        }
        return false;
    }

    public void addHealth(float h) {
        stats.u_health = Mathf.Clamp(stats.u_health + h, -1.0f, stats.getModifiedMaxHealth());
        if(h > 0)
            FindObjectOfType<DamageTextCanvas>().showDamageTextForUnit(gameObject, h, DamageTextCanvas.damageType.healed);
        else
            FindObjectOfType<DamageTextCanvas>().showDamageTextForUnit(gameObject, h, DamageTextCanvas.damageType.lostHealth);
    }

    public void prepareUnitForNextRound() {
        defending = false;

        takeBleedDamage();
    }

    public bool levelUpIfPossible() {
        if(stats.canLevelUp()) {
            FindObjectOfType<DamageTextCanvas>().showLevelUpTextForUnit(gameObject);
            stats.levelUp();
            FindObjectOfType<AudioManager>().playLevelUpSound();
            return true;
        }
        return false;
    }

    public void setDefending(bool b) {
        if(stunned) {
            stunned = false;
            return;
        }

        //  non value based traits after turn
        foreach(var i in stats.u_traits) {
            if(!stunned && GameVariables.chanceOutOfHundred((int)i.getPassiveMod(StatModifier.passiveModifierType.stunSelfChance, stats, false))) {
                stunned = true;
                break;
            }
        }
        if(stats.u_talent != null && !stunned)
            stunned = GameVariables.chanceOutOfHundred((int)stats.u_talent.getPassiveMod(StatModifier.passiveModifierType.stunSelfChance, stats, false));

        defending = b;
    }
    public void setUnstunned() {
        stunned = false;
    }
    public bool isStunned() {
        return stunned;
    }
    public void chanceGettingStunned(float chance) {
        foreach(var i in stats.getAllPassiveMods())
            chance *= i.getMod(StatModifier.passiveModifierType.stunSelfChance, stats, true);

        stunned = GameVariables.chanceOutOfHundred((int)chance);
    }

    public void attack(GameObject defender) {
        if(defender == gameObject)
            return;

        if(stunned) {
            stunned = false;
            return;
        }

        transform.DOComplete();
        //  triggers
        foreach(var i in stats.getAllTimedMods())
            i.triggerUseTime(this, StatModifier.useTimeType.beforeAttacking);

        //  triggers
        stats.weapon.applyAttributes(gameObject, defender);

        //  Flair
        attackAnim = StartCoroutine(attackingAnim(defender));
    }


    public void defend(GameObject attacker, float dmg) {
        //  triggers
        foreach(var i in stats.getAllTimedMods())
            i.triggerUseTime(this, StatModifier.useTimeType.beforeDefending);


        //  if defender is an enemy, check if it's weak or strong to the attack
        if(!combatStats.isPlayerUnit) {
            //  check if it's weak to the attack
            if(GetComponent<EnemyUnitInstance>().weakTo == attacker.GetComponent<UnitClass>().stats.weapon.aType)
                dmg *= 1.25f;
        }
        dmg *= (stats.getDefenceMult(defending, FindObjectOfType<PresetLibrary>(), combatStats.tempDefenceMod));

        float crit = attacker.GetComponent<UnitClass>().stats.getCritMult();
        dmg *= crit;
        //  triggers
        stats.armor.applyAttributes(gameObject, attacker, FindObjectOfType<TurnOrderSorter>().playingUnit);

        //  take damage
        stats.u_health = Mathf.Clamp(stats.u_health - dmg, -1.0f, stats.getModifiedMaxHealth());

        //  Flair
        if(attacker.GetComponent<UnitClass>().charging)
            FindObjectOfType<DamageTextCanvas>().showDamageTextForUnit(gameObject, dmg, DamageTextCanvas.damageType.charged);
        else if(defending)
            FindObjectOfType<DamageTextCanvas>().showDamageTextForUnit(gameObject, dmg, DamageTextCanvas.damageType.defended);
        else if(crit > 1.1f)
            FindObjectOfType<DamageTextCanvas>().showDamageTextForUnit(gameObject, dmg, DamageTextCanvas.damageType.crit);
        else
            FindObjectOfType<DamageTextCanvas>().showDamageTextForUnit(gameObject, dmg, DamageTextCanvas.damageType.weapon);
        var blood = Instantiate(combatStats.bloodParticles);
        Destroy(blood.gameObject, blood.main.startLifetimeMultiplier);
        blood.gameObject.transform.position = transform.position;
        FindObjectOfType<AudioManager>().playSound(combatStats.hurtSound);
        defendAnim = StartCoroutine(defendingAnim());

        //  chance worn state decrease
        if(stats.armor != null && !stats.armor.isEmpty() && stats.armor.wornAmount > GameInfo.wornState.Old && GameVariables.chanceEquipmentWornDecrease(stats) && combatStats.isPlayerUnit) {
            stats.armor.wornAmount--;
            FindObjectOfType<DamageTextCanvas>().showTatterTextForUnit(gameObject);
        }


        //  check if any unit died in the attack
        attacker.GetComponent<UnitClass>().checkIfDead(DeathInfo.killCause.murdered, gameObject);
        if(checkIfDead(DeathInfo.killCause.murdered, attacker.gameObject))
            return;

        //  end battle turn
        if(FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject)
            return;
        foreach(var i in stats.armor.attributes) {
            if(i == Armor.attribute.Reflex)
                attack(attacker.gameObject);
            return;
        }
    }

    public void die(DeathInfo.killCause cause, GameObject killer = null) {
        //  things to do with removing the unit from the party and what to do with equippment
        if(combatStats.isPlayerUnit && stats.u_type == GameInfo.combatUnitType.player) {
            stats.die(cause, FindObjectOfType<PresetLibrary>(), FindObjectOfType<FullInventoryCanvas>(), killer);
            FindObjectOfType<SummonSpotSpawner>().updateSpots();
        }
        else if(!combatStats.isPlayerUnit) {
            //  triggers items
            if(stats.item != null && !stats.item.isEmpty())
                stats.item.triggerUseTime(killer.GetComponent<UnitClass>(), StatModifier.useTimeType.afterKill);


            //  add exp
            if(killer != null && killer.GetComponent<PlayerUnitInstance>() != null) {
                killer.GetComponent<PlayerUnitInstance>().addWeaponTypeExpOnKill(GameVariables.getExpForEnemy(stats.u_type));
                killer.GetComponent<PlayerUnitInstance>().stats.addExp(GameVariables.getExpForEnemy(stats.u_type));
            }
            else if(killer != null && killer.GetComponent<SummonedUnitInstance>() != null) {
                int lvlBefore = killer.GetComponent<SummonedUnitInstance>().summoner.getSummonedLevel();

                killer.GetComponent<SummonedUnitInstance>().summoner.addSummonExp(GameVariables.getExpForEnemy(stats.u_type));

                if(lvlBefore != killer.GetComponent<SummonedUnitInstance>().summoner.getSummonedLevel())
                    FindObjectOfType<DamageTextCanvas>().showSummonLevelUpTextForUnit(FindObjectOfType<PartyObject>().getInstantiatedMember(killer.GetComponent<SummonedUnitInstance>().summoner).gameObject);

                if(killer.GetComponent<SummonedUnitInstance>().summoner.addExp(GameVariables.getExpForEnemy(stats.u_type)))
                    FindObjectOfType<DamageTextCanvas>().showLevelUpTextForUnit(FindObjectOfType<PartyObject>().getInstantiatedMember(killer.GetComponent<SummonedUnitInstance>().summoner).gameObject);
            }

            //  get enemy drops
            int chanceMod = 0;
            if(killer != null) {
                foreach(var i in killer.GetComponent<UnitClass>().stats.u_traits)
                    chanceMod += (int)i.getPassiveMod(StatModifier.passiveModifierType.modEnemyDropChance, stats, false);
                if(killer.GetComponent<UnitClass>().stats.u_talent != null)
                    chanceMod += (int)killer.GetComponent<UnitClass>().stats.u_talent.getPassiveMod(StatModifier.passiveModifierType.modEnemyDropChance, stats, false);
            }
            GetComponent<EnemyUnitInstance>().chanceWeaponDrop(chanceMod);
            GetComponent<EnemyUnitInstance>().chanceArmorDrop(chanceMod);
            GetComponent<EnemyUnitInstance>().chanceItemDrop(chanceMod);
            GetComponent<EnemyUnitInstance>().chanceUsableDrop(chanceMod);
            GetComponent<EnemyUnitInstance>().chanceUnusableDrop(chanceMod);

            //  increases acc quest counter
            for(int i = 0; i < ActiveQuests.getHolder().getObjectCount<KillQuest>(); i++) {
                var k = ActiveQuests.getHolder().getObject<KillQuest>(i);
                if(k.enemyType == GetComponent<EnemyUnitInstance>().stats.u_type && !k.completed) {
                    k.howManyToKill--;
                    if(k.howManyToKill > 0)
                        ActiveQuests.overrideQuest(i, k);
                    else
                        ActiveQuests.completeQuest(k, FindObjectOfType<QuestCompleteCanvas>());
                }
            }
        }

        //  flair
        FindObjectOfType<AudioManager>().playSound(combatStats.dieSound);
        foreach(var i in FindObjectsOfType<CombatSpot>()) {
            if(i.unit == gameObject) {
                i.unit = null;
                i.setColor();
            }
        }

        //  removes unit from game world
        if(FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject)
            FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
        FindObjectOfType<TurnOrderSorter>().removeUnitFromList(gameObject);
        Destroy(gameObject);
    }

    public abstract void setDefendingAnim();
    public abstract void setAttackingAnim();

    IEnumerator defendingAnim() {
        if(attackAnim != null)
            StopCoroutine(attackAnim);

        GetComponent<SpriteRenderer>().DOColor(combatStats.hitColor, 0.05f);
        transform.DOScale(combatStats.normalSize * 1.5f, 0.15f);

        setDefendingAnim();
        //  flair
        var blood = Instantiate(combatStats.bloodParticles, transform);
        blood.transform.position = transform.position;
        if(combatStats.isPlayerUnit)
            blood.transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        Destroy(blood, blood.GetComponent<ParticleSystem>().duration);

        if(GetComponentInChildren<UnitSpriteHandler>() == null) {
            yield return new WaitForEndOfFrame();
            for(int i = 0; i < transform.childCount; i++) {
                if(transform.GetChild(i).gameObject.activeInHierarchy) {
                    foreach(var s in transform.GetChild(i).gameObject.GetComponentsInChildren<SpriteRenderer>()) {
                        s.color = combatStats.hitColor;
                        s.DOColor(Color.white, .35f);
                    }
                }
            }
        }
        else {
            GetComponentInChildren<UnitSpriteHandler>().setATempColor(combatStats.hitColor);
            GetComponentInChildren<UnitSpriteHandler>().tweenColorToNormal(.35f);
        }

        yield return new WaitForSeconds(0.1f);

        GetComponent<SpriteRenderer>().DOColor(stats.u_sprite.color, 0.25f);

        yield return new WaitForSeconds(0.25f);

        transform.DOScale(combatStats.normalSize, 0.15f);
    }
    IEnumerator attackingAnim(GameObject defender) {
        //  play animation
        setAttackingAnim();

        //  windup
        yield return new WaitForSeconds(0.25f);

        //  move to target
        if(combatStats.isPlayerUnit)
            transform.DOMove(defender.transform.position - new Vector3(1.25f, 0.0f, 0.0f), 0.25f);
        else
            transform.DOMove(defender.transform.position + new Vector3(1.25f, 0.0f, 0.0f), 0.25f);
        transform.DOScale(combatStats.normalSize * 1.15f, 0.15f);

        //  wait for attacker to reach defender
        yield return new WaitForSeconds(0.35f);
        int bluntLvlBefore = stats.getBluntLevel();
        int edgedLvlBefore = stats.getEdgedLevel();
        int lvlBefore = stats.u_level;
        var missChance = stats.getModifiedMissChance() + defender.GetComponent<UnitClass>().stats.getModToAttackersMiss();
        bool miss = GameVariables.chanceOutOfHundred(missChance);

        //  damage logic
        if(!miss) {
            var dmg = stats.getDamageGiven(FindObjectOfType<PresetLibrary>()) + combatStats.tempPowerMod;
            bool healInstead = false;
            //  check if other modifications to damage
            foreach(var i in stats.getAllPassiveMods()) {
                if(i.getMod(StatModifier.passiveModifierType.healInsteadOfDamage, stats, false) != 0.0f) {
                    dmg *= stats.item.getPassiveMod(StatModifier.passiveModifierType.healInsteadOfDamage, stats, true);
                    defender.GetComponent<UnitClass>().addHealth(dmg);
                    healInstead = true;
                }
            }

            //  actually deal damage to defender
            if(!healInstead) {
                if(charging) {
                    var chargedMod = 2.0f;
                    foreach(var i in stats.getAllPassiveMods()) {
                        chargedMod += i.getMod(StatModifier.passiveModifierType.addChargedPower, stats, false);
                    }
                    dmg *= chargedMod;
                }
                defender.GetComponent<UnitClass>().defend(gameObject, dmg);
            }
        }
        else {
            FindObjectOfType<DamageTextCanvas>().showMissTextForUnit(gameObject);
            FindObjectOfType<AudioManager>().playSound(combatStats.missSound);
        }
        charging = false;
        yield return new WaitForSeconds(0.4f);

        //  move back to original position
        transform.DOScale(combatStats.normalSize, 0.15f);
        transform.DOLocalMove(combatStats.normalPos, 0.15f);

        yield return new WaitForSeconds(0.15f);

        if(bluntLvlBefore != stats.getBluntLevel())
            FindObjectOfType<DamageTextCanvas>().showBluntLevelUpTextForUnit(gameObject);
        if(edgedLvlBefore != stats.getEdgedLevel())
            FindObjectOfType<DamageTextCanvas>().showEdgedLevelUpTextForUnit(gameObject);
        if(lvlBefore != stats.u_level)
            FindObjectOfType<DamageTextCanvas>().showLevelUpTextForUnit(gameObject);

        //  non value based traits in attack
        foreach(var i in stats.getAllPassiveMods()) {
            if(!stunned && GameVariables.chanceOutOfHundred(Mathf.FloorToInt(i.getMod(StatModifier.passiveModifierType.stunSelfChance, stats, false)))) {
                stunned = true;
            }
            if(defender != null && !defender.GetComponent<UnitClass>().isStunned() && GameVariables.chanceOutOfHundred((int)i.getMod(StatModifier.passiveModifierType.stunTargetChance, stats, false)))
                defender.GetComponent<UnitClass>().stunned = true;
        }

        //  chance worn state decrease
        if(stats.weapon != null && !stats.weapon.isEmpty() && stats.weapon.wornAmount > GameInfo.wornState.Old && GameVariables.chanceEquipmentWornDecrease(stats) && combatStats.isPlayerUnit) {
            FindObjectOfType<DamageTextCanvas>().showTatterTextForUnit(gameObject);
            stats.weapon.wornAmount--;
        }

        attackAnim = null;
        GetComponent<CombatUnitUI>().updateUIInfo();
        if(defender.gameObject != null)
            defender.gameObject.GetComponent<CombatUnitUI>().updateUIInfo();
        FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
    }


    public int getNumberOfInPlaySummons() {
        int temp = 0;
        foreach(var i in FindObjectsOfType<SummonedUnitInstance>()) {
            if(i.summoner.isTheSameInstanceAs(stats))
                temp++;
        }
        return temp;
    }

    public void selfDestruct() {
        Destroy(this);
    }
}


[System.Serializable]
public class UnitCombatStats {
    public AudioClip hurtSound, dieSound, missSound;
    public GameObject attackingTarget = null;
    public ParticleSystem bloodParticles;
    public Color hitColor;

    public bool isPlayerUnit = true;

    public float tempPowerMod = 0.0f;   //  adds
    public float tempDefenceMod = 0.0f; //  adds
    public float tempSpeedMod = 0.0f;   //  adds 

    public float spotOffset = 0.0f;

    public Vector2 normalSize, normalPos;
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class UnitClass : MonoBehaviour {
    public UnitCombatStats combatStats;

    public UnitStats stats;

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

        if(FindObjectOfType<BattleOptionsCanvas>().battleState == 3) {
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

        combatStats.normalSize = new Vector3(1.0f, 1.0f);
        combatStats.normalPos = new Vector3(0.0f, combatStats.spotOffset);

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
            if(GameVariables.chanceCureBleed())
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
        FindObjectOfType<DamageTextCanvas>().showDamageTextForUnit(gameObject, h, DamageTextCanvas.damageType.healed);
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
            if(!stunned && i.shouldStunSelf()) {
                stunned = true;
                break;
            }
        }
        if(stats.u_talent != null && !stunned)
            stunned = stats.u_talent.shouldStunSelf();

        defending = b;
    }
    public void setStunned(bool b) {
        stunned = b;
    }
    public bool isStunned() {
        return stunned;
    }

    public void attack(GameObject defender) {
        if(stunned) {
            stunned = false;
            return;
        }
        transform.DOComplete();
        //  triggers
        if(stats.item != null && !stats.item.isEmpty())
            stats.item.triggerUseTime(this, Item.useTimes.beforeAttacking);

        //  triggers
        stats.weapon.applyAttributes(gameObject, defender);

        //  Flair
        attackAnim = StartCoroutine(attackingAnim(defender));
    }


    public void defend(GameObject attacker, float dmg) {
        //  triggers
        if(stats.item != null && !stats.item.isEmpty())
            stats.item.triggerUseTime(this, Item.useTimes.beforeDefending);


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
        if(stats.armor != null && !stats.armor.isEmpty() && stats.armor.wornAmount > GameInfo.wornState.old && GameVariables.chanceEquipmentWornDecrease() && combatStats.isPlayerUnit) {
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
        if(combatStats.isPlayerUnit) {
            stats.die(cause, killer);
        }
        else {
            //  triggers items
            if(stats.item != null && !stats.item.isEmpty())
                stats.item.triggerUseTime(killer.GetComponent<UnitClass>(), Item.useTimes.afterKill);


            //  add exp
            if(killer != null && killer.GetComponent<PlayerUnitInstance>() != null) {
                killer.GetComponent<PlayerUnitInstance>().addWeaponTypeExpOnKill(GameVariables.getExpForEnemy(GetComponent<EnemyUnitInstance>().enemyType));
                killer.GetComponent<PlayerUnitInstance>().stats.addExp(GameVariables.getExpForEnemy(GetComponent<EnemyUnitInstance>().enemyType));
            }
            else if(killer != null && killer.GetComponent<SummonedUnitInstance>() != null) {
                int lvlBefore = killer.GetComponent<SummonedUnitInstance>().summoner.getSummonedLevel();
                killer.GetComponent<SummonedUnitInstance>().summoner.u_summonedExp += GameVariables.getExpForEnemy(GetComponent<EnemyUnitInstance>().enemyType);
                if(lvlBefore != killer.GetComponent<SummonedUnitInstance>().summoner.getSummonedLevel())
                    FindObjectOfType<DamageTextCanvas>().showSummonLevelUpTextForUnit(FindObjectOfType<PartyObject>().getInstantiatedMember(killer.GetComponent<SummonedUnitInstance>().summoner).gameObject);

                if(killer.GetComponent<SummonedUnitInstance>().summoner.addExp(GameVariables.getExpForEnemy(GetComponent<EnemyUnitInstance>().enemyType)))
                    FindObjectOfType<DamageTextCanvas>().showLevelUpTextForUnit(FindObjectOfType<PartyObject>().getInstantiatedMember(killer.GetComponent<SummonedUnitInstance>().summoner).gameObject);
            }

            //  get enemy drops
            int chanceMod = 0;
            if(killer != null) {
                foreach(var i in killer.GetComponent<UnitClass>().stats.u_traits)
                    chanceMod += i.getEnemyDropChanceMod();
                if(killer.GetComponent<UnitClass>().stats.u_talent != null)
                    chanceMod += killer.GetComponent<UnitClass>().stats.u_talent.getEnemyDropChanceMod();
            }
            GetComponent<EnemyUnitInstance>().chanceWeaponDrop(chanceMod);
            GetComponent<EnemyUnitInstance>().chanceArmorDrop(chanceMod);
            GetComponent<EnemyUnitInstance>().chanceItemDrop(chanceMod);
            GetComponent<EnemyUnitInstance>().chanceUsableDrop(chanceMod);
            GetComponent<EnemyUnitInstance>().chanceUnusableDrop(chanceMod);

            //  increases acc quest counter
            for(int i = 0; i < ActiveQuests.getHolder().getObjectCount<KillQuest>(); i++) {
                var k = ActiveQuests.getHolder().getObject<KillQuest>(i);
                if(k.enemyType == GetComponent<EnemyUnitInstance>().enemyType && !k.completed) {
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
        bool miss = GameVariables.chanceOutOfHundred(stats.getModifiedMissChance());

        //  damage logic
        if(!miss) {
            var dmg = stats.getDamageGiven(FindObjectOfType<PresetLibrary>()) + combatStats.tempPowerMod;
            //  check if other modifications to damage
            if(stats.item != null && !stats.item.isEmpty()) {
                if(stats.item.getPassiveMod(Item.passiveEffectTypes.healInsteadOfDamage) != 0.0f) {
                    dmg *= stats.item.getPassiveMod(Item.passiveEffectTypes.healInsteadOfDamage);
                    defender.GetComponent<UnitClass>().addHealth(dmg);
                }
            }

            //  actually deal damage to defender
            else {
                if(charging) {
                    dmg *= 2.0f;
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
        foreach(var i in stats.u_traits) {
            if(!stunned && i.shouldStunSelf()) {
                stunned = true;
            }
            if(defender != null && !defender.GetComponent<UnitClass>().isStunned() && i.shouldStunTarget())
                defender.GetComponent<UnitClass>().setStunned(true);
        }
        if(stats.u_talent != null) {
            if(!stunned && stats.u_talent.shouldStunSelf()) {
                stunned = true;
            }
            if(defender != null && !defender.GetComponent<UnitClass>().isStunned() && stats.u_talent.shouldStunTarget())
                defender.GetComponent<UnitClass>().setStunned(true);
        }

        //  chance worn state decrease
        if(stats.weapon != null && !stats.weapon.isEmpty() && stats.weapon.wornAmount > GameInfo.wornState.old && GameVariables.chanceEquipmentWornDecrease() && combatStats.isPlayerUnit) {
            FindObjectOfType<DamageTextCanvas>().showTatterTextForUnit(gameObject);
            stats.weapon.wornAmount--;
        }

        attackAnim = null;
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
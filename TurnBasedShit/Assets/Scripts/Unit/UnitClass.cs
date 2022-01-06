using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class UnitClass : MonoBehaviour {
    [SerializeField] AudioClip hurtSound, dieSound;
    public GameObject attackingTarget = null;
    public ParticleSystem bloodParticles;
    public Color hitColor;

    public bool isPlayerUnit = true;
    public bool defending = false;
    public bool stunned = false;
    public bool charging = false;
    public bool isMouseOverUnit = false;

    public float tempPowerMod = 0.0f;   //  adds
    public float tempDefenceMod = 0.0f; //  adds
    public float tempSpeedMod = 0.0f;   //  adds 

    public float spotOffset = 0.0f;

    protected Vector2 normalSize, normalPos;
    public Coroutine attackAnim, defendAnim;


    public UnitStats stats;



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
        if(isPlayerUnit && GetComponentInChildren<UnitSpriteHandler>() != null) {
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
                normalSize = new Vector3(1.0f, 1.0f);
                normalPos = new Vector3(0.0f, spotOffset);
                break;
            }
        }

        transform.localPosition = normalPos;
        transform.localScale = normalSize;

        if(isPlayerUnit) {
            FindObjectOfType<PartyObject>().resaveInstantiatedUnit(stats);
        }
    }



    public float getSpeed() {
        return stats.getModifiedSpeed(tempSpeedMod);
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
        attackingTarget = null;
        defending = false;

        takeBleedDamage();
    }

    public bool levelUpIfPossible() {
        if(stats.canLevelUp()) {
            FindObjectOfType<DamageTextCanvas>().showLevelUpTextForUnit(gameObject);
            stats.levelUp();
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
            }
        }

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
        if(!isPlayerUnit) {
            //  check if it's weak to the attack
            if(GetComponent<EnemyUnitInstance>().weakTo == attacker.GetComponent<UnitClass>().stats.weapon.aType)
                dmg *= 1.25f;
        }
        dmg *= (stats.getDefenceMult(defending, FindObjectOfType<PresetLibrary>(), tempDefenceMod));

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
        var blood = Instantiate(bloodParticles);
        Destroy(blood.gameObject, blood.main.startLifetimeMultiplier);
        blood.gameObject.transform.position = transform.position;
        FindObjectOfType<AudioManager>().playSound(hurtSound);
        defendAnim = StartCoroutine(defendingAnim());

        //  chance worn state decrease
        if(stats.armor != null && !stats.armor.isEmpty() && stats.armor.wornAmount > GameInfo.wornState.old && GameVariables.chanceEquipmentWornDecrease() && isPlayerUnit) {
            stats.armor.wornAmount--;
            FindObjectOfType<DamageTextCanvas>().showBreakTextForUnit(gameObject);
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
        if(isPlayerUnit) {
            stats.die(cause, killer);
        }
        else {
            //  triggers items
            if(stats.item != null && !stats.item.isEmpty())
                stats.item.triggerUseTime(killer.GetComponent<UnitClass>(), Item.useTimes.afterKill);


            //  add exp
            if(killer != null && killer.GetComponent<PlayerUnitInstance>() != null) {
                killer.GetComponent<PlayerUnitInstance>().addWeaponTypeExpOnKill(GameVariables.getExpForDefeatedEnemy(GetComponent<EnemyUnitInstance>().enemyDiff));
                killer.GetComponent<PlayerUnitInstance>().stats.addExp(GameVariables.getExpForDefeatedEnemy(GetComponent<EnemyUnitInstance>().enemyDiff));
            }
            else if(killer != null && killer.GetComponent<SummonedUnitInstance>() != null) {
                int lvlBefore = killer.GetComponent<SummonedUnitInstance>().summoner.getSummonedLevel();
                killer.GetComponent<SummonedUnitInstance>().summoner.u_summonedExp += GameVariables.getExpForDefeatedEnemy(GetComponent<EnemyUnitInstance>().enemyDiff);
                if(lvlBefore != killer.GetComponent<SummonedUnitInstance>().summoner.getSummonedLevel())
                    FindObjectOfType<DamageTextCanvas>().showSummonLevelUpTextForUnit(FindObjectOfType<PartyObject>().getInstantiatedMember(killer.GetComponent<SummonedUnitInstance>().summoner).gameObject);

                if(killer.GetComponent<SummonedUnitInstance>().summoner.addExp(GameVariables.getExpForDefeatedEnemy(GetComponent<EnemyUnitInstance>().enemyDiff)))
                    FindObjectOfType<DamageTextCanvas>().showLevelUpTextForUnit(FindObjectOfType<PartyObject>().getInstantiatedMember(killer.GetComponent<SummonedUnitInstance>().summoner).gameObject);
            }

            //  get enemy drops
            int chanceMod = 0;
            if(killer != null) {
                foreach(var i in killer.GetComponent<UnitClass>().stats.u_traits)
                    chanceMod += i.getEnemyDropChanceMod();
            }
            GetComponent<EnemyUnitInstance>().chanceWeaponDrop(chanceMod);
            GetComponent<EnemyUnitInstance>().chanceArmorDrop(chanceMod);
            GetComponent<EnemyUnitInstance>().chanceItemDrop(chanceMod);
            GetComponent<EnemyUnitInstance>().chanceUsableDrop(chanceMod);
            GetComponent<EnemyUnitInstance>().chanceUnusableDrop(chanceMod);

            //  increases acc quest counter
            for(int i = 0; i < ActiveQuests.getQuestHolder().getObjectCount<KillQuest>(); i++) {
                var k = ActiveQuests.getQuestHolder().getObject<KillQuest>(i);
                if(k.enemyType == GetComponent<EnemyUnitInstance>().enemyType) {
                    k.howManyToKill--;
                    ActiveQuests.overrideQuest(i, k);
                }
            }
        }

        //  flair
        FindObjectOfType<AudioManager>().playSound(dieSound);
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

        GetComponent<SpriteRenderer>().DOColor(hitColor, 0.05f);
        transform.DOScale(normalSize * 1.5f, 0.15f);

        setDefendingAnim();
        //  flair
        var blood = Instantiate(bloodParticles, transform);
        blood.transform.position = transform.position;
        if(isPlayerUnit)
            blood.transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        Destroy(blood, blood.GetComponent<ParticleSystem>().duration);

        if(!isPlayerUnit) {
            yield return new WaitForEndOfFrame();
            for(int i = 0; i < transform.childCount; i++) {
                if(transform.GetChild(i).gameObject.activeInHierarchy) {
                    foreach(var s in transform.GetChild(i).gameObject.GetComponentsInChildren<SpriteRenderer>()) {
                        s.color = hitColor;
                        s.DOColor(Color.white, .35f);
                    }
                }
            }
        }
        else {
            GetComponentInChildren<UnitSpriteHandler>().setATempColor(hitColor);
            GetComponentInChildren<UnitSpriteHandler>().tweenColorToNormal(.35f);
        }

        yield return new WaitForSeconds(0.1f);

        GetComponent<SpriteRenderer>().DOColor(stats.u_sprite.color, 0.25f);

        yield return new WaitForSeconds(0.25f);

        transform.DOScale(normalSize, 0.15f);
    }
    IEnumerator attackingAnim(GameObject defender) {
        //  play animation
        setAttackingAnim();

        //  windup
        yield return new WaitForSeconds(0.25f);

        //  move to target
        if(isPlayerUnit)
            transform.DOMove(defender.transform.position - new Vector3(1.25f, 0.0f, 0.0f), 0.25f);
        else
            transform.DOMove(defender.transform.position + new Vector3(1.25f, 0.0f, 0.0f), 0.25f);
        transform.DOScale(normalSize * 1.15f, 0.15f);

        //  wait for attacker to reach defender
        yield return new WaitForSeconds(0.35f);
        int bluntLvlBefore = stats.getBluntLevel();
        int edgedLvlBefore = stats.getEdgedLevel();
        int lvlBefore = stats.u_level;
        bool miss = GameVariables.chanceOutOfHundred(stats.u_missChance);

        if(!miss) {
            //  actually deal damage to defender
            var dmg = stats.getDamageGiven(FindObjectOfType<PresetLibrary>()) + tempPowerMod;
            if(charging) {
                dmg *= 2.0f;
            }
            defender.GetComponent<UnitClass>().defend(gameObject, dmg);
        }
        else {
            FindObjectOfType<DamageTextCanvas>().showMissTextForUnit(gameObject);
        }
        charging = false;
        yield return new WaitForSeconds(0.4f);

        //  move back to original position
        transform.DOScale(normalSize, 0.15f);
        transform.DOLocalMove(normalPos, 0.15f);

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

        //  chance worn state decrease
        if(stats.weapon != null && !stats.weapon.isEmpty() && stats.weapon.wornAmount > GameInfo.wornState.old && GameVariables.chanceEquipmentWornDecrease() && isPlayerUnit) {
            FindObjectOfType<DamageTextCanvas>().showBreakTextForUnit(gameObject);
            stats.weapon.wornAmount--;
        }

        attackAnim = null;
        attackingTarget = null;
        FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
    }
}
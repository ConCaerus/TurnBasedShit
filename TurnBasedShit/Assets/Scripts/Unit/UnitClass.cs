using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class UnitClass : MonoBehaviour {
    [SerializeField] AudioClip hitSound, dieSound;
    public GameObject attackingTarget = null;
    public ParticleSystem bloodParticles;
    public Color hitColor;

    public bool isPlayerUnit = true;
    bool defending = false;
    public bool stunned = false;
    public bool isMouseOverUnit = false;

    public float tempPowerMod = 1.0f;
    public float tempDefenceMod = 0.0f;
    public float tempSpeedMod = 1.0f;

    public float spotOffset = 0.0f;

    [SerializeField] WeaponPreset weapon = null;
    [SerializeField] ArmorPreset armor = null;

    Vector2 normalSize, normalPos;
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
            GetComponent<CombatUnitUI>().moveLightHealthSliderToValue(stats.u_health + FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().stats.equippedWeapon.w_specialUsageAmount);
        }

        isMouseOverUnit = true;
        FindObjectOfType<UnitCombatHighlighter>().updateHighlights();
    }

    private void OnMouseOver() {
        if(FindObjectOfType<BattleOptionsCanvas>().battleState == 3) {
            GetComponent<CombatUnitUI>().showingWouldBeHealedValue = true;
            GetComponent<CombatUnitUI>().moveLightHealthSliderToValue(stats.u_health + FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().stats.equippedWeapon.w_specialUsageAmount);
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
        tempPowerMod = 1.0f;
        tempDefenceMod = 0.0f;
        tempSpeedMod = 1.0f;
        if(isPlayerUnit) {
            GetComponentInChildren<UnitSpriteHandler>().setEverything(stats.u_sprite, stats.equippedWeapon, stats.equippedArmor);
        }

        if(string.IsNullOrEmpty(stats.u_name)) {
            name = NameLibrary.getRandomUsablePlayerName();
        }
        else
            name = stats.u_name;


        normalSize = transform.localScale;

        if(isPlayerUnit)
            FindObjectOfType<PartyObject>().resaveInstantiatedUnit(stats);
    }



    public void takeBleedDamage() {
        if(stats.u_bleedCount > 0) {
            float temp = (stats.getModifiedMaxHealth() / 100.0f) * stats.u_bleedCount;
            stats.u_health -= temp;

            FindObjectOfType<DamageTextCanvas>().showTextForUnit(gameObject, temp, DamageTextCanvas.damageType.bleed);
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
        FindObjectOfType<DamageTextCanvas>().showTextForUnit(gameObject, h, DamageTextCanvas.damageType.healed);
    }

    public void prepareUnitForNextRound() {
        attackingTarget = null;
        defending = false;

        takeBleedDamage();
    }





    public void setRandomAttackingTarget() {
        List<GameObject> units = new List<GameObject>();
        foreach(var i in FindObjectsOfType<UnitClass>()) {
            if(i.isPlayerUnit != isPlayerUnit) {
                units.Add(i.gameObject);
            }
        }

        attackingTarget = units[Random.Range(0, units.Count)];
    }

    public void setEquippedWeapon(Weapon w = null) {
        if(w == null && weapon != null) {
            w = weapon.preset;
            weapon = null;
        }
        if(w == null) return;
        stats.equippedWeapon = w;

        FindObjectOfType<PartyObject>().resaveInstantiatedUnit(stats);
    }
    public void removeEquippedWeapon() {
        stats.equippedWeapon = null;
    }

    public void setEquippedArmor(Armor a = null) {
        if(a == null && armor != null) {
            a = armor.preset;
            armor = null;
        }
        if(a == null) return;
        stats.equippedArmor = a;

        FindObjectOfType<PartyObject>().resaveInstantiatedUnit(stats);
    }
    public void removeEquippedArmor() {
        stats.equippedArmor = null;
    }

    public void setEquipment(Weapon w = null, Armor a = null) {
        setEquippedWeapon(w);
        setEquippedArmor(a);
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
                Debug.Log("here");
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
        normalPos = transform.position;
        normalSize = transform.localScale;
        //  triggers
        FindObjectOfType<ItemUser>().triggerTime(Item.useTimes.beforeAttacking, this, true);

        //  triggers
        stats.equippedWeapon.applyAttributes(gameObject, defender);

        //  Flair
        attackAnim = StartCoroutine(attackingAnim(defender));
    }


    public void defend(GameObject attacker, float dmg) {
        //  triggers
        FindObjectOfType<ItemUser>().triggerTime(Item.useTimes.beforeDefending, this, true);


        //  if defender is an enemy, check if it's weak or strong to the attack
        if(!isPlayerUnit) {
            //  check if it's weak to the attack
            if(GetComponent<EnemyUnitInstance>().weakTo == attacker.GetComponent<UnitClass>().stats.equippedWeapon.w_attackType)
                dmg *= 1.25f;
        }
        dmg *= (stats.getDefenceMult(defending, FindObjectOfType<PresetLibrary>()) - tempDefenceMod);

        float crit = attacker.GetComponent<UnitClass>().stats.getCritMult(dmg);
        dmg *= crit;
        //  triggers
        stats.equippedArmor.applyAttributes(gameObject, attacker, FindObjectOfType<TurnOrderSorter>().playingUnit);

        //  take damage
        stats.u_health = Mathf.Clamp(stats.u_health - dmg, -1.0f, stats.getModifiedMaxHealth());

        //  Flair
        if(defending)
            FindObjectOfType<DamageTextCanvas>().showTextForUnit(gameObject, dmg, DamageTextCanvas.damageType.defended);
        else if(crit > 1.1f)
            FindObjectOfType<DamageTextCanvas>().showTextForUnit(gameObject, dmg, DamageTextCanvas.damageType.crit);
        else
            FindObjectOfType<DamageTextCanvas>().showTextForUnit(gameObject, dmg, DamageTextCanvas.damageType.weapon);
        var blood = Instantiate(bloodParticles);
        Destroy(blood.gameObject, blood.main.startLifetimeMultiplier);
        blood.gameObject.transform.position = transform.position;
        defendAnim = StartCoroutine(defendingAnim());


        //  check if any unit died in the attack
        attacker.GetComponent<UnitClass>().checkIfDead(DeathInfo.killCause.murdered, gameObject);
        if(checkIfDead(DeathInfo.killCause.murdered, attacker.gameObject))
            return;

        //  end battle turn
        if(FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject)
            return;
        foreach(var i in stats.equippedArmor.a_attributes) {
            if(i == Armor.attribute.Reflex)
                attack(attacker.gameObject);
            return;
        }
    }

    public void die(DeathInfo.killCause cause, GameObject killer = null) {
        //  things to do with removing the unit from the party and what to do with equippment
        if(isPlayerUnit)
            stats.die(cause, killer);
        else {
            //  triggers items
            FindObjectOfType<ItemUser>().triggerTime(Item.useTimes.afterKill, this, false);

            //  add exp to weapon
            if(killer != null && killer.GetComponent<PlayerUnitInstance>() != null)
                killer.GetComponent<PlayerUnitInstance>().addWeaponTypeExpOnKill(GetComponent<EnemyUnitInstance>().enemyDiff);

            //  get enemy drops
            int chanceMod = 0;
            if(killer != null) {
                foreach(var i in killer.GetComponent<UnitClass>().stats.u_traits)
                    chanceMod += i.getEnemyDropChanceMod();
            }
            GetComponent<EnemyUnitInstance>().chanceWeaponDrop(chanceMod);
            GetComponent<EnemyUnitInstance>().chanceArmorDrop(chanceMod);
            GetComponent<EnemyUnitInstance>().chanceItemDrop(chanceMod);
            GetComponent<EnemyUnitInstance>().chanceConsumableDrop(chanceMod);

            //  increases acc quest counter
            for(int i = 0; i < ActiveQuests.getKillQuestCount(); i++) {
                if(ActiveQuests.getKillQuest(i).enemyType == GetComponent<EnemyUnitInstance>().enemyType)
                    ActiveQuests.getKillQuest(i).howManyToKill++;
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
        //FindObjectOfType<DamageTextCanvas>().removeTextsWithUnit(gameObject);
        Destroy(gameObject);
    }

    public abstract void setDefendingAnim();
    public abstract void setAttackingAnim();

    IEnumerator defendingAnim() {
        if(attackAnim != null)
            StopCoroutine(attackAnim);
        else
            normalSize = transform.localScale;

        GetComponent<SpriteRenderer>().DOColor(hitColor, 0.05f);
        transform.DOScale(normalSize * 1.5f, 0.15f);

        setDefendingAnim();

        yield return new WaitForSeconds(0.1f);

        GetComponent<SpriteRenderer>().DOColor(stats.u_sprite.color, 0.25f);

        yield return new WaitForSeconds(0.25f);

        transform.DOScale(normalSize, 0.15f);
    }
    IEnumerator attackingAnim(GameObject defender) {
        //  move to target
        if(isPlayerUnit)
            transform.DOMove(defender.transform.position - new Vector3(1.25f, 0.0f, 0.0f), 0.25f);
        else
            transform.DOMove(defender.transform.position + new Vector3(1.25f, 0.0f, 0.0f), 0.25f);
        transform.DOScale(normalSize * 1.15f, 0.15f);

        //  wait for attacker to reach defender
        yield return new WaitForSeconds(0.35f);

        //  play animation and hold position
        setAttackingAnim();
        FindObjectOfType<AudioManager>().playSound(hitSound);

        //  actually deal damage to defender
        var dmg = stats.getDamageGiven(FindObjectOfType<PresetLibrary>()) * tempPowerMod;
        defender.GetComponent<UnitClass>().defend(gameObject, dmg);
        yield return new WaitForSeconds(0.4f);

        //  move back to original position
        transform.DOScale(normalSize, 0.15f);
        transform.DOMove(normalPos, 0.15f);

        //  non value based traits in attack
        foreach(var i in stats.u_traits) {
            if(!stunned && i.shouldStunSelf()) {
                stunned = true;
            }
            if(defender != null && !defender.GetComponent<UnitClass>().isStunned() && i.shouldStunTarget())
                defender.GetComponent<UnitClass>().setStunned(true);
        }

        attackAnim = null;
        FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
    }
}
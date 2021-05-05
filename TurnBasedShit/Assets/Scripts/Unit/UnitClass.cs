using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class UnitClass : MonoBehaviour {
    public GameObject attackingTarget = null;
    public ParticleSystem bloodParticles;
    public Color hitColor;

    public bool isPlayerUnit = true;
    public bool defending = false;
    public bool stunned = false;
    public bool isMouseOverUnit = false;

    [SerializeField] WeaponPreset weapon = null;
    [SerializeField] ArmorPreset armor = null;


    public UnitStats stats;



    private void OnMouseEnter() {
        isMouseOverUnit = true;
    }

    private void OnMouseExit() {
        isMouseOverUnit = false;
    }

    public void setup() {
        if(stats.u_name == "") {
            setNewRandomName();
        }
        else
            name = stats.u_name;

        Party.overrideUnit(stats);
    }



    public void takeBleedDamage() {
        if(stats.u_bleedCount > 0) {
            float temp = (stats.getModifiedMaxHealth() / 100.0f) * stats.u_bleedCount;
            stats.u_health -= temp;

            checkIfDead();

            FindObjectOfType<DamageTextCanvas>().showTextForUnit(gameObject, temp, DamageTextCanvas.damageType.poison);
        }
    }
    public void cureBleed() {
        stats.u_bleedCount = 0;
    }

    public bool checkIfDead() {
        if(stats.u_health <= 0.0f) {
            die();
            return true;
        }
        return false;
    }

    public void addHealth(float h) {
        if(checkIfDead())
            return;

        stats.u_health = Mathf.Clamp(stats.u_health + h, -1.0f, stats.getModifiedMaxHealth());

        checkIfDead();
    }


    public void addSpeed(float s) {
        stats.u_speed += s;
    }

    public void prepareUnitForNextRound() {
        attackingTarget = null;
        defending = false;

        takeBleedDamage();
    }


    public void attack(GameObject defender) {
        //  triggers
        FindObjectOfType<ItemUser>().triggerTime(Item.useTimes.beforeAttacking, this, true);

        //  Attack unit
        defender.GetComponent<UnitClass>().defend(gameObject, stats.getDamageGiven());

        //  triggers
        stats.equippedWeapon.applyAttributesAfterAttack(gameObject, defender);

        //  Flair
        gameObject.transform.DOPunchPosition(defender.transform.position - transform.position, 0.25f);
        FindObjectOfType<AudioManager>().playHitSound();
    }
    public void defend(GameObject attacker, float dmg) {
        //  triggers
        FindObjectOfType<ItemUser>().triggerTime(Item.useTimes.beforeDefending, this, true);


        //  if defender is an enemy, check if it's weak or strong to the attack
        if(!isPlayerUnit) {
            //  check if it's weak to the attack
            if(GetComponent<EnemyUnitInstance>().weakTo == attacker.GetComponent<UnitClass>().stats.equippedWeapon.w_element)
                dmg *= 1.25f;
            else if(GetComponent<EnemyUnitInstance>().strongTo == attacker.GetComponent<UnitClass>().stats.equippedWeapon.w_element)
                dmg *= 0.75f;
        }

        dmg = stats.getModifiedDamageTaken(dmg, defending);

        //  take damage
        stats.u_health = Mathf.Clamp(stats.u_health - dmg, -1.0f, stats.getModifiedMaxHealth());

        //  Flair
        if(defending)
            FindObjectOfType<DamageTextCanvas>().showTextForUnit(gameObject, dmg, DamageTextCanvas.damageType.defended);
        else
            FindObjectOfType<DamageTextCanvas>().showTextForUnit(gameObject, dmg, DamageTextCanvas.damageType.weapon);
        var blood = Instantiate(bloodParticles);
        Destroy(blood.gameObject, blood.main.startLifetimeMultiplier);
        blood.gameObject.transform.position = transform.position;
        StartCoroutine(hitEffect());

        //  triggers
        int armorReaction = stats.equippedArmor.applyAttributesAfterAttack(gameObject, attacker);
        //  reflex
        if(armorReaction == 1) {
            return;
        }

        //  check if any unit died in the attack
        attacker.GetComponent<UnitClass>().checkIfDead();
        checkIfDead();

        //  end battle turn
        FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
    }

    public void die() {
        //  things to do with removing the unit from the party and what to do with equippment
        if(isPlayerUnit)
            stats.die();

        //  flair
        FindObjectOfType<AudioManager>().playDieSound();

        //  removes unit from game world
        FindObjectOfType<TurnOrderSorter>().removeUnitFromList(gameObject);
        FindObjectOfType<HealthBarCanvas>().destroyHealthBarForUnit(gameObject);
        FindObjectOfType<DamageTextCanvas>().removeTextsWithUnit(gameObject);
        Destroy(gameObject);
    }



    public void setRandomAttackingTarget() {
        int unitCount = 0;
        foreach(var i in FindObjectsOfType<UnitClass>()) {
            if(i.isPlayerUnit != isPlayerUnit) {
                unitCount++;
            }
        }

        int rand = Random.Range(0, unitCount);
        unitCount = 0;

        foreach(var i in FindObjectsOfType<UnitClass>()) {
            if(i.isPlayerUnit != isPlayerUnit) {
                if(unitCount == rand)
                    attackingTarget = i.gameObject;

                unitCount++;
            }
        }
    }

    public void setEquippedWeapon(Weapon w = null) {
        if(w == null && weapon != null) {
            w = weapon.preset;
            weapon = null;
        }
        if(w == null) return;
        stats.equippedWeapon = w;

        Party.overrideUnit(stats);
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

        Party.overrideUnit(stats);
    }
    public void removeEquippedArmor() {
        stats.equippedArmor = null;
    }

    public void setEquipment(Weapon w = null, Armor a = null) {
        setEquippedWeapon(w);
        setEquippedArmor(a);
    }
    public void removeEquipment() {
        stats.equippedWeapon = null;
        stats.equippedArmor = null;
    }

    public void resetSavedEquippment() {
        if(weapon == null)
            stats.equippedWeapon.resetWeaponStats();
        else
            stats.equippedWeapon.setToPreset(weapon);

        if(armor == null)
            stats.equippedArmor.resetArmorStats();
        else
            stats.equippedArmor.setToPreset(armor);

        Party.overrideUnit(stats);
    }

    public void populateEmptyEquippment() {
        if(stats.equippedWeapon.isEmpty() && weapon != null)
            stats.equippedWeapon.setToPreset(weapon);
        else if(!stats.equippedWeapon.isEmpty())
            weapon = stats.equippedWeapon.weaponToPreset();


        if(stats.equippedArmor.isEmpty() && armor != null)
            stats.equippedArmor.setToPreset(armor);
        else if(!stats.equippedArmor.isEmpty())
            armor = stats.equippedArmor.armorToPreset();

        Party.overrideUnit(stats);
    }

    public void setNewRandomName() {
        stats.u_name = NameLibrary.getRandomPlayerName();
        name = stats.u_name;
    }

    public void resetSpriteAndColor() {
        stats.u_sprite.setSprite(GetComponent<SpriteRenderer>().sprite);
        if(stats.u_color == new Color())
            stats.u_color = GetComponent<SpriteRenderer>().color;
    }

    IEnumerator hitEffect() {
        GetComponent<SpriteRenderer>().DOColor(hitColor, 0.05f);

        yield return new WaitForSeconds(0.1f);

        GetComponent<SpriteRenderer>().DOColor(stats.u_color, 0.25f);
    }
}
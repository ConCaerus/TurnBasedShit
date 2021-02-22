using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class UnitClass : MonoBehaviour {
    public GameObject attackingTarget = null;

    public bool isPlayerUnit = true;
    public bool attacking = false, defending = false;
    public bool stunned = false;
    public bool isMouseOverUnit = false;

    [SerializeField] WeaponPreset weapon = null;
    [SerializeField] ArmorPreset armor = null;


    public UnitClassStats stats;



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

        Party.resaveUnit(stats);
    }


    public Vector2 getMousePos() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    public void takeDamage(GameObject attacker, float dmg) {
        float temp = haveArmorReduceDamage(dmg);
        stats.u_health -= temp;

        stats.equippedArmor.applyAttributesAfterAttack(gameObject, attacker);

        if(defending)
            FindObjectOfType<DamageTextCanvas>().showTextForUnit(gameObject, temp, DamageTextCanvas.damageType.defended);
        else
            FindObjectOfType<DamageTextCanvas>().showTextForUnit(gameObject, temp, DamageTextCanvas.damageType.weapon);
    }

    public void takePoisonDamage() {
        if(stats.u_poisonCount > 0) {
            float temp = (stats.u_maxHealth / 100.0f) * stats.u_poisonCount;
            stats.u_health -= temp;

            FindObjectOfType<DamageTextCanvas>().showTextForUnit(gameObject, temp, DamageTextCanvas.damageType.poison);
        }
    }
    public void curePoison() {
        stats.u_poisonCount = 0;
    }

    public void addHealth(float h) {
        if(stats.u_health <= 0.0f)
            die();
        stats.u_health += h;
        if(stats.u_health > stats.u_maxHealth)
            stats.u_health = stats.u_maxHealth;
    }


    public void die() {
        foreach(var i in FindObjectsOfType<UnitHighlighting>()) {
            i.dehighlightUnit(gameObject);
        }

        FindObjectOfType<TurnOrderSorter>().removeUnitFromList(gameObject);
        FindObjectOfType<HealthBarCanvas>().destroyHealthBarForUnit(gameObject);
        Party.removeUnit(stats);
        Destroy(gameObject);
    }


    public void addSpeed(float s) {
        stats.u_speed += s;
    }



    public void prepareUnitForNextRound() {
        attacking = false;
        attackingTarget = null;
        defending = false;

        takePoisonDamage();
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

    public void attackTargetUnit() {
        if(attackingTarget != null) {
            attackUnit(attackingTarget);
        }
    }

    public void attackUnit(GameObject unit) {
        if(stats.equippedWeapon == null)
            Debug.LogError("Unit " + name + "   is not equipped with a weapon");

        stats.equippedWeapon.applyAttributesAfterAttack(gameObject, unit);

        gameObject.transform.DOPunchPosition(unit.transform.position - transform.position, 0.25f);
        unit.GetComponent<UnitClass>().takeDamage(gameObject, stats.equippedWeapon.w_power + stats.equippedWeapon.getPowerBonusDamage());


        attacking = false;
    }


    public float haveArmorReduceDamage(float dmg) {
        float dmgBlocked = 0.0f;
        if(!defending)
            dmgBlocked = dmg * (stats.equippedArmor.a_defence / 100.0f);
        else if(defending)
            dmgBlocked = dmg * 1.5f * (stats.equippedArmor.a_defence / 100.0f);

        dmgBlocked += stats.equippedArmor.getTurtleBonusDefence();

        if(dmgBlocked >= dmg)
            return 0.0f;
        return dmg - dmgBlocked;
    }

    public void setEquippedWeapon(Weapon w = null) {
        if(w == null && weapon != null) {
            w = weapon.preset;
            weapon = null;
        }
        if(w == null) return;
        stats.equippedWeapon = w;

        Party.resaveUnit(stats);
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

        Party.resaveUnit(stats);
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

        Party.resaveUnit(stats);
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

        Party.resaveUnit(stats);
    }

    public void setNewRandomName() {
        stats.u_name = NameLibrary.getRandomName();
        name = stats.u_name;
    }

    public void resetSpriteAndColor() {
        stats.u_sprite.setLocation(GetComponent<SpriteRenderer>().sprite);
        stats.u_color = GetComponent<SpriteRenderer>().color;
    }
}
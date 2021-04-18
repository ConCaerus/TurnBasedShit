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

        Party.resaveUnit(stats);
    }


    public void takeDamage(GameObject attacker, float dmg) {
        //  Trigger Items
        FindObjectOfType<ItemUser>().triggerTime(Item.useTimes.beforeAttacked, this, true);

        //  Take damage
        float temp = stats.getModifiedDamageTaken(dmg);
        stats.u_health -= temp;


        //  Trigger armor events 
        stats.equippedArmor.applyAttributesAfterAttack(gameObject, attacker);


        //  Show damage text
        if(defending)
            FindObjectOfType<DamageTextCanvas>().showTextForUnit(gameObject, temp, DamageTextCanvas.damageType.defended);
        else
            FindObjectOfType<DamageTextCanvas>().showTextForUnit(gameObject, temp, DamageTextCanvas.damageType.weapon);
    }

    public void takePoisonDamage() {
        if(stats.u_poisonCount > 0) {
            float temp = (stats.getModifiedMaxHealth() / 100.0f) * stats.u_poisonCount;
            stats.u_health -= temp;

            FindObjectOfType<DamageTextCanvas>().showTextForUnit(gameObject, temp, DamageTextCanvas.damageType.poison);
        }
    }
    public void curePoison() {
        stats.u_poisonCount = 0;
    }

    public void addHealth(float h) {
        if(stats.u_health <= 0.0f) {
            die();
            return;
        }
        stats.u_health = Mathf.Clamp(stats.u_health + h, -1.0f, stats.getModifiedMaxHealth());
        if(stats.u_health <= 0.0f) {
            die();
            return;
        }
    }


    public void die() {
        foreach(var i in FindObjectsOfType<UnitHighlighting>()) {
            i.dehighlightUnit(gameObject);
        }

        if(stats.equippedItem != null && !stats.equippedItem.isEmpty()) {
            Inventory.removeItem(stats.equippedItem);
            stats.equippedItem = null;
        }
        FindObjectOfType<ItemUser>().resetInplayItems();

        if(stats.equippedWeapon != null && !stats.equippedWeapon.isEmpty()) {
            Inventory.removeWeapon(stats.equippedWeapon);
            stats.equippedWeapon = null;
        }

        if(stats.equippedArmor != null && !stats.equippedArmor.isEmpty()) {
            Inventory.removeArmor(stats.equippedArmor);
            stats.equippedArmor = null;
        }

        FindObjectOfType<AudioManager>().playDieSound();
        FindObjectOfType<TurnOrderSorter>().removeUnitFromList(gameObject);
        FindObjectOfType<HealthBarCanvas>().destroyHealthBarForUnit(gameObject);
        FindObjectOfType<DamageTextCanvas>().removeTextsWithUnit(gameObject);
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
        //  Trigger weapon attributes
        if(stats.equippedWeapon != null && !stats.equippedWeapon.isEmpty()) {
            stats.equippedWeapon.applyAttributesAfterAttack(gameObject, unit);
        }

        FindObjectOfType<AudioManager>().playHitSound();

        //  Attack animation
        gameObject.transform.DOPunchPosition(unit.transform.position - transform.position, 0.25f);
        unit.GetComponent<UnitClass>().takeDamage(gameObject, stats.getDamageGiven());

        attacking = false;
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
        stats.u_sprite.setSprite(GetComponent<SpriteRenderer>().sprite);
        if(stats.u_color == new Color())
            stats.u_color = GetComponent<SpriteRenderer>().color;
    }
}
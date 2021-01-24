using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class UnitClass : MonoBehaviour {
    public GameObject attackingTarget = null;

    public bool isPlayerUnit = true;
    public bool attacking = false, defending = false;
    public bool isMouseOverUnit = false;


    public UnitClassStats stats;



    private void OnMouseEnter() {
        isMouseOverUnit = true;
    }

    private void OnMouseExit() {
        isMouseOverUnit = false;
    }

    private void Start() {
        //loadEquipment();
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
        if(stats.u_health <= 0.0f)
            die();
    }

    public void takePoisonDamage() {
        if(stats.u_poisonCount > 0) {
            float temp = (stats.u_maxHealth / 100.0f) * stats.u_poisonCount;
            stats.u_health -= temp;

            FindObjectOfType<DamageTextCanvas>().showTextForUnit(gameObject, temp, DamageTextCanvas.damageType.poison);
            if(stats.u_health <= 0.0f)
                die();
        }
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
        Destroy(gameObject);
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
        else
            Debug.LogError("Attacking unit has no target");
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

    public void addEquippedWeapon(Weapon w) {
        stats.equippedWeapon = w;
        //saveWeapon();
    }
    public void removeEquippedWeapon() {
        stats.equippedWeapon = null;
        PlayerPrefs.DeleteKey("Unit" + stats.u_order.ToString() + " Weapon");
    }
    /*
    public void saveWeapon() {
        var data = JsonUtility.ToJson(stats.equippedWeapon);
        PlayerPrefs.SetString("Unit" + stats.u_order.ToString() + " Weapon", data);
    }
    public void loadWeapon() {
        if(PlayerPrefs.GetString("Unit" + stats.u_order.ToString() + " Weapon") != null) {
            var data = PlayerPrefs.GetString("Unit" + stats.u_order.ToString() + " Weapon");
            stats.equippedWeapon = JsonUtility.FromJson<Weapon>(data);
        }
        else
            stats.equippedWeapon = new Weapon();
    }
    */

    public void addEquippedArmor(Armor a) {
        stats.equippedArmor = a;
        //saveArmor();
    }
    public void removeEquippedArmor() {
        stats.equippedArmor = null;
        PlayerPrefs.DeleteKey("Unit" + stats.u_order.ToString() + " Armor");
    }
    /*
    public void saveArmor() {
        var data = JsonUtility.ToJson(stats.equippedArmor);
        PlayerPrefs.SetString("Unit" + stats.u_order.ToString() + " Armor", data);
    }
    public void loadArmor() {
        if(PlayerPrefs.GetString("Unit" + stats.u_order.ToString() + " Armor") != null) {
            var data = PlayerPrefs.GetString("Unit" + stats.u_order.ToString() + " Armor");
            stats.equippedArmor = JsonUtility.FromJson<Armor>(data);
        }
        else
            stats.equippedArmor = new Armor();
    }

    public void loadEquipment() {
        loadWeapon();
        loadArmor();

        stats.equippedArmor.weilder = gameObject;
        stats.equippedWeapon.weilder = gameObject;
    }
    */

    public void setNewOrderNumber(int n) {
        PlayerPrefs.DeleteKey("Unit" + stats.u_order.ToString() + " Armor");
        PlayerPrefs.DeleteKey("Unit" + stats.u_order.ToString() + " Weapon");
        stats.u_order = n;
        //saveArmor();
        //saveWeapon();
    }
}
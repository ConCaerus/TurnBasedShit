using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitStats {
    public string u_name = "";

    public float u_expCap = 25.0f;
    public float u_exp = 0.0f;
    public int u_level = 0;

    public Color u_color = Color.white;

    [SerializeField] float u_baseMaxHealth;
    public float u_health = 100.0f;

    public float u_speed = 0.0f;
    public float u_power = 0.0f;
    public float u_defence = 0.0f;

    public float u_critChance = 0.0f;
    public int u_bleedCount = 0;

    public int u_order = 0;

    public Weapon equippedWeapon;
    public Armor equippedArmor;
    public Item equippedItem;

    public SlaveStats u_slaveStats = new SlaveStats();

    public List<UnitTrait> u_traits = new List<UnitTrait>();

    public void setNewOrder(int i) {
        u_order = i;
    }

    public bool isEmpty() {
        return u_name == "" && u_order == 0;
    }

    public UnitStats() { }
    public UnitStats(UnitStats other) {
        if(other == null || other.isEmpty())
            return;
        u_name = other.u_name;
        u_health = other.u_health;
        u_baseMaxHealth = other.u_baseMaxHealth;
        u_speed = other.u_speed;
        u_power = other.u_power;
        u_bleedCount = other.u_bleedCount;

        u_order = other.u_order;
        u_color = other.u_color;

        equippedWeapon = other.equippedWeapon;
        equippedArmor = other.equippedArmor;
        equippedItem = other.equippedItem;

        u_slaveStats = other.u_slaveStats;
    }

    public bool equals(UnitStats other) {
        bool names = u_name == other.u_name;
        bool health = u_health == other.u_health;
        bool maxHealth = u_baseMaxHealth == other.u_baseMaxHealth;
        bool speed = u_speed == other.u_speed;
        bool power = u_power == other.u_power;
        bool order = u_order == other.u_order;
        bool weapon = equippedWeapon.isEqualTo(other.equippedWeapon);
        bool armor = equippedArmor.isEqualTo(other.equippedArmor);
        bool item = equippedItem.isEqualTo(other.equippedItem);
        bool color = u_color == other.u_color;

        return names && health && maxHealth && speed && power && order && weapon && armor && item && color;
    }

    public bool levelUpIfPossible() {
        if(u_exp >= u_expCap) {
            levelUp();
            return true;
        }
        return false;
    }
    public void levelUp() {
        u_expCap += u_expCap / 10.0f;
        u_exp = 0.0f;

        u_level++;

        float healthInc = Random.Range(5.0f, 10.0f);
        u_baseMaxHealth += healthInc;
        u_health += healthInc;

        u_speed += Random.Range(0.0f, 2.0f);
        u_power += Random.Range(0.0f, 2.0f);
        u_defence += Random.Range(0.0f, 2.0f);

        u_critChance = Mathf.Clamp(u_critChance + Random.Range(0.0f, 0.2f), 0.0f, 100.0f);
    }

    //  modifying stats
    public void addPower(float p) {
        u_power += p;
    }
    public void addDefence(float d) {
        u_defence += d;
    }
    public void addSpeed(float s) {
        u_speed += s;
    }

    //  Attack amount
    public float getPowerMod(float tempPower = 0.0f) {
        return u_power + equippedWeapon.w_power + tempPower;
    }
    public float getAverageDamageGiven(float tempPower = 0.0f) {
        float dmg = getPowerMod(tempPower);

        //  Weapon
        dmg += equippedWeapon.getBonusAttributeDamage();

        //  Traits modify damage
        foreach(var i in u_traits) {
            dmg += i.getDamageGivenMod() * getPowerMod();
        }

        //  Items modify damage
        if(equippedItem != null && !equippedItem.isEmpty()) {
            dmg += equippedItem.getDamageGivenMod() * getPowerMod();
        }


        return dmg;
    }
    public float getDamageGiven(float tempPower = 0.0f) {
        return randomizeDamage(getAverageDamageGiven(tempPower));
    }
    float randomizeDamage(float dmg = 0.0f) {
        //  normal mod
        dmg /= 2.0f;
        dmg *= Random.Range(1.75f, 2.25f);

        //  crit mod
        if(Random.Range(0, 101) <= (u_critChance * 100.0f))
            dmg *= Random.Range(2.0f, 3.0f);

        return dmg;
    }

    //  Defence amount
    public float getDefenceMod(float tempDefence = 0.0f) {
        return u_defence + equippedArmor.a_defence + tempDefence;
    }
    public float getDefenceMult(bool defending = false, float tempDefence = 0.0f) {
        //  starts with 100%
        float temp = 1.0f;
        temp -= getDefenceMod(tempDefence) / 100.0f;

        //  Trigger Traits
        foreach(var i in u_traits) {
            temp -= i.getDamageGivenMod();
        }

        //  Have Armor reduce damage
        if(equippedArmor != null && !equippedArmor.isEmpty()) {
            temp -= equippedArmor.getDefenceMult();
            temp -= equippedArmor.getBonusAttributeDefenceMult();
        }


        //  defending mult
        if(defending)
            temp -= 0.2f;

        //  Clamps temp to useful values
        temp = Mathf.Clamp01(temp);

        return temp;
    }
    public float getModifiedDamageTaken(float damage, bool defending = false, float tempDefence = 0.0f) {
        return damage * getDefenceMult(defending, tempDefence);
    }

    public float getModifiedMaxHealth() {
        float temp = u_baseMaxHealth;
        if(equippedItem != null && !equippedItem.isEmpty())
            temp += equippedItem.getMaxHealthMod();

        foreach(var i in u_traits)
            temp += i.getMaxHealthMod();

        return temp;
    }
    public float getModifiedSpeed() {
        float temp = u_speed;
        temp += equippedWeapon.w_speedMod;
        temp += equippedArmor.a_speedMod;
        temp += equippedItem.getSpeedMod();
        foreach(var i in u_traits)
            temp += i.getSpeedMod();

        return temp;
    }

    public void setBaseMaxHealth(float f) {
        u_baseMaxHealth = f;
    }
    public float getBaseMaxHealth() {
        return u_baseMaxHealth;
    }

    public int determineCost() {
        //  smallest cost for human life
        int cost = 5;

        cost += (int)u_power;

        int avgMaxHealth = 50;
        int healthSteps = 10;
        int steps = (int)((u_baseMaxHealth - avgMaxHealth) / healthSteps);
        cost += steps;

        return cost;
    }

    public bool canLevelUp() {
        return u_exp >= u_expCap;
    }

    public void die() {
        //  add equipped things back into the inventory
        if(equippedWeapon != null && !equippedWeapon.isEmpty())
            Inventory.addWeapon(equippedWeapon);
        if(equippedArmor != null && !equippedArmor.isEmpty())
            Inventory.addArmor(equippedArmor);
        if(equippedItem != null && !equippedItem.isEmpty())
            Inventory.addItem(equippedItem);

        equippedWeapon = null;
        equippedArmor = null;
        equippedItem = null;

        //  remove from party
        Party.removeUnit(u_order);
    }
}

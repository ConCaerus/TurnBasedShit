using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitStats {
    public string u_name = "";

    public float u_expCap = 25.0f;
    public float u_exp = 0.0f;
    public int u_level = 0;
    const int maxLevel = 5;
    public float u_skillExpCap = 100.0f;
    public float u_bluntExp;
    public float u_edgedExp;
    public float u_summonedExp;

    public UnitSpriteInfo u_sprite = new UnitSpriteInfo();

    [SerializeField] float u_baseMaxHealth;
    public float u_health = 100.0f;

    public float u_speed = 0.0f;
    public float u_power = 0.0f;
    public float u_defence = 0.0f;

    public float u_critChance = 0.0f;

    public int u_bleedCount = 0;

    public int u_instanceID = -1;

    public Weapon equippedWeapon;
    public Armor equippedArmor;
    public Item equippedItem;

    public List<UnitTrait> u_traits = new List<UnitTrait>();

    public DeathInfo u_deathInfo = null;

    public bool isEmpty() {
        return u_name == "" && u_instanceID == -1;
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

        u_sprite.setEqualTo(other.u_sprite);

        equippedWeapon.setEqualTo(other.equippedWeapon, true);
        equippedArmor.setEqualTo(other.equippedArmor, true);
        equippedItem.setEqualTo(other.equippedItem, true);

        foreach(var i in other.u_traits)
            u_traits.Add(i);
    }

    public bool isTheSameInstanceAs(UnitStats other) {
        if(other == null || other.isEmpty())
            return false;

        return u_instanceID == other.u_instanceID;
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
    public void addHealth(float h) {
        u_health += h;
        if(u_health > getModifiedMaxHealth())
            u_health = getModifiedMaxHealth();
    }
    public bool addExp(float ex) {
        u_exp += ex;
        if(canLevelUp()) {
            levelUp();
            return true;
        }
        return false;
    }

    //  Attack amount
    public float getBasePower() {
        float weaponPower = equippedWeapon.power * (((float)equippedWeapon.wornAmount + 7.0f) / 10.0f);
        return u_power + weaponPower;
    }
    float getLevelDamageMult() {
        float baseMult = 1.25f;
        if(equippedWeapon == null || equippedWeapon.isEmpty())
            return 1.0f;
        if(equippedWeapon.aType == Weapon.attackType.blunt && getBluntLevel() > 0)
            return baseMult * (getBluntLevel() / 2.0f);
        if(equippedWeapon.aType == Weapon.attackType.edged && getEdgedLevel() > 0)
            return baseMult * (getEdgedLevel() / 2.0f);
        return 1.0f;
    }
    public float getDamageGiven(PresetLibrary lib) {
        float dmg = getBasePower();
        float baseDmg = getBasePower();

        //  Weapon
        if(equippedWeapon != null && !equippedWeapon.isEmpty()) {   //  adds 10% of dmg for every power tolken in the weapon
            dmg += (0.1f * dmg) * equippedWeapon.getPowerAttCount();
        }

        //  Armor
        if(equippedArmor != null && !equippedArmor.isEmpty()) { //  adds 10% of dmg for every power tolken in the armor
            dmg += (0.1f * dmg) * equippedArmor.getPowerAttCount();
        }

        //  Traits modify damage
        foreach(var i in u_traits) {    //  adds whatever the trait's power mod is times the base damage
            dmg += i.getDamageGivenMod() * baseDmg;
        }

        //  levels modify damage
        dmg *= getLevelDamageMult();

        //  Items modify damage
        if(equippedItem != null && !equippedItem.isEmpty()) {   //  adds whatever the item's power mod is times the base damage
            dmg += equippedItem.getPassiveMod(Item.passiveEffectTypes.modPower) * baseDmg;

            if(equippedWeapon.aType == Weapon.attackType.edged)
                dmg += equippedItem.getPassiveMod(Item.passiveEffectTypes.modEdgedDamageGiven) * baseDmg;
            else if(equippedWeapon.aType == Weapon.attackType.blunt)
                dmg += equippedItem.getPassiveMod(Item.passiveEffectTypes.modBluntDamageGiven) * baseDmg;
        }

        //  equipment pair modify damage
        var pair = lib.getRelevantPair(this);
        if(pair != null) {   //  multiplies by the power mod for the equipment pair
            dmg *= pair.powerMod;
        }


        return dmg;
    }
    public float getCritMult() {
        //  crit mod
        if(Random.Range(0, 101) <= (u_critChance * 100.0f))
            return Random.Range(1.75f, 2.25f);

        return 1.0f;
    }

    //  Defence amount
    public float getBaseDefence() {
        float armorDefence = equippedArmor.defence * (((float)equippedArmor.wornAmount + 7.0f) / 10.0f);
        return Mathf.Clamp(u_defence + armorDefence, 0.0f, 100.0f);
    }
    public float getDefenceMult(bool defending, PresetLibrary lib) {    //  this value is multiplied by the damage taken
        //  starts with 100% of damage taken
        float temp = 1.0f;
        temp -= getBaseDefence() / 100.0f;

        //  Trigger Traits
        foreach(var i in u_traits) {
            temp -= i.getDamageTakenMod();
        }

        //  have item reduce damage
        if(equippedItem != null && !equippedItem.isEmpty())
            temp -= equippedItem.getPassiveMod(Item.passiveEffectTypes.modDefence);

        //  Have Armor reduce damage
        if(equippedArmor != null && !equippedArmor.isEmpty()) { //  takes off 10% of damage for every tolken of turtle
            temp -= equippedArmor.getTurtleAttCount() * 0.1f;
        }

        //  have pair reduce damage
        var pair = lib.getRelevantPair(this);
        if(pair != null) {
            temp -= (1 - pair.defenceMod);
        }

        //  defending mult
        if(defending)   //  takes off 20% of damage if defending
            temp -= 0.2f;

        //  Clamps temp to useful values
        temp = Mathf.Clamp01(temp);
        return temp;
    }

    public float getModifiedMaxHealth() {
        float temp = u_baseMaxHealth;

        foreach(var i in u_traits)
            temp += i.getMaxHealthMod();

        return temp;
    }
    public float getModifiedSpeed(float tempSpeedMod) {
        float temp = u_speed * tempSpeedMod;
        if(equippedWeapon != null && !equippedWeapon.isEmpty())
            temp += equippedWeapon.speedMod;
        if(equippedArmor != null && !equippedArmor.isEmpty())
            temp += equippedArmor.speedMod;
        if(equippedItem != null && !equippedItem.isEmpty())
            temp += equippedItem.getPassiveMod(Item.passiveEffectTypes.modSpeed);
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

    public bool hasTrait(UnitTrait t) {
        foreach(var i in u_traits) {
            if(i == t)
                return true;
        }
        return false;
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
    public int getEdgedLevel() {
        if(Mathf.FloorToInt(u_edgedExp / u_skillExpCap) + 1 > maxLevel)
            return maxLevel;
        return Mathf.FloorToInt(u_edgedExp / u_skillExpCap) + 1;
    }
    public int getBluntLevel() {
        if(Mathf.FloorToInt(u_bluntExp / u_skillExpCap) + 1 > maxLevel)
            return maxLevel;
        return Mathf.FloorToInt(u_bluntExp / u_skillExpCap) + 1;
    }
    public int getSummonedLevel() {
        if(Mathf.FloorToInt(u_summonedExp / u_skillExpCap) + 1 > maxLevel)
            return maxLevel;
        return Mathf.FloorToInt(u_summonedExp / u_skillExpCap) + 1;
    }

    public void die(DeathInfo.killCause cause, GameObject killer = null) {
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
        Party.removeUnit(this);

        u_deathInfo = new DeathInfo(cause, killer);
        //  adds to graveyard
        Graveyard.addUnit(this);
    }
}

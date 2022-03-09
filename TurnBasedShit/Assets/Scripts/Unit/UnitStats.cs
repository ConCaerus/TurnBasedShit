using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitStats {
    public string u_name = "";
    public GameInfo.combatUnitType u_type;

    public float u_expCap { get; private set; } = 25.0f;
    public float u_exp { get; private set; } = 0.0f;
    public int u_level { get; private set; } = 1;
    const int maxLevel = 6;
    public float u_skillExpCap { get; private set; } = 100.0f;
    public float u_bluntExp { get; private set; }
    public float u_edgedExp { get; private set; }
    public float u_summonedExp { get; private set; }

    public UnitSpriteInfo u_sprite = new UnitSpriteInfo();

    [SerializeField] float u_baseMaxHealth;
    public float u_health = 100.0f;

    public float u_speed = 0.0f;
    public float u_power = 0.0f;
    public float u_defence = 0.0f;

    public float u_critChance = 0.0f;
    public int u_missChance = 1; // this number out of one hundred

    public int u_bleedCount = 0;

    public bool u_isLeader = false;
    public int u_instanceID = -1;

    public Weapon weapon;
    public Armor armor;
    public Item item;

    public int maxTraitCount { get; private set; } = 8;
    public List<UnitTrait> u_traits = new List<UnitTrait>();
    public UnitTalent u_talent = null;

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
        u_critChance = other.u_critChance;
        u_missChance = other.u_missChance;

        u_bleedCount = other.u_bleedCount;

        u_sprite.setEqualTo(other.u_sprite);

        weapon.setEqualTo(other.weapon, true);
        armor.setEqualTo(other.armor, true);
        item.setEqualTo(other.item, true);

        foreach(var i in other.u_traits)
            u_traits.Add(i);
        u_talent = other.u_talent;
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

        u_critChance = Mathf.Clamp(u_critChance + Random.Range(0.0f, 2f), 0.0f, 100.0f);
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
        return canLevelUp();
    }

    public List<UnitTrait> getTraitsAndTalent() {
        var temp = new List<UnitTrait>();
        if(u_talent != null)
            temp.Add(u_talent);
        foreach(var i in u_traits)
            temp.Add(i);
        return temp;
    }
    public List<StatModifier.passiveMod> getAllPassiveMods() {
        var temp = new List<StatModifier.passiveMod>();
        if(u_talent != null) {
            foreach(var i in u_talent.passiveMods)
                temp.Add(i);
        }
        foreach(var i in u_traits) {
            foreach(var t in i.passiveMods)
                temp.Add(t);
        }
        if(item != null && !item.isEmpty()) {
            foreach(var i in item.passiveMods)
                temp.Add(i);
        }
        return temp;
    }
    public List<StatModifier.timedMod> getAllTimedMods() {
        var temp = new List<StatModifier.timedMod>();
        if(u_talent != null) {
            foreach(var i in u_talent.timedMods)
                temp.Add(i);
        }
        foreach(var i in u_traits) {
            foreach(var t in i.timedMods)
                temp.Add(t);
        }
        if(item != null && !item.isEmpty()) {
            foreach(var i in item.timedMods)
                temp.Add(i);
        }
        return temp;
    }


    //  Attack amount
    public float getBasePower() {
        float weaponPower = weapon.power * (((float)weapon.wornAmount + 7.0f) / 10.0f);
        return u_power + weaponPower;
    }
    float getLevelDamageMult() {
        float baseMult = 1.25f;
        if(weapon == null || weapon.isEmpty())
            return 1.0f;
        if(weapon.aType == Weapon.attackType.Blunt && getBluntLevel() > 0)
            return baseMult * (getBluntLevel() / 2.0f);
        if(weapon.aType == Weapon.attackType.Edged && getEdgedLevel() > 0)
            return baseMult * (getEdgedLevel() / 2.0f);
        return 1.0f;
    }
    public float getDamageGiven(PresetLibrary lib) {
        float dmg = getBasePower();
        float baseDmg = getBasePower();

        //  Weapon
        if(weapon != null && !weapon.isEmpty()) {   //  adds 10% of dmg for every power tolken in the weapon
            dmg += (0.1f * dmg) * weapon.getAttCount(Weapon.attribute.Power);
        }

        //  Armor
        if(armor != null && !armor.isEmpty()) { //  adds 10% of dmg for every power tolken in the armor
            dmg += (0.1f * dmg) * armor.getAttCount(Armor.attribute.Power);
        }

        var dmgType = (Weapon.attackType)(-1);
        if(weapon != null && !weapon.isEmpty())
            dmgType = weapon.aType;
        //  Traits modify damage
        foreach(var i in u_traits) {    //  adds whatever the trait's power mod is times the base damage
            dmg += i.getPassiveMod(StatModifier.passiveModifierType.modPower, this, false) * baseDmg;
        }
        if(u_talent != null)
            dmg += u_talent.getPassiveMod(StatModifier.passiveModifierType.modPower, this, false) * baseDmg;

        //  levels modify damage
        dmg *= getLevelDamageMult();

        //  Items modify damage
        if(item != null && !item.isEmpty()) {   //  adds whatever the item's power mod is times the base damage
            dmg += item.getPassiveMod(StatModifier.passiveModifierType.modPower, this, false) * baseDmg;

            if(weapon.aType == Weapon.attackType.Edged)
                dmg += item.getPassiveMod(StatModifier.passiveModifierType.modEdgedPower, this, false) * baseDmg;
            else if(weapon.aType == Weapon.attackType.Blunt)
                dmg += item.getPassiveMod(StatModifier.passiveModifierType.modBluntPower, this, false) * baseDmg;
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
        if(Random.Range(0.0f, 100.0f) <= u_critChance)
            return Random.Range(1.75f, 2.25f);

        return 1.0f;
    }

    //  Defence amount
    public float getBaseDefence() {
        float armorDefence = armor.defence * (((float)armor.wornAmount + 7.0f) / 10.0f);
        return Mathf.Clamp(u_defence + armorDefence, 0.0f, 100.0f);
    }
    public float getDefenceMult(bool defending, PresetLibrary lib, float tempDefenceMod) {    //  this value is multiplied by the damage taken
        //  starts with 100% of damage taken
        float temp = 1.0f - (tempDefenceMod / 100.0f);
        temp -= getBaseDefence() / 100.0f;

        //  Trigger Traits
        foreach(var i in u_traits) {
            temp -= i.getPassiveMod(StatModifier.passiveModifierType.modDefence, this, false);
        }
        if(u_talent != null)
            temp -= u_talent.getPassiveMod(StatModifier.passiveModifierType.modDefence, this, false);

        //  have item reduce damage
        if(item != null && !item.isEmpty())
            temp -= item.getPassiveMod(StatModifier.passiveModifierType.modDefence, this, false);

        //  Have Armor reduce damage
        if(armor != null && !armor.isEmpty()) { //  takes off 10% of damage for every tolken of turtle
            temp -= armor.getAttCount(Armor.attribute.Turtle) * 0.1f;
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
            temp *= i.getPassiveMod(StatModifier.passiveModifierType.modMaxHealth, this, true);
        if(u_talent != null)
            temp *= u_talent.getPassiveMod(StatModifier.passiveModifierType.modMaxHealth, this, true);

        return temp;
    }
    public float getModifiedSpeed(float tempSpeedMod) {
        float temp = u_speed + tempSpeedMod;
        if(weapon != null && !weapon.isEmpty())
            temp += weapon.speedMod;
        if(armor != null && !armor.isEmpty())
            temp += armor.speedMod;
        if(item != null && !item.isEmpty())
            temp += item.getPassiveMod(StatModifier.passiveModifierType.modSpeed, this, false);
        foreach(var i in u_traits)
            temp += i.getPassiveMod(StatModifier.passiveModifierType.modSpeed, this, false);
        if(u_talent != null)
            temp += u_talent.getPassiveMod(StatModifier.passiveModifierType.modSpeed, this, false);

        return temp;
    }

    public void setBaseMaxHealth(float f) {
        u_baseMaxHealth = f;
    }
    public float getBaseMaxHealth() {
        return u_baseMaxHealth;
    }

    public int getModifiedMissChance() {
        var temp = u_missChance;
        if(item != null && !item.isEmpty())
            temp += (int)item.getPassiveMod(StatModifier.passiveModifierType.missChance, this, false);
        return temp;
    }
    public int getModToAttackersMiss() {
        int temp = 0;
        if(item != null && !item.isEmpty())
            temp += (int)item.getPassiveMod(StatModifier.passiveModifierType.modEnemyMissChance, this, false);
        return temp;
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

    public void addSummonExp(float exp) {
        if(u_talent != null)
            exp *= u_talent.getPassiveMod(StatModifier.passiveModifierType.summonExpMod, this, true);

        foreach(var i in u_traits)
            exp *= i.getPassiveMod(StatModifier.passiveModifierType.summonExpMod, this, true);

        u_summonedExp += exp;
    }
    public void addBluntExp(float exp) {
        if(u_talent != null)
            exp *= u_talent.getPassiveMod(StatModifier.passiveModifierType.bluntExpMod, this, true);

        foreach(var i in u_traits)
            exp *= i.getPassiveMod(StatModifier.passiveModifierType.bluntExpMod, this, true);

        u_bluntExp += exp;
    }
    public void addEdgedExp(float exp) {
        if(u_talent != null)
            exp *= u_talent.getPassiveMod(StatModifier.passiveModifierType.edgedExpMod, this, true);

        foreach(var i in u_traits)
            exp *= i.getPassiveMod(StatModifier.passiveModifierType.edgedExpMod, this, true);

        u_edgedExp += exp;
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

    public void die(DeathInfo.killCause cause, PresetLibrary lib, FullInventoryCanvas fic, GameObject killer = null) {
        //  add equipped things back into the inventory
        if(weapon != null && !weapon.isEmpty())
            Inventory.addSingleCollectable(weapon, lib, fic);
        if(armor != null && !armor.isEmpty())
            Inventory.addSingleCollectable(armor, lib, fic);
        if(item != null && !item.isEmpty())
            Inventory.addSingleCollectable(item, lib, fic);

        weapon = null;
        armor = null;
        item = null;

        //  remove from party
        Party.removeUnit(this);

        u_deathInfo = new DeathInfo(cause, killer);
        //  adds to graveyard
        Graveyard.addUnit(this);
    }
}

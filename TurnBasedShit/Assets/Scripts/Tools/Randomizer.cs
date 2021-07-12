using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Randomizer {


    public static UnitStats createRandomUnitStats(bool fullHealth) {
        var stats = new UnitStats();

        stats.u_name = NameLibrary.getRandomUsablePlayerName();
        if(!fullHealth)
            stats.u_health = Random.Range(5.0f, stats.getModifiedMaxHealth());
        else
            stats.u_health = stats.getModifiedMaxHealth();
        stats.u_speed = Random.Range(0, 10.0f);

        //  at some point, equip the new unit with some random weapon and armor presets.
        //  but not now, god not now.

        return stats;
    }

    public static UnitStats randomizeUnitStats(UnitStats stats, bool fullHealth = true) {
        UnitStats s = new UnitStats(stats);
        //  health
        float maxHealthMod = stats.getModifiedMaxHealth() / 10.0f;
        s.setBaseMaxHealth(stats.getModifiedMaxHealth() + Random.Range(-maxHealthMod, maxHealthMod));
        if(!fullHealth) {
            float healthMin = stats.getModifiedMaxHealth() * 0.6f;
            s.u_health = Random.Range(healthMin, s.getModifiedMaxHealth());
        }
        else s.u_health = s.getModifiedMaxHealth();

        //  speed
        float speedMod = 2.0f;
        s.u_speed = stats.u_speed * Random.Range(-speedMod, speedMod);

        //  power
        float powerMod = 1.5f;
        s.u_power = stats.u_power * Random.Range(-powerMod, powerMod);

        //  defence
        float defenceMod = 1.5f;
        s.u_defence = stats.u_defence * Random.Range(-defenceMod, defenceMod);

        return s;
    }
    public static GameObject randomizeUnitStatsOnObject(GameObject unit, bool fullHealth = true) {
        //  health
        float maxHealthMod = unit.GetComponent<UnitClass>().stats.getModifiedMaxHealth() / 10.0f;
        unit.GetComponent<UnitClass>().stats.setBaseMaxHealth(unit.GetComponent<UnitClass>().stats.getModifiedMaxHealth() + Random.Range(-maxHealthMod, maxHealthMod));
        if(!fullHealth) {
            float healthMin = unit.GetComponent<UnitClass>().stats.getModifiedMaxHealth() * 0.6f;
            unit.GetComponent<UnitClass>().stats.u_health = Random.Range(healthMin, unit.GetComponent<UnitClass>().stats.getModifiedMaxHealth());
        }
        else unit.GetComponent<UnitClass>().stats.u_health = unit.GetComponent<UnitClass>().stats.getModifiedMaxHealth();

        //  speed
        float speedMod = 2.0f;
        unit.GetComponent<UnitClass>().stats.u_speed = unit.GetComponent<UnitClass>().stats.u_speed * Random.Range(-speedMod, speedMod);

        //  power
        float powerMod = 1.5f;
        unit.GetComponent<UnitClass>().stats.u_power = unit.GetComponent<UnitClass>().stats.u_power * Random.Range(-powerMod, powerMod);

        //  defence
        float defenceMod = 1.5f;
        unit.GetComponent<UnitClass>().stats.u_defence = unit.GetComponent<UnitClass>().stats.u_defence * Random.Range(-defenceMod, defenceMod);

        return unit;
    }

    public static SlaveStats randomizeSlaveStats(SlaveStats stats) {
        stats.isSlave = true;
        stats.escapeDesire = Random.Range(1.0f, 35.0f);
        stats.scaredModifier = Random.Range(1.15f, 1.85f);
        stats.idealWeathLevel = (GameInfo.rarityLvl)Random.Range((int)GameInfo.rarityLvl.Common, (int)GameInfo.rarityLvl.Rare + 1);
        return stats;
    }


    public static Weapon randomizeWeapon(Weapon we, GameInfo.diffLvl diff) {
        var temp = new Weapon();
        temp = randomizeWeaponStats(we);    //  I know that I pass we only in the first function, don't mess with it.
        temp = randomizeWeaponAttributesBasedOnRegion(temp, diff);

        return temp;
    }
    //  Eventually make it so the stats are affected by the region that the player is in
    public static Weapon randomizeWeaponStats(Weapon we) {
        var temp = new Weapon();
        temp.setEqualTo(we, true);
        temp.w_power = Mathf.Clamp(we.w_power + Random.Range(-10.0f, 25.0f), 1.0f, Mathf.Infinity);
        temp.w_speedMod = we.w_speedMod + Random.Range(-10.0f, 10.0f);

        temp.w_wornAmount = GameInfo.getRandomWornState();

        return temp;
    }
    public static Weapon randomizeWeaponAttributesBasedOnRegion(Weapon we, GameInfo.diffLvl diff) {
        int count = 0;
        switch((int)diff) {
            case 0:
                count = Random.Range(0, 1);
                break;

            case 1:
            case 2:
                count = Random.Range(0, 2);
                break;

            case 3:
            case 4:
                count = Random.Range(0, 3);
                break;

            case 5:
                count = Random.Range(1, 4);
                break;

            case 6:
                count = Random.Range(1, 5);
                break;
        }

        var temp = new Weapon();
        temp.setEqualTo(we, true);

        for(int i = 0; i < count; i++)
            temp.w_attributes.Add(temp.getRandAttribute());

        return temp;
    }

    public static Armor randomizeArmor(Armor ar, GameInfo.diffLvl diff) {
        var temp = new Armor();
        temp = randomizeArmorStats(ar);
        temp = randomizeArmorAttributesBasedOnRegion(temp, diff);

        return temp;
    }
    public static Armor randomizeArmorStats(Armor ar) {
        var temp = new Armor();
        temp.setEqualTo(ar, true);
        temp.a_defence = Mathf.Clamp(ar.a_defence + Random.Range(-10.0f, 25.0f), 1.0f, Mathf.Infinity);
        temp.a_speedMod = ar.a_speedMod + Random.Range(-10.0f, 10.0f);

        temp.a_wornAmount = GameInfo.getRandomWornState();

        return temp;
    }
    public static Armor randomizeArmorAttributesBasedOnRegion(Armor ar, GameInfo.diffLvl diff) {
        int count = 0;
        switch((int)diff) {
            case 0:
                count = Random.Range(0, 2);
                break;

            case 1:
            case 2:
                count = Random.Range(0, 3);
                break;

            case 3:
            case 4:
                count = Random.Range(0, 4);
                break;

            case 5:
                count = Random.Range(1, 4);
                break;

            case 6:
                count = Random.Range(1, 5);
                break;
        }

        var temp = new Armor();
        temp.setEqualTo(ar, true);

        for(int i = 0; i < count; i++)
            temp.a_attributes.Add(temp.getRandAttribute());

        return temp;
    }

    public static CombatLocation randomizeCombatLocation(CombatLocation cl) {
        //  randomize enemies
        for(int j = 0; j < cl.waves.Count; j++) {
            for(int i = 0; i < cl.waves[j].enemies.Count; i++) {
                var enemy = cl.waves[j].enemies[i];
                enemy.GetComponent<UnitClass>().stats = randomizeUnitStats(enemy.GetComponent<UnitClass>().stats, true);
                cl.waves[j].enemies[i] = enemy;
            }
        }
        return cl;
    }
}

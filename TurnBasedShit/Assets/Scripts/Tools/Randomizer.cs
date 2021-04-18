using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Randomizer {


    public static UnitStats createRandomUnitStats(bool fullHealth) {
        var stats = new UnitStats();

        stats.u_name = NameLibrary.getRandomUsableName();
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

        return s;
    }

    public static SlaveStats randomizeSlaveStats(SlaveStats stats) {
        stats.isSlave = true;
        stats.escapeDesire = Random.Range(1.0f, 35.0f);
        stats.scaredModifier = Random.Range(1.15f, 1.85f);
        stats.idealWeathLevel = (GameInfo.rarityLvl)Random.Range((int)GameInfo.rarityLvl.common, (int)GameInfo.rarityLvl.rare + 1);
        return stats;
    }


    public static Weapon randomizeWeapon(Weapon we) {
        //  sets random attributes
        for(int i = 0; i < Random.Range(0, 3); i++) {
            we.w_attributes.Add(we.getRandAttribute());
        }

        //  sets random values
        we.w_power = Mathf.Clamp(we.w_power + Random.Range(-10.0f, 25.0f), 1.0f, Mathf.Infinity);
        we.w_speedMod += Random.Range(-10.0f, 10.0f);


        return we;
    }
    public static Armor randomizeArmor(Armor ar) {
        //  sets random attributes
        for(int i = 0; i < Random.Range(0, 3); i++) {
            ar.a_attributes.Add(ar.getRandomAttribute());
        }

        //  sets random values
        ar.a_defence = Mathf.Clamp(ar.a_defence + Random.Range(-10.0f, 25.0f), 1.0f, Mathf.Infinity);
        ar.a_speedMod += Random.Range(-10.0f, 10.0f);

        return ar;
    }
    public static Consumable randomizeConsumable(Consumable con) {
        //  sets random attributes
        con.c_effectAmount = Random.Range(5.0f, 50.0f);

        return con;
    }

    public static CombatLocation randomizeCombatLocation(CombatLocation cl) {
        //  randomize enemies
        for(int i = 0; i < cl.enemies.Count; i++)
            cl.enemies[i] = randomizeUnitStats(cl.enemies[i], true);
        return cl;
    }


    public static Town createRandomTown() {
        var town = new Town(Random.Range(3, 9));
        town.shopPriceMod = Random.Range(-0.1f, 0.1f);
        town.shopSellReduction = Random.Range(0.0f, 0.15f);

        return town;
    }
}

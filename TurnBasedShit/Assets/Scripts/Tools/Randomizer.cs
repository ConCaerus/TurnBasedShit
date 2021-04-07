﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Randomizer {


    public static UnitClassStats createRandomUnitStats(bool fullHealth) {
        var stats = new UnitClassStats();

        stats.u_name = NameLibrary.getRandomUsableName();
        if(!fullHealth)
            stats.u_health = Random.Range(5.0f, stats.u_maxHealth);
        else
            stats.u_health = stats.u_maxHealth;
        stats.u_speed = Random.Range(0, 10.0f);

        //  at some point, equip the new unit with some random weapon and armor presets.
        //  but not now, god not now.

        return stats;
    }

    public static UnitClassStats randomizeUnitStats(UnitClassStats stats, bool fullHealth = true) {
        //  health
        float maxHealthMod = stats.u_maxHealth / 10.0f;
        stats.u_maxHealth += Random.Range(-maxHealthMod, maxHealthMod);
        if(!fullHealth) {
            float healthMin = stats.u_maxHealth * 0.6f;
            stats.u_health = Random.Range(healthMin, stats.u_maxHealth);
        }
        else stats.u_health = stats.u_maxHealth;

        //  speed
        float speedMod = 2.0f;
        stats.u_speed += Random.Range(-speedMod, speedMod);

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

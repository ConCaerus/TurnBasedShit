using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Randomizer {


    public static UnitClassStats createRandomUnitStats() {
        var stats = new UnitClassStats();

        stats.u_name = NameLibrary.getRandomUsableName();
        stats.u_health = Random.Range(5.0f, stats.u_maxHealth);
        stats.u_speed = Random.Range(0, 10.0f);

        //  at some point, equip the new unit with some random weapon and armor presets.
        //  but not now, god not now.

        return stats;
    }

    public static Weapon createRandomWeapon() {
        var we = Inventory.getRandomWeaponPreset();

        //  sets random attributes
        for(int i = 0; i < Random.Range(0,3); i++) {
            we.w_attributes.Add(we.getRandAttribute());
        }

        //  sets random values
        we.w_power += Mathf.Clamp(Random.Range(-10.0f, 25.0f), 1.0f, Mathf.Infinity);
        we.w_speedMod += Random.Range(-10.0f, 10.0f);


        return we;
    }
    public static Armor createRandomArmor() {
        var ar = Inventory.getRandomArmorPreset();

        //  sets random attributes
        for(int i = 0; i < Random.Range(0,3); i++) {
            ar.a_attributes.Add(ar.getRandomAttribute());
        }

        //  sets random values
        ar.a_defence += Mathf.Clamp(Random.Range(-10.0f, 25.0f), 1.0f, Mathf.Infinity);
        ar.a_speedMod += Random.Range(-10.0f, 10.0f);

        return ar;
    }
    public static Consumable createRandomConsumable() {
        var it = Inventory.getRandomConsumablePreset();
        

        //  sets random attributes
        it.c_effectAmount = Random.Range(5.0f, 50.0f);

        return it;
    }


    public static CombatLocation getRandomCombatLocation() {
        var locations = AssetDatabase.FindAssets("t:CombatLocationPreset", null);
        var rand = Random.Range(0, 101);
        int result = rand % locations.Length;

        var g = AssetDatabase.GUIDToAssetPath(locations[result]);
        CombatLocationPreset p = (CombatLocationPreset)AssetDatabase.LoadAssetAtPath(g, typeof(CombatLocationPreset));
        return p.preset;
    }
}

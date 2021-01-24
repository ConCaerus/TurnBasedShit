using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory {
    static List<Weapon> weapons = new List<Weapon>();
    static List<Armor> armor = new List<Armor>();


    public static void clearInventory() {
        weapons.Clear();
        armor.Clear();
    }
    public static void clearWeapons() {
        weapons.Clear();
    }
    public static void clearArmor() {
        armor.Clear();
    }


    public static void addWeaponToInventory(Weapon w) {
        weapons.Add(w);

        var data = JsonUtility.ToJson(w);
        PlayerPrefs.SetString("Inventory Weapon" + weapons.Count.ToString(), data);
    }
    public static void addArmorToInventory(Armor a) {
        armor.Add(a);
    }

    public static void removeWeaponFromInventory(Weapon w) {
        for(int i = 0; i < weapons.Count; i++) {
            if(weapons[i] == w) {
                PlayerPrefs.DeleteKey("Inventory Weapon" + i.ToString());
                weapons.Remove(w);
                break;
            }
        }
    }
    public static void removeArmorFromInventory(Armor a) {
        armor.Remove(a);
    }


    public static int getWeaponCount() {
        return weapons.Count;
    }
    public static Weapon getWeaponFromIndex(int i) {
        return weapons[i];
    }

    public static int getArmorCount() {
        return armor.Count;
    }
    public static Armor getArmorFromIndex(int i) {
        return armor[i];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//  Actual Inventory Script
public static class Inventory {
    const string weaponCount = "Inventory Weapon Count";
    const string armorCount = "Inventory Armor Count";
    const string consumableCount = "Inventory Consumable Count";

    static string weaponTag(int index) { return "Inventory Weapon" + index.ToString(); }
    static string armorTag(int index) { return "Inventory Armor" + index.ToString(); }
    static string consumableTag(int index) { return "Inventory Consumable" + index.ToString(); }

    
    public static void clearInventory() {
        clearWeapons();
        clearArmor();
        clearConsumables();
    }
    public static void clearWeapons() {
        for(int i = 0; i < PlayerPrefs.GetInt(weaponCount); i++) {
            PlayerPrefs.DeleteKey(weaponTag(i));
        }
        PlayerPrefs.DeleteKey(weaponCount);

        PlayerPrefs.Save();
    }
    public static void clearArmor() {
        for(int i = 0; i < PlayerPrefs.GetInt(armorCount); i++) {
            PlayerPrefs.DeleteKey(armorTag(i));
        }
        PlayerPrefs.DeleteKey(armorCount);

        PlayerPrefs.Save();
    }
    public static void clearConsumables() {
        for(int i = 0; i < PlayerPrefs.GetInt(consumableCount); i++) {
            PlayerPrefs.DeleteKey(consumableTag(i));
        }
        PlayerPrefs.DeleteKey(consumableCount);

        PlayerPrefs.Save();
    }

    public static void addNewWeapon(Weapon w) {
        int index = PlayerPrefs.GetInt(weaponCount);

        var data = JsonUtility.ToJson(w);
        PlayerPrefs.SetString(weaponTag(index), data);
        PlayerPrefs.SetInt(weaponCount, index + 1);

        PlayerPrefs.Save();
    }
    public static void addNewArmor(Armor a) {
        int index = PlayerPrefs.GetInt(armorCount);

        var data = JsonUtility.ToJson(a);
        PlayerPrefs.SetString(armorTag(index), data);
        PlayerPrefs.SetInt(armorCount, index + 1);

        PlayerPrefs.Save();
    }
    public static void addNewConsumable(Consumable i) {
        int index = PlayerPrefs.GetInt(consumableCount);

        var data = JsonUtility.ToJson(i);
        PlayerPrefs.SetString(consumableTag(index), data);
        PlayerPrefs.SetInt(consumableCount, index + 1);

        PlayerPrefs.Save();
    }

    public static void removeWeapon(Weapon w) {
        var wData = JsonUtility.ToJson(w);

        bool past = false;
        for(int i = 0; i < PlayerPrefs.GetInt(weaponCount); i++) {
            var data = PlayerPrefs.GetString(weaponTag(i));

            if(data == wData && !past) {
                PlayerPrefs.DeleteKey(weaponTag(i));
                past = true;
                continue;
            }
            else if(past) {
                PlayerPrefs.DeleteKey(weaponTag(i));
                overrideWeapon(i - 1, JsonUtility.FromJson<Weapon>(data));
            }
        }

        PlayerPrefs.SetInt(weaponCount, PlayerPrefs.GetInt(weaponCount) - 1);
        PlayerPrefs.Save();
    }
    public static void removeArmor(Armor a) {
        var aData = JsonUtility.ToJson(a);

        bool past = false;
        for(int i = 0; i < PlayerPrefs.GetInt(armorCount); i++) {
            var data = PlayerPrefs.GetString(armorTag(i));

            if(data == aData && !past) {
                PlayerPrefs.DeleteKey(armorTag(i));
                past = true;
                continue;
            }
            else if(past) {
                PlayerPrefs.DeleteKey(armorTag(i));
                overrideArmor(i - 1, JsonUtility.FromJson<Armor>(data));
            }
        }

        PlayerPrefs.SetInt(armorCount, PlayerPrefs.GetInt(armorCount) - 1);
        PlayerPrefs.Save();
    }
    public static void removeConsumable(Consumable j) {
        var jData = JsonUtility.ToJson(j);

        bool past = false;
        for(int i = 0; i < PlayerPrefs.GetInt(consumableCount); i++) {
            var data = PlayerPrefs.GetString(consumableTag(i));

            if(data == jData && !past) {
                PlayerPrefs.DeleteKey(consumableTag(i));
                past = true;
                continue;
            }
            else if(past) {
                PlayerPrefs.DeleteKey(consumableTag(i));
                overrideConsumable(i - 1, JsonUtility.FromJson<Consumable>(data));
            }
        }

        PlayerPrefs.SetInt(consumableCount, PlayerPrefs.GetInt(consumableCount) - 1);
        PlayerPrefs.Save();
    }

    public static void overrideWeapon(int index, Weapon w) {
        var data = JsonUtility.ToJson(w);
        PlayerPrefs.SetString(weaponTag(index), data);

        PlayerPrefs.Save();
    }
    public static void overrideArmor(int index, Armor a) {
        var data = JsonUtility.ToJson(a);
        PlayerPrefs.SetString(armorTag(index), data);

        PlayerPrefs.Save();
    }
    public static void overrideConsumable(int index, Consumable i) {
        var data = JsonUtility.ToJson(i);
        PlayerPrefs.SetString(consumableTag(index), data);

        PlayerPrefs.Save();
    }

    public static int getWeaponCount() {
        return PlayerPrefs.GetInt(weaponCount);
    }
    public static int getArmorCount() {
        return PlayerPrefs.GetInt(armorCount);
    }
    public static int getConsumableCount() {
        return PlayerPrefs.GetInt(consumableCount);
    }

    public static Weapon getWeapon(int i) {
        var data = PlayerPrefs.GetString(weaponTag(i));
        return JsonUtility.FromJson<Weapon>(data);
    }
    public static Armor getArmor(int i) {
        var data = PlayerPrefs.GetString(armorTag(i));
        return JsonUtility.FromJson<Armor>(data);
    }
    public static Consumable getConsumable(int i) {
        var data = PlayerPrefs.GetString(consumableTag(i));
        return JsonUtility.FromJson<Consumable>(data);
    }


    public static Weapon getRandomWeaponPreset() {
        var weaponPresets = AssetDatabase.FindAssets("t:WeaponPreset", null);
        var rand = Random.Range(0, 101);
        var result = rand % weaponPresets.Length;

        var g = AssetDatabase.GUIDToAssetPath(weaponPresets[result]);
        WeaponPreset p = (WeaponPreset)AssetDatabase.LoadAssetAtPath(g, typeof(WeaponPreset));

        return p.preset;
    }
    public static Armor getRandomArmorPreset() {
        var armorPresets = AssetDatabase.FindAssets("t:ArmorPreset", null);
        var rand = Random.Range(0, 101);
        var result = rand % armorPresets.Length;

        var g = AssetDatabase.GUIDToAssetPath(armorPresets[result]);
        ArmorPreset p = (ArmorPreset)AssetDatabase.LoadAssetAtPath(g, typeof(ArmorPreset));

        return p.preset;
    }
    public static Consumable getRandomConsumablePreset() {
        var conPresets = AssetDatabase.FindAssets("t:ConsumablePreset", null);
        var rand = Random.Range(0, 101);
        var result = rand % conPresets.Length;

        var g = AssetDatabase.GUIDToAssetPath(conPresets[result]);
        ConsumablePreset p = (ConsumablePreset)AssetDatabase.LoadAssetAtPath(g, typeof(ConsumablePreset));

        return p.preset;
    }



    public static int getWeaponIndex(Weapon w) {
        for(int i = 0; i < PlayerPrefs.GetInt(weaponCount); i++) {
            var data = PlayerPrefs.GetString(weaponTag(i));
            var temp = JsonUtility.FromJson<Weapon>(data);

            if(temp == w) 
                return i;
        }
        return -1;
    }
    public static int getArmorIndex(Armor a) {
        for(int i = 0; i < PlayerPrefs.GetInt(armorCount); i++) {
            var data = PlayerPrefs.GetString(armorTag(i));
            var temp = JsonUtility.FromJson<Armor>(data);

            if(temp == a) 
                return i;
        }
        return -1;
    }
    public static int getConsumableIndex(Consumable it) {
        for(int i = 0; i < PlayerPrefs.GetInt(consumableCount); i++) {
            var data = PlayerPrefs.GetString(consumableTag(i));
            var temp = JsonUtility.FromJson<Consumable>(data);

            if(temp == it)
                return i;
        }
        return -1;
    }


    public static int getNumberOfUniqueConsumables(Consumable con) {
        int count = 0;
        for(int i = 0; i < PlayerPrefs.GetInt(consumableCount); i++) {
            if(getConsumable(i).isEqualTo(con))
                count++;
        }

        return count;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//  Actual Inventory Script
public static class Inventory {
    const string weaponCount = "Inventory Weapon Count";
    const string armorCount = "Inventory Armor Count";
    const string itemCount = "Inventory Item Count";

    static string weaponTag(int index) { return "Inventory Weapon" + index.ToString(); }
    static string armorTag(int index) { return "Inventory Armor" + index.ToString(); }
    static string consumableTag(int index) { return "Inventory Item" + index.ToString(); }

    
    public static void clearInventory() {
        clearWeapons();
        clearArmor();
        clearItems();
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
    public static void clearItems() {
        for(int i = 0; i < PlayerPrefs.GetInt(itemCount); i++) {
            PlayerPrefs.DeleteKey(consumableTag(i));
        }
        PlayerPrefs.DeleteKey(itemCount);

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
    public static void addNewItem(Consumable i) {
        int index = PlayerPrefs.GetInt(itemCount);

        var data = JsonUtility.ToJson(i);
        PlayerPrefs.SetString(consumableTag(index), data);
        PlayerPrefs.SetInt(itemCount, index + 1);

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
    public static void removeItem(Consumable j) {
        var jData = JsonUtility.ToJson(j);

        bool past = false;
        for(int i = 0; i < PlayerPrefs.GetInt(itemCount); i++) {
            var data = PlayerPrefs.GetString(consumableTag(i));

            if(data == jData && !past) {
                PlayerPrefs.DeleteKey(consumableTag(i));
                past = true;
                continue;
            }
            else if(past) {
                PlayerPrefs.DeleteKey(consumableTag(i));
                overrideItem(i - 1, JsonUtility.FromJson<Consumable>(data));
            }
        }

        PlayerPrefs.SetInt(itemCount, PlayerPrefs.GetInt(itemCount) - 1);
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
    public static void overrideItem(int index, Consumable i) {
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
    public static int getItemCount() {
        return PlayerPrefs.GetInt(itemCount);
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
    public static int getItemIndex(Consumable it) {
        for(int i = 0; i < PlayerPrefs.GetInt(itemCount); i++) {
            var data = PlayerPrefs.GetString(consumableTag(i));
            var temp = JsonUtility.FromJson<Consumable>(data);

            if(temp == it)
                return i;
        }
        return -1;
    }
}
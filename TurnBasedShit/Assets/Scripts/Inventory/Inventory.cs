using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//  Actual Inventory Script
public static class Inventory {
    const string weaponCount = "Inventory Weapon Count";
    const string armorCount = "Inventory Armor Count";
    const string consumableCount = "Inventory Consumable Count";
    const string coinCount = "Coin Count";

    static string weaponTag(int index) { return "Inventory Weapon" + index.ToString(); }
    static string armorTag(int index) { return "Inventory Armor" + index.ToString(); }
    static string consumableTag(int index) { return "Inventory Consumable" + index.ToString(); }


    public static void createDefaultInventory() {

    }

    public static void clearInventory() {
        clearWeapons();
        clearArmor();
        clearConsumables();
        clearCoins();
    }
    public static void clearWeapons() {
        for(int i = 0; i < SaveData.getInt(weaponCount); i++) {
            SaveData.deleteKey(weaponTag(i));
        }
        SaveData.deleteKey(weaponCount);
    }
    public static void clearArmor() {
        for(int i = 0; i < SaveData.getInt(armorCount); i++) {
            SaveData.deleteKey(armorTag(i));
        }
        SaveData.deleteKey(armorCount);
    }
    public static void clearConsumables() {
        for(int i = 0; i < SaveData.getInt(consumableCount); i++) {
            SaveData.deleteKey(consumableTag(i));
        }
        SaveData.deleteKey(consumableCount);
    }
    public static void clearCoins() {
        SaveData.deleteKey(coinCount);
    }

    public static void addNewWeapon(Weapon w) {
        int index = SaveData.getInt(weaponCount);

        w.w_sprite.setSprite();
        var data = JsonUtility.ToJson(w);
        SaveData.setString(weaponTag(index), data);
        SaveData.setInt(weaponCount, index + 1);
    }
    public static void addNewArmor(Armor a) {
        int index = SaveData.getInt(armorCount);

        a.a_sprite.setSprite();
        var data = JsonUtility.ToJson(a);
        SaveData.setString(armorTag(index), data);
        SaveData.setInt(armorCount, index + 1);
    }
    public static void addNewConsumable(Consumable c) {
        int index = SaveData.getInt(consumableCount);

        c.c_sprite.setSprite();
        var data = JsonUtility.ToJson(c);
        SaveData.setString(consumableTag(index), data);
        SaveData.setInt(consumableCount, index + 1);
    }
    public static void addCoins(int count) {
        int temp = SaveData.getInt(coinCount);
        SaveData.setInt(coinCount, temp + count);
    }

    public static void removeWeapon(Weapon w) {
        var wData = JsonUtility.ToJson(w);

        bool past = false;
        for(int i = 0; i < SaveData.getInt(weaponCount); i++) {
            var data = SaveData.getString(weaponTag(i));

            if(data == wData && !past) {
                SaveData.deleteKey(weaponTag(i));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(weaponTag(i));
                overrideWeapon(i - 1, JsonUtility.FromJson<Weapon>(data));
            }
        }

        SaveData.setInt(weaponCount, SaveData.getInt(weaponCount) - 1);
    }
    public static void removeArmor(Armor a) {
        var aData = JsonUtility.ToJson(a);

        bool past = false;
        for(int i = 0; i < SaveData.getInt(armorCount); i++) {
            var data = SaveData.getString(armorTag(i));

            if(data == aData && !past) {
                SaveData.deleteKey(armorTag(i));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(armorTag(i));
                overrideArmor(i - 1, JsonUtility.FromJson<Armor>(data));
            }
        }

        SaveData.setInt(armorCount, SaveData.getInt(armorCount) - 1);
    }
    public static void removeConsumable(Consumable j) {
        var jData = JsonUtility.ToJson(j);

        bool past = false;
        for(int i = 0; i < SaveData.getInt(consumableCount); i++) {
            var data = SaveData.getString(consumableTag(i));

            if(data == jData && !past) {
                SaveData.deleteKey(consumableTag(i));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(consumableTag(i));
                overrideConsumable(i - 1, JsonUtility.FromJson<Consumable>(data));
            }
        }

        SaveData.setInt(consumableCount, SaveData.getInt(consumableCount) - 1);
    }

    public static void overrideWeapon(int index, Weapon w) {
        w.w_sprite.setSprite();
        var data = JsonUtility.ToJson(w);
        SaveData.setString(weaponTag(index), data);
    }
    public static void overrideArmor(int index, Armor a) {
        a.a_sprite.setSprite();
        var data = JsonUtility.ToJson(a);
        SaveData.setString(armorTag(index), data);
    }
    public static void overrideConsumable(int index, Consumable c) {
        c.c_sprite.setSprite();
        var data = JsonUtility.ToJson(c);
        SaveData.setString(consumableTag(index), data);
    }

    public static int getWeaponCount() {
        return SaveData.getInt(weaponCount);
    }
    public static int getArmorCount() {
        return SaveData.getInt(armorCount);
    }
    public static int getConsumableCount() {
        return SaveData.getInt(consumableCount);
    }
    public static int getCoinCount() {
        return SaveData.getInt(coinCount);
    }

    public static Weapon getWeapon(int i) {
        var data = SaveData.getString(weaponTag(i));
        return JsonUtility.FromJson<Weapon>(data);
    }
    public static Armor getArmor(int i) {
        var data = SaveData.getString(armorTag(i));
        return JsonUtility.FromJson<Armor>(data);
    }
    public static Consumable getConsumable(int i) {
        var data = SaveData.getString(consumableTag(i));
        return JsonUtility.FromJson<Consumable>(data);
    }

    public static int getWeaponIndex(Weapon w) {
        for(int i = 0; i < SaveData.getInt(weaponCount); i++) {
            var data = SaveData.getString(weaponTag(i));
            var temp = JsonUtility.FromJson<Weapon>(data);

            if(temp == w) 
                return i;
        }
        return -1;
    }
    public static int getArmorIndex(Armor a) {
        for(int i = 0; i < SaveData.getInt(armorCount); i++) {
            var data = SaveData.getString(armorTag(i));
            var temp = JsonUtility.FromJson<Armor>(data);

            if(temp == a) 
                return i;
        }
        return -1;
    }
    public static int getConsumableIndex(Consumable it) {
        for(int i = 0; i < SaveData.getInt(consumableCount); i++) {
            var data = SaveData.getString(consumableTag(i));
            var temp = JsonUtility.FromJson<Consumable>(data);

            if(temp == it)
                return i;
        }
        return -1;
    }


    public static int getNumberOfUniqueConsumables(Consumable con) {
        int count = 0;
        for(int i = 0; i < SaveData.getInt(consumableCount); i++) {
            if(getConsumable(i).isEqualTo(con))
                count++;
        }

        return count;
    }
}
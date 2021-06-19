using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//  Actual Inventory Script
public static class Inventory {
    const string coinCount = "Coin Count";

    static string objectCountTag(System.Type type) {
        if(type == typeof(Weapon))
            return "Inventory Weapon Count";
        if(type == typeof(Armor))
            return "Inventory Armor Count";
        if(type == typeof(Consumable))
            return "Inventory Consumable Count";
        if(type == typeof(Item))
            return "Inventory Item Count";
        return string.Empty;
    }
    static string objectTag(int index, System.Type type) {
        if(type == typeof(Weapon))
            return "Inventory Weapon" + index.ToString();
        if(type == typeof(Armor))
            return "Inventory Armor" + index.ToString();
        if(type == typeof(Consumable))
            return "Inventory Consumable" + index.ToString();
        if(type == typeof(Item))
            return "Inventory Item" + index.ToString();
        return string.Empty;
    }


    public static void createDefaultInventory() {

    }

    public static void clearInventory() {
        clearWeapons();
        clearArmor();
        clearConsumables();
        clearItems();
        clearCoins();
    }
    public static void clearWeapons() {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Weapon))); i++) {
            SaveData.deleteKey(objectTag(i, typeof(Weapon)));
        }
        SaveData.deleteKey(objectCountTag(typeof(Weapon)));
    }
    public static void clearArmor() {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Armor))); i++) {
            SaveData.deleteKey(objectTag(i, typeof(Armor)));
        }
        SaveData.deleteKey(objectCountTag(typeof(Armor)));
    }
    public static void clearConsumables() {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Consumable))); i++) {
            SaveData.deleteKey(objectTag(i, typeof(Consumable)));
        }
        SaveData.deleteKey(objectCountTag(typeof(Consumable)));
    }
    public static void clearItems() {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Item))); i++) {
            SaveData.deleteKey(objectTag(i, typeof(Item)));
        }
        SaveData.deleteKey(objectCountTag(typeof(Item)));
    }
    public static void clearCoins() {
        SaveData.deleteKey(coinCount);
    }

    public static void addWeapon(Weapon w) {
        int index = getTypeCount(typeof(Weapon));

        w.w_instanceID = getNextWeaponInstanceID();
        var data = JsonUtility.ToJson(w);
        SaveData.setString(objectTag(w.w_instanceID, typeof(Weapon)), data);
        SaveData.setInt(objectCountTag(typeof(Weapon)), index + 1);
    }
    public static void addArmor(Armor a) {
        int index = getTypeCount(typeof(Armor));

        a.a_instanceID = getNextArmorInstanceID();
        var data = JsonUtility.ToJson(a);
        SaveData.setString(objectTag(a.a_instanceID, typeof(Armor)), data);
        SaveData.setInt(objectCountTag(typeof(Armor)), index + 1);
    }
    public static void addConsumable(Consumable c) {
        int index = getTypeCount(typeof(Consumable));

        c.c_instanceID = getNextConsumableInstanceID();
        var data = JsonUtility.ToJson(c);
        SaveData.setString(objectTag(c.c_instanceID, typeof(Consumable)), data);
        SaveData.setInt(objectCountTag(typeof(Consumable)), index + 1);
    }
    public static void addItem(Item it) {
        int index = getTypeCount(typeof(Item));

        it.i_instanceID = getNextItemInstanceID();
        var data = JsonUtility.ToJson(it);
        SaveData.setString(objectTag(it.i_instanceID, typeof(Item)), data);
        SaveData.setInt(objectCountTag(typeof(Item)), index + 1);
    }
    public static void addCoins(int count) {
        int temp = SaveData.getInt(coinCount);
        SaveData.setInt(coinCount, temp + count);
    }

    public static void removeWeapon(Weapon w) {
        var tData = JsonUtility.ToJson(w);
        bool past = false;
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Weapon))); i++) {
            var data = SaveData.getString(objectTag(i, typeof(Weapon)));

            if(data == tData && !past) {
                SaveData.deleteKey(objectTag(i, typeof(Weapon)));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(objectTag(i, typeof(Weapon)));
                overrideWeapon(i - 1, JsonUtility.FromJson<Weapon>(data));
            }
        }
        SaveData.setInt(objectCountTag(typeof(Weapon)), SaveData.getInt(objectCountTag(typeof(Weapon))) - 1);
    }
    public static void removeArmor(Armor a) {
        var tData = JsonUtility.ToJson(a);
        bool past = false;
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Armor))); i++) {
            var data = SaveData.getString(objectTag(i, typeof(Armor)));

            if(data == tData && !past) {
                SaveData.deleteKey(objectTag(i, typeof(Armor)));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(objectTag(i, typeof(Armor)));
                overrideArmor(i - 1, JsonUtility.FromJson<Armor>(data));
            }
        }
        SaveData.setInt(objectCountTag(typeof(Armor)), SaveData.getInt(objectCountTag(typeof(Armor))) - 1);
    }
    public static void removeConsumable(Consumable c) {
        var tData = JsonUtility.ToJson(c);
        bool past = false;
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Consumable))); i++) {
            var data = SaveData.getString(objectTag(i, typeof(Consumable)));

            if(data == tData && !past) {
                SaveData.deleteKey(objectTag(i, typeof(Consumable)));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(objectTag(i, typeof(Consumable)));
                overrideConsumable(i - 1, JsonUtility.FromJson<Consumable>(data));
            }
        }
        SaveData.setInt(objectCountTag(typeof(Consumable)), SaveData.getInt(objectCountTag(typeof(Consumable))) - 1);
    }
    public static void removeItem(Item it) {
        var tData = JsonUtility.ToJson(it);
        bool past = false;
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Item))); i++) {
            var data = SaveData.getString(objectTag(i, typeof(Item)));

            if(data == tData && !past) {
                SaveData.deleteKey(objectTag(i, typeof(Item)));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(objectTag(i, typeof(Item)));
                overrideItem(i - 1, JsonUtility.FromJson<Item>(data));
            }
        }
        SaveData.setInt(objectCountTag(typeof(Item)), SaveData.getInt(objectCountTag(typeof(Item))) - 1);
    }
    public static void removeWeapon(int index) {
        removeWeapon(getWeapon(index));
    }
    public static void removeArmor(int index) {
        removeArmor(getArmor(index));
    }
    public static void removeConsumable(int index) {
        removeConsumable(getConsumable(index));
    }
    public static void removeItem(int index) {
        removeItem(getItem(index));
    }
    public static void removeCoins(int count) {
        int temp = SaveData.getInt(coinCount);
        SaveData.setInt(coinCount, temp - count);
    }

    public static void overrideWeapon(int index, Weapon w) {
        var data = JsonUtility.ToJson(w);
        SaveData.setString(objectTag(index, typeof(Weapon)), data);
    }
    public static void overrideArmor(int index, Armor a) {
        var data = JsonUtility.ToJson(a);
        SaveData.setString(objectTag(index, typeof(Armor)), data);
    }
    public static void overrideConsumable(int index, Consumable c) {
        var data = JsonUtility.ToJson(c);
        SaveData.setString(objectTag(index, typeof(Consumable)), data);
    }
    public static void overrideItem(int index, Item it) {
        var data = JsonUtility.ToJson(it);
        SaveData.setString(objectTag(index, typeof(Item)), data);
    }

    public static int getTypeCount(System.Type type) {
        if(type == typeof(Weapon))
            return SaveData.getInt(objectCountTag(type));
        if(type == typeof(Armor))
            return SaveData.getInt(objectCountTag(type));
        if(type == typeof(Consumable))
            return SaveData.getInt(objectCountTag(type));
        if(type == typeof(Item))
            return SaveData.getInt(objectCountTag(type));
        return -1;
    }
    public static int getCoinCount() {
        return SaveData.getInt(coinCount);
    }

    public static Weapon getWeapon(int i) {
        var data = SaveData.getString(objectTag(i, typeof(Weapon)));
        return JsonUtility.FromJson<Weapon>(data);
    }
    public static Armor getArmor(int i) {
        var data = SaveData.getString(objectTag(i, typeof(Armor)));
        return JsonUtility.FromJson<Armor>(data);
    }
    public static Consumable getConsumable(int i) {
        var data = SaveData.getString(objectTag(i, typeof(Consumable)));
        return JsonUtility.FromJson<Consumable>(data);
    }
    public static Item getItem(int i) {
        var data = SaveData.getString(objectTag(i, typeof(Item)));
        return JsonUtility.FromJson<Item>(data);
    }

    public static int getWeaponIndex(Weapon w) {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Weapon))); i++) {
            var temp = getWeapon(i);

            if(temp == w)
                return i;
        }
        return -1;
    }
    public static int getArmorIndex(Armor a) {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Armor))); i++) {
            var temp = getArmor(i);

            if(temp == a)
                return i;
        }
        return -1;
    }
    public static int getConsumableIndex(Consumable c) {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Consumable))); i++) {
            var temp = getConsumable(i);

            if(temp == c)
                return i;
        }
        return -1;
    }
    public static int getItemIndex(Item it) {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Item))); i++) {
            var temp = getItem(i);

            if(temp == it)
                return i;
        }
        return -1;
    }

    static int getNextWeaponInstanceID() {
        for(int i = 0; i < getTypeCount(typeof(Weapon)) + 1; i++) {
            if(getWeapon(i) == null)
                return i;
        }
        Debug.LogError("Could not find next weapon instance ID");
        return -1;
    }
    static int getNextArmorInstanceID() {
        for(int i = 0; i < getTypeCount(typeof(Armor)) + 1; i++) {
            if(getArmor(i) == null)
                return i;
        }
        Debug.LogError("Could not find next armor instance ID");
        return -1;
    }
    static int getNextConsumableInstanceID() {
        for(int i = 0; i < getTypeCount(typeof(Consumable)) + 1; i++) {
            if(getConsumable(i) == null)
                return i;
        }
        Debug.LogError("Could not find next cons instance ID");
        return -1;
    }
    static int getNextItemInstanceID() {
        for(int i = 0; i < getTypeCount(typeof(Item)) + 1; i++) {
            if(getItem(i) == null)
                return i;
        }
        Debug.LogError("Could not find next item instance ID");
        return -1;
    }
}
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

    public static void clearInventory(bool clearInstanceQueue) {
        clearWeapons();
        clearArmor();
        clearConsumables();
        clearItems();
        clearCoins();

        if(clearInstanceQueue)
            GameInfo.clearInventoryInstanceIDQueue();
    }
    public static void clearWeapons() {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Weapon))) + 10; i++) {
            SaveData.deleteKey(objectTag(i, typeof(Weapon)));
        }
        SaveData.deleteKey(objectCountTag(typeof(Weapon)));
    }
    public static void clearArmor() {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Armor))) + 10; i++) {
            SaveData.deleteKey(objectTag(i, typeof(Armor)));
        }
        SaveData.deleteKey(objectCountTag(typeof(Armor)));
    }
    public static void clearConsumables() {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Consumable))) + 10; i++) {
            SaveData.deleteKey(objectTag(i, typeof(Consumable)));
        }
        SaveData.deleteKey(objectCountTag(typeof(Consumable)));
    }
    public static void clearItems() {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Item))) + 10; i++) {
            SaveData.deleteKey(objectTag(i, typeof(Item)));
        }
        SaveData.deleteKey(objectCountTag(typeof(Item)));
    }
    public static void clearCoins() {
        SaveData.deleteKey(coinCount);
    }

    public static void addWeapon(Weapon w) {
        if(w == null || w.isEmpty())
            return;
        int index = getWeaponCount();

        var data = JsonUtility.ToJson(w);
        SaveData.setString(objectTag(index, typeof(Weapon)), data);
        SaveData.setInt(objectCountTag(typeof(Weapon)), index + 1);
    }
    public static void addArmor(Armor a) {
        if(a == null || a.isEmpty())
            return;
        int index = getArmorCount();

        var data = JsonUtility.ToJson(a);
        SaveData.setString(objectTag(index, typeof(Armor)), data);
        SaveData.setInt(objectCountTag(typeof(Armor)), index + 1);
    }
    public static void addConsumable(Consumable c) {
        if(c == null || c.isEmpty())
            return;
        int index = getConsumabeCount();

        var data = JsonUtility.ToJson(c);
        SaveData.setString(objectTag(index, typeof(Consumable)), data);
        SaveData.setInt(objectCountTag(typeof(Consumable)), index + 1);
    }
    public static void addItem(Item it) {
        if(it == null || it.isEmpty())
            return;
        int index = getItemCount();

        var data = JsonUtility.ToJson(it);
        SaveData.setString(objectTag(index, typeof(Item)), data);
        SaveData.setInt(objectCountTag(typeof(Item)), index + 1);
    }
    public static void addCoins(int count) {
        int temp = SaveData.getInt(coinCount);
        SaveData.setInt(coinCount, temp + count);
    }

    public static void removeWeapon(Weapon w) {
        if(w == null || w.isEmpty())
            return;
        List<Weapon> temp = new List<Weapon>();
        for(int i = 0; i < getWeaponCount(); i++) {
            Weapon invWeapon = getWeapon(i);
            if(invWeapon != null && !invWeapon.isEmpty() && !invWeapon.isEqualTo(w))
                temp.Add(invWeapon);
        }

        clearWeapons();
        foreach(var i in temp)
            addWeapon(i);
    }
    public static void removeArmor(Armor a) {
        if(a == null || a.isEmpty())
            return;
        List<Armor> temp = new List<Armor>();
        for(int i = 0; i < getArmorCount(); i++) {
            Armor invArmor = getArmor(i);
            if(invArmor != null && !invArmor.isEmpty() && !invArmor.isEqualTo(a))
                temp.Add(invArmor);
        }

        clearArmor();
        foreach(var i in temp)
            addArmor(i);
    }
    public static void removeConsumable(Consumable c) {
        if(c == null || c.isEmpty())
            return;
        List<Consumable> temp = new List<Consumable>();
        for(int i = 0; i < getConsumabeCount(); i++) {
            Consumable invCons = getConsumable(i);
            if(invCons != null && !invCons.isEmpty() && !invCons.isEqualTo(c))
                temp.Add(invCons);
        }

        clearConsumables();
        foreach(var i in temp)
            addConsumable(i);
    }
    public static void removeItem(Item it) {
        if(it == null || it.isEmpty())
            return;
        List<Item> temp = new List<Item>();
        for(int i = 0; i < getItemCount(); i++) {
            Item invItem = getItem(i);
            if(invItem != null && !invItem.isEmpty() && !invItem.isEqualTo(it))
                temp.Add(invItem);
        }

        clearItems();
        foreach(var i in temp)
            addItem(i);
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

    public static int getWeaponCount() {
        return SaveData.getInt(objectCountTag(typeof(Weapon)));
    }
    public static int getArmorCount() {
        return SaveData.getInt(objectCountTag(typeof(Armor)));
    }
    public static int getConsumabeCount() {
        return SaveData.getInt(objectCountTag(typeof(Consumable)));
    }
    public static int getItemCount() {
        return SaveData.getInt(objectCountTag(typeof(Item)));
    }

    public static int getCoinCount() {
        return SaveData.getInt(coinCount);
    }

    public static Weapon getWeapon(int i) {
        var data = SaveData.getString(objectTag(i, typeof(Weapon)));
        if(string.IsNullOrEmpty(data))
            return null;
        return JsonUtility.FromJson<Weapon>(data);
    }
    public static Armor getArmor(int i) {
        var data = SaveData.getString(objectTag(i, typeof(Armor)));
        if(string.IsNullOrEmpty(data))
            return null;
        return JsonUtility.FromJson<Armor>(data);
    }
    public static Consumable getConsumable(int i) {
        var data = SaveData.getString(objectTag(i, typeof(Consumable)));
        if(string.IsNullOrEmpty(data))
            return null;
        return JsonUtility.FromJson<Consumable>(data);
    }
    public static Consumable getUniqueConsumable(int ind) {
        int uniqueIndex = 0;
        List<Consumable> seen = new List<Consumable>();
        for(int i = 0; i < getConsumabeCount(); i++) {
            bool isUnique = true;
            foreach(var s in seen) {
                if(s.isTheSameTypeAs(getConsumable(i))) {
                    isUnique = false;
                    break;
                }
            }
            if(!isUnique)
                continue;
            seen.Add(getConsumable(i));

            if(ind == uniqueIndex)
                return getConsumable(i);

            uniqueIndex++;
        }
        return null;
    }
    public static Item getItem(int i) {
        var data = SaveData.getString(objectTag(i, typeof(Item)));
        if(string.IsNullOrEmpty(data))
            return null;
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

    public static int getUniqueConsumableCount() {
        int count = 0;
        for(int i = 0; i < getConsumabeCount(); i++) {
            if(getUniqueConsumable(i) != null) {
                count++;
            }
        }
        return count;
    }
    public static int getConsumableTypeCount(Consumable con) {
        int count = 0;
        for(int i = 0; i < getConsumabeCount(); i++) {
            if(getConsumable(i).isTheSameTypeAs(con))
                count++;
        }
        return count;
    }
}
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
        if(type == typeof(Usable))
            return "Inventory Usable Count";
        if(type == typeof(Unusable))
            return "Inventory Unusable Count";
        if(type == typeof(Item))
            return "Inventory Item Count";
        return string.Empty;
    }
    static string objectTag(int index, System.Type type) {
        if(type == typeof(Weapon))
            return "Inventory Weapon" + index.ToString();
        if(type == typeof(Armor))
            return "Inventory Armor" + index.ToString();
        if(type == typeof(Usable))
            return "Inventory Usable" + index.ToString();
        if(type == typeof(Unusable))
            return "Inventory Unusable" + index.ToString();
        if(type == typeof(Item))
            return "Inventory Item" + index.ToString();
        return string.Empty;
    }


    public static void createDefaultInventory() {

    }

    public static void clearInventory(bool clearInstanceQueue) {
        clearWeapons();
        clearArmor();
        clearUsables();
        clearUnusables();
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
    public static void clearUsables() {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Usable))) + 10; i++) {
            SaveData.deleteKey(objectTag(i, typeof(Usable)));
        }
        SaveData.deleteKey(objectCountTag(typeof(Usable)));
    }
    public static void clearUnusables() {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Unusable))) + 10; i++) {
            SaveData.deleteKey(objectTag(i, typeof(Unusable)));
        }
        SaveData.deleteKey(objectCountTag(typeof(Unusable)));
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

    public static void addCollectable(Collectable c) {
        if(c == null || c.isEmpty())
            return;
        if(c.type == Collectable.collectableType.weapon)
            addWeapon((Weapon)c);
        else if(c.type == Collectable.collectableType.armor)
            addArmor((Armor)c);
        else if(c.type == Collectable.collectableType.item)
            addItem((Item)c);
        else if(c.type == Collectable.collectableType.usable)
            addUsable((Usable)c);
        else if(c.type == Collectable.collectableType.unusable)
            addUnusable((Unusable)c);
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
    public static void addUsable(Usable c) {
        if(c == null || c.isEmpty())
            return;
        int index = getUsableCount();

        var data = JsonUtility.ToJson(c);
        SaveData.setString(objectTag(index, typeof(Usable)), data);
        SaveData.setInt(objectCountTag(typeof(Usable)), index + 1);
    }
    public static void addUnusable(Unusable c) {
        if(c == null || c.isEmpty())
            return;
        int index = getUnusableCount();

        var data = JsonUtility.ToJson(c);
        SaveData.setString(objectTag(index, typeof(Unusable)), data);
        SaveData.setInt(objectCountTag(typeof(Unusable)), index + 1);
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

    public static void removeCollectable(Collectable c) {
        switch(c.type) {
            case Collectable.collectableType.weapon:
                removeWeapon((Weapon)c);
                return;
            case Collectable.collectableType.armor:
                removeArmor((Armor)c);
                return;
            case Collectable.collectableType.item:
                removeItem((Item)c);
                return;
            case Collectable.collectableType.usable:
                removeUsable((Usable)c);
                return;
            case Collectable.collectableType.unusable:
                removeUnusable((Unusable)c);
                return;
        }
    }
    public static void removeWeapon(Weapon w) {
        if(w == null || w.isEmpty())
            return;
        List<Weapon> temp = new List<Weapon>();
        for(int i = 0; i < getWeaponCount(); i++) {
            Weapon invWeapon = getWeapon(i);
            if(invWeapon != null && !invWeapon.isEmpty() && !invWeapon.isTheSameInstanceAs(w))
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
            if(invArmor != null && !invArmor.isEmpty() && !invArmor.isTheSameInstanceAs(a))
                temp.Add(invArmor);
        }
        clearArmor();
        foreach(var i in temp)
            addArmor(i);
    }
    public static void removeUsable(Usable c) {
        if(c == null || c.isEmpty())
            return;
        List<Usable> temp = new List<Usable>();
        for(int i = 0; i < getUsableCount(); i++) {
            Usable invCons = getUsable(i);
            if(invCons != null && !invCons.isEmpty() && !invCons.isTheSameInstanceAs(c))
                temp.Add(invCons);
        }

        clearUsables();
        foreach(var i in temp)
            addUsable(i);
    }
    public static void removeUnusable(Unusable c) {
        if(c == null || c.isEmpty())
            return;
        List<Unusable> temp = new List<Unusable>();
        for(int i = 0; i < getUnusableCount(); i++) {
            Unusable invCons = getUnusable(i);
            if(invCons != null && !invCons.isEmpty() && !invCons.isTheSameInstanceAs(c))
                temp.Add(invCons);
        }

        clearUnusables();
        foreach(var i in temp)
            addUnusable(i);
    }
    public static void removeItem(Item it) {
        if(it == null || it.isEmpty())
            return;
        List<Item> temp = new List<Item>();
        for(int i = 0; i < getItemCount(); i++) {
            Item invItem = getItem(i);
            if(invItem != null && !invItem.isEmpty() && !invItem.isTheSameInstanceAs(it))
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
    public static void removeUsable(int index) {
        removeUsable(getUsable(index));
    }
    public static void removeUnusable(int index) {
        removeUnusable(getUnusable(index));
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
    public static void overrideUsable(int index, Usable c) {
        var data = JsonUtility.ToJson(c);
        SaveData.setString(objectTag(index, typeof(Usable)), data);
    }
    public static void overrideUnusable(int index, Unusable c) {
        var data = JsonUtility.ToJson(c);
        SaveData.setString(objectTag(index, typeof(Unusable)), data);
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
    public static int getUsableCount() {
        return SaveData.getInt(objectCountTag(typeof(Usable)));
    }
    public static int getUnusableCount() {
        return SaveData.getInt(objectCountTag(typeof(Unusable)));
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
    public static Usable getUsable(int i) {
        var data = SaveData.getString(objectTag(i, typeof(Usable)));
        if(string.IsNullOrEmpty(data))
            return null;
        return JsonUtility.FromJson<Usable>(data);
    }
    public static Unusable getUnusable(int i) {
        var data = SaveData.getString(objectTag(i, typeof(Unusable)));
        if(string.IsNullOrEmpty(data))
            return null;
        return JsonUtility.FromJson<Unusable>(data);
    }
    public static Usable getUniqueUsable(int ind) {
        int uniqueIndex = 0;
        List<Usable> seen = new List<Usable>();
        for(int i = 0; i < getUsableCount(); i++) {
            bool isUnique = true;
            foreach(var s in seen) {
                if(s.isTheSameTypeAs(getUsable(i))) {
                    isUnique = false;
                    break;
                }
            }
            if(!isUnique)
                continue;
            seen.Add(getUsable(i));

            if(ind == uniqueIndex)
                return getUsable(i);

            uniqueIndex++;
        }
        return null;
    }
    public static Unusable getUniqueUnusable(int ind) {
        int uniqueIndex = 0;
        List<Unusable> seen = new List<Unusable>();
        for(int i = 0; i < getUnusableCount(); i++) {
            bool isUnique = true;
            foreach(var s in seen) {
                if(s.isTheSameTypeAs(getUnusable(i))) {
                    isUnique = false;
                    break;
                }
            }
            if(!isUnique)
                continue;
            seen.Add(getUnusable(i));

            if(ind == uniqueIndex)
                return getUnusable(i);

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

    public static List<Weapon> getWeapons() {
        List<Weapon> temp = new List<Weapon>();
        for(int i = 0; i < getWeaponCount(); i++)
            temp.Add(getWeapon(i));
        return temp;
    }
    public static List<Armor> getArmors() {
        List<Armor> temp = new List<Armor>();
        for(int i = 0; i < getArmorCount(); i++)
            temp.Add(getArmor(i));
        return temp;
    }
    public static List<Item> getItems() {
        List<Item> temp = new List<Item>();
        for(int i = 0; i < getItemCount(); i++)
            temp.Add(getItem(i));
        return temp;
    }
    public static List<Usable> getUsables() {
        List<Usable> temp = new List<Usable>();
        for(int i = 0; i < getUsableCount(); i++)
            temp.Add(getUsable(i));
        return temp;
    }
    public static List<Unusable> getUnusables() {
        List<Unusable> temp = new List<Unusable>();
        for(int i = 0; i < getUnusableCount(); i++)
            temp.Add(getUnusable(i));
        return temp;
    }
    public static List<Usable> getUniqueUsables() {
        List<Usable> temp = new List<Usable>();
        for(int i = 0; i < getUsableCount(); i++) {
            var u = getUsable(i);

            bool add = true;
            foreach(var t in temp) {
                if(t.isTheSameTypeAs(u)) {
                    add = false;
                    break;
                }
            }

            if(add)
                temp.Add(getUsable(i));
        }
        return temp;
    }
    public static List<Unusable> getUniqueUnusables() {
        List<Unusable> temp = new List<Unusable>();
        for(int i = 0; i < getUnusableCount(); i++) {
            var u = getUnusable(i);

            bool add = true;
            foreach(var t in temp) {
                if(t.isTheSameTypeAs(u)) {
                    add = false;
                    break;
                }
            }

            if(add)
                temp.Add(getUnusable(i));
        }
        return temp;
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
    public static int getUsableIndex(Usable c) {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Usable))); i++) {
            var temp = getUsable(i);

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

    public static int getUniqueUsableCount() {
        int count = 0;
        for(int i = 0; i < getUsableCount(); i++) {
            if(getUniqueUsable(i) != null) {
                count++;
            }
        }
        return count;
    }
    public static Usable getFirstMatchingUsable(Usable u) {
        for(int i = 0; i < getUsableCount(); i++) {
            if(getUsable(i).isTheSameTypeAs(u))
                return getUsable(i);
        }
        return null;
    }
    public static int getNumberOfMatchingUsables(Usable con) {
        int count = 0;
        for(int i = 0; i < getUsableCount(); i++) {
            if(getUsable(i).isTheSameTypeAs(con))
                count++;
        }
        return count;
    }
    public static int getUniqueUnusableCount() {
        int count = 0;
        for(int i = 0; i < getUnusableCount(); i++) {
            if(getUniqueUnusable(i) != null) {
                count++;
            }
        }
        return count;
    }
    public static Unusable getFirstMatchingUnusable(Unusable u) {
        for(int i = 0; i < getUnusableCount(); i++) {
            if(getUnusable(i).isTheSameTypeAs(u))
                return getUnusable(i);
        }
        return null;
    }
    public static int getNumberOfMatchingUnusables(Unusable con) {
        int count = 0;
        for(int i = 0; i < getUnusableCount(); i++) {
            if(getUnusable(i).isTheSameTypeAs(con))
                count++;
        }
        return count;
    }
}
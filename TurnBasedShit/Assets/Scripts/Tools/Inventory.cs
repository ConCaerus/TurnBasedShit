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
    public static void removeWeapon(Weapon we) {
        if(we == null || we.isEmpty())
            return;

        int startingIndex = getWeaponIndex(we);
        for(int i = startingIndex; i < getWeaponCount(); i++) {
            overrideWeapon(i, getWeapon(i + 1));
        }

        SaveData.setInt(objectCountTag(typeof(Weapon)), getWeaponCount() - 1);
    }
    public static void removeArmor(Armor ar) {
        if(ar == null || ar.isEmpty())
            return;

        int startingIndex = getArmorIndex(ar);
        for(int i = startingIndex; i < getArmorCount(); i++) {
            overrideArmor(i, getArmor(i + 1));
        }

        SaveData.setInt(objectCountTag(typeof(Armor)), getArmorCount() - 1);
    }
    public static void removeUsable(Usable c) {
        if(c == null || c.isEmpty())
            return;

        int startingIndex = getUsableIndex(c);
        for(int i = startingIndex; i < getUsableCount(); i++) {
            overrideUsable(i, getUsable(i + 1));
        }

        SaveData.setInt(objectCountTag(typeof(Usable)), getUsableCount() - 1);
    }
    public static void removeUnusable(Unusable c) {
        if(c == null || c.isEmpty())
            return;

        int startingIndex = getUnusableIndex(c);
        for(int i = startingIndex; i < getUnusableCount(); i++) {
            overrideUnusable(i, getUnusable(i + 1));
        }

        SaveData.setInt(objectCountTag(typeof(Unusable)), getUnusableCount() - 1);
    }
    public static void removeItem(Item it) {
        if(it == null || it.isEmpty())
            return;

        int startingIndex = getItemIndex(it);
        for(int i = startingIndex; i < getItemCount(); i++) {
            overrideItem(i, getItem(i + 1));
        }

        SaveData.setInt(objectCountTag(typeof(Item)), getItemCount() - 1);
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

    public static void overrideCollectable(int index, Collectable c) {
        switch(c.type) {
            case Collectable.collectableType.weapon:
                overrideWeapon(index, (Weapon)c);
                return;
            case Collectable.collectableType.armor:
                overrideArmor(index, (Armor)c);
                return;
            case Collectable.collectableType.item:
                overrideItem(index, (Item)c);
                return;
            case Collectable.collectableType.usable:
                overrideUsable(index, (Usable)c);
                return;
            case Collectable.collectableType.unusable:
                overrideUnusable(index, (Unusable)c);
                return;
        }
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

    public static Usable getNextFood() {
        Usable food = null;
        for(int i = getUsableCount() - 1; i >= 0; i--) {
            if(getUsable(i).foodBiteCount > 0) {
                food = getUsable(i);
                break;
            }
        }

        return food;
    }
    public static Usable eatFood() {
        var food = getNextFood();
        if(food == null || food.isEmpty()) {
            return null;
        }

        int index = getUsableIndex(food);
        food.foodBiteCount--;
        if(food.foodBiteCount == 0)
            removeUsable(food);
        else {
            overrideCollectable(index, food);
        }
        return food;
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

    public static int getCollectableIndex(Collectable c) {
        switch(c.type) {
            case Collectable.collectableType.weapon:
                return getWeaponIndex((Weapon)c);
            case Collectable.collectableType.armor:
                return getArmorIndex((Armor)c);
            case Collectable.collectableType.item:
                return getItemIndex((Item)c);
            case Collectable.collectableType.usable:
                return getUsableIndex((Usable)c);
            case Collectable.collectableType.unusable:
                return getUnusableIndex((Unusable)c);
        }
        return -1;
    }
    public static int getWeaponIndex(Weapon w) {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Weapon))); i++) {
            var temp = getWeapon(i);

            if(temp.isTheSameInstanceAs(w))
                return i;
        }
        return -1;
    }
    public static int getArmorIndex(Armor a) {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Armor))); i++) {
            var temp = getArmor(i);

            if(temp.isTheSameInstanceAs(a))
                return i;
        }
        return -1;
    }
    public static int getUsableIndex(Usable c) {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Usable))); i++) {
            var temp = getUsable(i);

            if(temp.isTheSameInstanceAs(c))
                return i;
        }
        return -1;
    }
    public static int getUnusableIndex(Unusable c) {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Unusable))); i++) {
            var temp = getUnusable(i);

            if(temp.isTheSameInstanceAs(c))
                return i;
        }
        return -1;
    }
    public static int getItemIndex(Item it) {
        for(int i = 0; i < SaveData.getInt(objectCountTag(typeof(Item))); i++) {
            var temp = getItem(i);

            if(temp.isTheSameInstanceAs(it))
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//  Actual Inventory Script
public static class Inventory {
    const string coinCount = "Coin Count";
    const string maxCapacity = "Inventory Max Capacity";
    const string holderTag = "InventoryHolderTag";

    public static ObjectHolder getHolder() {
        var data = SaveData.getString(holderTag);
        return JsonUtility.FromJson<ObjectHolder>(data);
    }
    static void saveHolder(ObjectHolder holder) {
        var data = JsonUtility.ToJson(holder);
        SaveData.setString(holderTag, data);
    }


    public static void clear(bool clearInstanceQueue) {
        saveHolder(new ObjectHolder());
        clearCoins();
        resetCapacity();

        if(clearInstanceQueue)
            GameInfo.clearInventoryInstanceIDQueue();
    }

    //  use this function first because if the inventory is full, it will show all of the collectables trying to be added
    public static void addCollectables(List<Collectable> cols, PresetLibrary lib, FullInventoryCanvas fic) {
        if(getHolder() == null)
            saveHolder(new ObjectHolder());
        var holder = getHolder();

        //  not enough space
        if(holder.getCollectables().Count + cols.Count > getMaxCapacity()) {
            fic.show(cols);
            return;
        }

        foreach(var i in cols) {
            if(i != null && !i.isEmpty()) {
                holder.addObject<Collectable>(i);
                Collection.addCollectable(i, lib);
            }
        }
        saveHolder(holder);
    }
    public static void addSingleCollectable(Collectable col, PresetLibrary lib, FullInventoryCanvas fic) {
        addCollectables(new List<Collectable> { col }, lib, fic);
    }
    public static void overrideCollectable(int index, Collectable col) {
        if(col == null || index == -1)
            return;
        var holder = getHolder();
        holder.overrideObject<Collectable>(index, col);
        saveHolder(holder);
    }
    public static void removeCollectable(Collectable col) {
        if(col == null)
            return;
        var holder = getHolder();
        holder.removeCollectable(col);
        saveHolder(holder);
    }

    public static bool hasCollectable(Collectable col) {
        foreach(var i in getHolder().getCollectables()) {
            if(col.isTheSameInstanceAs(i))
                return true;
        }
        return false;
    }
    public static bool hasCollectableType(Collectable col) {
        foreach(var i in getHolder().getCollectables()) {
            if(col.isTheSameTypeAs(i))
                return true;
        }
        return false;
    }

    public static void resetCapacity() {
        SaveData.setInt(maxCapacity, 25);
    }
    public static void addCapacity(int amount) {
        SaveData.setInt(maxCapacity, SaveData.getInt(maxCapacity) + amount);
    }
    public static int getMaxCapacity() {
        return SaveData.getInt(maxCapacity);
    }
    public static int getFilledCapacity() {
        var holder = getHolder();
        int count = 0;
        count += holder.getObjectCount<Weapon>();
        count += holder.getObjectCount<Armor>();
        count += holder.getObjectCount<Item>();
        count += getUniqueUsables().Count;
        count += getUniqueUnusables().Count;
        return count;
    }
    public static bool hasAvailableSpace() {
        return getMaxCapacity() - getFilledCapacity() > 0;
    }

    public static void clearCoins() {
        SaveData.deleteKey(coinCount);
    }
    public static void addCoins(int count, CoinCount counter = null, bool smoothlyUpdateCoint = false) {
        int temp = SaveData.getInt(coinCount);
        SaveData.setInt(coinCount, temp + count);

        if(counter != null) {
            counter.updateCount(smoothlyUpdateCoint);
            if(count != 0)
                counter.createCoinChangeText(count);
        }
    }
    public static int getCoinCount() {
        return SaveData.getInt(coinCount);
    }
    /*
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
    }*/

    static int getNextFoodIndex() {
        for(int i = getHolder().getObjectCount<Usable>() - 1; i >= 0; i--) {
            if(getHolder().getObject<Usable>(i).foodBiteCount > 0) {
                return i;
            }
        }

        return -1;
    }
    public static Usable eatFood() {
        var ind = getNextFoodIndex();
        if(ind == -1) {
            return null;
        }

        var food = getHolder().getObject<Usable>(ind);

        food.foodBiteCount--;
        if(food.foodBiteCount == 0)
            removeCollectable(food);
        else {
            overrideCollectable(ind, food);
        }
        return food;
    }
    public static List<Usable> getUniqueUsables() {
        List<Usable> temp = new List<Usable>();
        for(int i = 0; i < getHolder().getObjectCount<Usable>(); i++) {
            var u = getHolder().getObject<Usable>(i);

            bool add = true;
            foreach(var t in temp) {
                if(t.isTheSameTypeAs(u)) {
                    add = false;
                    break;
                }
            }

            if(add)
                temp.Add(getHolder().getObject<Usable>(i));
        }
        return temp;
    }
    public static List<Unusable> getUniqueUnusables() {
        List<Unusable> temp = new List<Unusable>();
        for(int i = 0; i < getHolder().getObjectCount<Unusable>(); i++) {
            var u = getHolder().getObject<Unusable>(i);

            bool add = true;
            foreach(var t in temp) {
                if(t.isTheSameTypeAs(u)) {
                    add = false;
                    break;
                }
            }

            if(add)
                temp.Add(getHolder().getObject<Unusable>(i));
        }
        return temp;
    }

    public static Usable getFirstMatchingUsable(Usable u) {
        for(int i = 0; i < getHolder().getObjectCount<Usable>(); i++) {
            if(getHolder().getObject<Usable>(i).isTheSameTypeAs(u))
                return getHolder().getObject<Usable>(i);
        }
        return null;
    }
    public static int getNumberOfMatchingUsables(Usable con) {
        int count = 0;
        for(int i = 0; i < getHolder().getObjectCount<Usable>(); i++) {
            if(getHolder().getObject<Usable>(i).isTheSameTypeAs(con))
                count++;
        }
        return count;
    }
    public static Unusable getFirstMatchingUnusable(Unusable u) {
        for(int i = 0; i < getHolder().getObjectCount<Unusable>(); i++) {
            if(getHolder().getObject<Unusable>(i).isTheSameTypeAs(u))
                return getHolder().getObject<Unusable>(i);
        }
        return null;
    }
    public static int getNumberOfMatchingUnusables(Unusable con) {
        int count = 0;
        for(int i = 0; i < getHolder().getObjectCount<Unusable>(); i++) {
            if(getHolder().getObject<Unusable>(i).isTheSameTypeAs(con))
                count++;
        }
        return count;
    }
}
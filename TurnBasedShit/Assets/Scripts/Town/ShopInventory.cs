using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class ShopInventory {
    static string holderTag(int townIndex) { return "InventoryHolderTag: " + townIndex.ToString(); }

    public static ObjectHolder getHolder(int townInstance) {
        var data = SaveData.getString(holderTag(townInstance));
        return JsonUtility.FromJson<ObjectHolder>(data);
    }
    static void saveHolder(int townInstance, ObjectHolder holder) {
        var data = JsonUtility.ToJson(holder);
        SaveData.setString(holderTag(townInstance), data);
    }


    public static void clear(int townInstance) {
        saveHolder(townInstance, new ObjectHolder());
    }


    public static void addCollectable(int townIndex, Collectable col) {
        if(col == null || col.isEmpty())
            return;

        var holder = getHolder(townIndex);
        holder.addObject<Collectable>(col);
        saveHolder(townIndex, holder);
    }
    public static void addUnit(int townIndex, UnitStats stats) {
        if(stats == null)
            return;

        var holder = getHolder(townIndex);
        holder.addObject<UnitStats>(stats);
        saveHolder(townIndex, holder);
    }
    public static void overrideCollectable(int townIndex, int index, Collectable col) {
        if(col == null || index == -1)
            return;
        var holder = getHolder(townIndex);
        holder.overrideObject<Collectable>(index, col);
        saveHolder(townIndex, holder);
    }
    public static void removeCollectable(int townIndex, Collectable col) {
        if(col == null)
            return;
        var holder = getHolder(townIndex);
        holder.removeCollectable(col);
        saveHolder(townIndex, holder);
    }
    public static void removeUnit(int townIndex, int index) {
        if(index < 0)
            return;
        var holder = getHolder(townIndex);
        holder.removeObject<UnitStats>(index);
        saveHolder(townIndex, holder);
    }



    public static void populateShop(int townIndex, GameInfo.region lvl, PresetLibrary library) {
        clear(townIndex);

        int weaponCount = Random.Range(1, 11);
        for(int i = 0; i < weaponCount; i++) {
            addCollectable(townIndex, library.getRandomWeapon(lvl));
        }

        int armorCount = Random.Range(1, 11);
        for(int i = 0; i < armorCount; i++) {
            addCollectable(townIndex, library.getRandomArmor(lvl));
        }

        int consumableCount = Random.Range(1, 11);
        for(int i = 0; i < consumableCount; i++) {
            addCollectable(townIndex, library.getRandomUsable(lvl));
        }

        int itemCount = Random.Range(1, 11);
        for(int i = 0; i < itemCount; i++) {
            addCollectable(townIndex, library.getRandomItem(lvl));
        }

        int slaveCount = Random.Range(0, 3);
        for(int i = 0; i < slaveCount; i++) {
            addUnit(townIndex, library.createRandomPlayerUnitStats(true));
        }
    }

    /*
    static string objectTag(int townIndex, int index, System.Type type) {
        if(type == typeof(Weapon))
            return "Town:" + townIndex.ToString() + " Shop Weapon:" + index.ToString();
        else if(type == typeof(Armor))
            return "Town:" + townIndex.ToString() + " Shop Armor:" + index.ToString();
        else if(type == typeof(Usable))
            return "Town:" + townIndex.ToString() + " Shop Consumable:" + index.ToString();
        else if(type == typeof(Item))
            return "Town:" + townIndex.ToString() + " Shop Item:" + index.ToString();
        else if(type == typeof(UnitStats))
            return "Town:" + townIndex.ToString() + " Shop Slave:" + index.ToString();
        return string.Empty;
    }

    static string objectCountTag(int townIndex, System.Type type) {
        if(type == typeof(Weapon))
            return "Town:" + townIndex.ToString() + " Weapon Count";
        else if(type == typeof(Armor))
            return "Town:" + townIndex.ToString() + " Armor Count";
        else if(type == typeof(Usable))
            return "Town:" + townIndex.ToString() + " Usable Count";
        else if(type == typeof(Unusable))
            return "Town:" + townIndex.ToString() + " Unusable Count";
        else if(type == typeof(Item))
            return "Town:" + townIndex.ToString() + " Item Count";
        else if(type == typeof(UnitStats))
            return "Town:" + townIndex.ToString() + " Slave Count";
        return string.Empty;
    }


    public static void clearShop(int townIndex) {
        clearWeapons(townIndex);
        clearArmor(townIndex);
        clearUsables(townIndex);
        clearUnusables(townIndex);
        clearItems(townIndex);
        clearSlaves(townIndex);
    }
    public static void clearWeapons(int townIndex) {
        for(int i = 0; i < SaveData.getInt(objectCountTag(townIndex, typeof(Weapon))); i++) {
            SaveData.deleteKey(objectTag(townIndex, i, typeof(Weapon)));
        }
        SaveData.deleteKey(objectCountTag(townIndex, typeof(Weapon)));
    }
    public static void clearArmor(int townIndex) {
        for(int i = 0; i < SaveData.getInt(objectCountTag(townIndex, typeof(Armor))); i++) {
            SaveData.deleteKey(objectTag(townIndex, i, typeof(Armor)));
        }
        SaveData.deleteKey(objectCountTag(townIndex, typeof(Armor)));
    }
    public static void clearUsables(int townIndex) {
        for(int i = 0; i < SaveData.getInt(objectCountTag(townIndex, typeof(Usable))); i++) {
            SaveData.deleteKey(objectTag(townIndex, i, typeof(Usable)));
        }
        SaveData.deleteKey(objectCountTag(townIndex, typeof(Usable)));
    }
    public static void clearUnusables(int townIndex) {
        for(int i = 0; i < SaveData.getInt(objectCountTag(townIndex, typeof(Unusable))); i++) {
            SaveData.deleteKey(objectTag(townIndex, i, typeof(Unusable)));
        }
        SaveData.deleteKey(objectCountTag(townIndex, typeof(Unusable)));
    }
    public static void clearItems(int townIndex) {
        for(int i = 0; i < SaveData.getInt(objectCountTag(townIndex, typeof(Item))); i++) {
            SaveData.deleteKey(objectTag(townIndex, i, typeof(Item)));
        }
        SaveData.deleteKey(objectCountTag(townIndex, typeof(Item)));
    }
    public static void clearSlaves(int townIndex) {
        for(int i = 0; i < SaveData.getInt(objectCountTag(townIndex, typeof(UnitStats))); i++) {
            SaveData.deleteKey(objectTag(townIndex, i, typeof(UnitStats)));
        }
        SaveData.deleteKey(objectCountTag(townIndex, typeof(UnitStats)));
    }

    public static void populateShop(int townIndex, GameInfo.region lvl, PresetLibrary library) {
        clearShop(townIndex);

        int weaponCount = Random.Range(1, 11);
        for(int i = 0; i < weaponCount; i++) {
            addWeapon(townIndex, library.getRandomWeapon(lvl));
        }

        int armorCount = Random.Range(1, 11);
        for(int i = 0; i < armorCount; i++) {
            addArmor(townIndex, library.getRandomArmor(lvl));
        }

        int consumableCount = Random.Range(1, 11);
        for(int i = 0; i < consumableCount; i++) {
            addUsable(townIndex, library.getRandomUsable(lvl));
        }

        int itemCount = Random.Range(1, 11);
        for(int i = 0; i < itemCount; i++) {
            addItem(townIndex, library.getRandomItem(lvl));
        }

        int slaveCount = Random.Range(0, 3);
        for(int i = 0; i < slaveCount; i++) {
            addSlave(townIndex, library.createRandomPlayerUnitStats(true));
        }
    }

    public static void addCollectable(int townIndex, Collectable c) {
        switch(c.type) {
            case Collectable.collectableType.weapon:
                addWeapon(townIndex, (Weapon)c);
                return;
            case Collectable.collectableType.armor:
                addArmor(townIndex, (Armor)c);
                return;
            case Collectable.collectableType.item:
                addItem(townIndex, (Item)c);
                return;
            case Collectable.collectableType.usable:
                addUsable(townIndex, (Usable)c);
                return;
            case Collectable.collectableType.unusable:
                addUnusable(townIndex, (Unusable)c);
                return;
        }
    }
    public static void addWeapon(int townIndex, Weapon w) {
        var index = SaveData.getInt(objectCountTag(townIndex, typeof(Weapon)));
        var data = JsonUtility.ToJson(w);
        SaveData.setString(objectTag(townIndex, index, typeof(Weapon)), data);
        SaveData.setInt(objectCountTag(townIndex, typeof(Weapon)), index + 1);
    }
    public static void addArmor(int townIndex, Armor a) {
        var index = SaveData.getInt(objectCountTag(townIndex, typeof(Armor)));
        var data = JsonUtility.ToJson(a);
        SaveData.setString(objectTag(townIndex, index, typeof(Armor)), data);
        SaveData.setInt(objectCountTag(townIndex, typeof(Armor)), index + 1);
    }
    public static void addUsable(int townIndex, Usable c) {
        var index = SaveData.getInt(objectCountTag(townIndex, typeof(Usable)));
        var data = JsonUtility.ToJson(c);
        SaveData.setString(objectTag(townIndex, index, typeof(Usable)), data);
        SaveData.setInt(objectCountTag(townIndex, typeof(Usable)), index + 1);
    }
    public static void addUnusable(int townIndex, Unusable c) {
        var index = SaveData.getInt(objectCountTag(townIndex, typeof(Unusable)));
        var data = JsonUtility.ToJson(c);
        SaveData.setString(objectTag(townIndex, index, typeof(Unusable)), data);
        SaveData.setInt(objectCountTag(townIndex, typeof(Unusable)), index + 1);
    }
    public static void addItem(int townIndex, Item i) {
        var index = SaveData.getInt(objectCountTag(townIndex, typeof(Item)));
        var data = JsonUtility.ToJson(i);
        SaveData.setString(objectTag(townIndex, index, typeof(Item)), data);
        SaveData.setInt(objectCountTag(townIndex, typeof(Item)), index + 1);
    }
    public static void addSlave(int townIndex, UnitStats stats) {
        var index = SaveData.getInt(objectCountTag(townIndex, typeof(UnitStats)));
        var data = JsonUtility.ToJson(stats);
        SaveData.setString(objectTag(townIndex, index, typeof(UnitStats)), data);
        SaveData.setInt(objectCountTag(townIndex, typeof(UnitStats)), index + 1);
    }
    
    public static void removeCollectable(int townIndex, Collectable c) {
        switch(c.type) {
            case Collectable.collectableType.weapon:
                removeWeapon(townIndex, (Weapon)c);
                return;
            case Collectable.collectableType.armor:
                removeArmor(townIndex, (Armor)c);
                return;
            case Collectable.collectableType.item:
                removeItem(townIndex, (Item)c);
                return;
            case Collectable.collectableType.usable:
                removeUsable(townIndex, (Usable)c);
                return;
            case Collectable.collectableType.unusable:
                removeUnusable(townIndex, (Unusable)c);
                return;
        }
    }
    public static void removeWeapon(int townIndex, Weapon w) {
        if(w == null || w.isEmpty())
            return;
        List<Weapon> temp = new List<Weapon>();
        for(int i = 0; i < getTypeCount(townIndex, typeof(Weapon)); i++) {
            Weapon invWeapon = getWeapon(townIndex, i);
            if(invWeapon != null && !invWeapon.isEmpty() && !invWeapon.isTheSameInstanceAs(w))
                temp.Add(invWeapon);
        }

        clearWeapons(townIndex);
        foreach(var i in temp)
            addWeapon(townIndex, i);
    }
    public static void removeArmor(int townIndex, Armor a) {
        if(a == null || a.isEmpty())
            return;
        List<Armor> temp = new List<Armor>();
        for(int i = 0; i < getTypeCount(townIndex, typeof(Armor)); i++) {
            Armor invArmor = getArmor(townIndex, i);
            if(invArmor != null && !invArmor.isEmpty() && !invArmor.isTheSameInstanceAs(a))
                temp.Add(invArmor);
        }

        clearArmor(townIndex);
        foreach(var i in temp)
            addArmor(townIndex, i);
    }
    public static void removeUsable(int townIndex, Usable c) {
        if(c == null || c.isEmpty())
            return;
        List<Usable> temp = new List<Usable>();
        for(int i = 0; i < getTypeCount(townIndex, typeof(Usable)); i++) {
            Usable invCons = getUsable(townIndex, i);
            if(invCons != null && !invCons.isEmpty() && !invCons.isTheSameInstanceAs(c))
                temp.Add(invCons);
        }

        clearUsables(townIndex);
        foreach(var i in temp)
            addUsable(townIndex, i);
    }
    public static void removeUnusable(int townIndex, Unusable c) {
        if(c == null || c.isEmpty())
            return;
        List<Unusable> temp = new List<Unusable>();
        for(int i = 0; i < getTypeCount(townIndex, typeof(Unusable)); i++) {
            Unusable invCons = getUnusable(townIndex, i);
            if(invCons != null && !invCons.isEmpty() && !invCons.isTheSameInstanceAs(c))
                temp.Add(invCons);
        }

        clearUnusables(townIndex);
        foreach(var i in temp)
            addUnusable(townIndex, i);
    }
    public static void removeItem(int townIndex, Item it) {
        if(it == null || it.isEmpty())
            return;
        List<Item> temp = new List<Item>();
        for(int i = 0; i < getTypeCount(townIndex, typeof(Item)); i++) {
            Item invItem = getItem(townIndex, i);
            if(invItem != null && !invItem.isEmpty() && !invItem.isTheSameInstanceAs(it))
                temp.Add(invItem);
        }

        clearItems(townIndex);
        foreach(var i in temp)
            addItem(townIndex, i);
    }
    public static void removeSlave(int townIndex, UnitStats stats) {
        if(stats == null || stats.isEmpty())
            return;
        List<UnitStats> temp = new List<UnitStats>();
        for(int i = 0; i < getTypeCount(townIndex, typeof(UnitStats)); i++) {
            UnitStats invStats = getSlave(townIndex, i);
            if(invStats != null && !invStats.isEmpty() && !invStats.isTheSameInstanceAs(stats))
                temp.Add(invStats);
        }

        clearSlaves(townIndex);
        foreach(var i in temp)
            addSlave(townIndex, i);
    }
    public static void removeWeapon(int townIndex, int index) {
        removeWeapon(townIndex, getWeapon(townIndex, index));
    }
    public static void removeArmor(int townIndex, int index) {
        removeArmor(townIndex, getArmor(townIndex, index));
    }
    public static void removeUsable(int townIndex, int index) {
        removeUsable(townIndex, getUsable(townIndex, index));
    }
    public static void removeUnusable(int townIndex, int index) {
        removeUnusable(townIndex, getUnusable(townIndex, index));
    }
    public static void removeItem(int townIndex, int index) {
        removeItem(townIndex, getItem(townIndex, index));
    }
    public static void removeSlave(int townIndex, int index) {
        removeSlave(townIndex, getSlave(townIndex, index));
    }

    public static void overrideWeapon(int townIndex, int index, Weapon w) {
        var data = JsonUtility.ToJson(w);
        SaveData.setString(objectTag(townIndex, index, typeof(Weapon)), data);
    }
    public static void overrideArmor(int townIndex, int index, Armor a) {
        var data = JsonUtility.ToJson(a);
        SaveData.setString(objectTag(townIndex, index, typeof(Armor)), data);
    }
    public static void overrideUsable(int townIndex, int index, Usable c) {
        var data = JsonUtility.ToJson(c);
        SaveData.setString(objectTag(townIndex, index, typeof(Usable)), data);
    }
    public static void overrideUnusable(int townIndex, int index, Unusable c) {
        var data = JsonUtility.ToJson(c);
        SaveData.setString(objectTag(townIndex, index, typeof(Unusable)), data);
    }
    public static void overrideItem(int townIndex, int index, Item i) {
        var data = JsonUtility.ToJson(i);
        SaveData.setString(objectTag(townIndex, index, typeof(Item)), data);
    }
    public static void overrideSlave(int townIndex, int index, UnitStats stats) {
        var data = JsonUtility.ToJson(stats);
        SaveData.setString(objectTag(townIndex, index, typeof(UnitStats)), data);
    }

    public static int getTypeCount(int townIndex, System.Type type) {
        if(type == typeof(Weapon))
            return SaveData.getInt(objectCountTag(townIndex, typeof(Weapon)));

        else if(type == typeof(Armor))
            return SaveData.getInt(objectCountTag(townIndex, typeof(Armor)));

        else if(type == typeof(Usable))
            return SaveData.getInt(objectCountTag(townIndex, typeof(Usable)));

        else if(type == typeof(Item))
            return SaveData.getInt(objectCountTag(townIndex, typeof(Item)));

        else if(type == typeof(UnitStats))
            return SaveData.getInt(objectCountTag(townIndex, typeof(UnitStats)));

        return -1;
    }
    public static Weapon getWeapon(int townIndex, int index) {
        var data = SaveData.getString(objectTag(townIndex, index, typeof(Weapon)));
        if(!string.IsNullOrEmpty(data))
            return JsonUtility.FromJson<Weapon>(data);
        return null;
    }
    public static Armor getArmor(int townIndex, int index) {
        var data = SaveData.getString(objectTag(townIndex, index, typeof(Armor)));
        if(!string.IsNullOrEmpty(data))
            return JsonUtility.FromJson<Armor>(data);
        return null;
    }
    public static Usable getUsable(int townIndex, int index) {
        var data = SaveData.getString(objectTag(townIndex, index, typeof(Usable)));
        if(!string.IsNullOrEmpty(data))
            return JsonUtility.FromJson<Usable>(data);
        return null;
    }
    public static Unusable getUnusable(int townIndex, int index) {
        var data = SaveData.getString(objectTag(townIndex, index, typeof(Unusable)));
        if(!string.IsNullOrEmpty(data))
            return JsonUtility.FromJson<Unusable>(data);
        return null;
    }
    public static Item getItem(int townIndex, int index) {
        var data = SaveData.getString(objectTag(townIndex, index, typeof(Item)));
        if(!string.IsNullOrEmpty(data))
            return JsonUtility.FromJson<Item>(data);
        return null;
    }
    public static UnitStats getSlave(int townIndex, int index) {
        var data = SaveData.getString(objectTag(townIndex, index, typeof(UnitStats)));
        if(!string.IsNullOrEmpty(data))
            return JsonUtility.FromJson<UnitStats>(data);
        return null;
    }*/
}

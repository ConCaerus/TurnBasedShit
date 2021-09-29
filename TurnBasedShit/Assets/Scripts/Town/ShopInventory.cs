using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class ShopInventory {
    static string objectTag(int townIndex, int index, System.Type type) {
        if(type == typeof(Weapon))
            return "Town:" + townIndex.ToString() + " Shop Weapon:" + index.ToString();
        else if(type == typeof(Armor))
            return "Town:" + townIndex.ToString() + " Shop Armor:" + index.ToString();
        else if(type == typeof(Consumable))
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
        else if(type == typeof(Consumable))
            return "Town:" + townIndex.ToString() + " Consumable Count";
        else if(type == typeof(Item))
            return "Town:" + townIndex.ToString() + " Item Count";
        else if(type == typeof(UnitStats))
            return "Town:" + townIndex.ToString() + " Slave Count";
        return string.Empty;
    }


    public static void clearShop(int townIndex) {
        clearWeapons(townIndex);
        clearArmor(townIndex);
        clearConsumables(townIndex);
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
    public static void clearConsumables(int townIndex) {
        for(int i = 0; i < SaveData.getInt(objectCountTag(townIndex, typeof(Consumable))); i++) {
            SaveData.deleteKey(objectTag(townIndex, i, typeof(Consumable)));
        }
        SaveData.deleteKey(objectCountTag(townIndex, typeof(Consumable)));
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

    public static void populateShop(int townIndex, GameInfo.diffLvl lvl, PresetLibrary library) {
        clearShop(townIndex);

        int weaponCount = Random.Range(1, 11);
        for(int i = 0; i < weaponCount; i++) {
            Weapon temp = library.getRandomWeapon((GameInfo.rarityLvl)lvl);
            temp = Randomizer.randomizeWeapon(temp, lvl);

            addWeapon(townIndex, temp);
        }

        int armorCount = Random.Range(1, 11);
        for(int i = 0; i < armorCount; i++) {
            Armor temp = library.getRandomArmor((GameInfo.rarityLvl)lvl);
            temp = Randomizer.randomizeArmor(temp, lvl);

            addArmor(townIndex, temp);
        }

        int consumableCount = Random.Range(1, 11);
        for(int i = 0; i < consumableCount; i++) {
            Consumable temp = library.getRandomConsumable((GameInfo.rarityLvl)lvl);

            addConsumable(townIndex, temp);
        }

        int itemCount = Random.Range(1, 11);
        for(int i = 0; i < itemCount; i++) {
            Item temp = library.getRandomItem((GameInfo.rarityLvl)lvl);

            addItem(townIndex, temp);
        }

        int slaveCount = Random.Range(0, 3);
        for(int i = 0; i < slaveCount; i++) {
            UnitStats temp = library.createRandomPlayerUnitStats(true);

            addSlave(townIndex, temp);
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
    public static void addConsumable(int townIndex, Consumable c) {
        var index = SaveData.getInt(objectCountTag(townIndex, typeof(Consumable)));
        var data = JsonUtility.ToJson(c);
        SaveData.setString(objectTag(townIndex, index, typeof(Consumable)), data);
        SaveData.setInt(objectCountTag(townIndex, typeof(Consumable)), index + 1);
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
    
    public static void removeWeapon(int townIndex, Weapon w) {
        var tData = JsonUtility.ToJson(w);

        bool past = false;
        for(int i = 0; i < SaveData.getInt(objectCountTag(townIndex, typeof(Weapon))); i++) {
            var data = SaveData.getString(objectTag(townIndex, i, typeof(Weapon)));

            if(data == tData && !past) {
                SaveData.deleteKey(objectTag(townIndex, i, typeof(Weapon)));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(objectTag(townIndex, i, typeof(Weapon)));
                overrideWeapon(townIndex, i - 1, JsonUtility.FromJson<Weapon>(data));
            }
        }
        SaveData.setInt(objectCountTag(townIndex, typeof(Weapon)), SaveData.getInt(objectCountTag(townIndex, typeof(Weapon))) - 1);
    }
    public static void removeArmor(int townIndex, Armor a) {
        var tData = JsonUtility.ToJson(a);

        bool past = false;
        for(int i = 0; i < SaveData.getInt(objectCountTag(townIndex, typeof(Armor))); i++) {
            var data = SaveData.getString(objectTag(townIndex, i, typeof(Armor)));

            if(data == tData && !past) {
                SaveData.deleteKey(objectTag(townIndex, i, typeof(Armor)));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(objectTag(townIndex, i, typeof(Armor)));
                overrideArmor(townIndex, i - 1, JsonUtility.FromJson<Armor>(data));
            }
        }
        SaveData.setInt(objectCountTag(townIndex, typeof(Armor)), SaveData.getInt(objectCountTag(townIndex, typeof(Armor))) - 1);
    }
    public static void removeConsumable(int townIndex, Consumable c) {
        var tData = JsonUtility.ToJson(c);

        bool past = false;
        for(int i = 0; i < SaveData.getInt(objectCountTag(townIndex, typeof(Consumable))); i++) {
            var data = SaveData.getString(objectTag(townIndex, i, typeof(Consumable)));

            if(data == tData && !past) {
                SaveData.deleteKey(objectTag(townIndex, i, typeof(Consumable)));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(objectTag(townIndex, i, typeof(Consumable)));
                overrideConsumable(townIndex, i - 1, JsonUtility.FromJson<Consumable>(data));
            }
        }
        SaveData.setInt(objectCountTag(townIndex, typeof(Consumable)), SaveData.getInt(objectCountTag(townIndex, typeof(Consumable))) - 1);
    }
    public static void removeItem(int townIndex, Item it) {
        var tData = JsonUtility.ToJson(it);

        bool past = false;
        for(int i = 0; i < SaveData.getInt(objectCountTag(townIndex, typeof(Item))); i++) {
            var data = SaveData.getString(objectTag(townIndex, i, typeof(Item)));

            if(data == tData && !past) {
                SaveData.deleteKey(objectTag(townIndex, i, typeof(Item)));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(objectTag(townIndex, i, typeof(Item)));
                overrideItem(townIndex, i - 1, JsonUtility.FromJson<Item>(data));
            }
        }
        SaveData.setInt(objectCountTag(townIndex, typeof(Item)), SaveData.getInt(objectCountTag(townIndex, typeof(Item))) - 1);
    }
    public static void removeSlave(int townIndex, UnitStats stats) {
        var tData = JsonUtility.ToJson(stats);

        bool past = false;
        for(int i = 0; i < SaveData.getInt(objectCountTag(townIndex, typeof(UnitStats))); i++) {
            var data = SaveData.getString(objectTag(townIndex, i, typeof(UnitStats)));

            if(data == tData && !past) {
                SaveData.deleteKey(objectTag(townIndex, i, typeof(UnitStats)));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(objectTag(townIndex, i, typeof(UnitStats)));
                overrideSlave(townIndex, i - 1, JsonUtility.FromJson<UnitStats>(data));
            }
        }
        SaveData.setInt(objectCountTag(townIndex, typeof(UnitStats)), SaveData.getInt(objectCountTag(townIndex, typeof(UnitStats))) - 1);
    }
    public static void removeWeapon(int townIndex, int index) {
        removeWeapon(townIndex, getWeapon(townIndex, index));
    }
    public static void removeArmor(int townIndex, int index) {
        removeArmor(townIndex, getArmor(townIndex, index));
    }
    public static void removeConsumable(int townIndex, int index) {
        removeConsumable(townIndex, getConsumable(townIndex, index));
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
    public static void overrideConsumable(int townIndex, int index, Consumable c) {
        var data = JsonUtility.ToJson(c);
        SaveData.setString(objectTag(townIndex, index, typeof(Consumable)), data);
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

        else if(type == typeof(Consumable))
            return SaveData.getInt(objectCountTag(townIndex, typeof(Consumable)));

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
    public static Consumable getConsumable(int townIndex, int index) {
        var data = SaveData.getString(objectTag(townIndex, index, typeof(Consumable)));
        if(!string.IsNullOrEmpty(data))
            return JsonUtility.FromJson<Consumable>(data);
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
    }
}

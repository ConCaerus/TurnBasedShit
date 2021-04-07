using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class ShopInventory {
    static string weaponTag(int townIndex, int index) { return "Town:" + townIndex.ToString() + " Shop Weapon:" + index.ToString(); }
    static string armorTag(int townIndex, int index) { return "Town:" + townIndex.ToString() + " Shop Armor:" + index.ToString(); }
    static string consumableTag(int townIndex, int index) { return "Town:" + townIndex.ToString() + " Shop Consumable:" + index.ToString(); }
    static string itemTag(int townIndex, int index) { return "Town:" + townIndex.ToString() + " Shop Item:" + index.ToString(); }

    static string weaponCountTag(int townIndex) { return "Town:" + townIndex.ToString() + " Weapon Count"; }
    static string armorCountTag(int townIndex) { return "Town:" + townIndex.ToString() + " Armor Count"; }
    static string consumableCountTag(int townIndex) { return "Town:" + townIndex.ToString() + " Consumable Count"; }
    static string itemCountTag(int townIndex) { return "Town:" + townIndex.ToString() + " Item Count"; }


    public static void clearShop(int townIndex) {
        clearTypeFromShop(townIndex, typeof(Weapon));
        clearTypeFromShop(townIndex, typeof(Armor));
        clearTypeFromShop(townIndex, typeof(Consumable));
        clearTypeFromShop(townIndex, typeof(Item));
    }
    public static void clearTypeFromShop(int townIndex, System.Type type) {
        if(type == typeof(Weapon)) {
            for(int i = 0; i < SaveData.getInt(weaponCountTag(townIndex)); i++) {
                SaveData.deleteKey(weaponTag(townIndex, i));
            }
            SaveData.deleteKey(weaponCountTag(townIndex));
            return;
        }
        
        else if(type == typeof(Armor)) {
            for(int i = 0; i < SaveData.getInt(armorCountTag(townIndex)); i++) {
                SaveData.deleteKey(armorTag(townIndex, i));
            }
            SaveData.deleteKey(armorCountTag(townIndex));
            return;
        }

        else if(type == typeof(Consumable)) {
            for(int i = 0; i < SaveData.getInt(consumableCountTag(townIndex)); i++) {
                SaveData.deleteKey(consumableTag(townIndex, i));
            }
            SaveData.deleteKey(consumableCountTag(townIndex));
            return;
        }

        else if(type == typeof(Item)) {
            for(int i = 0; i < SaveData.getInt(itemCountTag(townIndex)); i++) {
                SaveData.deleteKey(itemTag(townIndex, i));
            }
            SaveData.deleteKey(itemCountTag(townIndex));
            return;
        }
    }
    public static void populateShop(int townIndex, GameInfo.diffLvl lvl, PresetLibrary library) {
        clearShop(townIndex);

        int weaponCount = Random.Range(1, 11);
        for(int i = 0; i < weaponCount; i++) {
            var temp = library.getRandomWeapon((GameInfo.rarityLvl)lvl);
            Randomizer.randomizeWeapon(temp);

            var data = JsonUtility.ToJson(temp);
            SaveData.setString(weaponTag(townIndex, i), data);
        }
        SaveData.setInt(weaponCountTag(townIndex), weaponCount);

        int armorCount = Random.Range(1, 11);
        for(int i = 0; i < armorCount; i++) {
            var temp = library.getRandomArmor((GameInfo.rarityLvl)lvl);
            Randomizer.randomizeArmor(temp);

            var data = JsonUtility.ToJson(temp);
            SaveData.setString(armorTag(townIndex, i), data);
        }
        SaveData.setInt(armorCountTag(townIndex), armorCount);

        int consumableCount = Random.Range(1, 11);
        for(int i = 0; i < consumableCount; i++) {
            var temp = library.getRandomConsumable((GameInfo.rarityLvl)lvl);
            Randomizer.randomizeConsumable(temp);

            var data = JsonUtility.ToJson(temp);
            SaveData.setString(consumableTag(townIndex, i), data);
        }
        SaveData.setInt(consumableCountTag(townIndex), consumableCount);

        int itemCount = Random.Range(1, 11);
        for(int i = 0; i < itemCount; i++) {
            var temp = library.getRandomItem((GameInfo.rarityLvl)lvl);

            var data = JsonUtility.ToJson(temp);
            SaveData.setString(itemTag(townIndex, i), data);
        }
        SaveData.setInt(itemCountTag(townIndex), itemCount);
    }

    public static void addToShop(int townIndex, object thing) {
        if(thing.GetType() == typeof(Weapon)) {
            var index = SaveData.getInt(weaponCountTag(townIndex));
            ((Weapon)thing).w_sprite.setSprite();
            var data = JsonUtility.ToJson((Weapon)thing);
            SaveData.setString(weaponTag(townIndex, index), data);
            SaveData.setInt(weaponCountTag(townIndex), index + 1);
            return;
        }

        else if(thing.GetType() == typeof(Armor)) {
            var index = SaveData.getInt(armorCountTag(townIndex));
            ((Armor)thing).a_sprite.setSprite();
            var data = JsonUtility.ToJson((Armor)thing);
            SaveData.setString(armorTag(townIndex, index), data);
            SaveData.setInt(armorCountTag(townIndex), index + 1);
            return;
        }

        else if(thing.GetType() == typeof(Consumable)) {
            var index = SaveData.getInt(consumableCountTag(townIndex));
            ((Consumable)thing).c_sprite.setSprite();
            var data = JsonUtility.ToJson((Consumable)thing);
            SaveData.setString(consumableTag(townIndex, index), data);
            SaveData.setInt(consumableCountTag(townIndex), index + 1);
            return;
        }

        else if(thing.GetType() == typeof(Item)) {
            var index = SaveData.getInt(itemCountTag(townIndex));
            ((Item)thing).i_sprite.setSprite();
            var data = JsonUtility.ToJson((Item)thing);
            SaveData.setString(itemTag(townIndex, index), data);
            SaveData.setInt(itemCountTag(townIndex), index + 1);
            return;
        }
    }

    public static void removeFromShop(int townIndex, object thing) {
        if(thing.GetType() == typeof(Weapon)) {
            var data = JsonUtility.ToJson((Weapon)thing);

            bool past = false;
            for(int i = 0; i < SaveData.getInt(weaponCountTag(townIndex)); i++) {
                var tData = SaveData.getString(weaponTag(townIndex, i));

                if(data == tData && !past) {
                    SaveData.deleteKey(weaponTag(townIndex, i));
                    past = true;
                    continue;
                }
                else if(past) {
                    SaveData.deleteKey(weaponTag(townIndex, i));
                    overrideObject(townIndex, i - 1, JsonUtility.FromJson<Weapon>(data));
                }
            }
            SaveData.setInt(weaponCountTag(townIndex), SaveData.getInt(weaponCountTag(townIndex)) - 1);
            return;
        }

        else if(thing.GetType() == typeof(Armor)) {
            var data = JsonUtility.ToJson((Armor)thing);

            bool past = false;
            for(int i = 0; i < SaveData.getInt(armorCountTag(townIndex)); i++) {
                var tData = SaveData.getString(armorTag(townIndex, i));

                if(data == tData && !past) {
                    SaveData.deleteKey(armorTag(townIndex, i));
                    past = true;
                    continue;
                }
                else if(past) {
                    SaveData.deleteKey(armorTag(townIndex, i));
                    overrideObject(townIndex, i - 1, JsonUtility.FromJson<Armor>(data));
                }
            }
            SaveData.setInt(armorCountTag(townIndex), SaveData.getInt(armorCountTag(townIndex)) - 1);
            return;
        }

        else if(thing.GetType() == typeof(Consumable)) {
            var data = JsonUtility.ToJson((Consumable)thing);

            bool past = false;
            for(int i = 0; i < SaveData.getInt(consumableCountTag(townIndex)); i++) {
                var tData = SaveData.getString(consumableTag(townIndex, i));

                if(data == tData && !past) {
                    SaveData.deleteKey(consumableTag(townIndex, i));
                    past = true;
                    continue;
                }
                else if(past) {
                    SaveData.deleteKey(consumableTag(townIndex, i));
                    overrideObject(townIndex, i - 1, JsonUtility.FromJson<Consumable>(data));
                }
            }
            SaveData.setInt(consumableCountTag(townIndex), SaveData.getInt(consumableCountTag(townIndex)) - 1);
            return;
        }

        else if(thing.GetType() == typeof(Item)) {
            var data = JsonUtility.ToJson((Item)thing);

            bool past = false;
            for(int i = 0; i < SaveData.getInt(itemCountTag(townIndex)); i++) {
                var tData = SaveData.getString(itemTag(townIndex, i));

                if(data == tData && !past) {
                    SaveData.deleteKey(itemTag(townIndex, i));
                    past = true;
                    continue;
                }
                else if(past) {
                    SaveData.deleteKey(itemTag(townIndex, i));
                    overrideObject(townIndex, i - 1, JsonUtility.FromJson<Item>(data));
                }
            }
            SaveData.setInt(itemCountTag(townIndex), SaveData.getInt(itemCountTag(townIndex)) - 1);
            return;
        }
    }
    public static object removeFromIndex(int townIndex, int index, System.Type type) {
        if(type == typeof(Weapon)) {
            var data = SaveData.getString(weaponTag(townIndex, index));
            if(!string.IsNullOrEmpty(data)) {
                Weapon temp = JsonUtility.FromJson<Weapon>(data);
                removeFromShop(townIndex, temp);
                return temp;
            }
        }

        else if(type == typeof(Armor)) {
            var data = SaveData.getString(armorTag(townIndex, index));
            if(!string.IsNullOrEmpty(data)) {
                Armor temp = JsonUtility.FromJson<Armor>(data);
                removeFromShop(townIndex, temp);
                return temp;
            }
        }

        else if(type == typeof(Consumable)) {
            var data = SaveData.getString(consumableTag(townIndex, index));
            if(!string.IsNullOrEmpty(data)) {
                Consumable temp = JsonUtility.FromJson<Consumable>(data);
                removeFromShop(townIndex, temp);
                return temp;
            }
        }

        else if(type == typeof(Item)) {
            var data = SaveData.getString(itemTag(townIndex, index));
            if(!string.IsNullOrEmpty(data)) {
                Item temp = JsonUtility.FromJson<Item>(data);
                removeFromShop(townIndex, temp);
                return temp;
            }
        }
        return null;
    }

    public static void overrideObject(int townIndex, int index, object thing) {
        if(thing.GetType() == typeof(Weapon)) {
            ((Weapon)thing).w_sprite.setSprite();
            var data = JsonUtility.ToJson((Weapon)thing);
            SaveData.setString(weaponTag(townIndex, index), data);
            return;
        }

        else if(thing.GetType() == typeof(Armor)) {
            ((Armor)thing).a_sprite.setSprite();
            var data = JsonUtility.ToJson((Armor)thing);
            SaveData.setString(armorTag(townIndex, index), data);
            return;
        }

        else if(thing.GetType() == typeof(Consumable)) {
            ((Consumable)thing).c_sprite.setSprite();
            var data = JsonUtility.ToJson((Consumable)thing);
            SaveData.setString(consumableTag(townIndex, index), data);
            return;
        }

        else if(thing.GetType() == typeof(Item)) {
            ((Item)thing).i_sprite.setSprite();
            var data = JsonUtility.ToJson((Item)thing);
            SaveData.setString(itemTag(townIndex, index), data);
            return;
        }
    }

    public static int getTypeCount(int townIndex, System.Type type) {
        if(type == typeof(Weapon))
            return SaveData.getInt(weaponCountTag(townIndex));

        else if(type == typeof(Armor))
            return SaveData.getInt(armorCountTag(townIndex));

        else if(type == typeof(Consumable))
            return SaveData.getInt(consumableCountTag(townIndex));

        else if(type == typeof(Item))
            return SaveData.getInt(itemCountTag(townIndex));

        return -1;
    }
    public static object getObject(int townIndex, int index, System.Type type) {
        if(type == typeof(Weapon)) {
            var data = SaveData.getString(weaponTag(townIndex, index));
            if(!string.IsNullOrEmpty(data))
                return JsonUtility.FromJson<Weapon>(data);
        }

        else if(type == typeof(Armor)) {
            var data = SaveData.getString(armorTag(townIndex, index));
            if(!string.IsNullOrEmpty(data))
                return JsonUtility.FromJson<Armor>(data);
        }

        else if(type == typeof(Consumable)) {
            var data = SaveData.getString(consumableTag(townIndex, index));
            if(!string.IsNullOrEmpty(data))
                return JsonUtility.FromJson<Consumable>(data);
        }

        else if(type == typeof(Item)) {
            var data = SaveData.getString(itemTag(townIndex, index));
            if(!string.IsNullOrEmpty(data))
                return JsonUtility.FromJson<Item>(data);
        }

        return null;
    }
}

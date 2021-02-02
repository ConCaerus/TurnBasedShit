using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory {
    static List<Weapon> weapons = new List<Weapon>();
    static List<Armor> armor = new List<Armor>();


    public static void addWeaponToInventory(Weapon w) {
        w.w_sprite.setLocation();

        int count = PlayerPrefs.GetInt("Inventory Weapon Count");
        weapons.Add(w);

        var data = JsonUtility.ToJson(w);
        PlayerPrefs.SetString("Inventory Weapon" + count.ToString(), data);

        PlayerPrefs.SetInt("Inventory Weapon Count", count + 1);
        PlayerPrefs.Save();
    }
    public static void addArmorToInventory(Armor a) {
        a.a_sprite.setLocation();

        int count = PlayerPrefs.GetInt("Inventory Armor Count");
        armor.Add(a);

        var data = JsonUtility.ToJson(a);
        PlayerPrefs.SetString("Inventory Armor" + count.ToString(), data);

        PlayerPrefs.SetInt("Inventory Armor Count", count + 1);
        PlayerPrefs.Save();
    }

    public static void removeWeaponFromInventory(Weapon w) {
        for(int i = 0; i < weapons.Count; i++) {
            if(weapons[i] == w) {
                weapons.Remove(w);
                saveAllEquippment();
                break;
            }
        }

        PlayerPrefs.DeleteKey("Inventory Weapon" + weapons.Count.ToString());
    }
    public static void removeArmorFromInventory(Armor a) {
        for(int i = 0; i < armor.Count; i++) {
            if(armor[i] == a) {
                armor.Remove(a);

                saveAllEquippment();
                break;
            }
        }

        PlayerPrefs.DeleteKey("Inventory Armor" + armor.Count.ToString());
    }


    public static void saveAllEquippment() {
        clearPrefs();
        for(int i = 0; i < weapons.Count; i++) {
            var data = JsonUtility.ToJson(weapons[i]);
            PlayerPrefs.SetString("Inventory Weapon" + i.ToString(), data);
        }
        PlayerPrefs.SetInt("Inventory Weapon Count", weapons.Count);

        for(int i = 0; i < armor.Count; i++) {
            var data = JsonUtility.ToJson(armor[i]);
            PlayerPrefs.SetString("Inventory Armor" + i.ToString(), data);
        }
        PlayerPrefs.SetInt("Inventory Armor Count", armor.Count);
        PlayerPrefs.Save();
    }
    public static void loadAllEquippment() {
        weapons.Clear();
        armor.Clear();

        for(int i = 0; i < PlayerPrefs.GetInt("Inventory Weapon Count"); i++) {
            var data = PlayerPrefs.GetString("Inventory Weapon" + i.ToString());
            weapons.Add(JsonUtility.FromJson<Weapon>(data));
        }

        for(int i = 0; i < PlayerPrefs.GetInt("Inventory Armor Count"); i++) {
            var data = PlayerPrefs.GetString("Inventory Armor" + i.ToString());
            armor.Add(JsonUtility.FromJson<Armor>(data));
        }
        PlayerPrefs.Save();
    }


    public static void clearPrefs() {
        for(int i = 0; i < PlayerPrefs.GetInt("Inventory Weapon Count"); i++)
            PlayerPrefs.DeleteKey("Inventory Weapon" + i.ToString());

        for(int i = 0; i < PlayerPrefs.GetInt("Inventory Armor Count"); i++)
            PlayerPrefs.DeleteKey("Inventory Armor" + i.ToString());
        PlayerPrefs.Save();
    }
    public static void clearInventory() {
        weapons.Clear();
        armor.Clear();
        clearPrefs();
    }


    public static int getWeaponCount() {
        return weapons.Count;
    }
    public static Weapon loadWeapon(int i) {
        var data = PlayerPrefs.GetString("Inventory Weapon" + i.ToString());
        weapons[i] = JsonUtility.FromJson<Weapon>(data);
        PlayerPrefs.Save();
        return weapons[i];
    }
    public static Weapon takeWeaponAtIndex(int i) {
        var temp = weapons[i];
        removeWeaponFromInventory(weapons[i]);
        return temp;
    }

    public static int getArmorCount() {
        return armor.Count;
    }
    public static Armor loadArmor(int i) {
        var data = PlayerPrefs.GetString("Inventory Armor" + i.ToString());
        armor[i] = JsonUtility.FromJson<Armor>(data);
        PlayerPrefs.Save();
        return armor[i];
    }
    public static Armor takeArmorAtIndex(int i) {
        var temp = armor[i];
        removeArmorFromInventory(armor[i]);
        return temp;
    }
}

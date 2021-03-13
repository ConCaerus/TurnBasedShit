using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatLocation {
    public enum diffLevel {
        cake, easy, normal, inter, hard, heroic, legendary
    }

    public List<UnitClassStats> enemies = new List<UnitClassStats>();
    public diffLevel difficulty;

    public List<Weapon> weapons = new List<Weapon>();
    public List<Armor> armor = new List<Armor>();
    public List<Consumable> items = new List<Consumable>();


    public void addSpoilsToInventory() {
        foreach(var i in weapons)
            Inventory.addNewWeapon(i);
        foreach(var i in armor)
            Inventory.addNewArmor(i);
        foreach(var i in items)
            Inventory.addNewConsumable(i);
    }
}

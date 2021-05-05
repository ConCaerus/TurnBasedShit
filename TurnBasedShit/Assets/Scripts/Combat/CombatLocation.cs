using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatLocation {

    public List<UnitStats> enemies = new List<UnitStats>();
    public GameInfo.diffLvl difficulty;

    public int coinReward = 0;

    public List<Weapon> weapons = new List<Weapon>();
    public List<Armor> armor = new List<Armor>();
    public List<Consumable> consumables = new List<Consumable>();
    public List<Item> items = new List<Item>();


    public void addSpoilsToInventory() {
        Inventory.addCoins(coinReward);

        foreach(var i in weapons)
            Inventory.addWeapon(i);
        foreach(var i in armor)
            Inventory.addArmor(i);
        foreach(var i in consumables)
            Inventory.addConsumable(i);
        foreach(var i in items)
            Inventory.addItem(i);
    }
}

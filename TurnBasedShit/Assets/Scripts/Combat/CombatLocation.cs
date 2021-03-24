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

    public int coinReward = 0;

    public List<Weapon> weapons = new List<Weapon>();
    public List<Armor> armor = new List<Armor>();
    public List<Consumable> consumables = new List<Consumable>();


    public void addSpoilsToInventory() {
        Inventory.addCoins(coinReward);

        foreach(var i in weapons)
            Inventory.addNewWeapon(i);
        foreach(var i in armor)
            Inventory.addNewArmor(i);
        foreach(var i in consumables)
            Inventory.addNewConsumable(i);
    }
}

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

    public List<Weapon> getAndAddWeaponReward(PresetLibrary lib) {
        List<Weapon> spoils = new List<Weapon>();
        //  set weapons, weapons that would be here for quests
        foreach(var i in weapons)
            spoils.Add(i);

        //  random weapons, weapons that randomly drop after a fight
        int dropCount = 0;
        while((true || Random.Range(0, 11) == 0) && dropCount < 3) {   //  10% chance
            spoils.Add(Randomizer.randomizeWeapon(lib.getRandomWeapon((GameInfo.rarityLvl)difficulty)));
            dropCount++;
        }

        foreach(var i in spoils)
            Inventory.addWeapon(i);

        return spoils;
    }

    public List<Armor> getAndAddArmorReward(PresetLibrary lib) {
        List<Armor> spoils = new List<Armor>();
        //  set weapons, weapons that would be here for quests
        foreach(var i in armor)
            spoils.Add(i);

        //  random weapons, weapons that randomly drop after a fight
        int dropCount = 0;
        while(Random.Range(0, 11) == 0 && dropCount < 3) {   //  10% chance
            spoils.Add(Randomizer.randomizeArmor(lib.getRandomArmor((GameInfo.rarityLvl)difficulty)));
            dropCount++;
        }

        foreach(var i in spoils)
            Inventory.addArmor(i);

        return spoils;
    }

    public List<Consumable> getAndAddConsumableReward(PresetLibrary lib) {
        List<Consumable> spoils = new List<Consumable>();
        //  set weapons, weapons that would be here for quests
        foreach(var i in consumables)
            spoils.Add(i);

        //  random weapons, weapons that randomly drop after a fight
        int dropCount = 0;
        while(Random.Range(0, 2) == 0 && dropCount < 3) {   //  50% chance
            spoils.Add(lib.getRandomConsumable((GameInfo.rarityLvl)difficulty));
            dropCount++;
        }

        foreach(var i in spoils)
            Inventory.addConsumable(i);

        return spoils;
    }

    public List<Item> getAndAddItemReward(PresetLibrary lib) {
        List<Item> spoils = new List<Item>();
        //  set weapons, weapons that would be here for quests
        foreach(var i in items)
            spoils.Add(i);

        //  random weapons, weapons that randomly drop after a fight
        int dropCount = 0;
        while(Random.Range(0, 11) == 0 && dropCount < 3) {   //  10% chance
            spoils.Add(lib.getRandomItem((GameInfo.rarityLvl)difficulty));
            dropCount++;
        }

        foreach(var i in spoils)
            Inventory.addItem(i);

        return spoils;
    }
}

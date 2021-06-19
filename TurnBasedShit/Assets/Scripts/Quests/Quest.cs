using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest {
    /*
     * mapLocation - special map locations like boss fights or weapon pickups
     * acumlative - things that the player completes over time, such as kill 25 enemies
     * delivery - has the player deliver an item or a person to a different location
     * pickup - combat location that has a specific reward
     */

    //  when adding new quests, remember to increase the value in the presetLibrary for getting a random quest
    public enum questType {
        bossFight, equipmentPickup, accumulative, delivery
    }


    public questType q_type;
    public AccumulativeQuest accRef = null;
    public BossFightQuest bossRef = null;
    public DeliveryQuest delRef = null;
    public PickupQuest pickupRef = null;


    public int coinReward = 0;
    public List<Weapon> weaponReward = new List<Weapon>();
    public List<Armor> armorReward = new List<Armor>();
    public List<Consumable> consumableReward = new List<Consumable>();
    public List<Item> itemReward = new List<Item>();
    public List<UnitStats> unitReward = new List<UnitStats>();


    //  runs for shit like if the player gets a special item before the quest starts
    protected bool initialized = false;
    public abstract void questInit();
    public abstract void questDestory();


    //  gives the player a reward for completing the quest
    public void addReward() {
        Inventory.addCoins(coinReward);
        foreach(var i in weaponReward)
            Inventory.addWeapon(i);
        foreach(var i in armorReward)
            Inventory.addArmor(i);
        foreach(var i in consumableReward)
            Inventory.addConsumable(i);
        foreach(var i in itemReward)
            Inventory.addItem(i);
        foreach(var i in unitReward)
            Party.addNewUnit(i);
    }
}

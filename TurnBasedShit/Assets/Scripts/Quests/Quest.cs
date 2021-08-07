using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest {
    /*
     * mapLocation - special map locations like boss fights or weapon pickups
     * kill - player has to kill a certain number of enemies
     * delivery - has the player deliver an item or a person to a different location
     * pickup - combat location that has a specific reward
     */

    //  when adding new quests, remember to increase the value in the presetLibrary for getting a random quest
    public enum questType {
        bossFight, pickup, delivery, kill
    }

    bool q_completed = false;

    public questType q_type;

    public int coinReward = 0;
    public List<Weapon> weaponReward = new List<Weapon>();
    public List<Armor> armorReward = new List<Armor>();
    public List<Consumable> consumableReward = new List<Consumable>();
    public List<Item> itemReward = new List<Item>();
    public List<UnitStats> unitReward = new List<UnitStats>();

    public int q_instanceID = -1;


    //  runs for shit like if the player gets a special item before the quest starts
    protected bool initialized = false;
    public abstract void questInit(bool setInstanceID);
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


    public bool isEqualTo(Quest q) {
        if(q == null || q_instanceID == -1)
            return false;
        return q_instanceID == q.q_instanceID;
    }

    public void setCompleted(bool b) {
        q_completed = b;
    }

    public bool isCompleted() {
        return q_completed;
    }
}

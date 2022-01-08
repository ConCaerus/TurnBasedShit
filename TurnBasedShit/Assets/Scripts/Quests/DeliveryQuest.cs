using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeliveryQuest : Quest {
    public enum deliveryType {
        weapon, armor, consumable, item, unit
    }

    public TownLocation deliveryLocation;

    public deliveryType type;

    public List<Weapon> weaponsToDeliver = new List<Weapon>();
    public List<Armor> armorToDeliver = new List<Armor>();
    public List<Usable> consumablesToDeliver = new List<Usable>();
    public List<Item> itemsToDeliver = new List<Item>();
    public List<UnitStats> unitsToDeliver = new List<UnitStats>();


    public DeliveryQuest(TownLocation t, List<Weapon> w, bool setID) {
        if(setID)
            instanceID = GameInfo.getNextQuestInstanceID();

        deliveryLocation = t;

        foreach(var i in w)
            weaponsToDeliver.Add(i);

        type = deliveryType.weapon;

        reward = GameVariables.getCoinRewardForQuest(this);
    }
    public DeliveryQuest(TownLocation t, List<Armor> a, bool setID) {
        if(setID)
            instanceID = GameInfo.getNextQuestInstanceID();

        deliveryLocation = t;

        foreach(var i in a)
            armorToDeliver.Add(i);

        type = deliveryType.armor;

        reward = GameVariables.getCoinRewardForQuest(this);
    }
    public DeliveryQuest(TownLocation t, List<Usable> c, bool setID) {
        if(setID)
            instanceID = GameInfo.getNextQuestInstanceID();

        deliveryLocation = t;

        foreach(var i in c)
            consumablesToDeliver.Add(i);

        type = deliveryType.consumable;

        reward = GameVariables.getCoinRewardForQuest(this);
    }
    public DeliveryQuest(TownLocation t, List<Item> it, bool setID) {
        if(setID)
            instanceID = GameInfo.getNextQuestInstanceID();

        deliveryLocation = t;

        foreach(var i in it)
            itemsToDeliver.Add(i);

        type = deliveryType.item;

        reward = GameVariables.getCoinRewardForQuest(this);
    }
    public DeliveryQuest(TownLocation t, List<UnitStats> u, bool setID) {
        if(setID)
            instanceID = GameInfo.getNextQuestInstanceID();

        deliveryLocation = t;

        foreach(var i in u)
            unitsToDeliver.Add(i);

        type = deliveryType.weapon;

        reward = GameVariables.getCoinRewardForQuest(this);
    }


    public bool canDeliverGoods() {
        return true;
        //  check if the goods are still in the inventory
    }


    public override questType getQuestType() {
        return questType.delivery;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeliveryQuest {
    public enum deliveryType {
        weapon, armor, consumable, item, unit
    }
    public int q_instanceID = -1;


    public TownLocation deliveryLocation;

    public deliveryType type;

    public List<Weapon> weaponsToDeliver = new List<Weapon>();
    public List<Armor> armorToDeliver = new List<Armor>();
    public List<Usable> consumablesToDeliver = new List<Usable>();
    public List<Item> itemsToDeliver = new List<Item>();
    public List<UnitStats> unitsToDeliver = new List<UnitStats>();


    public DeliveryQuest(TownLocation t, List<Weapon> w, bool setID) {
        if(setID)
            q_instanceID = GameInfo.getNextQuestInstanceID();

        deliveryLocation = t;

        foreach(var i in w)
            weaponsToDeliver.Add(i);

        type = deliveryType.weapon;
    }
    public DeliveryQuest(TownLocation t, List<Armor> a, bool setID) {
        if(setID)
            q_instanceID = GameInfo.getNextQuestInstanceID();

        deliveryLocation = t;

        foreach(var i in a)
            armorToDeliver.Add(i);

        type = deliveryType.armor;
    }
    public DeliveryQuest(TownLocation t, List<Usable> c, bool setID) {
        if(setID)
            q_instanceID = GameInfo.getNextQuestInstanceID();

        deliveryLocation = t;

        foreach(var i in c)
            consumablesToDeliver.Add(i);

        type = deliveryType.consumable;
    }
    public DeliveryQuest(TownLocation t, List<Item> it, bool setID) {
        if(setID)
            q_instanceID = GameInfo.getNextQuestInstanceID();

        deliveryLocation = t;

        foreach(var i in it)
            itemsToDeliver.Add(i);

        type = deliveryType.item;
    }
    public DeliveryQuest(TownLocation t, List<UnitStats> u, bool setID) {
        if(setID)
            q_instanceID = GameInfo.getNextQuestInstanceID();

        deliveryLocation = t;

        foreach(var i in u)
            unitsToDeliver.Add(i);

        type = deliveryType.weapon;
    }


    public bool canDeliverGoods() {
        return true;
        //  check if the goods are still in the inventory
    }


    public bool isEqualTo(DeliveryQuest other) {
        return q_instanceID == other.q_instanceID;
    }

    public bool isInstanced() {
        return q_instanceID > -1;
    }


    public GameInfo.questType getType() {
        return GameInfo.questType.delivery;
    }
}

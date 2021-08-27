using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeliveryQuest {
    public enum deliveryType {
        weapon, armor, consumable, item, unit
    }
    public int q_instanceID = -1;


    public Town deliveryLocation;

    public deliveryType type;

    public List<Weapon> weaponsToDeliver = new List<Weapon>();
    public List<Armor> armorToDeliver = new List<Armor>();
    public List<Consumable> consumablesToDeliver = new List<Consumable>();
    public List<Item> itemsToDeliver = new List<Item>();
    public List<UnitStats> unitsToDeliver = new List<UnitStats>();


    public DeliveryQuest(Town t, List<Weapon> w, bool setID) {
        if(setID)
            q_instanceID = GameInfo.getNextQuestInstanceID();

        deliveryLocation = t;

        foreach(var i in w)
            weaponsToDeliver.Add(i);

        type = deliveryType.weapon;
    }
    public DeliveryQuest(Town t, List<Armor> a, bool setID) {
        if(setID)
            q_instanceID = GameInfo.getNextQuestInstanceID();

        deliveryLocation = t;

        foreach(var i in a)
            armorToDeliver.Add(i);

        type = deliveryType.armor;
    }
    public DeliveryQuest(Town t, List<Consumable> c, bool setID) {
        if(setID)
            q_instanceID = GameInfo.getNextQuestInstanceID();

        deliveryLocation = t;

        foreach(var i in c)
            consumablesToDeliver.Add(i);

        type = deliveryType.consumable;
    }
    public DeliveryQuest(Town t, List<Item> it, bool setID) {
        if(setID)
            q_instanceID = GameInfo.getNextQuestInstanceID();

        deliveryLocation = t;

        foreach(var i in it)
            itemsToDeliver.Add(i);

        type = deliveryType.item;
    }
    public DeliveryQuest(Town t, List<UnitStats> u, bool setID) {
        if(setID)
            q_instanceID = GameInfo.getNextQuestInstanceID();

        deliveryLocation = t;

        foreach(var i in u)
            unitsToDeliver.Add(i);

        type = deliveryType.weapon;
    }


    public void questInit() {
        foreach(var i in weaponsToDeliver)
            Inventory.addWeapon(i);
        foreach(var i in armorToDeliver)
            Inventory.addArmor(i);
        foreach(var i in consumablesToDeliver)
            Inventory.addConsumable(i);
        foreach(var i in itemsToDeliver)
            Inventory.addItem(i);
        foreach(var i in unitsToDeliver)
            Party.addNewUnit(i);
    }
    public void questDestory() {
        //  something
    }


    public bool canDeliverGoods() {
        return true;
        //  check if the goods are still in the inventory
    }


    public void setEqualTo(DeliveryQuest other, bool takeID) {
        if(other == null)
            return;

        if(takeID)
            q_instanceID = other.q_instanceID;

        deliveryLocation = other.deliveryLocation;
        type = other.type;
        weaponsToDeliver = other.weaponsToDeliver;
        armorToDeliver = other.armorToDeliver;
        consumablesToDeliver = other.consumablesToDeliver;
        itemsToDeliver = other.itemsToDeliver;
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

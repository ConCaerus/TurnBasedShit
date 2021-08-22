using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryQuest : Quest {
    public enum deliveryType {
        weapon, armor, consumable, item, unit
    }

    public Town deliveryLocation;

    public deliveryType type;

    public List<Weapon> weaponsToDeliver = new List<Weapon>();
    public List<Armor> armorToDeliver = new List<Armor>();
    public List<Consumable> consumablesToDeliver = new List<Consumable>();
    public List<Item> itemsToDeliver = new List<Item>();
    public List<UnitStats> unitsToDeliver = new List<UnitStats>();


    public DeliveryQuest(Town t, List<Weapon> w) {
        deliveryLocation = t;

        foreach(var i in w)
            weaponsToDeliver.Add(i);

        type = deliveryType.weapon;
        q_type = questType.delivery;
    }
    public DeliveryQuest(Town t, List<Armor> a) {
        deliveryLocation = t;

        foreach(var i in a)
            armorToDeliver.Add(i);

        type = deliveryType.armor;
        q_type = questType.delivery;
    }
    public DeliveryQuest(Town t, List<Consumable> c) {
        deliveryLocation = t;

        foreach(var i in c)
            consumablesToDeliver.Add(i);

        type = deliveryType.consumable;
        q_type = questType.delivery;
    }
    public DeliveryQuest(Town t, List<Item> it) {
        deliveryLocation = t;

        foreach(var i in it)
            itemsToDeliver.Add(i);

        type = deliveryType.item;
        q_type = questType.delivery;
    }
    public DeliveryQuest(Town t, List<UnitStats> u) {
        deliveryLocation = t;

        foreach(var i in u)
            unitsToDeliver.Add(i);

        type = deliveryType.weapon;
        q_type = questType.delivery;
    }


    public override void questInit(bool setInstanceID) {
        if(setInstanceID)
            q_instanceID = GameInfo.getNextQuestInstanceID();

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
    public override void questDestory() {
        //  something
    }


    public bool canDeliverGoods() {
        return true;
        //  check if the goods are still in the inventory
    }
}

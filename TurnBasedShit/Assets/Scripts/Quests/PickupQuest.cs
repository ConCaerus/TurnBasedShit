using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupQuest : Quest {
    PickupLocation location;

    public Weapon pickupWeapon = null;
    public Armor pickupArmor = null;
    public Consumable pickupConsumable = null;
    public Item pickupItem = null;


    public PickupQuest(PickupLocation pickup) {
        location = pickup;
        q_type = questType.equipmentPickup;
        pickupRef = this;

        pickupWeapon = pickup.pickupWeapon;
        pickupArmor = pickup.pickupArmor;
        pickupConsumable = pickup.pickupConsumable;
        pickupItem = pickup.pickupItem;
    }

    public override void questInit() {
        initialized = true;
        MapLocationHolder.saveNewLocation(location);
    }
    public override void questDestory() {
        if(initialized)
            MapLocationHolder.removePickupLocation(location);
    }
}

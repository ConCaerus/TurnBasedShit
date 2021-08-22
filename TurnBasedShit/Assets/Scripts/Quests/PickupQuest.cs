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
        q_type = questType.pickup;

        if(pickup == null)
            return;

        if(pickup.pickupWeapon != null && !pickup.pickupWeapon.isEmpty())
            pickupWeapon.setEqualTo(pickup.pickupWeapon, true);
        if(pickupArmor != null && !pickup.pickupArmor.isEmpty())
            pickupArmor.setEqualTo(pickup.pickupArmor, true);
        if(pickup.pickupConsumable != null && !pickup.pickupConsumable.isEmpty())
            pickupConsumable.setEqualTo(pickup.pickupConsumable, true);
        if(pickup.pickupItem != null && !pickup.pickupItem.isEmpty())
            pickupItem.setEqualTo(pickup.pickupItem, true);
    }

    public override void questInit(bool setInstanceID) {
        initialized = true;
        if(setInstanceID)
            q_instanceID = GameInfo.getNextQuestInstanceID();
        MapLocationHolder.addLocation(location);
    }
    public override void questDestory() {
        if(initialized)
            MapLocationHolder.removePickupLocation(location);
    }
}

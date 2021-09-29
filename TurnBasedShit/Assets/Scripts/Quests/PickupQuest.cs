using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PickupQuest {

    public int q_instanceID = -1;

    public PickupLocation.pickupType pType = (PickupLocation.pickupType)(-1);

    public PickupLocation location;

    public Weapon pickupWeapon = null;
    public Armor pickupArmor = null;
    public Consumable pickupConsumable = null;
    public Item pickupItem = null;


    public PickupQuest(PickupLocation pickup, bool setID) {
        if(setID)
            q_instanceID = GameInfo.getNextQuestInstanceID();

        pType = pickup.pType;

        if(pickup == null) {
            Debug.Log("fucking here");
            return;
        }

        if(pickup.pickupWeapon != null && !pickup.pickupWeapon.isEmpty()) {
            pickupWeapon = new Weapon();
            pickupWeapon.setEqualTo(pickup.pickupWeapon, true);
        }
        if(pickupArmor != null && !pickup.pickupArmor.isEmpty()) {
            pickupArmor = new Armor();
            pickupArmor.setEqualTo(pickup.pickupArmor, true);
        }
        if(pickup.pickupConsumable != null && !pickup.pickupConsumable.isEmpty()) {
            pickupConsumable = new Consumable();
            pickupConsumable.setEqualTo(pickup.pickupConsumable, true);
        }
        if(pickup.pickupItem != null && !pickup.pickupItem.isEmpty()) {
            pickupItem = new Item();
            pickupItem.setEqualTo(pickup.pickupItem, true);
        }

        location = pickup;
        //pickup.attachedQuest = this;
    }

    public bool isEqualTo(PickupQuest other) {
        return q_instanceID == other.q_instanceID;
    }

    public bool isInstanced() {
        return q_instanceID > -1;
    }


    public GameInfo.questType getType() {
        return GameInfo.questType.pickup;
    }
}

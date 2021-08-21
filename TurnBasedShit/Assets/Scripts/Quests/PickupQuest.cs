using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PickupQuest {

    public int q_instanceID = -1;

    PickupLocation location;

    public PickupLocation.pickupType pType = (PickupLocation.pickupType)(-1);

    public Weapon pickupWeapon = null;
    public Armor pickupArmor = null;
    public Consumable pickupConsumable = null;
    public Item pickupItem = null;


    public PickupQuest(PickupLocation pickup, bool setID) {
        if(setID)
            q_instanceID = GameInfo.getNextQuestInstanceID();

        location = pickup;
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
    }

    public void questInit() {
        MapLocationHolder.addLocation(location);
    }
    public void questDestory() {
    }


    public void setEqualTo(PickupQuest other, bool takeID) {
        if(other == null)
            return;

        if(takeID)
            q_instanceID = other.q_instanceID;

        location = other.location;
        pickupWeapon = other.pickupWeapon;
        pickupArmor = other.pickupArmor;
        pickupConsumable = other.pickupConsumable;
        pickupItem = other.pickupItem;
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

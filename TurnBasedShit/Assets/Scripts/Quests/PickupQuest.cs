using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PickupQuest : Quest {
    public PickupLocation.pickupType pType = (PickupLocation.pickupType)(-1);

    public PickupLocation location;

    public Collectable col;


    public PickupQuest(PickupLocation pickup, bool setID) {
        if(setID)
            instanceID = GameInfo.getNextQuestInstanceID();

        pType = pickup.pType;

        if(pickup == null) {
            return;
        }

        if(pickup.col != null && !pickup.col.isEmpty()) {
            col = pickup.col;
        }

        location = pickup;
    }

    public override questType getType() {
        return questType.pickup;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PickupQuest : Quest {
    public PickupLocation.pickupType pType = (PickupLocation.pickupType)(-1);

    public PickupLocation location;


    public PickupQuest(PickupLocation pickup, bool setID) {
        if(setID)
            instanceID = GameInfo.getNextQuestInstanceID();

        pType = pickup.pType;

        if(pickup == null) {
            return;
        }

        location = pickup;

        reward = GameVariables.getCoinRewardForQuest(this);
    }

    public override questType getQuestType() {
        return questType.pickup;
    }
}

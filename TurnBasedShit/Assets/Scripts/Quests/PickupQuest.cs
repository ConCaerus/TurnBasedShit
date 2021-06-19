using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupQuest : Quest {
    PickupLocation location;


    public PickupQuest(PickupLocation pickup) {
        location = pickup;
        q_type = questType.equipmentPickup;
        pickupRef = this;
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

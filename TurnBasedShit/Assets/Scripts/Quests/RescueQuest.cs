using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RescueQuest : Quest {
    public RescueLocation location;


    public RescueQuest(RescueLocation rescue, bool setID) {
        if(setID)
            instanceID = GameInfo.getNextQuestInstanceID();

        location = rescue;
    }

    public override questType getType() {
        return questType.rescue;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingQuest : Quest {

    public FishingQuest(FishingLocation loc, bool setID) {
        if(setID)
            instanceID = GameInfo.getNextQuestInstanceID();
    }

    public override questType getQuestType() {
        return questType.fishing;
    }
}

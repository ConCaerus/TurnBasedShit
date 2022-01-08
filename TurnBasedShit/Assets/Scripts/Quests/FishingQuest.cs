using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingQuest : Quest {
    public Collectable fish;

    public FishingQuest(Collectable f, bool setID) {
        if(setID)
            instanceID = GameInfo.getNextQuestInstanceID();

        fish = f;

        reward = GameVariables.getCoinRewardForQuest(this);
    }

    public override questType getQuestType() {
        return questType.fishing;
    }
}

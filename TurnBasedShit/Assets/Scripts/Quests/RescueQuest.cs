using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RescueQuest {
    public int q_instanceID = -1;

    public RescueLocation location;


    public RescueQuest(RescueLocation rescue, bool setID) {
        if(setID)
            q_instanceID = GameInfo.getNextQuestInstanceID();

        location = rescue;
    }

    public bool isEqualTo(RescueQuest other) {
        return q_instanceID == other.q_instanceID;
    }

    public bool isInstanced() {
        return q_instanceID > -1;
    }


    public GameInfo.questType getType() {
        return GameInfo.questType.rescue;
    }
}

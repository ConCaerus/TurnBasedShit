using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Quest {   //  Update object holder, quest canvas, mapQuestMenu
    public enum questType {
        bossFight, pickup, delivery, kill, rescue, fishing
    }

    public int instanceID = -1;

    public bool isTheSameInstanceAs(Quest other) {
        return instanceID == other.instanceID;
    }

    public bool isInstanced() {
        return instanceID > -1;
    }


    public abstract questType getQuestType();
}

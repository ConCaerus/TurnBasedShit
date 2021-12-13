using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Quest : MonoBehaviour {
    public enum questType {
        bossFight, pickup, delivery, kill, rescue, fishing
    }

    public int instanceID = -1;

    public bool isEqualTo(Quest other) {
        return instanceID == other.instanceID;
    }

    public bool isInstanced() {
        return instanceID > -1;
    }


    public abstract questType getType();
}

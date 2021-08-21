using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossFightQuest {
    public int q_instanceID = -1;
    public BossLocation location;

    public UnitStats bossUnit;


    public BossFightQuest(BossLocation boss, bool setID) {
        if(setID)
            q_instanceID = GameInfo.getNextQuestInstanceID();

        location = boss;
        bossUnit = boss.bossUnit;
    }


    public void questInit() {
        MapLocationHolder.addLocation(location);
    }
    public void questDestory() {
        MapLocationHolder.removeBossLocation(location);
    }

    public void setEqualTo(BossFightQuest other, bool takeID) {
        if(other == null)
            return;

        if(takeID)
            q_instanceID = other.q_instanceID;

        location = other.location;
        bossUnit = other.bossUnit;
    }
    public bool isEqualTo(BossFightQuest other) {
        return q_instanceID == other.q_instanceID;
    }

    public bool isInstanced() {
        return q_instanceID > -1;
    }


    public GameInfo.questType getType() {
        return GameInfo.questType.bossFight;
    }
}

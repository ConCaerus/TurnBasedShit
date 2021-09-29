using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossFightQuest {
    public int q_instanceID = -1;

    public UnitStats bossUnit;

    public BossLocation location;


    public BossFightQuest(BossLocation boss, bool setID) {
        if(setID)
            q_instanceID = GameInfo.getNextQuestInstanceID();

        bossUnit = boss.bossUnit;
        location = boss;
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

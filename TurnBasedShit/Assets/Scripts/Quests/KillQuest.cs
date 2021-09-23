using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KillQuest {
    public int q_instanceID = -1;

    public int howManyToKill;
    public int killCount;
    public EnemyUnitInstance.type enemyType;


    public KillQuest(int c, EnemyUnitInstance.type type, bool setID) {
        if(setID)
            q_instanceID = GameInfo.getNextQuestInstanceID();

        howManyToKill = c;
        enemyType = type;
    }


    public bool isEqualTo(KillQuest other) {
        return q_instanceID == other.q_instanceID;
    }

    public bool isInstanced() {
        return q_instanceID > -1;
    }


    public GameInfo.questType getType() {
        return GameInfo.questType.kill;
    }
}

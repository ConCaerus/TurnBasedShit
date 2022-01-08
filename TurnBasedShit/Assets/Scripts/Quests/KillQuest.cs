using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KillQuest : Quest {
    public int howManyToKill;
    public EnemyUnitInstance.type enemyType;


    public KillQuest(int c, EnemyUnitInstance.type type, bool setID) {
        if(setID)
            instanceID = GameInfo.getNextQuestInstanceID();

        howManyToKill = c;
        enemyType = type;

        reward = GameVariables.getCoinRewardForQuest(this);
    }

    public override questType getQuestType() {
        return questType.kill;
    }
}

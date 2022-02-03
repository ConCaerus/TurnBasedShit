using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KillQuest : Quest {
    public int howManyToKill;
    public GameInfo.combatUnitType enemyType;


    public KillQuest(int c, GameInfo.combatUnitType type, bool setID) {
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

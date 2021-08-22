using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillQuest : Quest {
    public int howManyToKill;
    public int killCount;
    public EnemyUnitInstance.type enemyType;


    public KillQuest(int c, EnemyUnitInstance.type type) {
        howManyToKill = c;
        enemyType = type;
        q_type = Quest.questType.kill;
    }


    public override void questInit(bool setInstanceID) {
        if(setInstanceID)
            q_instanceID = GameInfo.getNextQuestInstanceID();
    }
    public override void questDestory() {
        //  something
    }
}

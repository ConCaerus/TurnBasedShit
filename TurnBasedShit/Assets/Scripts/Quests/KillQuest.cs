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
        killRef = this;
    }


    public override void questInit() {
        //  something
    }
    public override void questDestory() {
        //  something
    }
}

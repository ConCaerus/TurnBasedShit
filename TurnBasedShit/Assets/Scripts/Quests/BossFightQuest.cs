using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossFightQuest : Quest {
    public UnitStats bossUnit;

    public BossLocation location;


    public BossFightQuest(BossLocation boss, bool setID) {
        if(setID)
            instanceID = GameInfo.getNextQuestInstanceID();

        bossUnit = boss.bossUnit;
        location = boss;
    }


    public override questType getType() {
        return questType.bossFight;
    }
}

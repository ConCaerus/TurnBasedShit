using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightQuest : Quest {
    public BossLocation location;

    public UnitStats bossUnit;


    public BossFightQuest(BossLocation boss) {
        location = boss;
        q_type = questType.bossFight;
        bossUnit = boss.bossUnit;
    }


    public override void questInit(bool setInstanceID) {
        initialized = true;
        if(setInstanceID)
            q_instanceID = GameInfo.getNextQuestInstanceID();
        MapLocationHolder.addLocation(location);
    }
    public override void questDestory() {
        if(initialized)
            MapLocationHolder.removeBossLocation(location);
    }
}

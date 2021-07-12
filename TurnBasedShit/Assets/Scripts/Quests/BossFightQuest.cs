using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightQuest : Quest {
    public BossLocation location;

    public UnitStats bossUnit;


    public BossFightQuest(BossLocation boss) {
        location = boss;
        q_type = questType.bossFight;
        bossRef = this;
        bossUnit = boss.bossUnit;
    }


    public override void questInit() {
        initialized = true;
        MapLocationHolder.saveNewLocation(location);
    }
    public override void questDestory() {
        if(initialized)
            MapLocationHolder.removeBossLocation(location);
    }
}

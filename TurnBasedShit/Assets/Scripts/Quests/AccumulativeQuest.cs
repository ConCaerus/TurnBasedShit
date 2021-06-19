using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccumulativeQuest : Quest {
    //  when adding, increase number in create random quest in presetlib
    public enum type {
        killEnemies
    }


    public type questType;
    public int count;


    public AccumulativeQuest(type t, int c = 0) {
        questType = t;
        count = c;
        q_type = Quest.questType.accumulative;
        accRef = this;
    }


    public override void questInit() {
        //  something
    }
    public override void questDestory() {
        //  something
    }
}

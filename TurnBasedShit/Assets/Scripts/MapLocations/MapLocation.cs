using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public abstract class MapLocation {

    [Serializable]
    public enum locationType {
        town, pickup, upgrade, rescue, boss, fishing, eye, bridge, loot
    }



    public Vector2 pos;
    public locationType type = (locationType)(-1);
    public GameInfo.region region;

    public CombatLocation combatLocation = null;

    public abstract void enterLocation(TransitionCanvas tc);

    public bool isEqualTo(MapLocation other) {  
        if(other == null)
            return false;

        //  two locations shouldn't have the same position and the same region so this should be enough
        return pos == other.pos && type == other.type && region == other.region;
    }
}

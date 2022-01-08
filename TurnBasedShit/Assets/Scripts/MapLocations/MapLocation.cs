using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public abstract class MapLocation {

    [Serializable]
    public enum locationType {
        town, pickup, upgrade, rescue, nest, boss, fishing, eye, bridge
    }



    public Vector2 pos;
    public locationType type = (locationType)(-1);
    public GameInfo.region region;

    public CombatLocation combatLocation = null;

    public abstract void enterLocation(TransitionCanvas tc);

    public abstract bool isEqualTo(MapLocation other);
}

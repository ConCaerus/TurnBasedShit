using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public abstract class MapLocation {

    [Serializable]
    public enum locationType {
        empty, town, pickup, upgrade, rescue, nest, boss
    }



    public Vector2 pos;
    public locationType type = locationType.empty;

    public CombatLocation combatLocation = null;

    public abstract void enterLocation();

    public abstract bool isEqualTo(MapLocation other);
}

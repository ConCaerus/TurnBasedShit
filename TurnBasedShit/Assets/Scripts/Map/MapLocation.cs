using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public abstract class MapLocation {

    [Serializable]
    public enum locationType {
        empty, town, equipmentPickup, equipmentUpgrade
    }



    public Vector2 pos;
    public locationType type = locationType.empty;
    public SpriteLoader sprite = new SpriteLoader();

    public CombatLocation combatLocation = null;

    public abstract void enterLocation();

    public bool equals(MapLocation other) {
        return pos == other.pos && type == other.type;
    }
}

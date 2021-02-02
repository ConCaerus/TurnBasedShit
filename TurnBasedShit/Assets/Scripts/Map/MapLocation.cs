using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MapLocation {
    [Serializable]
    public enum locationType {
        town
    }
    public locationType type;
    public Vector2 pos;

    public MapLocation(locationType t, Vector2 p) {
        type = t;
        pos = p;
    }
    public MapLocation(locationType t) {
        type = t;
    }
}

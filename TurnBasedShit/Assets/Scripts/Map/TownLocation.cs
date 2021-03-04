using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TownLocation : MapLocation {


    public TownLocation(Vector2 p, Sprite s) {
        type = locationType.town;
        pos = p;
        sprite.setLocation(s);
    }
}

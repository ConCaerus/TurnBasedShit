using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TownLocation : MapLocation {
    public Town town;


    public TownLocation(Vector2 p, Sprite s, Town t) {
        type = locationType.town;
        pos = p;
        sprite.setSprite(s);
        town = TownLibrary.addNewTownAndSetIndex(t);
    }
}

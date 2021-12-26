using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeLocation : MapLocation {

    public EyeLocation(Vector2 p, GameInfo.region reg) {
        type = locationType.eye;
        region = reg;
        pos = p;
    }



    public void activate(MapFogTexture fog, MapLocationSpawner spawner) {
        fog.clearFogAroundPos(pos, 10.0f, true);
        MapLocationHolder.removeLocation(this);
        spawner.removeIconAtPos(pos);
    }

    public override void enterLocation(TransitionCanvas tc) {
    }

    public override bool isEqualTo(MapLocation other) {
        if(other.type != locationType.eye)
            return false;

        return other.pos == pos;
    }
}

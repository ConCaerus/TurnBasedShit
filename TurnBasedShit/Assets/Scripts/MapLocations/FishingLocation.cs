using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FishingLocation : MapLocation {
    public Unusable fish;

    public FishingLocation(Vector2 p, Unusable f) {
        pos = p;
        type = locationType.fishing;
        fish = f;
    }


    public override void enterLocation(TransitionCanvas tc) {
        GameInfo.setCurrentLocationAsFishing(this);
        tc.loadSceneWithTransition("Fishing");
    }

    public override bool isEqualTo(MapLocation other) {
        if(other.type != locationType.nest)
            return false;

        return pos == other.pos;
    }
}

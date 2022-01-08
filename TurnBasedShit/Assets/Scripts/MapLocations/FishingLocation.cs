using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class FishingLocation : MapLocation {
    public Collectable fish;

    public FishingLocation(Vector2 p, Collectable f) {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TownLocation : MapLocation {
    public Town town;

    public GameInfo.region region = (GameInfo.region)(-1);

    public TownLocation(Vector2 p, GameInfo.region diff, PresetLibrary lib, Town t = null) {
        type = locationType.town;
        pos = p;

        region = diff;

        //  create a random town
        if(t == null) {
            t = new Town(diff, lib, true);
        }

        town = t;
    }

    public override void enterLocation(TransitionCanvas tc) {
        GameInfo.clearCombatDetails();
        GameInfo.setCurrentLocationAsTown(this);
        tc.loadSceneWithTransition("Town");
    }

    public override bool isEqualTo(MapLocation other) {
        if(other.type != locationType.town)
            return false;

        return town.isEqualTo(((TownLocation)other).town);
    }
}

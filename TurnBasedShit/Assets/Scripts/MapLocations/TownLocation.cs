using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TownLocation : MapLocation {
    public Town town;

    public TownLocation(Vector2 p, GameInfo.region reg, PresetLibrary lib, Town t = null) {
        type = locationType.town;
        pos = p;

        region = reg;

        //  create a random town
        if(t == null) {
            t = new Town(reg, lib, true);
        }

        town = t;
    }

    public override void enterLocation(TransitionCanvas tc) {
        GameInfo.clearCombatDetails();
        GameInfo.setCurrentLocationAsTown(this);
        tc.loadSceneWithTransition("Town");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NestLocation : MapLocation {

    public NestLocation(Vector2 p, int waveNumber, PresetLibrary lib) {
        pos = p;
        type = locationType.nest;

        var diff = Map.getDiffForX(p.x);
        combatLocation = lib.createCombatLocation(diff);
        combatLocation.createWaves(diff, lib, waveNumber);
    }


    public override void enterLocation(TransitionCanvas tc) {
        GameInfo.setCombatDetails(combatLocation);
        GameInfo.setCurrentLocationAsNest(this);
        MapLocationHolder.removeLocation(this);
        tc.loadSceneWithTransition("Combat");
    }

    public override bool isEqualTo(MapLocation other) {
        if(other.type != locationType.nest)
            return false;

        return pos == other.pos;
    }
}

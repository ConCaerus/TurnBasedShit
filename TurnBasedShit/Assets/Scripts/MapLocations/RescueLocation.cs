using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RescueLocation : MapLocation {
    public UnitStats unit = null;

    public RescueLocation(Vector2 p, UnitStats stats, PresetLibrary lib) {
        pos = p;
        type = locationType.rescue;

        combatLocation = lib.createCombatLocation(Map.getDiffForX(pos.x));
        combatLocation.rescuedUnits.Add(stats);

        unit = stats;
    }
    public RescueLocation(Vector2 p, PresetLibrary lib) {
        pos = p;
        type = locationType.rescue;

        var stats = lib.createRandomPlayerUnitStats(true);

        combatLocation = lib.createCombatLocation(Map.getDiffForX(pos.x));
        combatLocation.rescuedUnits.Add(stats);

        unit = stats;
    }

    public override void enterLocation(TransitionCanvas tc) {
        GameInfo.setCombatDetails(combatLocation);
        GameInfo.setCurrentLocationAsRescue(this);
        MapLocationHolder.removeLocation(this);
        tc.loadSceneWithTransition("Combat");
    }

    public override bool isEqualTo(MapLocation other) {
        if(other.type != locationType.rescue)
            return false;

        return unit == ((RescueLocation)other).unit && pos == other.pos;
    }
}

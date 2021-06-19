using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RescueLocation : MapLocation {
    public UnitStats unit = null;

    public RescueLocation(Vector2 p, UnitStats stats, PresetLibrary lib, GameInfo.diffLvl diff) {
        pos = p;
        type = locationType.rescue;

        combatLocation = lib.createCombatLocation(diff);
        combatLocation.rescuedUnits.Add(stats);

        unit = stats;
    }
    public RescueLocation(Vector2 p, PresetLibrary lib, GameInfo.diffLvl diff) {
        pos = p;
        type = locationType.rescue;

        var stats = Randomizer.createRandomUnitStats(false);

        combatLocation = lib.createCombatLocation(diff);
        combatLocation.rescuedUnits.Add(stats);

        unit = stats;
    }

    public override void enterLocation() {
        GameInfo.setCombatDetails(combatLocation);
        GameInfo.setCurrentMapLocation(MapLocationHolder.getIndex((RescueLocation)this));
        MapLocationHolder.removeLocation(this);
        SceneManager.LoadScene("Combat");
    }
}

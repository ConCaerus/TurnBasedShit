using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TownLocation : MapLocation {
    public Town town;

    public TownLocation(Vector2 p, GameInfo.diffLvl diff, PresetLibrary lib, Town t = null) {
        type = locationType.town;
        pos = p;

        //  create a random town
        if(t == null) {
            t = lib.createRandomTown(diff);
        }

        town = t;
    }

    public override void enterLocation() {
        GameInfo.resetCombatDetails();
        GameInfo.setCurrentLocationAsTown(this);
        SceneManager.LoadScene("Town");
    }

    public override bool isEqualTo(MapLocation other) {
        if(other.type != locationType.town)
            return false;

        return town.isEqualTo(((TownLocation)other).town);
    }
}

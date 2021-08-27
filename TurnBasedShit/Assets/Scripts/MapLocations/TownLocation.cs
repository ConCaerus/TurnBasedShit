using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TownLocation : MapLocation {
    public Town town;

    public int region = -1;

    public TownLocation(Vector2 p, GameInfo.diffLvl diff, PresetLibrary lib, Town t = null) {
        type = locationType.town;
        pos = p;

        region = (int)Map.getDiffForX(p.x);

        //  create a random town
        if(t == null) {
            t = new Town(diff, lib, true);
        }

        town = t;
    }

    public override void enterLocation() {
        GameInfo.resetCombatDetails();
        GameInfo.setCurrentLocationAsTown(this);
        town.visited = true;
        SceneManager.LoadScene("Town");
    }

    public override bool isEqualTo(MapLocation other) {
        if(other.type != locationType.town)
            return false;

        return town.isEqualTo(((TownLocation)other).town);
    }
}

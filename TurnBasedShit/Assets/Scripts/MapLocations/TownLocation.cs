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

        town = TownLibrary.addNewTownAndSetIndex(t);
    }

    public override void enterLocation() {
        GameInfo.resetCombatDetails();
        GameInfo.setCurrentMapLocation(MapLocationHolder.getIndex(this));
        SceneManager.LoadScene("Town");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TownLocation : MapLocation {
    public Town town;


    public TownLocation(Vector2 p, Sprite s, Town t) {
        type = locationType.town;
        pos = p;
        sprite.setSprite(s);
        town = TownLibrary.addNewTownAndSetIndex(t);
    }

    public override void enterLocation() {
        GameInfo.resetCombatDetails();
        GameInfo.setCurrentMapLocation(MapLocationHolder.getIndex(this));
        SceneManager.LoadScene("Town");
    }
}

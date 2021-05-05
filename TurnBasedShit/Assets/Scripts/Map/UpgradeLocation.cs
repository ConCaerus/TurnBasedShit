using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeLocation : MapLocation {
    public int state = 0;


    public UpgradeLocation(Vector2 p, Sprite s, int st) {
        type = locationType.equipmentUpgrade;
        pos = p;
        sprite.setSprite(s);
        state = st;
    }


    public override void enterLocation() {
        GameInfo.resetCombatDetails();
        GameInfo.setCurrentMapLocation(MapLocationHolder.getIndex((UpgradeLocation)this));
        MapLocationHolder.removeLocation(this);
        SceneManager.LoadScene("UpgradeLocation");
    }
}

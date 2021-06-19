using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeLocation : MapLocation {
    //  0 - weapon, 1 - armor
    public int state = 0;


    public UpgradeLocation(Vector2 p, int st) {
        type = locationType.equipmentUpgrade;
        pos = p;
        state = st;
    }


    public override void enterLocation() {
        GameInfo.resetCombatDetails();
        GameInfo.setCurrentMapLocation(MapLocationHolder.getIndex((UpgradeLocation)this));
        MapLocationHolder.removeLocation(this);
        SceneManager.LoadScene("UpgradeLocation");
    }
}

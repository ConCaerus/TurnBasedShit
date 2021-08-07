using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeLocation : MapLocation {
    //  0 - weapon, 1 - armor
    public int state = 0;


    public UpgradeLocation(Vector2 p, int st) {
        type = locationType.upgrade;
        pos = p;
        state = st;
    }


    public override void enterLocation() {
        GameInfo.resetCombatDetails();
        GameInfo.setCurrentLocationAsUpgrade(this);
        MapLocationHolder.removeLocation(this);
        SceneManager.LoadScene("UpgradeLocation");
    }

    public override bool isEqualTo(MapLocation other) {
        if(other.type != locationType.upgrade)
            return false;

        return state == ((UpgradeLocation)other).state && other.pos == pos;
    }
}

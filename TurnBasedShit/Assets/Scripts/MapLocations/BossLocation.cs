using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossLocation : MapLocation {

    public BossLocation(Vector2 p, GameObject boss, GameInfo.diffLvl diff, PresetLibrary lib, bool areOtherEnemiesBesidesBoss = false) {
        pos = p;
        type = locationType.boss;

        combatLocation = lib.createCombatLocation(diff);
        if(!areOtherEnemiesBesidesBoss)
            combatLocation.createWaves(diff, lib, 1, 0, 0);
        else
            combatLocation.createWaves(diff, lib, 1);

        combatLocation.waves[0].enemies.Add(boss);
    }
    


    public override void enterLocation() {
        GameInfo.setCombatDetails(combatLocation);
        GameInfo.setCurrentMapLocation(MapLocationHolder.getIndex((BossLocation)this));
        MapLocationHolder.removeLocation(this);
        SceneManager.LoadScene("Combat");
    }
}

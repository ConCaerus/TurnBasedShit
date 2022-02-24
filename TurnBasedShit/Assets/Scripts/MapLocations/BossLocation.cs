using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class BossLocation : MapLocation {
    public UnitStats bossUnit;

    public BossLocation(Vector2 p, GameObject boss, GameInfo.region diff, PresetLibrary lib) {
        pos = p;
        type = locationType.boss;
        region = diff;

        combatLocation = lib.createCombatLocationForBoss(boss.GetComponent<UnitClass>().stats.u_type);

        bossUnit = boss.GetComponent<UnitClass>().stats;
    }
    


    public override void enterLocation(TransitionCanvas tc) {
        GameInfo.setCombatDetails(combatLocation);
        GameInfo.setCurrentLocationAsBoss(this);
        MapLocationHolder.removeLocation(this);
        tc.loadSceneWithTransition("Combat");
    }
}

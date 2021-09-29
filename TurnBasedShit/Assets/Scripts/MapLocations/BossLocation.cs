using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class BossLocation : MapLocation {
    public UnitStats bossUnit;

    public BossLocation(Vector2 p, GameObject boss, GameInfo.diffLvl diff, PresetLibrary lib, bool areOtherEnemiesBesidesBoss = false) {
        pos = p;
        type = locationType.boss;

        combatLocation = lib.createCombatLocation(diff);
        if(!areOtherEnemiesBesidesBoss)
            combatLocation.createWaves(diff, lib, 1, 0, 0);
        else
            combatLocation.createWaves(diff, lib, 1);

        combatLocation.waves[0].enemies.Add(boss);

        bossUnit = boss.GetComponent<UnitClass>().stats;
    }
    


    public override void enterLocation() {
        GameInfo.setCombatDetails(combatLocation);
        GameInfo.setCurrentLocationAsBoss(this);
        MapLocationHolder.removeLocation(this);
        SceneManager.LoadScene("Combat");
    }

    public override bool isEqualTo(MapLocation other) {
        if(other.type != locationType.boss)
            return false;

        return bossUnit == ((BossLocation)other).bossUnit && pos == other.pos;
    }
}

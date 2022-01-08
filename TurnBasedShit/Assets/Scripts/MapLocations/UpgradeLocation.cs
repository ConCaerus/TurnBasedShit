using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class UpgradeLocation : MapLocation {
    //  0 - weapon, 1 - armor
    public int state = 0;

    public int attUpgrade = -1;
    public float statUpgrade = 1.0f;    //  multiplies by the existing power


    public UpgradeLocation(Vector2 p, GameInfo.region reg, int st) {
        type = locationType.upgrade;
        pos = p;
        state = st;

        while(attUpgrade == -1 && statUpgrade == 1.0f) {
            if(state == 0)
                attUpgrade = Random.Range(-1, Weapon.attributeCount);
            else if(state == 1)
                attUpgrade = Random.Range(-1, Armor.attributeCount);

            statUpgrade = Random.Range(1.0f, 2.0f) * (int)reg;
            if(statUpgrade < 1.1f)
                statUpgrade = 1.0f;
        }
    }


    public override void enterLocation(TransitionCanvas tc) {
        GameInfo.clearCombatDetails();
        GameInfo.setCurrentLocationAsUpgrade(this);
        MapLocationHolder.removeLocation(this);
        tc.loadSceneWithTransition("UpgradeLocation");
    }

    public override bool isEqualTo(MapLocation other) {
        if(other.type != locationType.upgrade)
            return false;

        return state == ((UpgradeLocation)other).state && other.pos == pos;
    }
}

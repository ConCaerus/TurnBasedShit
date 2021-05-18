using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NestLocation : MapLocation {

    public NestLocation(Vector2 p, Sprite sp, int waveNumber, PresetLibrary lib, GameInfo.diffLvl diff) {
        pos = p;
        type = locationType.nest;
        sprite = new SpriteLoader();
        sprite.setSprite(sp);

        combatLocation = lib.createCombatLocation(diff);
        combatLocation.createWaves(diff, lib, waveNumber);
    }


    public override void enterLocation() {
        GameInfo.setCombatDetails(combatLocation);
        GameInfo.setCurrentMapLocation(MapLocationHolder.getIndex((NestLocation)this));
        MapLocationHolder.removeLocation(this);
        SceneManager.LoadScene("Combat");
    }
}

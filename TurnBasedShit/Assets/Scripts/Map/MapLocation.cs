using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class MapLocation {
    public Vector2 pos;
    public locationType type;
    public SpriteLocation sprite = new SpriteLocation();

    public CombatLocation combatLocation = null;


    [Serializable]
    public enum locationType {
        town, equipmentPickup
    }

    public void enterLocation() {
        switch(type) {
            case locationType.town:
                GameState.resetCombatDetails();
                SceneManager.LoadScene("Town");
                break;

            case locationType.equipmentPickup:
                GameState.setCombatDetails(combatLocation);
                MapLocationHolder.removeLocation(this);
                SceneManager.LoadScene("Combat");
                break;
        }
    }
    public bool equals(MapLocation other) {
        return pos == other.pos && type == other.type;
    }
}

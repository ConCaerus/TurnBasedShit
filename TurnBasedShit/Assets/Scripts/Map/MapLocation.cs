using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class MapLocation {
    public Vector2 pos;
    public locationType type;
    public SpriteLoader sprite = new SpriteLoader();

    public CombatLocation combatLocation = null;


    [Serializable]
    public enum locationType {
        town, equipmentPickup
    }

    public void enterLocation() {
        switch(type) {
            case locationType.town:
                GameInfo.resetCombatDetails();
                GameInfo.setCurrentTownIndex(((TownLocation)this).town.t_index);
                SceneManager.LoadScene("Town");
                break;

            case locationType.equipmentPickup:
                GameInfo.setCombatDetails(combatLocation);
                GameInfo.resetCurrentTownIndex();
                MapLocationHolder.removeLocation(this);
                SceneManager.LoadScene("Combat");
                break;
        }
    }
    public bool equals(MapLocation other) {
        return pos == other.pos && type == other.type;
    }
}

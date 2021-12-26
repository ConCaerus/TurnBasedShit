using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PickupLocation : MapLocation {
    public enum pickupType {
        weapon, armor, consumable, item
    }


    public Collectable col;


    public pickupType pType = (pickupType)(-1);


    public PickupLocation(Vector2 p, Collectable c, GameInfo.region reg, PresetLibrary lib) {
        pos = p;
        type = locationType.pickup;
        pType = pickupType.weapon;

        combatLocation = lib.createCombatLocation(reg);

        col = c;
        combatLocation.collectables.Add(col);
        //  adds more if the type is usable
        if(c.type == Collectable.collectableType.usable) {
            int count = Random.Range(0, 11);
            for(int i = 0; i < count; i++)
                combatLocation.collectables.Add(col);
        }
    }


    public override void enterLocation(TransitionCanvas tc) {
        GameInfo.setCombatDetails(combatLocation);
        GameInfo.setCurrentLocationAsPickup(this);
        MapLocationHolder.removeLocation(this);
        tc.loadSceneWithTransition("Combat");
    }

    public override bool isEqualTo(MapLocation other) {
        if(other.type != locationType.pickup)
            return false;

        PickupLocation o = (PickupLocation)other;

        return pos == other.pos && col.isTheSameInstanceAs(o.col);   
    }
}

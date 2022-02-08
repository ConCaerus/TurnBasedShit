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
        if(col != null && !col.isEmpty()) {
            switch(col.type) {
                case Collectable.collectableType.Weapon:
                    combatLocation.spoils.addObject<Weapon>((Weapon)col);
                    break;
                case Collectable.collectableType.Armor:
                    combatLocation.spoils.addObject<Armor>((Armor)col);
                    break;
                case Collectable.collectableType.Item:
                    combatLocation.spoils.addObject<Item>((Item)col);
                    break;
                case Collectable.collectableType.Usable:
                    combatLocation.spoils.addObject<Usable>((Usable)col);
                    break;
                case Collectable.collectableType.Unusable:
                    combatLocation.spoils.addObject<Unusable>((Unusable)col);
                    break;
            }
        }
        //  adds more if the type is usable
        if(c.type == Collectable.collectableType.Usable) {
            int count = Random.Range(0, 11);
            for(int i = 0; i < count; i++)
                combatLocation.spoils.addObject<Usable>((Usable)col);
        }
    }


    public override void enterLocation(TransitionCanvas tc) {
        GameInfo.setCombatDetails(combatLocation);
        GameInfo.setCurrentLocationAsPickup(this);
        MapLocationHolder.removeLocation(this);
        tc.loadSceneWithTransition("Combat");
    }
}

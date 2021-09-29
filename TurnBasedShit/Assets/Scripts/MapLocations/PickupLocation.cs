using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PickupLocation : MapLocation {
    public enum pickupType {
        weapon, armor, consumable, item
    }


    public Weapon pickupWeapon = null;
    public Armor pickupArmor = null;
    public Consumable pickupConsumable = null;
    public Item pickupItem = null;

    //public PickupQuest attachedQuest = null;


    public pickupType pType = (pickupType)(-1);

    //  weapon
    public PickupLocation(Vector2 p, Weapon w, PresetLibrary lib, GameInfo.diffLvl diff) {
        pos = p;
        type = locationType.pickup;
        pType = pickupType.weapon;

        combatLocation = lib.createCombatLocation(diff);
        combatLocation.weapons.Add(w);

        pickupWeapon = w;
    }

    //  armor
    public PickupLocation(Vector2 p, Armor a, PresetLibrary lib, GameInfo.diffLvl diff) {
        pos = p;
        type = locationType.pickup;
        pType = pickupType.armor;

        combatLocation = lib.createCombatLocation(diff);
        combatLocation.armor.Add(a);

        pickupArmor = a;
    }

    //  consumable
    public PickupLocation(Vector2 p, Consumable con, int count, PresetLibrary lib, GameInfo.diffLvl diff) {
        pos = p;
        pType = pickupType.consumable;

        combatLocation = lib.createCombatLocation(diff);
        for(int i = 0; i < count; i++)
            combatLocation.consumables.Add(con);

        pickupConsumable = con;
    }

    //  item
    public PickupLocation(Vector2 p, Item it, PresetLibrary lib, GameInfo.diffLvl diff) {
        pos = p;
        pType = pickupType.item;

        combatLocation = lib.createCombatLocation(diff);
        combatLocation.items.Add(it);

        pickupItem = it;
    }


    public override void enterLocation() {
        GameInfo.setCombatDetails(combatLocation);
        GameInfo.setCurrentLocationAsPickup(this);
        MapLocationHolder.removeLocation(this);
        SceneManager.LoadScene("Combat");
    }

    public override bool isEqualTo(MapLocation other) {
        if(other.type != locationType.pickup)
            return false;

        PickupLocation o = (PickupLocation)other;

        return pos == other.pos &&
            pickupWeapon.isEqualTo(o.pickupWeapon) &&
            pickupArmor.isEqualTo(o.pickupArmor) &&
            pickupConsumable.isEqualTo(o.pickupConsumable) &&
            pickupItem.isEqualTo(o.pickupItem);           
    }
}

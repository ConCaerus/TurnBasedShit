using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLocation : MapLocation {

    //  weapon
    public PickupLocation(Vector2 p, Weapon w, CombatLocation cl) {
        pos = p;
        type = locationType.equipmentPickup;
        sprite.setSprite(w.w_sprite.getSprite());

        combatLocation = cl;
        combatLocation.weapons.Add(w);
    }

    //  armor
    public PickupLocation(Vector2 p, Armor a, CombatLocation cl) {
        pos = p;
        type = locationType.equipmentPickup;
        sprite.setSprite(a.a_sprite.getSprite());

        combatLocation = cl;
        combatLocation.armor.Add(a);
    }

    //  item
    public PickupLocation(Vector2 p, Consumable con, CombatLocation cl) {
        pos = p;
        type = locationType.equipmentPickup;
        sprite.setSprite(con.c_sprite.getSprite());

        combatLocation = cl;
        combatLocation.consumables.Add(con);
    }
}

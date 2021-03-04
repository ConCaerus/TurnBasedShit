using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLocation : MapLocation {

    //  weapon
    public PickupLocation(Vector2 p, Weapon w) {
        pos = p;
        type = locationType.equipmentPickup;
        sprite.setLocation(w.w_sprite.getSprite());

        //  creates a new combatLocation
        var loc = new CombatLocation();
        foreach(var i in Randomizer.getRandomCombatLocation().enemies)
            loc.enemies.Add(i);
        loc.weapons.Add(w);
        combatLocation = loc;
    }

    //  armor
    public PickupLocation(Vector2 p, Armor a) {
        pos = p;
        type = locationType.equipmentPickup;
        sprite.setLocation(a.a_sprite.getSprite());

        //  creates a new combatLocation
        var loc = new CombatLocation();
        foreach(var i in Randomizer.getRandomCombatLocation().enemies)
            loc.enemies.Add(i);
        loc.armor.Add(a);
        combatLocation = loc;
    }

    //  item
    public PickupLocation(Vector2 p, Consumable it) {
        pos = p;
        type = locationType.equipmentPickup;
        sprite.setLocation(it.i_sprite.getSprite());

        //  creates a new combatLocation
        var loc = new CombatLocation();
        foreach(var i in Randomizer.getRandomCombatLocation().enemies)
            loc.enemies.Add(i);
        loc.items.Add(it);
        combatLocation = loc;
    }
}

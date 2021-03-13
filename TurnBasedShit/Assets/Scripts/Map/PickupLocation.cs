using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLocation : MapLocation {

    //  weapon
    public PickupLocation(Vector2 p, Weapon w) {
        pos = p;
        type = locationType.equipmentPickup;
        sprite.setSprite(w.w_sprite.getSprite());

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
        sprite.setSprite(a.a_sprite.getSprite());

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
        sprite.setSprite(it.c_sprite.getSprite());

        //  creates a new combatLocation
        var loc = new CombatLocation();
        foreach(var i in Randomizer.getRandomCombatLocation().enemies)
            loc.enemies.Add(i);
        loc.items.Add(it);
        combatLocation = loc;
    }
}

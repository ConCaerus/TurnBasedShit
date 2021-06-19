﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickupLocation : MapLocation {
    public Weapon pickupWeapon = null;
    public Armor pickupArmor = null;
    public Consumable pickupConsumable = null;
    public Item pickupItem = null;

    //  weapon
    public PickupLocation(Vector2 p, Weapon w, PresetLibrary lib, GameInfo.diffLvl diff) {
        pos = p;
        type = locationType.equipmentPickup;

        combatLocation = lib.createCombatLocation(diff);
        combatLocation.weapons.Add(w);

        pickupWeapon = w;
    }

    //  armor
    public PickupLocation(Vector2 p, Armor a, PresetLibrary lib, GameInfo.diffLvl diff) {
        pos = p;
        type = locationType.equipmentPickup;

        combatLocation = lib.createCombatLocation(diff);
        combatLocation.armor.Add(a);

        pickupArmor = a;
    }

    //  consumable
    public PickupLocation(Vector2 p, Consumable con, int count, PresetLibrary lib, GameInfo.diffLvl diff) {
        pos = p;
        type = locationType.equipmentPickup;

        combatLocation = lib.createCombatLocation(diff);
        for(int i = 0; i < count; i++)
            combatLocation.consumables.Add(con);

        pickupConsumable = con;
    }

    //  item
    public PickupLocation(Vector2 p, Item it, PresetLibrary lib, GameInfo.diffLvl diff) {
        pos = p;
        type = locationType.equipmentPickup;

        combatLocation = lib.createCombatLocation(diff);
        combatLocation.items.Add(it);

        pickupItem = it;
    }


    public override void enterLocation() {
        GameInfo.setCombatDetails(combatLocation);
        GameInfo.setCurrentMapLocation(MapLocationHolder.getIndex((PickupLocation)this));
        MapLocationHolder.removeLocation(this);
        SceneManager.LoadScene("Combat");
    }
}

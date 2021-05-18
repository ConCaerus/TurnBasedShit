using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickupLocation : MapLocation {

    //  weapon
    public PickupLocation(Vector2 p, Weapon w, PresetLibrary lib, GameInfo.diffLvl diff) {
        pos = p;
        type = locationType.equipmentPickup;
        sprite = w.w_sprite;

        combatLocation = lib.createCombatLocation(diff);
        combatLocation.weapons.Add(w);
    }

    //  armor
    public PickupLocation(Vector2 p, Armor a, PresetLibrary lib, GameInfo.diffLvl diff) {
        pos = p;
        type = locationType.equipmentPickup;
        sprite = a.a_sprite;

        combatLocation = lib.createCombatLocation(diff);
        combatLocation.armor.Add(a);
    }

    //  item
    public PickupLocation(Vector2 p, Consumable con, int count, PresetLibrary lib, GameInfo.diffLvl diff) {
        pos = p;
        type = locationType.equipmentPickup;
        sprite = con.c_sprite;

        combatLocation = lib.createCombatLocation(diff);
        for(int i = 0; i < count; i++)
            combatLocation.consumables.Add(con);
    }


    public override void enterLocation() {
        GameInfo.setCombatDetails(combatLocation);
        GameInfo.setCurrentMapLocation(MapLocationHolder.getIndex((PickupLocation)this));
        MapLocationHolder.removeLocation(this);
        SceneManager.LoadScene("Combat");
    }
}

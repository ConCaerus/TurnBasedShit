using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickupLocation : MapLocation {

    //  weapon
    public PickupLocation(Vector2 p, Weapon w, CombatLocation cl) {
        pos = p;
        type = locationType.equipmentPickup;
        sprite = w.w_sprite;

        combatLocation = cl;
        combatLocation.weapons.Add(w);
    }

    //  armor
    public PickupLocation(Vector2 p, Armor a, CombatLocation cl) {
        pos = p;
        type = locationType.equipmentPickup;
        sprite = a.a_sprite;

        combatLocation = cl;
        combatLocation.armor.Add(a);
    }

    //  item
    public PickupLocation(Vector2 p, Consumable con, int count, CombatLocation cl) {
        pos = p;
        type = locationType.equipmentPickup;
        sprite = con.c_sprite;

        combatLocation = cl;
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

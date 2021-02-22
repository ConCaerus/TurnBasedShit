using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitCreator {


    public static UnitClassStats createNewUnitStats() {
        var stats = new UnitClassStats();

        stats.u_name = NameLibrary.getRandomUsableName();
        stats.u_health = Random.Range(5.0f, stats.u_maxHealth);
        stats.u_speed = Random.Range(0, 10.0f);

        //  at some point, equip the new unit with some random weapon and armor presets.
        //  but not now, god not now.

        return stats;
    }
}

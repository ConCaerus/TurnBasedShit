using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Randomizer {

    public static UnitStats randomizePlayerUnitStats(UnitStats stats, PresetLibrary lib) {
        var u = new UnitStats(stats);

        u.u_name = NameLibrary.getRandomUsablePlayerName();
        u.u_power = Random.Range(1.0f, 3.5f);
        u.u_speed = Random.Range(1.0f, 3.5f);
        u.u_defence = Random.Range(1.0f, 3.5f);
        u.u_critChance = Random.Range(0.015f, 0.15f);
        u.u_sprite.randomize(lib);

        int tCount = Random.Range(0, 4);
        for(int i = 0; i < tCount; i++)
            u.u_traits.Add(lib.getRandomUnusedUnitTrait(stats));

        return u;
    }

    public static CombatLocation randomizeCombatLocation(CombatLocation cl) {
        //  this function currently does nothing
        return cl;
    }

    public static HospitalBuilding randomizeBuilding(HospitalBuilding hos) {
        var temp = new HospitalBuilding();
        temp.setEqualTo(hos);
        temp.freeHeals = Random.Range(0, 4);

        switch(GameInfo.getCurrentDiff()) {
            case GameInfo.region.grassland:
            case GameInfo.region.forest:
                temp.pricePerHeal = Random.Range(1, 6);
                break;

            case GameInfo.region.swamp:
                temp.pricePerHeal = Random.Range(5, 11);
                break;

            case GameInfo.region.mountains:
                temp.pricePerHeal = Random.Range(10, 16);
                break;

            case GameInfo.region.hell:
                temp.pricePerHeal = Random.Range(10, 31);
                break;
        }

        return temp;
    }
    public static ChurchBuilding randomizeBuilding(ChurchBuilding ch) {
        var temp = new ChurchBuilding();
        temp.setEqualTo(ch);
        return temp;
    }
    public static ShopBuilding randomizeBuilding(ShopBuilding shop) {
        var temp = new ShopBuilding();
        temp.setEqualTo(shop);
        temp.sellReduction = Random.Range(0.0f, 0.15f);
        temp.priceMod = Random.Range(-0.2f, 0.2f);

        return temp;
    }
    public static CasinoBuilding randomizeBuilding(CasinoBuilding cas) {
        var temp = new CasinoBuilding();
        return temp;
    }
    public static BlacksmithBuilding randomizeBuilding(BlacksmithBuilding blac) {
        var temp = new BlacksmithBuilding();
        return temp;
    }
}

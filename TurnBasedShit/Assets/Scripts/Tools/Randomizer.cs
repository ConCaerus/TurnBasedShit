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


    public static Weapon randomizeWeapon(Weapon we, GameInfo.diffLvl diff) {
        var temp = new Weapon();
        temp = randomizeWeaponStats(we);    //  I know that I pass we only in the first function, don't mess with it.
        temp = randomizeWeaponAttributesBasedOnRegion(temp, diff);

        return temp;
    }
    //  Eventually make it so the stats are affected by the region that the player is in
    public static Weapon randomizeWeaponStats(Weapon we) {
        var temp = new Weapon();
        temp.setEqualTo(we, true);
        temp.w_power = Mathf.Clamp(we.w_power + Random.Range(-10.0f, 25.0f), 1.0f, Mathf.Infinity);
        temp.w_speedMod = we.w_speedMod + Random.Range(-10.0f, 10.0f);

        temp.w_wornAmount = GameInfo.getRandomWornState();

        return temp;
    }
    public static Weapon randomizeWeaponAttributesBasedOnRegion(Weapon we, GameInfo.diffLvl diff) {
        int count = 0;
        switch((int)diff) {
            case 0:
                count = Random.Range(0, 1);
                break;

            case 1:
            case 2:
                count = Random.Range(0, 2);
                break;

            case 3:
            case 4:
                count = Random.Range(0, 3);
                break;

            case 5:
                count = Random.Range(1, 4);
                break;

            case 6:
                count = Random.Range(1, 5);
                break;
        }

        var temp = new Weapon();
        temp.setEqualTo(we, true);

        for(int i = 0; i < count; i++)
            temp.w_attributes.Add(temp.getRandAttribute());

        return temp;
    }

    public static Armor randomizeArmor(Armor ar, GameInfo.diffLvl diff) {
        var temp = new Armor();
        temp = randomizeArmorStats(ar);
        temp = randomizeArmorAttributesBasedOnRegion(temp, diff);

        return temp;
    }
    public static Armor randomizeArmorStats(Armor ar) {
        var temp = new Armor();
        temp.setEqualTo(ar, true);
        temp.a_defence = Mathf.Clamp(ar.a_defence + Random.Range(-10.0f, 25.0f), 1.0f, Mathf.Infinity);
        temp.a_speedMod = ar.a_speedMod + Random.Range(-10.0f, 10.0f);

        temp.a_wornAmount = GameInfo.getRandomWornState();

        return temp;
    }
    public static Armor randomizeArmorAttributesBasedOnRegion(Armor ar, GameInfo.diffLvl diff) {
        int count = 0;
        switch((int)diff) {
            case 0:
                count = Random.Range(0, 2);
                break;

            case 1:
            case 2:
                count = Random.Range(0, 3);
                break;

            case 3:
            case 4:
                count = Random.Range(0, 4);
                break;

            case 5:
                count = Random.Range(1, 4);
                break;

            case 6:
                count = Random.Range(1, 5);
                break;
        }

        var temp = new Armor();
        temp.setEqualTo(ar, true);

        for(int i = 0; i < count; i++)
            temp.a_attributes.Add(temp.getRandAttribute());

        return temp;
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
            case GameInfo.diffLvl.Cake:
            case GameInfo.diffLvl.Easy:
                temp.pricePerHeal = Random.Range(1, 6);
                break;

            case GameInfo.diffLvl.Normal:
            case GameInfo.diffLvl.Inter:
                temp.pricePerHeal = Random.Range(5, 11);
                break;

            case GameInfo.diffLvl.Hard:
            case GameInfo.diffLvl.Heroic:
                temp.pricePerHeal = Random.Range(10, 16);
                break;

            case GameInfo.diffLvl.Legendary:
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameVariables {
    //  variables
    public static int getMaxPlayerUnitNameLength() {
        return 18;
    }

    public static float getExpForDefeatedEnemy(GameInfo.diffLvl lvl) {
        return 10.0f * (1 + (int)lvl);
    }

    //  chances
    //  combat
    public static bool chanceBleed() {
        return chanceOutOfHundred(20);
    }
    public static bool chanceCureBleed() {
        return chanceOutOfHundred(25);
    }
    public static bool chanceStun() {
        return chanceOutOfHundred(15);
    }

    public static bool chanceWeaponDrop() {
        return chanceOutOfHundred(15);
    }
    public static bool chanceArmorDrop() {
        return chanceOutOfHundred(15);
    }
    public static bool chanceConsumableDrop() {
        return chanceOutOfHundred(25);
    }
    public static bool chanceItemDrop() {
        return chanceOutOfHundred(15);
    }

    public static bool chanceEquipmentWornDecrease() {
        return chanceOutOfHundred(10);
    }


    public static bool shouldMemberHaveQuest() {
        return chanceOutOfHundred(35);
    }
    public static bool shouldTownHaveBuilding(Building.type type) {
        switch(type) {
            case Building.type.Church:
                return chanceOutOfHundred(15);
            case Building.type.Hospital:
                return chanceOutOfHundred(50);
            case Building.type.Shop:
                return chanceOutOfHundred(80);
            case Building.type.Casino:
                return chanceOutOfHundred(100);
            case Building.type.Blacksmith:
                return chanceOutOfHundred(100);
        }
        return false;
    }
    public static int createTownMemberCount(int buildingCount) {
        int count = 1;
        for(int i = 0; i < buildingCount; i++) {
            count += Random.Range(0, 3);
        }

        return count;
    }


    //  Building chances
    public static int getRandomCasinoSpinCost() {
        return Random.Range(3, 6);
    }

    public static bool chanceOutOfHundred(int i) {
        return Random.Range(0, 101) <= i;
    }
}

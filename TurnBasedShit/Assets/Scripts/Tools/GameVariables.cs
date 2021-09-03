﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameVariables {
    //  variables
    public static int getMaxPlayerUnitNameLength() {
        return 18;
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

    static bool chanceOutOfHundred(int i) {
        return Random.Range(0, 101) < i;
    }
}

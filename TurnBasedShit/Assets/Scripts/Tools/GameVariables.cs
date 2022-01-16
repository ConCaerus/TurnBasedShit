using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameVariables {
    //  variables
    public static int getMaxPlayerUnitNameLength() {
        return 18;
    }

    public static float getExpForEnemy(GameInfo.region lvl) {
        return 10.0f * (1 + (int)lvl);
    }
    public static float getExpForEnemy(EnemyUnitInstance.type t) {
        return getExpForEnemy(GameInfo.getRegionForEnemyType(t));
    }

    public static bool chanceUnitHasTalent() {
        return chanceOutOfHundred(25);
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
        return chanceOutOfHundred(4);
    }


    //  rewards
    public static int getCoinRewardForQuest(Quest qu) {
        switch(qu.getQuestType()) {
            case Quest.questType.bossFight:
                var b = (BossFightQuest)qu;
                return Random.Range(b.bossUnit.u_level * 5, b.bossUnit.u_level * 10 + 1);

            case Quest.questType.pickup:
                return 0;

            case Quest.questType.delivery:
                var d = (DeliveryQuest)qu;
                return Random.Range(((int)d.deliveryLocation.region + 1) * 5, ((int)d.deliveryLocation.region + 1) * 10 + 1);

            case Quest.questType.kill:
                var k = (KillQuest)qu;
                var min = k.howManyToKill * ((int)GameInfo.getRegionForEnemyType(k.enemyType) + 1) / 5;
                var max = k.howManyToKill * ((int)GameInfo.getRegionForEnemyType(k.enemyType) + 1) / 2;
                return Random.Range(min, max + 1);

            case Quest.questType.rescue:
                var r = (RescueQuest)qu;
                return Random.Range(r.location.unit.u_level * 2, r.location.unit.u_level * 5 + 1);

            case Quest.questType.fishing:
                var f = (FishingQuest)qu;
                return Random.Range((int)(f.fish.coinCost * 1.5f), (int)(f.fish.coinCost * 2.5f) + 1);
        }
        return 0;
    }


    public static float getChanceToCatchFish(GameInfo.fishCatchRate rate) {
        switch(rate) {
            case GameInfo.fishCatchRate.almostImpossible:
                return 0.1f;
            case GameInfo.fishCatchRate.rare:
                return 0.25f;
            case GameInfo.fishCatchRate.normal:
                return .5f;
            case GameInfo.fishCatchRate.easy:
                return .75f;
            default:
                return 1.0f;
        }
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
    public static int createTownNPCCount(int memberCount) {
        var rand = Random.Range(0, 101);
        if(rand < 25)
            return 3;
        if(rand < 50)
            return 2;
        if(rand < 75)
            return 1;
        return 0;
    }

    //  Map chances
    public static bool chanceEnemyBeingSpecial() {
        return chanceOutOfHundred(100);
    }


    //  Building chances
    public static int getRandomCasinoSpinCost() {
        return Random.Range(3, 6);
    }

    public static bool chanceOutOfHundred(int i) {
        return Random.Range(0, 101) <= i;
    }
}

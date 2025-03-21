﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfo {

    public enum state {
        combat, town, map
    }
    public enum region {
        grassland, forest, swamp, mountains, hell
    }
    public enum rarity {
        Common, Uncommon, Unusual, Rare, Legendary
    }
    public enum wornState {
        Old, Used, Normal, Perfect
    }
    public enum fishCatchRate {
        almostImpossible, rare, normal, easy,
    }

    public enum combatUnitType {
        player, slime, groundBird, stumpSpider, rockCrawler, chicken, deadUnit, turtle, spiderLeg, spiderHead
    }


    const string stateTag = "Current Game State";

    //  current combat location that the player is in
    const string combatDetails = "CombatLocation";

    const string currentRegion = "Current Region";
    const string currentMapLocation = "Current Map Location";
    const string currentMapPosX = "Current Map Position x";
    const string currentMapPosY = "Current Map Position y";

    const string nextCombatLocationID = "Next Combat Location ID";
    const string nextObjectHolderID = "Next Object Holder ID";

    const string nextUnitID = "Next Unit ID";

    const string nextWeaponID = "Next Weapon ID";
    const string nextArmorID = "Next Armor ID";
    const string nextUsableID = "Next Usable ID";
    const string nextUnusableID = "Next Unusable ID";
    const string nextItemID = "Next Item ID";

    const string nextTownID = "Next Town ID";
    const string nextTownMemberID = "Next Town Member ID";

    const string nextQuestID = "Next Quest ID";

    const string combatTutorialStatus = "Combat Tutorial Status";
    const string mapTutorialStatus = "Map Tutorial Status";
    const string townTutorialStatus = "Town Tutorial Status";


    public static void clearEverything() {
        clearInventoryInstanceIDQueue();
        clearTownInstanceIDQueue();
        clearQuestInstanceIDQueue();
        clearTownMemberInstanceIDQueue();

        clearCurrentMapPos();

        clearCombatDetails();
        clearCurrentMapLocation();
    }

    public static void clearCombatDetails() {
        SaveData.deleteKey(combatDetails);
    }
    public static void clearCurrentMapLocation() {
        SaveData.deleteKey(currentMapLocation);
    }

    public static void setCombatDetails(CombatLocation cl) {
        var data = JsonUtility.ToJson(cl);
        SaveData.setString(combatDetails, data);
    }

    public static CombatLocation getCombatDetails() {
        var data = SaveData.getString(combatDetails);
        return JsonUtility.FromJson<CombatLocation>(data);
    }

    public static void setCurrentLocationAsTown(TownLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(currentMapLocation, data);
    }
    public static void setCurrentLocationAsPickup(PickupLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(currentMapLocation, data);
    }
    public static void setCurrentLocationAsUpgrade(UpgradeLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(currentMapLocation, data);
    }
    public static void setCurrentLocationAsRescue(RescueLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(currentMapLocation, data);
    }
    public static void setCurrentLocationAsBoss(BossLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(currentMapLocation, data);
    }
    public static void setCurrentLocationAsFishing(FishingLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(currentMapLocation, data);
    }

    public static TownLocation getCurrentLocationAsTown() {
        var data = SaveData.getString(currentMapLocation);
        return JsonUtility.FromJson<TownLocation>(data);
    }
    public static PickupLocation getCurrentLocationAsPickup() {
        var data = SaveData.getString(currentMapLocation);
        return JsonUtility.FromJson<PickupLocation>(data);
    }
    public static UpgradeLocation getCurrentLocationAsUpgrade() {
        var data = SaveData.getString(currentMapLocation);
        return JsonUtility.FromJson<UpgradeLocation>(data);
    }
    public static RescueLocation getCurrentLocationAsRescue() {
        var data = SaveData.getString(currentMapLocation);
        return JsonUtility.FromJson<RescueLocation>(data);
    }
    public static BossLocation getCurrentLocationAsBoss() {
        var data = SaveData.getString(currentMapLocation);
        return JsonUtility.FromJson<BossLocation>(data);
    }

    public static void clearCurrentMapPos() {
        SaveData.deleteKey(currentMapPosX);
        SaveData.deleteKey(currentMapPosY);
    }
    public static void setCurrentMapPos(Vector2 pos) {
        SaveData.setFloat(currentMapPosX, pos.x);
        SaveData.setFloat(currentMapPosY, pos.y);
    }
    public static Vector2 getCurrentMapPos() {
        return new Vector2(SaveData.getFloat(currentMapPosX), SaveData.getFloat(currentMapPosY));
    }

    public static void setCurrentRegion(region reg) {
        SaveData.setInt(currentRegion, (int)reg);
    }
    public static region getCurrentRegion() {
        return (region)SaveData.getInt(currentRegion);
    }


    public static Vector2 getMousePos() {
        if(Camera.main.orthographic)
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        else {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit)) {
                return raycastHit.point;
            }
        }
        return Vector2.zero;
    }


    public static state currentGameState {
        get {
            return (state)SaveData.getInt(stateTag);
        }
        set {
            SaveData.setInt(stateTag, (int)value);
        }
    }
    public static wornState getRandomWornState() {
        return (wornState)Random.Range(0, 5);
    }
    public static Quest.questType getRandomQuestType() {
        return (Quest.questType)Random.Range(0, 4);
    }
    public static region getRandomReg() {
        return (region)Random.Range(0, 5);
    }
    public static region getRegionForEnemyType(combatUnitType t) {  //  needs to be this way
        switch(t) {
            case combatUnitType.groundBird:
                return region.grassland;
            case combatUnitType.slime:
                return region.swamp;
            case combatUnitType.stumpSpider:
                return region.forest;
            case combatUnitType.rockCrawler:
                return region.mountains;
            case combatUnitType.deadUnit:
                return region.hell;
            case combatUnitType.spiderHead:
            case combatUnitType.spiderLeg:
                return region.grassland;
        }
        return (region)(-1);
    }

    public static int getNextCombatLocationInstanceID() {
        int index = SaveData.getInt(nextCombatLocationID);
        SaveData.setInt(nextCombatLocationID, index + 1);
        return index;
    }
    public static int getNextObjectHolderInstanceID() {
        int index = SaveData.getInt(nextObjectHolderID);
        SaveData.setInt(nextObjectHolderID, index + 1);
        return index;
    }

    public static int getNextUnitInstanceID() {
        int index = SaveData.getInt(nextUnitID);
        SaveData.setInt(nextUnitID, index + 1);
        return index;
    }

    public static int getNextCollectableInstanceID(Collectable c) {
        switch(c.type) {
            case Collectable.collectableType.Weapon:
                return getNextWeaponInstanceID();
            case Collectable.collectableType.Armor:
                return getNextArmorInstanceID();
            case Collectable.collectableType.Item:
                return getNextItemInstanceID();
            case Collectable.collectableType.Usable:
                return getNextUsableInstanceID();
            case Collectable.collectableType.Unusable:
                return getNextUnusableInstanceID();
        }
        Debug.LogError("fuck you");
        return -1;
    }
    public static int getNextWeaponInstanceID() {
        int index = SaveData.getInt(nextWeaponID);
        SaveData.setInt(nextWeaponID, index + 1);
        return index;
    }
    public static int getNextArmorInstanceID() {
        int index = SaveData.getInt(nextArmorID);
        SaveData.setInt(nextArmorID, index + 1);
        return index;
    }
    public static int getNextUsableInstanceID() {
        int index = SaveData.getInt(nextUsableID);
        SaveData.setInt(nextUsableID, index + 1);
        return index;
    }
    public static int getNextUnusableInstanceID() {
        int index = SaveData.getInt(nextUnusableID);
        SaveData.setInt(nextUnusableID, index + 1);
        return index;
    }
    public static int getNextItemInstanceID() {
        int index = SaveData.getInt(nextItemID);
        SaveData.setInt(nextItemID, index + 1);
        return index;
    }

    public static int getNextTownInstanceID() {
        int index = SaveData.getInt(nextTownID);
        SaveData.setInt(nextTownID, index + 1);
        return index;
    }
    public static int getNextTownMemberInstanceID() {
        int index = SaveData.getInt(nextTownMemberID);
        SaveData.setInt(nextTownMemberID, index + 1);
        return index;
    }
    public static int getNextQuestInstanceID() {
        int index = SaveData.getInt(nextQuestID);
        SaveData.setInt(nextQuestID, index + 1);
        return index;
    }

    public static void clearInventoryInstanceIDQueue() {
        clearWeaponInstanceIDQueue();
        clearArmorInstanceIDQueue();
        clearUsableInstanceIDQueue();
        clearUnusableInstanceIDQueue();
        clearItemInstanceIDQueue();
    }
    public static void clearUnitInstanceIDQueue() {
        SaveData.deleteKey(nextUnitID);
    }
    public static void clearWeaponInstanceIDQueue() {
        SaveData.deleteKey(nextWeaponID);
    }
    public static void clearArmorInstanceIDQueue() {
        SaveData.deleteKey(nextArmorID);
    }
    public static void clearUsableInstanceIDQueue() {
        SaveData.deleteKey(nextUsableID);
    }
    public static void clearUnusableInstanceIDQueue() {
        SaveData.deleteKey(nextUnusableID);
    }
    public static void clearItemInstanceIDQueue() {
        SaveData.deleteKey(nextItemID);
    }

    public static void clearTownInstanceIDQueue() {
        SaveData.deleteKey(nextTownID);
    }
    public static void clearTownMemberInstanceIDQueue() {
        SaveData.deleteKey(nextTownMemberID);
    }
    public static void clearQuestInstanceIDQueue() {
        SaveData.deleteKey(nextQuestID);
    }

    //  tutorial status
    public static bool hasCompletedCombatTutorial() {
        return SaveData.getInt(combatTutorialStatus) != 0;
    }
    public static void completeCombatTutorial() {
        SaveData.setInt(combatTutorialStatus, 1);
    }
    public static bool hasCompletedMapTutorial() {
        return SaveData.getInt(mapTutorialStatus) != 0;
    }
    public static void completeMapTutorial() {
        SaveData.setInt(mapTutorialStatus, 1);
    }
    public static bool hasCompletedTownTutorial() {
        return SaveData.getInt(townTutorialStatus) != 0;
    }
    public static void completeTownTutorial() {
        SaveData.setInt(townTutorialStatus, 1);
    }
}

[System.Serializable]
public class DeathInfo {
    public string nameOfKiller;
    public killCause causeOfDeath;
    public GameInfo.combatUnitType combatType;

    [System.Serializable]
    public enum killCause {
        bleed, murdered
    }

    public DeathInfo(killCause cause, GameObject killer = null) {
        causeOfDeath = cause;

        if(cause == killCause.murdered && killer != null) {
            if(killer.GetComponent<EnemyUnitInstance>() != null)
                combatType = killer.GetComponent<EnemyUnitInstance>().stats.u_type;
            else
                combatType = (GameInfo.combatUnitType)(-1);

            nameOfKiller = killer.GetComponent<UnitClass>().stats.u_name;
        }
    }
}

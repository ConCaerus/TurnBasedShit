using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfo {

    public enum state {
        Combat, Town, Map
    }
    public enum diffLvl {
        Cake, Easy, Normal, Inter, Hard, Heroic, Legendary
    }
    public enum rarityLvl {
        Worthless, Common, Uncommon, Unusual, Rare, Legendary, Mythical
    }
    public enum element {
        Bronze, Gold, Iron, Obsidian
    }
    public enum wornState {
        Old, Used, Normal, New, Perfect
    }


    const string stateTag = "Current Game State";

    //  current combat location that the player is in
    public const string combatDetails = "CombatLocation";
    public const string currentDiffRegion = "Difficulty Region";

    public const string currentMapLocation = "Current Map Location";


    public const string nextWeaponID = "Next Weapon ID";
    public const string nextArmorID = "Next Armor ID";
    public const string nextConsumableID = "Next Consumable ID";
    public const string nextItemID = "Next Item ID";

    public const string nextTownID = "Next Town ID";
    public const string nextTownMemberID = "Next Town Member ID";

    public const string nextQuestID = "Next Quest ID";


    public static void resetCombatDetails() {
        SaveData.deleteKey(combatDetails);
    }
    public static void resetCurrentMapLocation() {
        SaveData.deleteKey(currentMapLocation);
    }

    public static void setCombatDetails(CombatLocation cl) {
        var data = JsonUtility.ToJson(cl);
        SaveData.setString(combatDetails, data);
    }
    public static void setCurrentRegionDiff(diffLvl lvl) {
        int temp = (int)lvl;
        SaveData.setInt(currentDiffRegion, temp);
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
    public static void setCurrentLocationAsNest(NestLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(currentMapLocation, data);
    }
    public static void setCurrentLocationAsBoss(BossLocation loc) {
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
    public static NestLocation getCurrentLocationAsNest() {
        var data = SaveData.getString(currentMapLocation);
        return JsonUtility.FromJson<NestLocation>(data);
    }
    public static BossLocation getCurrentLocationAsBoss() {
        var data = SaveData.getString(currentMapLocation);
        return JsonUtility.FromJson<BossLocation>(data);
    }

    public static diffLvl getCurrentDiff() {
        var data = SaveData.getInt(currentDiffRegion);
        return (diffLvl)data;
    }


    public static Vector2 getMousePos() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
    public static int getNextConsumableInstanceID() {
        int index = SaveData.getInt(nextConsumableID);
        SaveData.setInt(nextConsumableID, index + 1);
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
}

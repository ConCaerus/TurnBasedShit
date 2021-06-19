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

    public const string currentMapLocationIndex = "Current Map Location Index";


    public static void resetCombatDetails() {
        SaveData.deleteKey(combatDetails);
    }
    public static void resetCurrentMapLocation() {
        SaveData.deleteKey(currentMapLocationIndex);
    }

    public static void setCombatDetails(CombatLocation cl) {
        var data = JsonUtility.ToJson(cl);
        SaveData.setString(combatDetails, data);
    }
    public static void setCurrentMapLocation(int index) {
        SaveData.setInt(currentMapLocationIndex, index);
    }

    public static CombatLocation getCombatDetails() {
        var data = SaveData.getString(combatDetails);
        return JsonUtility.FromJson<CombatLocation>(data);
    }
    public static int getCurrentMapLocationIndex() {
        return SaveData.getInt(currentMapLocationIndex);
    }
    public static MapLocation getCurrentMapLocation() {
        return MapLocationHolder.getMapLocation(SaveData.getInt(currentMapLocationIndex));
    }
    public static TownLocation getCurrentMapLocationAsTown() {
        return MapLocationHolder.getTownLocation(SaveData.getInt(currentMapLocationIndex));
    }
    public static PickupLocation getCurrentMapLocationAsPickup() {
        return MapLocationHolder.getPickupLocation(SaveData.getInt(currentMapLocationIndex));
    }
    public static UpgradeLocation getCurrentMapLocationAsUpgrade() {
        return MapLocationHolder.getUpgradeLocation(SaveData.getInt(currentMapLocationIndex));
    }
    public static diffLvl getDiffRegion() {
        var data = SaveData.getString(currentDiffRegion);
        if(string.IsNullOrEmpty(data))
            return 0;
        return (diffLvl)JsonUtility.FromJson<int>(data);
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
}

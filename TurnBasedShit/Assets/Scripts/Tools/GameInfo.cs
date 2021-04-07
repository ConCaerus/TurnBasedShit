using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfo {

    public enum state {
        combat, town, map
    }
    public enum diffLvl {
        cake, easy, normal, inter, hard, heroic, legendary
    }
    public enum rarityLvl {
        worthless, common, uncommon, unusual, rare, legendary, mythical
    }


    const string stateTag = "Current Game State";

    //  current combat location that the player is in
    public const string combatDetailsTag = "CombatLocation";
    public const string currentDiffRegion = "Difficulty Region";

    public const string currentTownIndex = "Current Town Index";


    public static void resetCombatDetails() {
        var data = JsonUtility.ToJson(new CombatLocation());
        SaveData.setString(combatDetailsTag, data);
    }
    public static void resetCurrentTownIndex() {
        SaveData.setInt(currentTownIndex, -1);
    }

    public static void setCombatDetails(CombatLocation cl) {
        var data = JsonUtility.ToJson(cl);
        SaveData.setString(combatDetailsTag, data);
    }
    public static void setCurrentTownIndex(int index) {
        SaveData.setInt(currentTownIndex, index);
    }

    public static CombatLocation getCombatDetails() {
        var data = SaveData.getString(combatDetailsTag);
        return JsonUtility.FromJson<CombatLocation>(data);
    }
    public static int getCurrentTownIndex() {
        return SaveData.getInt(currentTownIndex);
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
}

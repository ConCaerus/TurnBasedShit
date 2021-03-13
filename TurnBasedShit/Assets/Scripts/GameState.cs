using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState {

    public enum states {
        combat, town, map
    }

    //  current combat location that the player is in
    public const string combatDetailsTag = "CombatLocation";



    public static void resetCombatDetails() {
        var data = JsonUtility.ToJson(new CombatLocation());
        SaveData.setString(combatDetailsTag, data);
    }

    public static void setCombatDetails(CombatLocation cl) {
        var data = JsonUtility.ToJson(cl);
        SaveData.setString(combatDetailsTag, data);
    }
    public static CombatLocation getCombatDetails() {
        var data = SaveData.getString(combatDetailsTag);
        return JsonUtility.FromJson<CombatLocation>(data);
    }


    public static Vector2 getMousePos() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}

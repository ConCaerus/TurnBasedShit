using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Graveyard {
    public const string graveyardSizeTag = "Graveyard Size";
    public static string deadTag(int index) { return "Graveyard" + index.ToString(); }



    public static void clearGraveyard() {
        for(int i = 0; i < SaveData.getInt(graveyardSizeTag); i++) {
            SaveData.deleteKey(deadTag(i));
        }
        SaveData.deleteKey(graveyardSizeTag);
    }

    public static void addUnit(UnitStats stats) {
        int index = SaveData.getInt(graveyardSizeTag);
        var data = JsonUtility.ToJson(stats);

        SaveData.setString(deadTag(index), data);
        SaveData.setInt(graveyardSizeTag, index + 1);
    }
    public static void removeUnit(UnitStats stats) {
        if(stats == null || stats.isEmpty())
            return;
        List<UnitStats> temp = new List<UnitStats>();
        for(int i = 0; i < getDeadCount(); i++) {
            UnitStats mem = getDeadStats(i);
            if(mem != null && !mem.isEmpty() && !mem.isEqualTo(stats))
                temp.Add(mem);
        }

        clearGraveyard();
        foreach(var i in temp)
            addUnit(i);
    }

    public static void overrideUnit(UnitStats stats) {
        int index = getDeadIndex(stats);
        if(index == -1)
            return;
        var data = JsonUtility.ToJson(stats);
        SaveData.setString(deadTag(index), data);
    }
    public static void overrideUnit(int i, UnitStats stats) {
        var data = JsonUtility.ToJson(stats);
        SaveData.setString(deadTag(i), data);
    }


    public static int getDeadCount() {
        return SaveData.getInt(graveyardSizeTag);
    }
    public static int getDeadIndex(UnitStats stats) {
        for(int i = 0; i < getDeadCount(); i++) {
            if(getDeadStats(i) == stats)
                return i;
        }
        return -1;
    }
    public static UnitStats getDeadStats(int index) {
        var data = SaveData.getString(deadTag(index));
        return JsonUtility.FromJson<UnitStats>(data);
    }
}

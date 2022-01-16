using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Graveyard {
    const string holderTag = "GraveyardHolderTag";

    public static ObjectHolder getHolder() {
        var data = SaveData.getString(holderTag);
        return JsonUtility.FromJson<ObjectHolder>(data);
    }
    static void saveHolder(ObjectHolder holder) {
        var data = JsonUtility.ToJson(holder);
        SaveData.setString(holderTag, data);
    }


    public static void clear() {
        saveHolder(new ObjectHolder());
    }


    public static void addUnit(UnitStats unit) {
        if(unit == null || unit.isEmpty())
            return;

        if(getHolder() == null)
            saveHolder(new ObjectHolder());

        var holder = getHolder();
        holder.addObject<UnitStats>(unit);
        saveHolder(holder);
    }
    public static void overrideUnit(int index, UnitStats unit) {
        if(unit == null || unit.isEmpty() || index == -1)
            return;
        var holder = getHolder();
        holder.overrideObject<UnitStats>(index, unit);
        saveHolder(holder);
    }
    public static void removeUnit(UnitStats unit) {
        if(unit == null || unit.isEmpty())
            return;
        var holder = getHolder();
        holder.removeObject<UnitStats>(holder.getUnitStatsIndex(unit));
        saveHolder(holder);
    }

    /*
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

        int startingIndex = getDeadIndex(stats);
        for(int i = startingIndex; i < getDeadCount(); i++) {
            overrideUnit(i, getDeadStats(i + 1));
        }

        SaveData.setInt(graveyardSizeTag, getDeadCount() - 1);
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
    } */
}

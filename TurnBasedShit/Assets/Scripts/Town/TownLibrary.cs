using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TownLibrary {
    static string townTag(int index) { return "Town:" + index.ToString(); }

    const string townCount = "Town Count";




    public static void clearTowns() {
        for(int i = 0; i < SaveData.getInt(townCount); i++) {
            SaveData.deleteKey(townTag(i));
        }
        SaveData.deleteKey(townCount);
    }

    public static void addNewTown(Town t) {
        var data = JsonUtility.ToJson(t);
        SaveData.setString(townTag(SaveData.getInt(townCount)), data);
        SaveData.setInt(townCount, SaveData.getInt(townCount) + 1);
    }
    public static Town addNewTownAndSetIndex(Town t) {
        t.t_index = SaveData.getInt(townCount);
        addNewTown(t);
        return t;
    }

    public static void deleteTown(Town t) {
        var data = JsonUtility.ToJson(t);

        bool past = false;
        for(int i = 0; i < SaveData.getInt(townCount); i++) {
            var tData = SaveData.getString(townTag(i));

            if(data == tData && !past) {
                SaveData.deleteKey(townTag(i));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(townTag(i));
                overrideTown(i - 1, JsonUtility.FromJson<Town>(data));
            }
        }
        SaveData.setInt(townCount, SaveData.getInt(townCount) - 1);
    }
    public static void deleteTown(int index) {
        var data = SaveData.getString(townTag(index));
        if(!string.IsNullOrEmpty(data))
            deleteTown(JsonUtility.FromJson<Town>(data));
    }

    public static void overrideTown(int index, Town t) {
        var data = JsonUtility.ToJson(t);
        SaveData.setString(townTag(index), data);
    }

    public static int getTownCount() {
        return SaveData.getInt(townCount);
    }

    public static Town getTown(int index) {
        var data = SaveData.getString(townTag(index));
        if(!string.IsNullOrEmpty(data))
            return JsonUtility.FromJson<Town>(data);
        return null;
    }
}

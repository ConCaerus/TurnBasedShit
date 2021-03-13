using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapLocationHolder {
    static string locationTag(int i) { return "MapLocation" + i.ToString(); }
    static string locationCountTag() { return "MapLocationCount"; }


    public static void clearSaveData() {
        for(int i = 0; i < SaveData.getInt(locationCountTag()); i++) {
            SaveData.deleteKey(locationTag(i));
        }
        SaveData.deleteKey(locationCountTag());
        SaveData.save();
    }

    public static void saveNewLocation(MapLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(locationTag(getLocationCount()), data);

        SaveData.setInt(locationCountTag(), SaveData.getInt(locationCountTag()) + 1);
        SaveData.save();
    }

    public static void removeLocation(MapLocation loc) {
        int index = 0;
        bool shrinkCount = false;

        for(int i = 0; i < SaveData.getInt(locationCountTag()); i++) {
            var data = SaveData.getString(locationTag(i));
            var temp = JsonUtility.FromJson<MapLocation>(data);

            //  remove this location
            if(temp.equals(loc)) {
                shrinkCount = true;
            }

            //  else set new order for the unit
            else {
                data = JsonUtility.ToJson(temp);
                SaveData.setString(locationTag(index), data);
                index++;
            }
        }
        if(shrinkCount) {
            SaveData.deleteKey(locationTag(getLocationCount() - 1));
            SaveData.setInt(locationCountTag(), SaveData.getInt(locationCountTag()) - 1);
        }
        SaveData.save();
    }


    public static int getLocationCount() {
        return SaveData.getInt(locationCountTag());
    }
    public static MapLocation getMapLocation(int index) {
        var data = SaveData.getString(locationTag(index));
        var temp = JsonUtility.FromJson<MapLocation>(data);

        return temp;
    }
}

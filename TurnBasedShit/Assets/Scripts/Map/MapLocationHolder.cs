using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapLocationHolder {
    static string locationTag(int i) { return "MapLocation" + i.ToString(); }
    static string locationCountTag() { return "MapLocationCount"; }


    public static void clearSaveData() {
        for(int i = 0; i < PlayerPrefs.GetInt(locationCountTag()); i++) {
            PlayerPrefs.DeleteKey(locationTag(i));
        }
        PlayerPrefs.DeleteKey(locationCountTag());
        PlayerPrefs.Save();
    }

    public static void saveNewLocation(MapLocation loc) {
        var data = JsonUtility.ToJson(loc);
        PlayerPrefs.SetString(locationTag(getLocationCount()), data);

        PlayerPrefs.SetInt(locationCountTag(), PlayerPrefs.GetInt(locationCountTag()) + 1);
        PlayerPrefs.Save();
    }

    public static void removeLocation(MapLocation loc) {
        int index = 0;
        bool shrinkCount = false;

        for(int i = 0; i < PlayerPrefs.GetInt(locationCountTag()); i++) {
            var data = PlayerPrefs.GetString(locationTag(i));
            var temp = JsonUtility.FromJson<MapLocation>(data);

            //  remove this location
            if(temp.equals(loc)) {
                shrinkCount = true;
            }

            //  else set new order for the unit
            else {
                data = JsonUtility.ToJson(temp);
                PlayerPrefs.SetString(locationTag(index), data);
                index++;
            }
        }
        if(shrinkCount) {
            PlayerPrefs.DeleteKey(locationTag(getLocationCount() - 1));
            PlayerPrefs.SetInt(locationCountTag(), PlayerPrefs.GetInt(locationCountTag()) - 1);
        }
        PlayerPrefs.Save();
    }


    public static int getLocationCount() {
        return PlayerPrefs.GetInt(locationCountTag());
    }
    public static MapLocation getMapLocation(int index) {
        var data = PlayerPrefs.GetString(locationTag(index));
        var temp = JsonUtility.FromJson<MapLocation>(data);

        return temp;
    }
}

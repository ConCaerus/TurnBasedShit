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
    }

    public static void saveNewLocation(TownLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(locationTag(getLocationCount()), data);

        SaveData.setInt(locationCountTag(), SaveData.getInt(locationCountTag()) + 1);
    }
    public static void saveNewLocation(PickupLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(locationTag(getLocationCount()), data);

        SaveData.setInt(locationCountTag(), SaveData.getInt(locationCountTag()) + 1);
    }
    public static void saveNewLocation(UpgradeLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(locationTag(getLocationCount()), data);

        SaveData.setInt(locationCountTag(), SaveData.getInt(locationCountTag()) + 1);
    }

    public static void removeLocation(MapLocation loc) {
        switch(loc.type) {
            case MapLocation.locationType.town:
                removeTownLocation((TownLocation)loc);
                break;
            case MapLocation.locationType.equipmentPickup:
                removePickupLocation((PickupLocation)loc);
                break;
            case MapLocation.locationType.equipmentUpgrade:
                removeUpgradeLocation((UpgradeLocation)loc);
                break;
        }
    }
    public static void removeTownLocation(TownLocation loc) {
        int index = 0;
        bool shrinkCount = false;

        for(int i = 0; i < SaveData.getInt(locationCountTag()); i++) {
            var data = SaveData.getString(locationTag(i));
            var temp = JsonUtility.FromJson<TownLocation>(data);

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
    }
    public static void removePickupLocation(PickupLocation loc) {
        int index = 0;
        bool shrinkCount = false;

        for(int i = 0; i < SaveData.getInt(locationCountTag()); i++) {
            var data = SaveData.getString(locationTag(i));
            var temp = JsonUtility.FromJson<PickupLocation>(data);

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
    }
    public static void removeUpgradeLocation(UpgradeLocation loc) {
        int index = 0;
        bool shrinkCount = false;

        for(int i = 0; i < SaveData.getInt(locationCountTag()); i++) {
            var data = SaveData.getString(locationTag(i));
            var temp = JsonUtility.FromJson<UpgradeLocation>(data);

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
    }


    public static int getLocationCount() {
        return SaveData.getInt(locationCountTag());
    }
    public static int getTownCount() {
        int count = 0;

        for(int i = 0; i < getLocationCount(); i++) {
            var data = SaveData.getString(locationTag(i));
            if(JsonUtility.FromJson<TownLocation>(data).type == MapLocation.locationType.town)
                count++;
        }
        return count;
    }
    public static int getPickupCount() {
        int count = 0;

        for(int i = 0; i < getLocationCount(); i++) {
            var data = SaveData.getString(locationTag(i));
            if(JsonUtility.FromJson<PickupLocation>(data).type == MapLocation.locationType.equipmentPickup)
                count++;
        }
        return count;
    }
    public static int getUpgradeCount() {
        int count = 0;

        for(int i = 0; i < getLocationCount(); i++) {
            var data = SaveData.getString(locationTag(i));
            if(JsonUtility.FromJson<UpgradeLocation>(data).type == MapLocation.locationType.equipmentUpgrade)
                count++;
        }
        return count;
    }

    public static MapLocation.locationType getLocationTypeForMapLocation(int index) {
        var loc = getMapLocation(index);
        return loc.type;
    }
    public static MapLocation getMapLocation(int index) {
        if(getTownLocation(index) != null)
            return getTownLocation(index);
        if(getPickupLocation(index) != null)
            return getPickupLocation(index);
        if(getUpgradeLocation(index) != null)
            return getUpgradeLocation(index);
        return null;
    }
    public static TownLocation getTownLocation(int index) {
        var data = SaveData.getString(locationTag(index));
        var temp = JsonUtility.FromJson<TownLocation>(data);

        if(temp != null && temp.type == MapLocation.locationType.town)
            return temp;
        return null;
    }
    public static PickupLocation getPickupLocation(int index) {
        var data = SaveData.getString(locationTag(index));
        var temp = JsonUtility.FromJson<PickupLocation>(data);

        if(temp != null && temp.type == MapLocation.locationType.equipmentPickup)
            return temp;
        return null;
    }
    public static UpgradeLocation getUpgradeLocation(int index) {
        var data = SaveData.getString(locationTag(index));
        var temp = JsonUtility.FromJson<UpgradeLocation>(data);

        if(temp != null && temp.type == MapLocation.locationType.equipmentUpgrade)
            return temp;
        return null;
    }


    public static int getIndex(MapLocation loc) {
        for(int i = 0; i < getLocationCount(); i++) {
            if(getMapLocation(i) == loc)
                return i;
        }
        return -1;
    }
    public static int getIndex(TownLocation loc) {
        for(int i = 0; i < getLocationCount(); i++) {
            if(getMapLocation(i) == loc)
                return i;
        }
        return -1;
    }
    public static int getIndex(PickupLocation loc) {
        for(int i = 0; i < getLocationCount(); i++) {
            if(getMapLocation(i) == loc)
                return i;
        }
        return -1;
    }
    public static int getIndex(UpgradeLocation loc) {
        for(int i = 0; i < getLocationCount(); i++) {
            if(getMapLocation(i) == loc)
                return i;
        }
        return -1;
    }
}

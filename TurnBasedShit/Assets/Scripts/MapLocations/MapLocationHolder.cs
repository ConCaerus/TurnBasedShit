using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapLocationHolder {
    const string holderTag = "MapLocationHolderTag";

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


    public static void addLocation(MapLocation loc) {
        if(loc == null)
            return;

        if(getHolder() == null)
            saveHolder(new ObjectHolder());

        var holder = getHolder();
        holder.addObject<MapLocation>(loc);
        saveHolder(holder);
    }
    public static void overrideLocation(int index, MapLocation loc) {
        if(loc == null || index == -1)
            return;
        var holder = getHolder();
        holder.overrideObject<MapLocation>(index, loc);
        saveHolder(holder);
    }
    public static void overrideLocationOfSameType(MapLocation loc) {
        if(loc == null)
            return;
        var holder = getHolder();
        holder.overrideMapLocationOfSameType(loc);
        saveHolder(holder);
    }
    public static void removeLocation(MapLocation loc) {
        if(loc == null)
            return;
        var holder = getHolder();
        holder.removeMapLocation(loc);
        saveHolder(holder);
    }


    public static void populateMapLocations(PresetLibrary lib) {
        //  these are the only ones that need to be populated on start
        //  these are the number of locations to be used across all of the regions
        int upgradeCount = Random.Range(10, 26);
        int fishCount = Random.Range(10, 31);
        int eyeCount = Random.Range(10, 51);
        int lootCount = Random.Range(10, 31);

        for(int i = 0; i < upgradeCount; i++) 
            lib.createUpgradeLocation(GameInfo.getRandomReg(), true);
        for(int i = 0; i < fishCount; i++) 
            lib.createFishingLocation(GameInfo.getRandomReg(), true);
        for(int i = 0; i < eyeCount; i++) 
            lib.createEyeLocation(GameInfo.getRandomReg(), true);
        for(int i = 0; i < lootCount; i++)
            lib.createLootLocation(GameInfo.getRandomReg(), true);

        //  bridges
        float lastY = Map.getRandPos().y;
        for(int i = 0; i < 5; i++) {
            if(i > 0)   //  doesn't add a privious bridge to grasslands
                lib.createBridgeLocation(lastY, false, (GameInfo.region)i, true);
            else
                GameInfo.setCurrentMapPos(new Vector2(Map.leftBound(), lastY));

            if(i < 4) { //  doesn't add a next bridge for hell
                lastY = Map.getRandPos().y;
                lib.createBridgeLocation(lastY, true, (GameInfo.region)i, true);
            }
        }
    }
    public static MapLocation getLocationAtPos(Vector2 p) {
        foreach(var i in getHolder().getMapLocations()) {
            if(i.pos == p)
                return i;
        }

        return null;
    }
    public static TownLocation getRandomTownLocationWithBuilding(Building.type type) {
        List<TownLocation> locs = new List<TownLocation>();
        for(int i = 0; i < getHolder().getObjectCount<TownLocation>(); i++) {
            if(getHolder().getObject<TownLocation>(i).town.hasBuilding(type))
                locs.Add(getHolder().getObject<TownLocation>(i));
        }
        if(locs.Count == 0)
            return null;
        return locs[Random.Range(0, locs.Count)];
    }
}

    public class locationFindInfo {
    public MapLocation.locationType type = (MapLocation.locationType)(-1);
    public int referenceIndex = -1;

    public locationFindInfo(MapLocation.locationType t, int ind) {
        type = t;
        referenceIndex = ind;
    }
}

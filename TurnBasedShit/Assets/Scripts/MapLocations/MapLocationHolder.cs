using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapLocationHolder {
    static string holderTag(GameInfo.region reg) {
        if(reg == (GameInfo.region)(-1))
            return null;
        return "MapLocationHolderTag: " + reg.ToString();
    }

    public static ObjectHolder getHolder(GameInfo.region reg) {
        var data = SaveData.getString(holderTag(reg));
        return JsonUtility.FromJson<ObjectHolder>(data);
    }
    static void saveHolder(ObjectHolder holder, GameInfo.region reg) {
        var data = JsonUtility.ToJson(holder);
        SaveData.setString(holderTag(reg), data);
    }


    public static void clear() {
        for(int i = 0; i < 5; i++)
            saveHolder(new ObjectHolder(), (GameInfo.region)i);
    }


    public static void addLocation(MapLocation loc) {
        if(loc == null)
            return;

        if(getHolder(loc.region) == null)
            saveHolder(new ObjectHolder(), loc.region);

        var holder = getHolder(loc.region);
        holder.addObject<MapLocation>(loc);
        saveHolder(holder, loc.region);
    }
    public static void overrideLocation(int index, MapLocation loc) {
        if(loc == null || index == -1)
            return;
        var holder = getHolder(loc.region);
        holder.overrideObject<MapLocation>(index, loc);
        saveHolder(holder, loc.region);
    }
    public static void overrideLocationOfSameType(MapLocation loc) {
        if(loc == null)
            return;
        var holder = getHolder(loc.region);
        holder.overrideMapLocationOfSameType(loc);
        saveHolder(holder, loc.region);
    }
    public static void removeLocation(MapLocation loc) {
        if(loc == null)
            return;
        var holder = getHolder(loc.region);
        holder.removeMapLocation(loc);
        saveHolder(holder, loc.region);
    }
    public static void removeLocation<T>(int index, GameInfo.region reg) {
        var holder = getHolder(reg);
        holder.removeObject<T>(index);
        saveHolder(holder, reg);
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
            if(i > 0)   //  if not grassland, add a prev bridge
                lib.createBridgeLocation(lastY, false, (GameInfo.region)i, true);

            if(i < 4) { //  if not hell, add a next bridge
                lastY = Map.getRandPos().y;
                lib.createBridgeLocation(lastY, true, (GameInfo.region)i, true);
            }
        }
    }
    public static MapLocation getLocationAtPos(Vector2 p, GameInfo.region reg) {
        foreach(var i in getHolder(reg).getMapLocations()) {
            if(i.pos == p)
                return i;
        }

        return null;
    }
    public static TownLocation getRandomTownLocationWithBuilding(Building.type type, GameInfo.region reg) {
        List<TownLocation> locs = new List<TownLocation>();
        for(int i = 0; i < getHolder(reg).getObjectCount<TownLocation>(); i++) {
            if(getHolder(reg).getObject<TownLocation>(i).town.hasBuilding(type))
                locs.Add(getHolder(reg).getObject<TownLocation>(i));
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

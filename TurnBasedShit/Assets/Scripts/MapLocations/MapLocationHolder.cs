using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapLocationHolder {
    const string holderTag = "InventoryHolderTag";

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
        int upgradeCount = Random.Range(10, 26);
        //int nestCount = Random.Range(7, 16);
        int fishCount = Random.Range(10, 31);
        int eyeCount = Random.Range(10, 51);

        for(int i = 0; i < upgradeCount; i++) {
            var u = lib.createUpgradeLocation(GameInfo.getRandomReg());
            addLocation(u);
        }
        /*
        for(int i = 0; i < nestCount; i++) {
            var n = lib.createNestLocation();
            addLocation(n);
        }*/
        for(int i = 0; i < fishCount; i++) {
            var f = lib.createFishingLocation(GameInfo.getRandomReg());
            addLocation(f);
        }
        for(int i = 0; i < eyeCount; i++) {
            var e = lib.createEyeLocation(GameInfo.getRandomReg());
            addLocation(e);
        }

        //  bridges
        float lastY = Map.getRandPos().y;
        for(int i = 0; i < 5; i++) {
            if(i > 0)   //  doesn't add a privious bridge to grasslands
                addLocation(lib.createBridgeLocation(lastY, false, (GameInfo.region)i));
            else
                GameInfo.setCurrentMapPos(new Vector2(Map.leftBound(), lastY));

            if(i < 4) { //  doesn't add a next bridge for hell
                lastY = Map.getRandPos().y;
                addLocation(lib.createBridgeLocation(lastY, true, (GameInfo.region)i));
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

    /*
    static string townTag(int i) { return "TownLocation" + i.ToString(); }
    static string pickupTag(int i) { return "PickupLocation" + i.ToString(); }
    static string upgradeTag(int i) { return "UpgradeLocation" + i.ToString(); }
    static string rescueTag(int i) { return "RescueLocation" + i.ToString(); }
    static string nestTag(int i) { return "NestLocation" + i.ToString(); }
    static string bossTag(int i) { return "BossLocation" + i.ToString(); }
    static string fishTag(int i) { return "FishingLocation" + i.ToString(); }
    static string eyeTag(int i) { return "EyeLocation" + i.ToString(); }
    static string bridgeTag(int i) { return "BridgeLocation" + i.ToString(); }

    static string townCountTag = "TownLocationCount";
    static string pickupCountTag = "PickupLocationCount";
    static string upgradeCountTag = "UpgradeLocationCount";
    static string rescueCountTag = "RescueLocationCount";
    static string nestCountTag = "NestLocationCount";
    static string bossCountTag = "BossLocationCount";
    static string fishCountTag = "FishingLocationCount";
    static string eyeCountTag = "EyeLocationCount";
    static string bridgeCountTag = "BridgeLocationCount";

    public static void populateMapLocations(PresetLibrary lib) {
        //  these are the only ones that need to be populated on start
        int upgradeCount = Random.Range(10, 26);
        //int nestCount = Random.Range(7, 16);
        int fishCount = Random.Range(10, 31);
        int eyeCount = Random.Range(10, 51);

        for(int i = 0; i < upgradeCount; i++) {
            var u = lib.createUpgradeLocation(GameInfo.getRandomReg());
            addLocation(u);
        }
        /*
        for(int i = 0; i < nestCount; i++) {
            var n = lib.createNestLocation();
            addLocation(n);
        }
        for(int i = 0; i < fishCount; i++) {
            var f = lib.createFishingLocation(GameInfo.getRandomReg());
            addLocation(f);
        }
        for(int i = 0; i < eyeCount; i++) {
            var e = lib.createEyeLocation(GameInfo.getRandomReg());
            addLocation(e);
        }

        //  bridges
        float lastY = Map.getRandPos().y;
        for(int i = 0; i < 5; i++) {
            if(i > 0)   //  doesn't add a privious bridge to grasslands
                addLocation(lib.createBridgeLocation(lastY, false, (GameInfo.region)i));
            else
                GameInfo.setCurrentMapPos(new Vector2(Map.leftBound(), lastY));

            if(i < 4) { //  doesn't add a next bridge for hell
                lastY = Map.getRandPos().y;
                addLocation(lib.createBridgeLocation(lastY, true, (GameInfo.region)i));
            }
        }
    }

    public static void clear() {
        clearTownLocations();
        clearPickupLocations();
        clearUpgradeLocations();
        clearRescueLocations();
        clearNestLocations();
        clearBossLocations();
        clearFishingLocations();
        clearEyeLocations();
        clearBridgeLocations();
    }
    public static void clearTownLocations() {
        for(int i = 0; i < getTownCount() + 10; i++) {
            SaveData.deleteKey(townTag(i));
        }

        SaveData.deleteKey(townCountTag);
        GameInfo.clearTownInstanceIDQueue();
    }
    public static void clearPickupLocations() {
        for(int i = 0; i < getPickupCount(); i++) {
            SaveData.deleteKey(pickupTag(i));
        }

        SaveData.deleteKey(pickupCountTag);
    }
    public static void clearUpgradeLocations() {
        for(int i = 0; i < getUpgradeCount(); i++) {
            SaveData.deleteKey(upgradeTag(i));
        }

        SaveData.deleteKey(upgradeCountTag);
    }
    public static void clearRescueLocations() {
        for(int i = 0; i < getRescueCount(); i++) {
            SaveData.deleteKey(rescueTag(i));
        }

        SaveData.deleteKey(rescueCountTag);
    }
    public static void clearNestLocations() {
        for(int i = 0; i < getNestCount(); i++) {
            SaveData.deleteKey(nestTag(i));
        }

        SaveData.deleteKey(nestCountTag);
    }
    public static void clearBossLocations() {
        for(int i = 0; i < getBossCount(); i++) {
            SaveData.deleteKey(bossTag(i));
        }

        SaveData.deleteKey(bossCountTag);
    }
    public static void clearFishingLocations() {
        for(int i = 0; i < getFishingCount(); i++) {
            SaveData.deleteKey(fishTag(i));
        }

        SaveData.deleteKey(fishCountTag);
    }
    public static void clearEyeLocations() {
        for(int i = 0; i < getEyeCount(); i++) {
            SaveData.deleteKey(eyeTag(i));
        }

        SaveData.deleteKey(eyeCountTag);
    }
    public static void clearBridgeLocations() {
        for(int i = 0; i < getBridgeCount(); i++) {
            SaveData.deleteKey(bridgeTag(i));
        }

        SaveData.deleteKey(bridgeCountTag);
    }

    public static void addLocation(TownLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(townTag(getTownCount()), data);

        SaveData.setInt(townCountTag, getTownCount() + 1);
    }
    public static void addLocation(PickupLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(pickupTag(getPickupCount()), data);

        SaveData.setInt(pickupCountTag, getPickupCount() + 1);
    }
    public static void addLocation(UpgradeLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(upgradeTag(getUpgradeCount()), data);

        SaveData.setInt(upgradeCountTag, getUpgradeCount() + 1);
    }
    public static void addLocation(RescueLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(rescueTag(getRescueCount()), data);

        SaveData.setInt(rescueCountTag, getRescueCount() + 1);
    }
    public static void addLocation(NestLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(nestTag(getNestCount()), data);

        SaveData.setInt(nestCountTag, getNestCount() + 1);
    }
    public static void addLocation(BossLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(bossTag(getBossCount()), data);

        SaveData.setInt(bossCountTag, getBossCount() + 1);
    }
    public static void addLocation(FishingLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(fishTag(getFishingCount()), data);

        SaveData.setInt(fishCountTag, getFishingCount() + 1);
    }
    public static void addLocation(EyeLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(eyeTag(getEyeCount()), data);

        SaveData.setInt(eyeCountTag, getEyeCount() + 1);
    }
    public static void addLocation(BridgeLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(bridgeTag(getBridgeCount()), data);

        SaveData.setInt(bridgeCountTag, getBridgeCount() + 1);
    }

    public static void removeLocation(MapLocation loc) {
        switch(loc.type) {
            case MapLocation.locationType.town:
                removeTownLocation((TownLocation)loc);
                break;
            case MapLocation.locationType.pickup:
                removePickupLocation((PickupLocation)loc);
                break;
            case MapLocation.locationType.upgrade:
                removeUpgradeLocation((UpgradeLocation)loc);
                break;
            case MapLocation.locationType.rescue:
                removeRescueLocation((RescueLocation)loc);
                break;
            case MapLocation.locationType.nest:
                removeNestLocation((NestLocation)loc);
                break;
            case MapLocation.locationType.boss:
                removeBossLocation((BossLocation)loc);
                break;
            case MapLocation.locationType.eye:
                removeEyeLocation((EyeLocation)loc);
                break;
            case MapLocation.locationType.bridge:
                removeBridgeLocation((BridgeLocation)loc);
                break;
        }
    }
    public static void removeTownLocation(TownLocation loc) {
        if(loc == null)
            return;

        int startingIndex = getIndex(loc);
        for(int i = startingIndex; i < getTownCount(); i++) {
            overrideTownLocation(i, getTownLocation(i + 1));
        }

        SaveData.setInt(townCountTag, getTownCount() - 1);
    }
    public static void removePickupLocation(PickupLocation loc) {
        if(loc == null)
            return;

        int startingIndex = getIndex(loc);
        for(int i = startingIndex; i < getPickupCount(); i++) {
            overridePickupLocation(i, getPickupLocation(i + 1));
        }

        SaveData.setInt(pickupCountTag, getPickupCount() - 1);
    }
    public static void removeUpgradeLocation(UpgradeLocation loc) {
        if(loc == null)
            return;

        int startingIndex = getIndex(loc);
        for(int i = startingIndex; i < getUpgradeCount(); i++) {
            overrideUpgradeLocation(i, getUpgradeLocation(i + 1));
        }

        SaveData.setInt(upgradeCountTag, getUpgradeCount() - 1);
    }
    public static void removeRescueLocation(RescueLocation loc) {
        if(loc == null)
            return;

        int startingIndex = getIndex(loc);
        for(int i = startingIndex; i < getRescueCount(); i++) {
            overrideRescueLocation(i, getRescueLocation(i + 1));
        }

        SaveData.setInt(rescueCountTag, getRescueCount() - 1);
    }
    public static void removeNestLocation(NestLocation loc) {
        if(loc == null)
            return;

        int startingIndex = getIndex(loc);
        for(int i = startingIndex; i < getNestCount(); i++) {
            overrideNestLocation(i, getNestLocation(i + 1));
        }

        SaveData.setInt(nestCountTag, getNestCount() - 1);
    }
    public static void removeBossLocation(BossLocation loc) {
        if(loc == null)
            return;

        int startingIndex = getIndex(loc);
        for(int i = startingIndex; i < getBossCount(); i++) {
            overrideBossLocation(i, getBossLocation(i + 1));
        }

        SaveData.setInt(bossCountTag, getBossCount() - 1);
    }
    public static void removeFishingLocation(FishingLocation loc) {
        if(loc == null)
            return;

        int startingIndex = getIndex(loc);
        for(int i = startingIndex; i < getFishingCount(); i++) {
            overrideFishingLocation(i, getFishingLocation(i + 1));
        }

        SaveData.setInt(fishCountTag, getFishingCount() - 1);
    }
    public static void removeEyeLocation(EyeLocation loc) {
        if(loc == null)
            return;

        int startingIndex = getIndex(loc);
        for(int i = startingIndex; i < getEyeCount(); i++) {
            overrideEyeLocation(i, getEyeLocation(i + 1));
        }

        SaveData.setInt(eyeCountTag, getEyeCount() - 1);
    }
    public static void removeBridgeLocation(BridgeLocation loc) {
        if(loc == null)
            return;

        int startingIndex = getIndex(loc);
        for(int i = startingIndex; i < getBridgeCount(); i++) {
            overrideBridgeLocation(i, getBridgeLocation(i + 1));
        }

        SaveData.setInt(bridgeCountTag, getBridgeCount() - 1);
    }

    public static void overrideTownLocation(TownLocation loc) {
        for(int i = 0; i < getTownCount(); i++) {
            if(getTownLocation(i).town.isEqualTo(loc.town)) {
                var data = JsonUtility.ToJson(loc);
                SaveData.setString(townTag(i), data);
                return;
            }
        }
    }
    public static void overrideTownLocation(int index, TownLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(townTag(index), data);
    }
    public static void overridePickupLocation(int index, PickupLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(pickupTag(index), data);
    }
    public static void overrideUpgradeLocation(int index, UpgradeLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(upgradeTag(index), data);
    }
    public static void overrideRescueLocation(int index, RescueLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(rescueTag(index), data);
    }
    public static void overrideNestLocation(int index, NestLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(nestTag(index), data);
    }
    public static void overrideBossLocation(int index, BossLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(bossTag(index), data);
    }
    public static void overrideFishingLocation(int index, FishingLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(fishTag(index), data);
    }
    public static void overrideEyeLocation(int index, EyeLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(eyeTag(index), data);
    }
    public static void overrideBridgeLocation(int index, BridgeLocation loc) {
        var data = JsonUtility.ToJson(loc);
        SaveData.setString(bridgeTag(index), data);
    }


    public static int getTownCount() {
        return SaveData.getInt(townCountTag);
    }
    public static int getPickupCount() {
        return SaveData.getInt(pickupCountTag);
    }
    public static int getUpgradeCount() {
        return SaveData.getInt(upgradeCountTag);
    }
    public static int getRescueCount() {
        return SaveData.getInt(rescueCountTag);
    }
    public static int getNestCount() {
        return SaveData.getInt(nestCountTag);
    }
    public static int getBossCount() {
        return SaveData.getInt(bossCountTag);
    }
    public static int getFishingCount() {
        return SaveData.getInt(fishCountTag);
    }
    public static int getEyeCount() {
        return SaveData.getInt(eyeCountTag);
    }
    public static int getBridgeCount() {
        return SaveData.getInt(bridgeCountTag);
    }

    public static TownLocation getTownLocation(int index) {
        var data = SaveData.getString(townTag(index));
        var temp = JsonUtility.FromJson<TownLocation>(data);

        if(temp != null)
            return temp;
        return null;
    }
    public static TownLocation getTownLocationWithTown(Town t) {
        for(int i = 0; i < getTownCount(); i++) {
            if(getTownLocation(i).town.isEqualTo(t))
                return getTownLocation(i);
        }
        return null;
    }
    public static PickupLocation getPickupLocation(int index) {
        var data = SaveData.getString(pickupTag(index));
        var temp = JsonUtility.FromJson<PickupLocation>(data);

        if(temp != null)
            return temp;
        return null;
    }
    public static UpgradeLocation getUpgradeLocation(int index) {
        var data = SaveData.getString(upgradeTag(index));
        var temp = JsonUtility.FromJson<UpgradeLocation>(data);

        if(temp != null)
            return temp;
        return null;
    }
    public static RescueLocation getRescueLocation(int index) {
        var data = SaveData.getString(rescueTag(index));
        var temp = JsonUtility.FromJson<RescueLocation>(data);

        if(temp != null)
            return temp;
        return null;
    }
    public static NestLocation getNestLocation(int index) {
        var data = SaveData.getString(nestTag(index));
        var temp = JsonUtility.FromJson<NestLocation>(data);

        if(temp != null)
            return temp;
        return null;
    }
    public static BossLocation getBossLocation(int index) {
        var data = SaveData.getString(bossTag(index));
        var temp = JsonUtility.FromJson<BossLocation>(data);

        if(temp != null)
            return temp;
        return null;
    }
    public static FishingLocation getFishingLocation(int index) {
        var data = SaveData.getString(fishTag(index));
        var temp = JsonUtility.FromJson<FishingLocation>(data);

        if(temp != null)
            return temp;
        return null;
    }
    public static EyeLocation getEyeLocation(int index) {
        var data = SaveData.getString(eyeTag(index));
        var temp = JsonUtility.FromJson<EyeLocation>(data);

        if(temp != null)
            return temp;
        return null;
    }
    public static BridgeLocation getBridgeLocation(int index) {
        var data = SaveData.getString(bridgeTag(index));
        var temp = JsonUtility.FromJson<BridgeLocation>(data);

        if(temp != null)
            return temp;
        return null;
    }

    public static bool locationAtPosition(Vector2 p, GameInfo.region reg = (GameInfo.region)(-1)) {
        foreach(var i in getLocations(null, reg)) {
            if(i.pos == p)
                return true;
        }
        return false;
    }
    public static bool locationCloseToPos(Vector2 p, float maxDist) {
        foreach(var i in getLocations()) {
            if(Vector2.Distance(i.pos, p) < maxDist)
                return true;
        }
        return false;
    }
    public static MapLocation getClostestLocation(Vector2 p, GameInfo.region reg = (GameInfo.region)(-1)) {
        float closestDist = 1000.0f;
        MapLocation ml = null;
        foreach(var i in getLocations(null, reg)) {
            if(Vector2.Distance(i.pos, p) < closestDist) {
                closestDist = Vector2.Distance(i.pos, p);
                ml = i;
            }
        }

        return ml;
    }
    public static MapLocation getLocationAtPos(Vector2 p, GameInfo.region reg = (GameInfo.region)(-1)) {
        foreach(var i in getLocations(null, reg)) {
            if(i.pos == p)
                return i;
        }

        return null;
    }
    public static TownLocation getTownLocationAtPosition(Vector2 p) {
        for(int i = 0; i < getTownCount(); i++) {
            if(getTownLocation(i).pos == p)
                return getTownLocation(i);
        }
        return null;
    }
    public static PickupLocation getPickupLocationAtPos(Vector2 p) {
        for(int i = 0; i < getPickupCount(); i++) {
            if(getPickupLocation(i).pos == p)
                return getPickupLocation(i);
        }
        return null;
    }
    public static UpgradeLocation getUpgradeLocationAtPos(Vector2 p) {
        for(int i = 0; i < getUpgradeCount(); i++) {
            if(getUpgradeLocation(i).pos == p)
                return getUpgradeLocation(i);
        }
        return null;
    }
    public static RescueLocation getRescueLocationAtPos(Vector2 p) {
        for(int i = 0; i < getRescueCount(); i++) {
            if(getRescueLocation(i).pos == p)
                return getRescueLocation(i);
        }
        return null;
    }
    public static NestLocation getNestLocationAtPos(Vector2 p) {
        for(int i = 0; i < getNestCount(); i++) {
            if(getNestLocation(i).pos == p)
                return getNestLocation(i);
        }
        return null;
    }
    public static BossLocation getBossLocationAtPos(Vector2 p) {
        for(int i = 0; i < getBossCount(); i++) {
            if(getBossLocation(i).pos == p)
                return getBossLocation(i);
        }
        return null;
    }
    public static FishingLocation getFishingLocationAtPos(Vector2 p) {
        for(int i = 0; i < getFishingCount(); i++) {
            if(getFishingLocation(i).pos == p)
                return getFishingLocation(i);
        }
        return null;
    }
    public static EyeLocation getEyeLocationAtPos(Vector2 p) {
        for(int i = 0; i < getEyeCount(); i++) {
            if(getEyeLocation(i).pos == p)
                return getEyeLocation(i);
        }
        return null;
    }
    public static BridgeLocation getBridgeLocationAtPos(Vector2 p) {
        for(int i = 0; i < getBridgeCount(); i++) {
            if(getBridgeLocation(i).pos == p)
                return getBridgeLocation(i);
        }
        return null;
    }


    public static int getIndex(TownLocation loc) {
        for(int i = 0; i < getTownCount(); i++) {
            if(getTownLocation(i).isEqualTo(loc))
                return i;
        }
        return -1;
    }
    public static int getIndex(PickupLocation loc) {
        for(int i = 0; i < getPickupCount(); i++) {
            if(getPickupLocation(i).isEqualTo(loc))
                return i;
        }
        return -1;
    }
    public static int getIndex(UpgradeLocation loc) {
        for(int i = 0; i < getUpgradeCount(); i++) {
            if(getUpgradeLocation(i).isEqualTo(loc))
                return i;
        }
        return -1;
    }
    public static int getIndex(RescueLocation loc) {
        for(int i = 0; i < getRescueCount(); i++) {
            if(getRescueLocation(i).isEqualTo(loc))
                return i;
        }
        return -1;
    }
    public static int getIndex(NestLocation loc) {
        for(int i = 0; i < getNestCount(); i++) {
            if(getNestLocation(i).isEqualTo(loc))
                return i;
        }
        return -1;
    }
    public static int getIndex(BossLocation loc) {
        for(int i = 0; i < getBossCount(); i++) {
            if(getBossLocation(i).isEqualTo(loc))
                return i;
        }
        return -1;
    }
    public static int getIndex(FishingLocation loc) {
        for(int i = 0; i < getFishingCount(); i++) {
            if(getFishingLocation(i).isEqualTo(loc))
                return i;
        }
        return -1;
    }
    public static int getIndex(EyeLocation loc) {
        for(int i = 0; i < getEyeCount(); i++) {
            if(getEyeLocation(i).isEqualTo(loc))
                return i;
        }
        return -1;
    }
    public static int getIndex(BridgeLocation loc) {
        for(int i = 0; i < getBridgeCount(); i++) {
            if(getBridgeLocation(i).isEqualTo(loc))
                return i;
        }
        return -1;
    }
    public static int getIndex(MapLocation loc) {
        switch(loc.type) {
            case MapLocation.locationType.town:
                return getIndex((TownLocation)loc);

            case MapLocation.locationType.pickup:
                return getIndex((PickupLocation)loc);

            case MapLocation.locationType.upgrade:
                return getIndex((UpgradeLocation)loc);

            case MapLocation.locationType.rescue:
                return getIndex((RescueLocation)loc);

            case MapLocation.locationType.nest:
                return getIndex((NestLocation)loc);

            case MapLocation.locationType.boss:
                return getIndex((BossLocation)loc);

            case MapLocation.locationType.fishing:
                return getIndex((FishingLocation)loc);

            case MapLocation.locationType.eye:
                return getIndex((EyeLocation)loc);

            case MapLocation.locationType.bridge:
                return getIndex((BridgeLocation)loc);
        }

        return -1;
    }


    public static List<MapLocation> getLocations(List<MapLocation.locationType> includes = null, GameInfo.region reg = (GameInfo.region)(-1)) {
        if(includes == null) {
            includes = new List<MapLocation.locationType>() {
                MapLocation.locationType.town, MapLocation.locationType.pickup,
                MapLocation.locationType.upgrade, MapLocation.locationType.rescue,
                MapLocation.locationType.nest, MapLocation.locationType.boss, MapLocation.locationType.fishing, MapLocation.locationType.eye, MapLocation.locationType.bridge
            };
        }
        var temp = new List<MapLocation>();

        for(int i = 0; i < includes.Count; i++) {
            switch(includes[i]) {
                case MapLocation.locationType.town:
                    for(int t = 0; t < getTownCount(); t++) {
                        var town = getTownLocation(t);
                        if(reg == (GameInfo.region)(-1) || town.region == reg)
                            temp.Add(getTownLocation(t));
                    }
                    break;

                case MapLocation.locationType.pickup:
                    for(int p = 0; p < getPickupCount(); p++) {
                        var pick = getPickupLocation(p);
                        if(reg == (GameInfo.region)(-1) || pick.region == reg)
                            temp.Add(getPickupLocation(p));
                    }
                    break;

                case MapLocation.locationType.upgrade:
                    for(int u = 0; u < getUpgradeCount(); u++) {
                        var up = getUpgradeLocation(u);
                        if(reg == (GameInfo.region)(-1) || up.region == reg)
                            temp.Add(getUpgradeLocation(u));
                    }
                    break;

                case MapLocation.locationType.rescue:
                    for(int r = 0; r < getRescueCount(); r++) {
                        var resc = getRescueLocation(r);
                        if(reg == (GameInfo.region)(-1) || resc.region == reg)
                            temp.Add(getRescueLocation(r));
                    }
                    break;

                case MapLocation.locationType.nest:
                    for(int n = 0; n < getNestCount(); n++) {
                        var nest = getNestLocation(n);
                        if(reg == (GameInfo.region)(-1) || nest.region == reg)
                            temp.Add(getNestLocation(n));
                    }
                    break;

                case MapLocation.locationType.boss:
                    for(int b = 0; b < getBossCount(); b++) {
                        var boss = getBossLocation(b);
                        if(reg == (GameInfo.region)(-1) || boss.region == reg)
                            temp.Add(getBossLocation(b));
                    }
                    break;

                case MapLocation.locationType.fishing:
                    for(int f = 0; f < getFishingCount(); f++) {
                        var fish = getFishingLocation(f);
                        if(reg == (GameInfo.region)(-1) || fish.region == reg)
                            temp.Add(getFishingLocation(f));
                    }
                    break;

                case MapLocation.locationType.eye:
                    for(int f = 0; f < getEyeCount(); f++) {
                        var eye = getEyeLocation(f);
                        if(reg == (GameInfo.region)(-1) || eye.region == reg)
                            temp.Add(getEyeLocation(f));
                    }
                    break;

                case MapLocation.locationType.bridge:
                    for(int f = 0; f < getBridgeCount(); f++) {
                        var brid = getBridgeLocation(f);
                        if(reg == (GameInfo.region)(-1) || brid.region == reg)
                            temp.Add(getBridgeLocation(f));
                    }
                    break;
            }
        }

        return temp;
    } */
}

    public class locationFindInfo {
    public MapLocation.locationType type = (MapLocation.locationType)(-1);
    public int referenceIndex = -1;

    public locationFindInfo(MapLocation.locationType t, int ind) {
        type = t;
        referenceIndex = ind;
    }
}

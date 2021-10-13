using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapLocationHolder {
    static string townTag(int i) { return "TownLocation" + i.ToString(); }
    static string pickupTag(int i) { return "PickupLocation" + i.ToString(); }
    static string upgradeTag(int i) { return "UpgradeLocation" + i.ToString(); }
    static string rescueTag(int i) { return "RescueLocation" + i.ToString(); }
    static string nestTag(int i) { return "NestLocation" + i.ToString(); }
    static string bossTag(int i) { return "BossLocation" + i.ToString(); }

    static string townCountTag = "TownLocationCount";
    static string pickupCountTag = "PickupLocationCount";
    static string upgradeCountTag = "UpgradeLocationCount";
    static string rescueCountTag = "RescueLocationCount";
    static string nestCountTag = "NestLocationCount";
    static string bossCountTag = "BossLocationCount";


    public static void clear() {
        clearTownLocations();
        clearPickupLocations();
        clearUpgradeLocations();
        clearRescueLocations();
        clearNestLocations();
        clearBossLocations();
    }
    public static void clearTownLocations() {
        for(int i = 0; i < getTownCount() + 10; i++) {
            SaveData.deleteKey(townTag(i));
        }

        SaveData.deleteKey(townCountTag);
        GameInfo.clearTownInstanceIDQueue();
    }
    public static void clearPickupLocations() {
        for(int i = 0; i < getPickupCount() + 10; i++) {
            SaveData.deleteKey(pickupTag(i));
        }

        SaveData.deleteKey(pickupCountTag);
    }
    public static void clearUpgradeLocations() {
        for(int i = 0; i < getUpgradeCount() + 10; i++) {
            SaveData.deleteKey(upgradeTag(i));
        }

        SaveData.deleteKey(upgradeCountTag);
    }
    public static void clearRescueLocations() {
        for(int i = 0; i < getRescueCount() + 10; i++) {
            SaveData.deleteKey(rescueTag(i));
        }

        SaveData.deleteKey(rescueCountTag);
    }
    public static void clearNestLocations() {
        for(int i = 0; i < getNestCount() + 10; i++) {
            SaveData.deleteKey(nestTag(i));
        }

        SaveData.deleteKey(nestCountTag);
    }
    public static void clearBossLocations() {
        for(int i = 0; i < getBossCount() + 10; i++) {
            SaveData.deleteKey(bossTag(i));
        }

        SaveData.deleteKey(bossCountTag);
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
        }
    }
    public static void removeTownLocation(TownLocation loc) {
        List<TownLocation> temp = new List<TownLocation>();
        for(int i = 0; i < getTownCount(); i++) {
            TownLocation t = getTownLocation(i);
            if(t != null && !t.isEqualTo(loc))
                temp.Add(t);
        }

        clearTownLocations();
        foreach(var i in temp)
            addLocation(i);
    }
    public static void removePickupLocation(PickupLocation loc) {
        List<PickupLocation> temp = new List<PickupLocation>();
        for(int i = 0; i < getPickupCount(); i++) {
            PickupLocation p = getPickupLocation(i);
            if(p != null && !p.isEqualTo(loc))
                temp.Add(p);
        }

        clearPickupLocations();
        foreach(var i in temp)
            addLocation(i);
    }
    public static void removeUpgradeLocation(UpgradeLocation loc) {
        List<UpgradeLocation> temp = new List<UpgradeLocation>();
        for(int i = 0; i < getUpgradeCount(); i++) {
            UpgradeLocation u = getUpgradeLocation(i);
            if(u != null && !u.isEqualTo(loc))
                temp.Add(u);
        }

        clearUpgradeLocations();
        foreach(var i in temp)
            addLocation(i);
    }
    public static void removeRescueLocation(RescueLocation loc) {
        List<RescueLocation> temp = new List<RescueLocation>();
        for(int i = 0; i < getRescueCount(); i++) {
            RescueLocation r = getRescueLocation(i);
            if(r != null && !r.isEqualTo(loc))
                temp.Add(r);
        }

        clearRescueLocations();
        foreach(var i in temp)
            addLocation(i);
    }
    public static void removeNestLocation(NestLocation loc) {
        List<NestLocation> temp = new List<NestLocation>();
        for(int i = 0; i < getNestCount(); i++) {
            NestLocation n = getNestLocation(i);
            if(n != null && !n.isEqualTo(loc))
                temp.Add(n);
        }

        clearNestLocations();
        foreach(var i in temp)
            addLocation(i);
    }
    public static void removeBossLocation(BossLocation loc) {
        List<BossLocation> temp = new List<BossLocation>();
        for(int i = 0; i < getBossCount(); i++) {
            BossLocation b = getBossLocation(i);
            if(b != null && !b.isEqualTo(loc))
                temp.Add(b);
        }

        clearBossLocations();
        foreach(var i in temp)
            addLocation(i);
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
    public static TownLocation getRandomTownLocationWithBuilding(Building.type type) {
        List<TownLocation> locs = new List<TownLocation>();
        for(int i = 0; i < getTownCount(); i++) {
            if(getTownLocation(i).town.hasBuilding(type))
                locs.Add(getTownLocation(i));
        }
        if(locs.Count == 0)
            return null;
        return locs[Random.Range(0, locs.Count)];
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

    public static bool locationAtPosition(Vector2 p) {
        foreach(var i in getLocations()) {
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
    public static MapLocation getClostestLocation(Vector2 p) {
        float closestDist = 1000.0f;
        MapLocation ml = null;
        foreach(var i in getLocations()) {
            if(Vector2.Distance(i.pos, p) < closestDist) {
                closestDist = Vector2.Distance(i.pos, p);
                ml = i;
            }
        }

        return ml;
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
        }

        return -1;
    }


    public static List<MapLocation> getLocations(List<MapLocation.locationType> includes = null) {
        if(includes == null) {
            includes = new List<MapLocation.locationType>() { 
                MapLocation.locationType.town, MapLocation.locationType.pickup, 
                MapLocation.locationType.upgrade, MapLocation.locationType.rescue, 
                MapLocation.locationType.nest, MapLocation.locationType.boss 
            };
        }
        var temp = new List<MapLocation>();
        
        for(int i = 0; i < includes.Count; i++) {
            switch(includes[i]) {
                case MapLocation.locationType.town:
                    for(int t = 0; t < getTownCount(); t++) {
                        temp.Add(getTownLocation(t));
                    }
                    break;

                case MapLocation.locationType.pickup:
                    for(int p = 0; p < getPickupCount(); p++) {
                        temp.Add(getPickupLocation(p));
                    }
                    break;

                case MapLocation.locationType.upgrade:
                    for(int u = 0; u < getUpgradeCount(); u++) {
                        temp.Add(getUpgradeLocation(u));
                    }
                    break;

                case MapLocation.locationType.rescue:
                    for(int r = 0; r < getRescueCount(); r++) {
                        temp.Add(getRescueLocation(r));
                    }
                    break;

                case MapLocation.locationType.nest:
                    for(int n = 0; n < getNestCount(); n++) {
                        temp.Add(getNestLocation(n));
                    }
                    break;

                case MapLocation.locationType.boss:
                    for(int b = 0; b < getBossCount(); b++) {
                        temp.Add(getBossLocation(b));
                    }
                    break;
            }
        }

        return temp;
    }
}

public class locationFindInfo {
    public MapLocation.locationType type = MapLocation.locationType.empty;
    public int referenceIndex = -1;

    public locationFindInfo(MapLocation.locationType t, int ind) {
        type = t;
        referenceIndex = ind;
    }
}

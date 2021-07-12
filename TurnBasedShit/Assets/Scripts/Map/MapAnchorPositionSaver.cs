using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapAnchorPositionSaver {
    const string anchorCount = "Map Anchor Count";
    static string anchorTag(int i) { return "Map Anchor" + i.ToString(); }
    const string partyAnchorTag = "Party Anchor";


    public static void clearPositions() {
        for(int i = 0; i < SaveData.getInt(anchorCount); i++) {
            SaveData.deleteKey(anchorTag(i));
        }
        SaveData.deleteKey(anchorCount);

        SaveData.deleteKey(partyAnchorTag);
    }

    public static void addNewMapAnchor(RoadSegmentInfo seg) {
        int index = SaveData.getInt(anchorCount);

        var data = JsonUtility.ToJson(seg);
        SaveData.setString(anchorTag(index), data);
        SaveData.setInt(anchorCount, index + 1);
    }
    public static void setPartyPointIndex(int ind) {
        SaveData.setInt(partyAnchorTag, ind);
    }

    public static void removeAnchor(RoadSegmentInfo seg) {
        var tData = JsonUtility.ToJson(seg);
        bool past = false;
        for(int i = 0; i < SaveData.getInt(anchorCount); i++) {
            var data = SaveData.getString(anchorTag(i));

            if(data == tData && !past) {
                SaveData.deleteKey(anchorTag(i));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(anchorTag(i));
                overrideAnchor(i - 1, JsonUtility.FromJson<RoadSegmentInfo>(data));
            }
        }
        SaveData.setInt(anchorCount, SaveData.getInt(anchorCount) - 1);
    }
    public static void removeAnchor(int index) {
        var data = SaveData.getString(anchorTag(index));
        removeAnchor(JsonUtility.FromJson<RoadSegmentInfo>(data));
    }


    public static void overrideAnchor(int index, RoadSegmentInfo seg) {
        var data = JsonUtility.ToJson(seg);
        SaveData.setString(anchorTag(index), data);
    }

    public static RoadSegmentInfo getAnchorPos(int index) {
        var data = SaveData.getString(anchorTag(index));
        RoadSegmentInfo p = JsonUtility.FromJson<RoadSegmentInfo>(data);
        if(p == null)
            return null;
        return p;
    }
    public static int getPartyPointLocationIndex() {
        var data = SaveData.getInt(partyAnchorTag);
        return data;
    }
    public static int getAnchorCount() {
        return SaveData.getInt(anchorCount);
    }
}

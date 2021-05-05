using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapAnchorPositionSaver {
    const string anchorCount = "Map Anchor Count";
    static string anchorTag(int i) { return "Map Anchor" + i.ToString(); }
    const string partyAnchorTag = "Party Anchor";

    [System.Serializable]
    public class pos {
        public float x, y;

        public pos(float a, float b) {
            x = a;
            y = b;
        }
    }


    public static void clearPositions() {
        for(int i = 0; i < SaveData.getInt(anchorCount); i++) {
            SaveData.deleteKey(anchorTag(i));
        }
        SaveData.deleteKey(anchorCount);
    }

    public static void addNewMapAnchor(GameObject anchor) {
        pos p = new pos(anchor.transform.position.x, anchor.transform.position.y);
        int index = SaveData.getInt(anchorCount);

        var data = JsonUtility.ToJson(p);
        SaveData.setString(anchorTag(index), data);
        SaveData.setInt(anchorCount, index + 1);
    }
    public static void setPartyAnchor(GameObject anchor) {
        pos p = new pos(anchor.transform.position.x, anchor.transform.position.y);
        var data = JsonUtility.ToJson(p);
        SaveData.setString(partyAnchorTag, data);
    }

    public static void removeAnchor(GameObject anchor) {
        removeAnchor(new pos(anchor.transform.position.x, anchor.transform.position.y));
    }
    public static void removeAnchor(pos p) {
        var tData = JsonUtility.ToJson(p);
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
                overrideAnchor(i - 1, JsonUtility.FromJson<pos>(data));
            }
        }
        SaveData.setInt(anchorCount, SaveData.getInt(anchorCount) - 1);
    }
    public static void removeAnchor(int index) {
        var data = SaveData.getString(anchorTag(index));
        removeAnchor(JsonUtility.FromJson<pos>(data));
    }


    public static void overrideAnchor(int index, pos p) {
        var data = JsonUtility.ToJson(p);
        SaveData.setString(anchorTag(index), data);
    }
    public static void overrideAnchor(int index, GameObject anchor) {
        //  tried to override an anchor at an unregistered index
        if(index >= SaveData.getInt(anchorCount)) {
            addNewMapAnchor(anchor);
            return;
        }

        pos p = new pos(anchor.transform.position.x, anchor.transform.position.y);
        overrideAnchor(index, p);
    }

    public static Vector2 getAnchorPos(int index) {
        var data = SaveData.getString(anchorTag(index));
        pos p = JsonUtility.FromJson<pos>(data);
        if(p == null)
            return Vector2.zero;
        return new Vector2(p.x, p.y);
    }
    public static Vector2 getPartyPos() {
        var data = SaveData.getString(partyAnchorTag);
        pos p = JsonUtility.FromJson<pos>(data);
        if(p == null)
            return Vector2.zero;
        return new Vector2(p.x, p.y);
    }
    public static int getAnchorCount() {
        return SaveData.getInt(anchorCount);
    }
}

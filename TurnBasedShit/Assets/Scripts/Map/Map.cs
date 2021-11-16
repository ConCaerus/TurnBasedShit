using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Map {
    public static float leftBound = -25.0f, rightBound = 250.0f;
    public static float botBound = -25.0f, topBound = 25.0f;
    public static float width() {
        return rightBound - leftBound;
    }
    public static float height() {
        return topBound - botBound;
    }


    const string saveTag = "Map Fog Texture Map";

    public static Vector2 getRandPos() {
        return new Vector2(Random.Range(leftBound, rightBound), Random.Range(botBound, topBound));
    }

    public static GameInfo.region getDiffForX(float x) {
        for(int i = 0; i < 6; i++) {
            if(x < getRegionXStartPoint(i))
                return (GameInfo.region)(i);
        }
        if(x > 250f)
            return GameInfo.region.hell;
        else
            return GameInfo.region.grassland;   //forgive me
    }
    public static float getRegionXLength(int regionIndex) {
        float totalLength = rightBound - leftBound;
        switch(regionIndex) {
            case 0: return totalLength / 5f;
            case 1: return totalLength / 5f;
            case 2: return totalLength / 5f;
            case 3: return totalLength / 5f;
            case 4: return totalLength / 5f;
            default: return 0.0f;
        }
    }
    public static float getRegionXStartPoint(int regionIndex) {
        float dist = leftBound;
        for(int i = 0; i <= regionIndex; i++) {
            dist += getRegionXLength(i);
        }
        return dist;
    }
    public static float getRegionMidXPoint(int regionIndex) {
        return getRegionXStartPoint(regionIndex) + (getRegionXLength(regionIndex) / 2.0f);
    }
    public static Vector2 getRandomPosInRegion(int regionIndex) {
        var randY = getRandPos().y;
        var randX = getRegionMidXPoint(regionIndex) + Random.Range(-(getRegionXLength(regionIndex) / 2.0f), getRegionXLength(regionIndex) / 2.0f);
        return new Vector2(randX, randY);
    }

    public static void populateTowns(PresetLibrary lib) {
        int grassCount = 1;
        int forestCount = 3;
        int swampCount = 5;
        int mountainsCount = 8;
        int hell = 10;

        for(int i = 0; i < grassCount + forestCount + swampCount + mountainsCount + hell; i++) {
            if(i < grassCount)
                MapLocationHolder.addLocation(new TownLocation(getRandomPosInRegion(0), GameInfo.region.grassland, lib));
            else if(i < forestCount)
                MapLocationHolder.addLocation(new TownLocation(getRandomPosInRegion(1), GameInfo.region.forest, lib));
            else if(i < swampCount)
                MapLocationHolder.addLocation(new TownLocation(getRandomPosInRegion(2), GameInfo.region.swamp, lib));
            else if(i < mountainsCount)
                MapLocationHolder.addLocation(new TownLocation(getRandomPosInRegion(3), GameInfo.region.mountains, lib));
            else if(i < hell)
                MapLocationHolder.addLocation(new TownLocation(getRandomPosInRegion(4), GameInfo.region.hell, lib));
        }
    }


    public static void clearFogTexture() {
        SaveData.deleteKey(saveTag);
    }
    public static void saveFogTexture(Texture2D map) {
        FowData data = new FowData(map.GetRawTextureData());
        SaveData.setString(saveTag, JsonUtility.ToJson(data));
    }
    public static bool hasSavedFogTexture() {
        return !string.IsNullOrEmpty(SaveData.getString(saveTag));
    }
    public static Texture2D getFogTexture() {
        var temp = new Texture2D((int)width(), (int)height());
        if(string.IsNullOrEmpty(SaveData.getString(saveTag)))
            return temp;
        var thing = JsonUtility.FromJson<FowData>(SaveData.getString(saveTag));
        temp.LoadRawTextureData(thing.data);
        return temp;
    }

    public static TownLocation getRandomTownLocationInRegion(GameInfo.region regionIndex) {
        if(MapLocationHolder.getTownCount() == 0) {
            Debug.LogError("No Towns My Guy");
            return null;
        }

        var pos = new List<TownLocation>();
        for(int i = 0; i < MapLocationHolder.getTownCount(); i++) {
            if(MapLocationHolder.getTownLocation(i).region == regionIndex)
                pos.Add(MapLocationHolder.getTownLocation(i));
        }

        return pos[Random.Range(0, pos.Count)];
    }
}


[System.Serializable]
public struct FowData {
    public byte[] data;

    public FowData(byte[] d) {
        data = d;
    }
}

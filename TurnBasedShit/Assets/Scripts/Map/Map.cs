using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Map {
    public static int width = 75, height = 50;
    public static float leftBound() {
        return -width / 2.0f;
    }
    public static float rightBound() {
        return width / 2.0f;
    }
    public static float topBound() {
        return height / 2.0f;
    }
    public static float botBound() {
        return -height / 2.0f;
    }


    static string saveTag(GameInfo.region reg) {
        switch(reg) {
            case GameInfo.region.grassland: return "Grasslands Map Fog Texture Map";
            case GameInfo.region.forest: return "Forest Map Fog Texture Map";
            case GameInfo.region.swamp: return "Swamp Map Fog Texture Map";
            case GameInfo.region.mountains: return "Mountains Map Fog Texture Map";
            case GameInfo.region.hell: return "Hell Map Fog Texture Map";
        }
        return "";
    }

    public static Vector2 getRandPos() {
        float minDistFromEdge = 1.0f;
        return new Vector2(Random.Range(leftBound() + minDistFromEdge, rightBound() - minDistFromEdge), Random.Range(botBound() + minDistFromEdge, topBound() - minDistFromEdge));
    }

    public static void populateTowns(PresetLibrary lib) {
        int grassCount = 1;
        int forestCount = 3;
        int swampCount = 5;
        int mountainsCount = 8;
        int hell = 10;

        for(int i = 0; i < grassCount + forestCount + swampCount + mountainsCount + hell; i++) {
            if(i < grassCount)
                MapLocationHolder.addLocation(new TownLocation(getRandPos(), GameInfo.region.grassland, lib));
            else if(i < forestCount)
                MapLocationHolder.addLocation(new TownLocation(getRandPos(), GameInfo.region.forest, lib));
            else if(i < swampCount)
                MapLocationHolder.addLocation(new TownLocation(getRandPos(), GameInfo.region.swamp, lib));
            else if(i < mountainsCount)
                MapLocationHolder.addLocation(new TownLocation(getRandPos(), GameInfo.region.mountains, lib));
            else if(i < hell)
                MapLocationHolder.addLocation(new TownLocation(getRandPos(), GameInfo.region.hell, lib));
        }
    }

    public static void createFogTexture() {
        var temp = new Texture2D(width, height);
        temp.filterMode = FilterMode.Trilinear;
        temp.wrapMode = TextureWrapMode.Clamp;
        for(int x = 0; x < temp.width; x++) {
            for(int y = 0; y < temp.height; y++) {
                temp.SetPixel(x, y, Color.white);
            }
        }

        saveFogTexture(temp, GameInfo.region.grassland);
        saveFogTexture(temp, GameInfo.region.forest);
        saveFogTexture(temp, GameInfo.region.swamp);
        saveFogTexture(temp, GameInfo.region.mountains);
        saveFogTexture(temp, GameInfo.region.hell);
    }
    public static void clearFogTexture() {
        for(int i = 0; i < 5; i++) {
            SaveData.deleteKey(saveTag((GameInfo.region)i));
        }
    }
    public static void saveFogTexture(Texture2D map, GameInfo.region reg) {
        FowData data = new FowData(map.GetRawTextureData());
        SaveData.setString(saveTag(reg), JsonUtility.ToJson(data));
    }
    public static Texture2D getFogTexture(GameInfo.region reg) {
        if(string.IsNullOrEmpty(SaveData.getString(saveTag(reg))))
            return null;
        var temp = new Texture2D(width, height);
        var thing = JsonUtility.FromJson<FowData>(SaveData.getString(saveTag(reg)));
        temp.LoadRawTextureData(thing.data);
        return temp;
    }

    public static TownLocation getRandomTownLocationInRegion(GameInfo.region regionIndex) {
        if(MapLocationHolder.getHolder(regionIndex).getObjectCount<TownLocation>() == 0) {
            Debug.LogError("No Towns My Guy");
            return null;
        }

        var pos = new List<TownLocation>();
        for(int i = 0; i < MapLocationHolder.getHolder(regionIndex).getObjectCount<TownLocation>(); i++) {
            if(MapLocationHolder.getHolder(regionIndex).getObject<TownLocation>(i).region == regionIndex)
                pos.Add(MapLocationHolder.getHolder(regionIndex).getObject<TownLocation>(i));
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

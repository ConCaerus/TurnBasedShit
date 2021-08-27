using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Map {
    public static float leftBound = -18.0f, rightBound = 18.0f;
    public static float botBound = -18.0f, topBound = 18.0f;


    public static Vector2 getRandPos() {
        return new Vector2(Random.Range(leftBound, rightBound), Random.Range(botBound, topBound));
    }

    public static GameInfo.diffLvl getDiffForX(float x) {
        float startingPoint = Map.leftBound;

        for(int i = 1; i < 8; i++) {
            if(x < startingPoint)
                return (GameInfo.diffLvl)(i - 1);
            else
                startingPoint += getRegionXLength(i - 1);
        }

        return (GameInfo.diffLvl)(-1);
    }
    public static float getRegionXLength(int regionIndex) {
        switch(regionIndex) {
            case 0: return 10.0f;
            case 1: return 12.0f;
            case 2: return 17.0f;
            case 3: return 20.0f;
            case 4: return 35.0f;
            case 5: return 40.0f;
            case 6: return 50.0f;
            default: return 0.0f;
        }
    }
    public static Vector2 getRandomPosInRegion(int regionIndex) {
        var rand = getRandPos();
        return new Vector2(leftBound + getRegionXLength(regionIndex), rand.y);
    }

    public static void populateTowns(PresetLibrary lib) {
        int cakeCount = 5;
        int easyCount = 7;
        int normalCount = 10;
        int interCount = 15;
        int hardCount = 20;
        int heroicCount = 30;
        int legendaryCount = 40;

        for(int i = 0; i < cakeCount + easyCount + normalCount + interCount + hardCount + heroicCount + legendaryCount; i++) {
            if(i < cakeCount)
                MapLocationHolder.addLocation(new TownLocation(getRandomPosInRegion(0), GameInfo.diffLvl.Cake, lib));
            else if(i < easyCount)
                MapLocationHolder.addLocation(new TownLocation(getRandomPosInRegion(1), GameInfo.diffLvl.Easy, lib));
            else if(i < normalCount)
                MapLocationHolder.addLocation(new TownLocation(getRandomPosInRegion(2), GameInfo.diffLvl.Normal, lib));
            else if(i < interCount)
                MapLocationHolder.addLocation(new TownLocation(getRandomPosInRegion(3), GameInfo.diffLvl.Inter, lib));
            else if(i < hardCount)
                MapLocationHolder.addLocation(new TownLocation(getRandomPosInRegion(4), GameInfo.diffLvl.Hard, lib));
            else if(i < heroicCount)
                MapLocationHolder.addLocation(new TownLocation(getRandomPosInRegion(5), GameInfo.diffLvl.Heroic, lib));
            else if(i < legendaryCount)
                MapLocationHolder.addLocation(new TownLocation(getRandomPosInRegion(6), GameInfo.diffLvl.Legendary, lib));
        }
    }

    public static TownLocation getRandomTownLocationInRegion(int regionIndex) {
        if(MapLocationHolder.getTownCount() == 0)
            return null;

        var pos = new List<TownLocation>();
        for(int i = 0; i < MapLocationHolder.getTownCount(); i++) {
            if(MapLocationHolder.getTownLocation(i).region == regionIndex)
                pos.Add(MapLocationHolder.getTownLocation(i));
        }

        return pos[Random.Range(0, pos.Count)];
    }
}

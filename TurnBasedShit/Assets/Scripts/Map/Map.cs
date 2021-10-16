using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Map {
    public static float leftBound = -25.0f, rightBound = 250.0f;
    public static float botBound = -25.0f, topBound = 25.0f;


    public static Vector2 getRandPos() {
        return new Vector2(Random.Range(leftBound, rightBound), Random.Range(botBound, topBound));
    }

    public static GameInfo.diffLvl getDiffForX(float x) {
        for(int i = 0; i < 6; i++) {
            if(x < getRegionXStartPoint(i + 1))
                return (GameInfo.diffLvl)(i);
        }
        return (GameInfo.diffLvl)(-1);
    }
    public static float getRegionXLength(int regionIndex) {
        float totalLength = rightBound - leftBound;
        switch(regionIndex) {
            case 0: return totalLength / 7f;
            case 1: return totalLength / 7f;
            case 2: return totalLength / 7f;
            case 3: return totalLength / 7f;
            case 4: return totalLength / 7f;
            case 5: return totalLength / 7f;
            case 6: return totalLength / 7f;
            default: return 0.0f;
        }
    }
    public static float getRegionXStartPoint(int regionIndex) {
        float dist = leftBound;
        for(int i = 0; i < regionIndex; i++) {
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
        int cakeCount = 2;
        int easyCount = 3;
        int normalCount = 4;
        int interCount = 5;
        int hardCount = 7;
        int heroicCount = 8;
        int legendaryCount = 10;

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

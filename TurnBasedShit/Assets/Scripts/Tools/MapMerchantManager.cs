using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class MapMerchantManager {
    public static string merchantTag = "Map Merchants Tag";

    public static void createStartingMerchantData(PresetLibrary lib) {
        var temp = new MapMerchantContainer();

        for(int i = 0; i < 5; i++) {
            var mCount = Random.Range(3, 9);
            for(int j = 0; j < mCount; j++) {
                var colCount = Random.Range(5, 21);
                var statsCount = Random.Range(0, 6);
                var coinCount = Random.Range(15, 51);

                temp.addInfo(new MerchantInfo(colCount, statsCount, coinCount, j, (GameInfo.region)i, lib));
            }
        }

        overrideSavedContainer(temp);
    }


    public static MerchantInfo getMerchantInRegion(int index, GameInfo.region reg) {
        var data = SaveData.getString(merchantTag);
        var thing = JsonUtility.FromJson<MapMerchantContainer>(data);

        return thing.getMercsForRegion(reg)[index];
    }
    public static void updateMerchant(MerchantInfo info) {
        var data = SaveData.getString(merchantTag);
        var thing = JsonUtility.FromJson<MapMerchantContainer>(data);
        thing.overrideInfo(info);
        overrideSavedContainer(thing);
    }

    public static void overrideSavedContainer(MapMerchantContainer cont) {
        var data = JsonUtility.ToJson(cont);
        SaveData.setString(merchantTag, data);
    }

    public static MapMerchantContainer getContainer() {
        var data = SaveData.getString(merchantTag);
        return JsonUtility.FromJson<MapMerchantContainer>(data);
    }
}

[System.Serializable]
public class MerchantInfo {
    public ObjectHolder inv;
    public int coinCount;
    public GameInfo.region region;
    public int index;

    public MerchantInfo(int colCount, int unitCount, int cCount, int ind, GameInfo.region reg, PresetLibrary lib) {
        inv = new ObjectHolder();
        for(int i = 0; i < colCount; i++) {
            inv.addObject<Collectable>(lib.getRandomCollectable(reg));
        }
        for(int i = 0; i < unitCount; i++) {
            inv.addObject<UnitStats>(lib.createRandomPlayerUnitStats(true));
        }


        coinCount = cCount;
        index = ind;
        region = reg;
    }
}

[System.Serializable]
public class MapMerchantContainer {
    public List<MerchantInfo> grasslandMercs = new List<MerchantInfo>();
    public List<MerchantInfo> forestMercs = new List<MerchantInfo>();
    public List<MerchantInfo> swampMercs = new List<MerchantInfo>();
    public List<MerchantInfo> mountainMercs = new List<MerchantInfo>();
    public List<MerchantInfo> hellMercs = new List<MerchantInfo>();

    public void addInfo(MerchantInfo info) {
        switch(info.region) {
            case GameInfo.region.grassland:
                grasslandMercs.Add(info);
                return;
            case GameInfo.region.forest:
                forestMercs.Add(info);
                return;
            case GameInfo.region.swamp:
                swampMercs.Add(info);
                return;
            case GameInfo.region.mountains:
                mountainMercs.Add(info);
                return;
            case GameInfo.region.hell:
                hellMercs.Add(info);
                return;
        }
    }

    public List<MerchantInfo> getMercsForRegion(GameInfo.region reg) {
        return reg == GameInfo.region.grassland ? grasslandMercs : reg == GameInfo.region.forest ? forestMercs : reg == GameInfo.region.swamp ? swampMercs : reg == GameInfo.region.mountains ? mountainMercs : reg == GameInfo.region.hell ? hellMercs : null;
    }
    public void overrideInfo(MerchantInfo info) {
        switch(info.region) {
            case GameInfo.region.grassland:
                grasslandMercs[info.index] = info;
                return;
            case GameInfo.region.forest:
                forestMercs[info.index] = info;
                return;
            case GameInfo.region.swamp:
                swampMercs[info.index] = info;
                return;
            case GameInfo.region.mountains:
                mountainMercs[info.index] = info;
                return;
            case GameInfo.region.hell:
                hellMercs[info.index] = info;
                return;
        }
    }
}

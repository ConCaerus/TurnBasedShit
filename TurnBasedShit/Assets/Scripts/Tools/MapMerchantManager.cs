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
                var coinCount = Random.Range(15, 51);

                temp.merchants[i].Add(new MerchantInfo(colCount, coinCount, j, (GameInfo.region)(i), lib));
            }
            Debug.Log("B4: " + temp.merchants[i].Count);
        }

        temp.fuck = 8;

        overrideSavedContainer(temp);

        for(int i = 0; i < getContainer().merchants.Length; i++) {
            Debug.Log(((GameInfo.region)(i)).ToString() + ": " + getContainer().merchants[i].Count.ToString());
        }
        Debug.Log(temp.fuck);
    }


    public static MerchantInfo getMerchantInRegion(int index, GameInfo.region reg) {
        var data = SaveData.getString(merchantTag);
        var thing = JsonUtility.FromJson<MapMerchantContainer>(data);
        return thing.merchants[(int)reg][index];
    }
    public static void updateMerchant(MerchantInfo info) {
        var data = SaveData.getString(merchantTag);
        var thing = JsonUtility.FromJson<MapMerchantContainer>(data);
        thing.merchants[(int)info.region][info.index] = info;
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

    public MerchantInfo(int colCount, int cCount, int ind, GameInfo.region reg, PresetLibrary lib) {
        inv = new ObjectHolder();
        for(int i = 0; i < colCount; i++) {
            inv.addObject<Collectable>(lib.getRandomCollectable(reg));
        }

        coinCount = cCount;
        index = ind;
        region = reg;
    }
}

[System.Serializable]
public class MapMerchantContainer {
    public List<MerchantInfo>[] merchants = new List<MerchantInfo>[5] {
        new List<MerchantInfo>(), new List<MerchantInfo>(), new List<MerchantInfo>(), new List<MerchantInfo>(), new List<MerchantInfo>()
    };

    public int fuck = 0;
}

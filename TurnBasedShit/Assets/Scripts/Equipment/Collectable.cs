using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Collectable {
    [System.Serializable]
    public enum collectableType {
        weapon, armor, item, usable, unusable
    }

    public int instanceID = -1;

    public string name;
    public collectableType type;
    public GameInfo.region rarity;
    public int coinCost;
    public bool canBeFished = false;
    public FishedLootData fishedData = null;

    public string flavor = "Choco";

    public bool isTheSameInstanceAs(Collectable other) {
        if(other == null || other.instanceID == -1)
            return false;
        return other.instanceID == instanceID;
    }
    public bool isEmpty() {
        return instanceID == -1 && string.IsNullOrEmpty(name);
    }

    public bool isTheSameTypeAs(Collectable other) {
        if(other == null)
            return false;
        return name == other.name;
    }

    protected void matchParentValues(Collectable other, bool takeInstanceID) {
        name = other.name;
        type = other.type;
        rarity = other.rarity;
        coinCost = other.coinCost;
        canBeFished = other.canBeFished;
        fishedData = other.fishedData;

        if(takeInstanceID)
            instanceID = other.instanceID;
    }

    public abstract void setEqualTo(Collectable other, bool takeInstanceID);
}


[System.Serializable]
public class FishedLootData {
    public Vector2 hookedPos;
    public float scale;
    public float hookedRot;
    public GameInfo.region diffRegion;
    public GameInfo.fishCatchRate chanceToCatch;
}

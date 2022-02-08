using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Collectable {
    [System.Serializable]
    public enum collectableType {
        Weapon, Armor, Item, Usable, Unusable
    }

    public int instanceID = -1;

    public string name = "";
    public collectableType type = (collectableType)(-1);
    public GameInfo.region rarity = (GameInfo.region)(-1);
    public int coinCost = 0;
    public bool canBeFished = false;
    public int maxStackCount = 1;
    public FishedLootData fishedData = null;

    public string flavor = "Choco";
    [TextArea]
    public string description = "Collectable thing";

    public bool isTheSameInstanceAs(Collectable other) {
        if(other == null || other.isEmpty())
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
        maxStackCount = other.maxStackCount;
        flavor = other.flavor;
        description = other.description;

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
    public GameInfo.fishCatchRate chanceToCatch;
}

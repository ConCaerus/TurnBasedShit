using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {
    //  times at which the item is used
    public enum useTimes {
        beforeEachTurn, afterEachTurn, beforeTurn, afterTurn, afterRound
    }
    //  conditions that have to be met before the item can be used
    public enum useConditions {
        healthAboveHalf
    }
    //  effects the item has
    public enum useEffects {
        recoverHealth
    }


    public string i_name;
    public GameInfo.rarityLvl i_rarity;

    public List<useTimes> i_useTimes = new List<useTimes>();
    public List<useConditions> i_useConditions = new List<useConditions>();
    public List<useEffects> i_useEffects = new List<useEffects>();

    public List<float> i_effectMods = new List<float>();

    public int i_coinCost;

    public SpriteLoader i_sprite;



    public bool isEqualTo(Item other) {
        for(int i = 0; i < i_useTimes.Count; i++) {
            if(i_useTimes[i] != other.i_useTimes[i])
                return false;
        }
        for(int i = 0; i < i_useConditions.Count; i++) {
            if(i_useConditions[i] != other.i_useConditions[i])
                return false;
        }
        for(int i = 0; i < i_useEffects.Count; i++) {
            if(i_useEffects[i] != other.i_useEffects[i])
                return false;
        }
        for(int i = 0; i < i_effectMods.Count; i++) {
            if(i_effectMods[i] != other.i_effectMods[i])
                return false;
        }

        return i_name == other.i_name && i_rarity == other.i_rarity && i_coinCost == other.i_coinCost;
    }
}

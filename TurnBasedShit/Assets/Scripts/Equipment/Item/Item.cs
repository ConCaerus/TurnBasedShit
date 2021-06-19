using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {
    //  times at which the item is used
    [System.Serializable]
    public enum useTimes {
        beforeEachTurn, afterEachTurn, beforeTurn, afterTurn, afterRound, beforeDefending, beforeAttacking, afterKill
    }
    //  conditions that have to be met before the item can be used
    [System.Serializable]
    public enum useConditions {
        healthAboveHalf
    }
    //  effects the item has
    [System.Serializable]
    public enum useEffectTypes {
        modHealth, modMaxHealth, modSpeed, modDamageGiven, modDamageTaken
    }


    [System.Serializable]
    public struct useEffects {
        public useEffectTypes effect;
        public float effectAmount;


        public bool isEqualTo(useEffects other) {
            return effect == other.effect && effectAmount == other.effectAmount;
        }
    }


    public int i_instanceID = -1;

    public string i_name;
    public GameInfo.rarityLvl i_rarity;

    public List<useTimes> i_useTimes = new List<useTimes>();
    public List<useConditions> i_useConditions = new List<useConditions>();
    public List<useEffects> i_useEffects = new List<useEffects>();

    public int i_coinCost;

    [SerializeField] ItemSpriteHolder i_sprite;

    public float getHealthMod() {
        float temp = 0.0f;
        foreach(var i in i_useEffects) {
            if(i.effect == useEffectTypes.modHealth)
                temp += i.effectAmount;
        }
        return temp;
    }
    public float getMaxHealthMod() {
        float temp = 0.0f;
        foreach(var i in i_useEffects) {
            if(i.effect == useEffectTypes.modMaxHealth)
                temp += i.effectAmount;
        }
        return temp;
    }
    public float getDamageGivenMod() {
        float temp = 0.0f;
        foreach(var i in i_useEffects) {
            if(i.effect == useEffectTypes.modDamageGiven)
                temp += i.effectAmount;
        }
        return temp;
    }
    public float getDamageTakenMod() {
        float temp = 0.0f;
        foreach(var i in i_useEffects) {
            if(i.effect == useEffectTypes.modDamageTaken)
                temp += i.effectAmount;
        }
        return temp;
    }
    public float getSpeedMod() {
        float temp = 0.0f;
        foreach(var i in i_useEffects) {
            if(i.effect == useEffectTypes.modSpeed)
                temp += i.effectAmount;
        }
        return temp;
    }



    public bool isEqualTo(Item other) {
        if(other == null || this == null)
            return false;

        if(i_useTimes.Count != other.i_useTimes.Count)
            return false;
        for(int i = 0; i < i_useTimes.Count; i++) {
            if(i_useTimes[i] != other.i_useTimes[i])
                return false;
        }
        if(i_useConditions.Count != other.i_useConditions.Count)
            return false;
        for(int i = 0; i < i_useConditions.Count; i++) {
            if(i_useConditions[i] != other.i_useConditions[i])
                return false;
        }
        if(i_useEffects.Count != other.i_useEffects.Count)
            return false;
        for(int i = 0; i < i_useEffects.Count; i++) {
            if(!i_useEffects[i].isEqualTo(other.i_useEffects[i]))
                return false;
        }

        bool name = i_name == other.i_name;
        bool rarity = i_rarity == other.i_rarity;
        bool cost = i_coinCost == other.i_coinCost;
        return name && rarity && cost;
    }

    public void setEqualTo(Item other) {

        i_useTimes.Clear();
        for(int i = 0; i < i_useTimes.Count; i++) {
            i_useTimes[i] = other.i_useTimes[i];
        }

        i_useConditions.Clear();
        for(int i = 0; i < i_useConditions.Count; i++) {
            i_useConditions[i] = other.i_useConditions[i];
        }

        i_useEffects.Clear();
        for(int i = 0; i < i_useEffects.Count; i++) {
            i_useEffects[i] = other.i_useEffects[i];
        }

        i_name = other.i_name;
        i_rarity = other.i_rarity;
        i_coinCost = other.i_coinCost;
    }

    public bool isEmpty() {
        return string.IsNullOrEmpty(i_name) && i_useTimes.Count == 0 && i_useConditions.Count == 0 && i_useEffects.Count == 0;
    }

    public ItemSpriteHolder getSpriteHolder() {
        return i_sprite;
    }
}


[System.Serializable]
public class ItemSpriteHolder {
    public Sprite sprite;
}
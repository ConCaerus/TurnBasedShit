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
    //  effects the item has
    [System.Serializable]
    public enum passiveEffectTypes {
        modPower, modSpeed, modDefence, modHealGiven, modSummonDamageGiven, modEdgedDamageGiven, modBluntDamageGiven, modChanceToBeAttacked
    }

    [System.Serializable]
    public enum timedEffectTypes { 
        healSelf, addTempSpeed, addTempPower, addTempDefence
    }


    [System.Serializable]
    public struct timedEffects {
        public timedEffectTypes effect;
        public useTimes time;
        public float effectAmount;
    }

    [System.Serializable]
    public struct passiveEffects {
        public passiveEffectTypes effect;
        public float effectAmount;
    }


    public int i_instanceID = -1;

    public string i_name;
    public GameInfo.rarityLvl i_rarity;

    public List<timedEffects> i_timedEffects = new List<timedEffects>();
    public List<passiveEffects> i_passiveEffects = new List<passiveEffects>();

    public int i_coinCost;

    [SerializeField] ItemSpriteHolder i_sprite;

    //  passive shit
    public float getPassiveMod(passiveEffectTypes type) {
        float temp = 0.0f;
        foreach(var i in i_passiveEffects) {
            if(i.effect == type)
                temp += i.effectAmount;
        }
        return temp;
    }


    public void triggerUseTime(UnitClass unit, useTimes time) {
        foreach(var i in i_timedEffects) {
            if(i.time == time) {
                switch(i.effect) {
                    case timedEffectTypes.healSelf:
                        var healAmount = unit.stats.getModifiedMaxHealth() * i.effectAmount;
                        unit.addHealth(healAmount);
                        break;

                    case timedEffectTypes.addTempSpeed:
                        unit.tempSpeedMod += i.effectAmount;
                        break;

                    case timedEffectTypes.addTempPower:
                        unit.tempPowerMod += i.effectAmount;
                        break;

                    case timedEffectTypes.addTempDefence:
                        unit.tempDefenceMod += i.effectAmount;
                        break;
                }
            }
        }
    }



    public bool isEqualTo(Item other) {
        if(other == null || other.isEmpty())
            return false;
        return i_instanceID == other.i_instanceID;
    }

    public bool isTheSameTypeAs(Item other) {
        if(other == null || other.isEmpty())
            return false;
        return i_name == other.i_name && i_rarity == other.i_rarity;
    }

    public void setEqualTo(Item other, bool takeID) {
        if(other == null || other.isEmpty())
            return;

        i_passiveEffects.Clear();
        for(int i = 0; i < other.i_passiveEffects.Count; i++)
            i_passiveEffects.Add(other.i_passiveEffects[i]);

        i_timedEffects.Clear();
        for(int i = 0; i < other.i_timedEffects.Count; i++)
            i_timedEffects.Add(other.i_timedEffects[i]);

        i_name = other.i_name;
        i_rarity = other.i_rarity;
        i_coinCost = other.i_coinCost;

        if(takeID)
            i_instanceID = other.i_instanceID;
    }

    public bool isEmpty() {
        return string.IsNullOrEmpty(i_name) && i_timedEffects.Count == 0 && i_passiveEffects.Count == 0;
    }

    public ItemSpriteHolder getSpriteHolder() {
        return i_sprite;
    }
}


[System.Serializable]
public class ItemSpriteHolder {
    public Sprite sprite;
}
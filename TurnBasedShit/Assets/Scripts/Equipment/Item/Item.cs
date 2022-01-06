using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : Collectable {
    //  times at which the item is used
    [System.Serializable]
    public enum useTimes {
        beforeEachTurn, afterEachTurn, beforeTurn, afterTurn, afterRound, beforeDefending, beforeAttacking, afterKill
    }
    //  effects the item has
    [System.Serializable]
    public enum passiveEffectTypes {
        modPower, modSpeed, modDefence, modHealGiven, modSummonDamageGiven, modEdgedDamageGiven, modBluntDamageGiven, modChanceToBeAttacked, extraTurn
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


    public List<timedEffects> tEffects = new List<timedEffects>();
    public List<passiveEffects> pEffects = new List<passiveEffects>();

    [SerializeField] ItemSpriteHolder sprite;

    //  passive shit
    public float getPassiveMod(passiveEffectTypes type) {
        float temp = 0.0f;
        foreach(var i in pEffects) {
            if(i.effect == type)
                temp += i.effectAmount;
        }
        return temp;
    }


    public void triggerUseTime(UnitClass unit, useTimes time) {
        foreach(var i in tEffects) {
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

    public override void setEqualTo(Collectable col, bool takeID) {
        if(col.type != collectableType.item || col == null)
            return;

        var other = (Item)col;

        if(other == null)
            return;

        matchParentValues(col, takeID);

        pEffects.Clear();
        for(int i = 0; i < other.pEffects.Count; i++)
            pEffects.Add(other.pEffects[i]);

        tEffects.Clear();
        for(int i = 0; i < other.tEffects.Count; i++)
            tEffects.Add(other.tEffects[i]);
    }

    public ItemSpriteHolder getSpriteHolder() {
        return sprite;
    }
}


[System.Serializable]
public class ItemSpriteHolder {
    public Sprite sprite;
}
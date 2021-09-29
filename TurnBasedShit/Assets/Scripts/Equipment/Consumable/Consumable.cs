using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Consumable {
    const int numOfEffects = 4;
    public enum effects {
        heal, cureBleed, powerBuff, speedBuff, defenceBuff
    }

    public int c_instanceID = -1;

    public string c_name;
    public GameInfo.rarityLvl c_rarity;
    public int c_coinCost;
    public int c_maxStackCount;
    public effects c_effect;
    public float c_effectAmount;


    [SerializeField] ConsumableSpriteHolder c_sprite;

    
    public UnitStats applyEffect(GameObject unit) {
        var uc = unit.GetComponent<UnitClass>();
        switch(c_effect) {
            case effects.heal:
                uc.addHealth(c_effectAmount);
                break;

            case effects.cureBleed:
                uc.stats.u_bleedCount = 0;
                break;

            case effects.powerBuff:
                uc.tempPowerMod += c_effectAmount;
                break;

            case effects.defenceBuff:
                uc.tempDefenceMod += c_effectAmount;
                break;

            case effects.speedBuff:
                uc.tempSpeedMod += c_effectAmount;
                break;
        }

        Party.overrideUnit(uc.stats);
        return uc.stats;
    }

    public void setEqualTo(Consumable other, bool takeID) {
        c_name = other.c_name;
        c_rarity = other.c_rarity;
        c_coinCost = other.c_coinCost;
        c_maxStackCount = other.c_maxStackCount;
        c_effect = other.c_effect;
        c_effectAmount = other.c_effectAmount;

        if(takeID)
            c_instanceID = other.c_instanceID;
    }
    public bool isEqualTo(Consumable obj) {
        return c_instanceID == obj.c_instanceID;
    }

    public bool isTheSameTypeAs(Consumable other) {
        if(other == null || other.isEmpty())
            return false;
        return c_name == other.c_name;
    }

    public bool isEmpty() {
        return string.IsNullOrEmpty(c_name) && c_maxStackCount == 0 && c_effectAmount == 0;
    }

    public ConsumableSpriteHolder getSpriteHolder() {
        return c_sprite;
    }
}


[System.Serializable]
public class ConsumableSpriteHolder {
    public Sprite sprite;
}
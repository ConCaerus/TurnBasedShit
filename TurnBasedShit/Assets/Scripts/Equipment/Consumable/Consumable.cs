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
                uc.cureBleed();
                break;

            case effects.powerBuff:
                uc.tempPower += c_effectAmount;
                break;

            case effects.defenceBuff:
                uc.tempDefence += c_effectAmount;
                break;

            case effects.speedBuff:
                uc.tempSpeed += c_effectAmount;
                break;
        }

        Party.overrideUnit(uc.stats);
        return uc.stats;
    }

    public bool isEqualTo(Consumable obj) {
        if(obj.GetType() != typeof(Consumable))
            return false;

        return obj.c_effect == c_effect && obj.c_effectAmount == c_effectAmount && 
            obj.c_name == c_name && obj.c_maxStackCount == c_maxStackCount && c_rarity == obj.c_rarity;
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
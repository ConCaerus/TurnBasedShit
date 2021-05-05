using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Consumable {
    const int numOfEffects = 4;
    public enum effects {
        heal, cureBleed
    }

    public string c_name;
    public GameInfo.rarityLvl c_rarity;
    public int c_coinCost;
    public int c_maxStackCount;
    public effects c_effect;
    public float c_effectAmount;
    

    public SpriteLoader c_sprite;

    
    public UnitStats applyEffect(GameObject unit) {
        var uc = unit.GetComponent<UnitClass>();
        switch(c_effect) {
            case effects.heal:
                uc.addHealth(c_effectAmount);
                break;

            case effects.cureBleed:
                uc.cureBleed();
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
        return string.IsNullOrEmpty(c_name) && c_maxStackCount == 0 && c_effectAmount == 0 && c_sprite.getSprite(true) == null;
    }
}


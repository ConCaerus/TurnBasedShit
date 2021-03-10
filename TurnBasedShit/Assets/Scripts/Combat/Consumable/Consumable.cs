using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Consumable {
    public int numOfEffects = 4;
    public enum effects {
        heal, curePoison, cureBleed
    }

    public string c_name;
    public int c_maxStackCount;
    public effects c_effect;
    public float c_effectAmount;

    public SpriteLocation c_sprite;

    
    public UnitClassStats applyEffect(GameObject unit) {
        var uc = unit.GetComponent<UnitClass>();
        switch(c_effect) {
            case effects.heal:
                uc.addHealth(c_effectAmount);
                break;

            case effects.curePoison:
                uc.curePoison();
                break;

            case effects.cureBleed:
                //  add a bleed status to the unit class before you can heal it.
                break;
        }

        Party.resaveUnit(uc.stats);
        return uc.stats;
    }

    public bool isEqualTo(Consumable obj) {
        if(obj.GetType() != typeof(Consumable))
            return false;

        return obj.c_effect == c_effect && obj.c_effectAmount == c_effectAmount && 
            obj.c_name == c_name && obj.c_maxStackCount == c_maxStackCount;
    }

    public bool isEmpty() {
        return string.IsNullOrEmpty(c_name) && c_maxStackCount == 0 && c_effectAmount == 0;
    }
}

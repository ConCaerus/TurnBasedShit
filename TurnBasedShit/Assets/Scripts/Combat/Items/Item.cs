using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {
    public enum effects {
        heal, curePoison, speed, cureBleed
    }

    public string i_name;
    public int i_maxStackCount;
    public effects i_effect;
    public float i_effectAmount;

    public SpriteLocation i_sprite;

    
    public void applyEffect(GameObject unit) {
        var uc = unit.GetComponent<UnitClass>();
        switch(i_effect) {
            case effects.heal:
                uc.addHealth(i_effectAmount);
                break;

            case effects.curePoison:
                uc.curePoison();
                break;

            case effects.speed:
                uc.addSpeed(i_effectAmount);
                break;

            case effects.cureBleed:
                break;
        }

        Party.resaveUnit(uc.stats);
    }

    public bool isEqualTo(Item obj) {
        if(obj.GetType() != typeof(global::Item))
            return false;

        return obj.i_effect == i_effect && obj.i_effectAmount == i_effectAmount && 
            obj.i_name == i_name && obj.i_maxStackCount == i_maxStackCount;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Armor {
    public enum attributes {
        turtle, reflex, healing
    }


    public List<attributes> a_attributes = new List<attributes>();

    public float a_defence;
    public float a_speedMod;


    public float getTurtleBonusDefence() {
        float temp = 0.0f;
        foreach(var i in a_attributes) {
            if(i == attributes.turtle) {
                temp += a_defence * 0.15f;
            }
        }
        return temp;
    }


    //  NOTE: this function is applied by the defending unit while the 
    //          weapon class has its function called by the attacker
    public void applyAttributesAfterAttack(GameObject weilder, GameObject attacker) {
        foreach(var i in a_attributes) {
            if(i == attributes.reflex && !weilder.GetComponent<UnitClass>().attacking) {
                weilder.GetComponent<UnitClass>().attackUnit(attacker);
            }
            else if(i == attributes.healing) {
                weilder.GetComponent<UnitClass>().addHealth(a_defence * 0.25f);
            }
        }
    }
}

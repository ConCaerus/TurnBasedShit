using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Armor {
    public enum attributes {
        turtle, reflex, healing
    }


    public List<attributes> a_attributes = new List<attributes>();

    public float a_defence;
    public float a_speedMod;

    public SpriteLocation a_sprite;


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


    public float getTurtleBonusDefence() {
        float temp = 0.0f;
        foreach(var i in a_attributes) {
            if(i == attributes.turtle) {
                temp += a_defence * 0.15f;
            }
        }
        return temp;
    }


    public void resetArmorStats() {
        a_defence = 0;
        a_speedMod = 0;
        a_attributes.Clear();
        a_sprite.clear();
    }

    public bool isEmpty() {
        return a_attributes.Count == 0 && a_defence == 0 && a_speedMod == 0;
    }


    public void setToPreset(ArmorPreset preset) {
        var temp = preset.preset;
        a_defence = temp.a_defence;
        a_speedMod = temp.a_speedMod;
        a_attributes = temp.a_attributes;
        a_sprite = temp.a_sprite;
        a_sprite.setLocation();
    }

    public ArmorPreset armorToPreset() {
        ArmorPreset preset = new ArmorPreset();
        preset.preset = this;
        return preset;
    }
}

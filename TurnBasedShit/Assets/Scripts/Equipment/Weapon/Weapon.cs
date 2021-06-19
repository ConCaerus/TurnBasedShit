﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Weapon {
    public const int attributeCount = 3;
    public enum attribute {
        Power, Bleed, Healing
    }

    public int w_instanceID = -1;

    public string w_name;
    public GameInfo.rarityLvl w_rarity;
    public GameInfo.element w_element;
    public GameInfo.wornState w_wornAmount;
    public List<attribute> w_attributes = new List<attribute>();

    public float w_power;
    public float w_speedMod;
    public int w_coinCost;

    [SerializeField] WeaponSpriteHolder w_sprite;




    public void applyAttributesAfterAttack(GameObject weilder, GameObject attackedUnit) {
        foreach(var i in w_attributes) {
            if(i == attribute.Bleed) {
                attackedUnit.GetComponent<UnitClass>().stats.u_bleedCount++;
            }

            else if(i == attribute.Healing) {
                weilder.GetComponent<UnitClass>().addHealth(weilder.GetComponent<UnitClass>().stats.getModifiedMaxHealth() * 0.05f);
            }
        }
    }


    public float getBonusAttributeDamage() {
        float temp = 0.0f;
        foreach(var i in w_attributes) {
            if(i == attribute.Power)
                temp += w_power * 0.15f;
        }

        return temp;
    }


    public void resetWeaponStats() {
        w_power = 0;
        w_speedMod = 0;
        w_attributes.Clear();
        w_element = 0;
    }

    public bool isEmpty() {
        return w_attributes.Count == 0 && w_power == 0 && w_speedMod == 0;
    }

    public bool isEqualTo(Weapon other) {
        if(other == null)
            return false;
        if(w_attributes.Count == other.w_attributes.Count) {
            for(int i = 0; i < w_attributes.Count; i++) {
                if(w_attributes[i] != other.w_attributes[i]) {
                    return false;
                }
            }
        }


        bool power = w_power == other.w_power;
        bool speed = w_speedMod == other.w_speedMod;
        bool rarity = w_rarity == other.w_rarity;
        bool ele = w_element == other.w_element;


        return power && speed && ele && rarity;
    }

    public bool isSameInstanceAs(Weapon other) {
        if(other.w_instanceID == -1)
            return false;
        return w_instanceID == other.w_instanceID;
    }


    public void setToPreset(WeaponPreset preset) {
        var temp = preset.preset;
        w_power = temp.w_power;
        w_speedMod = temp.w_speedMod;
        w_attributes = temp.w_attributes;
        w_element = temp.w_element;
        w_sprite = temp.w_sprite;
        w_rarity = temp.w_rarity;
    }

    public WeaponPreset weaponToPreset() {
        WeaponPreset preset = (WeaponPreset)ScriptableObject.CreateInstance("WeaponPreset");
        preset.preset = this;
        return preset;
    }

    public void setEqualTo(Weapon other) {
        if(other == null)
            return;
        w_name = other.w_name;
        w_power = other.w_power;
        w_speedMod = other.w_speedMod;
        w_attributes = other.w_attributes;
        w_element = other.w_element;
        w_sprite = other.w_sprite;
        w_rarity = other.w_rarity;
    }


    public int howManyOfAttribute(attribute a) {
        var count = 0;
        foreach(var i in w_attributes) {
            if(i == a)
                count++;
        }
        return count;
    }


    public attribute getRandAttribute() {
        var rand = Random.Range(0, 101);
        float step = 100.0f / (float)attributeCount;

        int index = 0;
        while(rand >= step) {
            rand -= (int)step;
            index++;
        }

        return (attribute)index;
    }


    public WeaponSpriteHolder getSpriteHolder() {
        return w_sprite;
    }
}


[System.Serializable]
public class WeaponSpriteHolder {
    public Sprite sprite;

    public float equippedX, equippedY, equippedRot, equippedSize;
}
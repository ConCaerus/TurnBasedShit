using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Weapon {
    const int attributeCount = 3;
    public enum attributes {
        power, poison, healing
    }


    public string w_name;
    public GameInfo.rarityLvl w_rarity;

    public List<attributes> w_attributes = new List<attributes>();

    public float w_power;
    public float w_speedMod;

    public int w_coinCost;

    public SpriteLoader w_sprite;



    public void applyAttributesAfterAttack(GameObject weilder, GameObject attackedUnit) {
        foreach(var i in w_attributes) {
            if(i == attributes.poison) {
                attackedUnit.GetComponent<UnitClass>().stats.u_poisonCount++;
            }

            else if(i == attributes.healing) {
                weilder.GetComponent<UnitClass>().addHealth(weilder.GetComponent<UnitClass>().stats.u_maxHealth * 0.05f);
            }
        }
    }


    public float getPowerBonusDamage() {
        float temp = 0.0f;
        foreach(var i in w_attributes) {
            if(i == attributes.power)
                temp += w_power * 0.15f;
        }

        return temp;
    }


    public void resetWeaponStats() {
        w_power = 0;
        w_speedMod = 0;
        w_attributes.Clear();
        w_sprite.clear();
    }

    public bool isEmpty() {
        return w_attributes.Count == 0 && w_power == 0 && w_speedMod == 0;
    }

    public bool isEqualTo(Weapon other) {
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


        return power && speed && rarity;
    }


    public void setToPreset(WeaponPreset preset) {
        var temp = preset.preset;
        w_power = temp.w_power;
        w_speedMod = temp.w_speedMod;
        w_attributes = temp.w_attributes;
        w_sprite = temp.w_sprite;
        w_rarity = temp.w_rarity;
    }

    public WeaponPreset weaponToPreset() {
        WeaponPreset preset = (WeaponPreset)ScriptableObject.CreateInstance("WeaponPreset");
        preset.preset = this;
        return preset;
    }


    public int howManyOfAttribute(attributes a) {
        var count = 0;
        foreach(var i in w_attributes) {
            if(i == a)
                count++;
        }
        return count;
    }


    public attributes getRandAttribute() {
        var rand = Random.Range(0, 101);
        float step = 100.0f / (float)attributeCount;

        int index = 0;
        while(rand >= step) {
            rand -= (int)step;
            index++;
        }

        return (attributes)index;
    }
}


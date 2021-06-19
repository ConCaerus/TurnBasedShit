using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Armor {
    public const int attributeCount = 2;
    public enum attributes {
        Turtle, Reflex
    }

    public int a_instanceID = -1;

    public string a_name;

    public GameInfo.rarityLvl a_rarity;
    public GameInfo.wornState a_wornAmount;
    public List<attributes> a_attributes = new List<attributes>();

    public float a_defence;
    public float a_speedMod;
    public int a_coinCost;

    [SerializeField] ArmorSpriteHolder a_sprite;

    //  NOTE: this function is applied by the defending unit while the 
    //          weapon class has its function called by the attacker
    public int applyAttributesAfterAttack(GameObject weilder, GameObject attacker) {
        foreach(var i in a_attributes) {
            if(i == attributes.Reflex) {
                weilder.GetComponent<UnitClass>().attack(attacker);
                return 1;
            }
        }
        return 0;
    }


    public float getDefenceMult() {
        return a_defence / 100.0f;
    }


    public float getBonusAttributeDefenceMult() {
        float temp = 0.0f;
        foreach(var i in a_attributes) {
            if(i == attributes.Turtle) {
                temp += 0.15f;
            }
        }
        return temp;
    }


    public void resetArmorStats() {
        a_defence = 0;
        a_speedMod = 0;
        a_attributes.Clear();
    }

    public bool isEmpty() {
        return a_attributes.Count == 0 && a_defence == 0 && a_speedMod == 0;
    }

    public bool isEqualTo(Armor other) {
        if(other == null)
            return false;
        if(a_attributes.Count == other.a_attributes.Count) {
            for(int i = 0; i < a_attributes.Count; i++) {
                if(a_attributes[i] != other.a_attributes[i]) {
                    return false;
                }
            }
        }


        bool defence = a_defence == other.a_defence;
        bool speed = a_speedMod == other.a_speedMod;
        bool rarity = a_rarity == other.a_rarity;


        return defence && speed && rarity;
    }


    public void setToPreset(ArmorPreset preset) {
        var temp = preset.preset;
        a_defence = temp.a_defence;
        a_speedMod = temp.a_speedMod;
        a_attributes = temp.a_attributes;
        a_sprite = temp.a_sprite;
        a_rarity = temp.a_rarity;
    }

    public ArmorPreset armorToPreset() {
        ArmorPreset preset = (ArmorPreset)ScriptableObject.CreateInstance("ArmorPreset");
        preset.preset = this;
        return preset;
    }

    public void setEqualTo(Armor other) {
        a_name = other.a_name;
        a_defence = other.a_defence;
        a_speedMod = other.a_speedMod;
        a_attributes = other.a_attributes;
        a_sprite = other.a_sprite;
        a_rarity = other.a_rarity;
    }


    public int howManyOfAttribute(attributes a) {
        var count = 0;
        foreach(var i in a_attributes) {
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


    public ArmorSpriteHolder getSpriteHolder() {
        return a_sprite;
    }
}


[System.Serializable]
public class ArmorSpriteHolder {
    public Sprite sprite, equippedSprite;
}
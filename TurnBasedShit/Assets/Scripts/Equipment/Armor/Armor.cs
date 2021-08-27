using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Armor {
    public const int attributeCount = 2;
    public enum attribute {
        Turtle, Reflex
    }

    public int a_instanceID = -1;

    public string a_name;

    public GameInfo.rarityLvl a_rarity;
    public GameInfo.wornState a_wornAmount;
    public List<attribute> a_attributes = new List<attribute>();

    public float a_defence;
    public float a_speedMod;
    public int a_coinCost;

    [SerializeField] ArmorSpriteHolder a_sprite;

    //  NOTE: this function is applied by the defending unit while the 
    //          weapon class has its function called by the attacker
    public int applyAttributesAfterAttack(GameObject weilder, GameObject attacker, GameObject turnTaker) {
        foreach(var i in a_attributes) {
            if(i == attribute.Reflex && turnTaker != weilder) {
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
            if(i == attribute.Turtle) {
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
        return a_instanceID == other.a_instanceID;
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

    public void setEqualTo(Armor other, bool takeID) {
        a_name = other.a_name;
        a_defence = other.a_defence;
        a_speedMod = other.a_speedMod;
        a_attributes = other.a_attributes;
        a_sprite = other.a_sprite;
        a_rarity = other.a_rarity;

        if(takeID)
            a_instanceID = other.a_instanceID;
    }


    public int howManyOfAttribute(attribute a) {
        var count = 0;
        foreach(var i in a_attributes) {
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


    public ArmorSpriteHolder getSpriteHolder() {
        return a_sprite;
    }
}


[System.Serializable]
public class ArmorSpriteHolder {
    public Sprite sprite, equippedSprite;

    public float xPos0, yPos0, xSize0, ySize0;
    public float xPos1, yPos1, xSize1, ySize1;
    public float xPos2, yPos2, xSize2, ySize2;

    public Vector2 getRelevantPos(int bodyIndex) {
        if(bodyIndex == 0)
            return new Vector2(xPos0, yPos0);
        else if(bodyIndex == 1)
            return new Vector2(xPos1, yPos1);
        else if(bodyIndex == 2)
            return new Vector2(xPos1, yPos1);

        return Vector2.zero;
    }

    public Vector2 getRelevantSize(int bodyIndex) {
        if(bodyIndex == 0)
            return new Vector2(xSize0, ySize0);
        else if(bodyIndex == 1)
            return new Vector2(xSize1, ySize1);
        else if(bodyIndex == 2)
            return new Vector2(xSize2, ySize2);

        Debug.Log(bodyIndex);
        return Vector2.zero;
    }
}
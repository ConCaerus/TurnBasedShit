using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Armor : Collectable {
    public const int attributeCount = 2;
    public enum attribute {
        Turtle, Reflex, Power
    }
    public GameInfo.wornState wornAmount = GameInfo.wornState.Perfect;
    public List<attribute> attributes = new List<attribute>();

    public float defence;
    public float speedMod;

    public ArmorSpriteHolder sprite;

    //  this function is applied by the defending unit while the weapon class has its function called by the attacker
    public int applyAttributes(GameObject weilder, GameObject attacker, GameObject turnTaker) {
        foreach(var i in attributes) {
            if(i == attribute.Reflex && turnTaker != weilder) {
                weilder.GetComponent<UnitClass>().attack(attacker);
                return 1;
            }
        }
        return 0;
    }


    public float getBonusAttributeDefenceMult() {
        float temp = 0.0f;
        foreach(var i in attributes) {
            if(i == attribute.Turtle) {
                temp += 0.15f;
            }
        }
        return temp;
    }

    public int getTurtleAttCount() {
        int count = 0;
        foreach(var i in attributes) {
            if(i == attribute.Turtle) {
                count++;
            }
        }
        return count;
    }

    public int getPowerAttCount() {
        int count = 0;
        foreach(var i in attributes) {
            if(i == attribute.Power) {
                count++;
            }
        }
        return count;
    }

    public override void setEqualTo(Collectable col, bool takeID) {
        if(col.type != collectableType.Armor || col == null)
            return;

        var other = (Armor)col;
        if(other == null)
            return;

        matchParentValues(col, takeID);
        defence = other.defence;
        speedMod = other.speedMod;
        attributes = other.attributes;
        sprite = other.sprite;
        wornAmount = other.wornAmount;
    }


    public int howManyOfAttribute(attribute a) {
        var count = 0;
        foreach(var i in attributes) {
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
}


[System.Serializable]
public class ArmorSpriteHolder {
    public Sprite sprite, equippedSprite, equippedShoulder, equippedHat;

    //  main
    public Vector2[] pos = new Vector2[3];
    public Vector2[] size = new Vector2[3];

    //  shoulders
    public Vector2[] shoulderPos = new Vector2[3];
    public Vector2[] shoulderSize = new Vector2[3];
    public float shoulderRot;

    //  hat
    public bool hatInfrontOfHead = true;
    public Vector2[] hatPos = new Vector2[3];
    public Vector2[] hatSize = new Vector2[3];
    public float hatRot;

    public Vector2 getRelevantPos(int bodyIndex) {
        return pos[bodyIndex];
    }
    public Vector2 getRelevantSize(int bodyIndex) {
        return size[bodyIndex];
    }

    public Vector2 getRelevantShoulderPos(int bodyIndex) {
        return shoulderPos[bodyIndex];
    }
    public Vector2 getRelevantShoulderSize(int bodyIndex) {
        return shoulderSize[bodyIndex];
    }

    public Vector2 getRelevantHatPos(int headIndex) {
        return hatPos[headIndex];
    }
    public Vector2 getRelevantHatSize(int headIndex) {
        return hatSize[headIndex];
    }
}
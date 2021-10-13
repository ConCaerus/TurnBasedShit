using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Weapon {
    public const int attributeCount = 4;
    public enum attribute {
        Power, Bleed, Healing, Stun
    }

    public enum attackType {
        blunt, edged
    }
    public enum specialUsage {
        none, healing, summoning
    }

    public int w_instanceID = -1;

    public string w_name;
    public GameInfo.rarityLvl w_rarity;
    public GameInfo.element w_element;
    public GameInfo.wornState w_wornAmount;
    public List<attribute> w_attributes = new List<attribute>();
    public specialUsage w_specialUsage = specialUsage.none;
    public float w_specialUsageAmount = 0.0f;

    public attackType w_attackType;
    public float w_power;
    public float w_speedMod;
    public int w_coinCost;

    [SerializeField] WeaponSpriteHolder w_sprite;




    public void applyAttributes(GameObject weilder, GameObject attackedUnit) {
        foreach(var i in w_attributes) {
            if(i == attribute.Bleed) {
                if(!GameVariables.chanceBleed())
                    continue;
                attackedUnit.GetComponent<UnitClass>().stats.u_bleedCount++;
            }

            else if(i == attribute.Healing) {
                weilder.GetComponent<UnitClass>().addHealth(weilder.GetComponent<UnitClass>().stats.getModifiedMaxHealth() * 0.05f);
            }

            else if(i == attribute.Stun) {
                if(attackedUnit.GetComponent<UnitClass>().isStunned())
                    continue;
                attackedUnit.GetComponent<UnitClass>().setStunned(GameVariables.chanceStun());
            }
        }
    }

    public void applySpecailUsage(UnitStats weilder, UnitClass affectedObject) {
        if(w_specialUsage == specialUsage.none)
            return;
        if(w_specialUsage == specialUsage.healing) {
            var healAmount = w_specialUsageAmount * weilder.getCritMult();

            //  apply item effects
            if(weilder.equippedItem != null && !weilder.equippedItem.isEmpty()) {
                healAmount += healAmount * weilder.equippedItem.getPassiveMod(Item.passiveEffectTypes.modHealGiven);
            }
            affectedObject.addHealth(healAmount);
        }
    }

    public int getPowerAttCount() {
        int count = 0;
        foreach(var i in w_attributes) {
            if(i == attribute.Power)
                count++;
        }

        return count;
    }

    public bool isEmpty() {
        return w_attributes.Count == 0 && w_power == 0 && w_speedMod == 0;
    }

    public bool isEqualTo(Weapon other) {
        if(other == null || other.isEmpty())
            return false;
        return w_instanceID == other.w_instanceID;
    }

    public bool isTheSameTypeAs(Weapon other) {
        if(other == null || other.isEmpty())
            return false;
        return w_name == other.w_name && w_element == other.w_element && w_rarity == other.w_rarity;
    }

    public WeaponPreset weaponToPreset() {
        WeaponPreset preset = (WeaponPreset)ScriptableObject.CreateInstance("WeaponPreset");
        preset.preset = this;
        return preset;
    }

    public void setEqualTo(Weapon other, bool takeID) {
        if(other == null)
            return;
        w_name = other.w_name;
        w_power = other.w_power;
        w_speedMod = other.w_speedMod;
        w_attributes = other.w_attributes;
        w_element = other.w_element;
        w_sprite = other.w_sprite;
        w_rarity = other.w_rarity;
        w_specialUsage = other.w_specialUsage;
        w_specialUsageAmount = other.w_specialUsageAmount;
        w_wornAmount = other.w_wornAmount;
        w_attackType = other.w_attackType;

        if(takeID)
            w_instanceID = other.w_instanceID;
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

    public float equippedX, equippedY, equippedRot;
    public float equippedXSize, equippedYSize;
}
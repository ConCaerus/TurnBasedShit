using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Weapon : Collectable {
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

    public GameInfo.wornState wornAmount = GameInfo.wornState.perfect;
    public List<attribute> attributes = new List<attribute>();
    public specialUsage sUsage = specialUsage.none;
    public float sUsageAmount = 0.0f;

    public attackType aType;
    public float power;
    public float speedMod;

    [SerializeField] WeaponSpriteHolder sprite;




    public void applyAttributes(GameObject weilder, GameObject attackedUnit) {
        foreach(var i in attributes) {
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
        if(sUsage == specialUsage.none)
            return;
        if(sUsage == specialUsage.healing) {
            var healAmount = sUsageAmount * weilder.getCritMult();

            //  apply item effects
            if(weilder.equippedItem != null && !weilder.equippedItem.isEmpty()) {
                healAmount += healAmount * weilder.equippedItem.getPassiveMod(Item.passiveEffectTypes.modHealGiven);
            }
            affectedObject.addHealth(healAmount);
        }
    }

    public int getPowerAttCount() {
        int count = 0;
        foreach(var i in attributes) {
            if(i == attribute.Power)
                count++;
        }

        return count;
    }


    public WeaponPreset weaponToPreset() {
        WeaponPreset preset = (WeaponPreset)ScriptableObject.CreateInstance("WeaponPreset");
        preset.preset = this;
        return preset;
    }

    public override void setEqualTo(Collectable col, bool takeID) {
        if(col.type != collectableType.weapon || col.isEmpty())
            return;
        var other = (Weapon)col;
        if(other == null || other.isEmpty())
            return;

        if(other == null)
            return;
        matchParentValues(col, takeID);
        power = other.power;
        speedMod = other.speedMod;
        attributes = other.attributes;
        sprite = other.sprite;
        sUsage = other.sUsage;
        sUsageAmount = other.sUsageAmount;
        wornAmount = other.wornAmount;
        aType = other.aType;
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


    public WeaponSpriteHolder getSpriteHolder() {
        return sprite;
    }
}


[System.Serializable]
public class WeaponSpriteHolder {
    public Sprite sprite;

    public float equippedX, equippedY, equippedRot;
    public float equippedXSize, equippedYSize;
}
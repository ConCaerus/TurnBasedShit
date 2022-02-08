using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitTrait {

    [System.Serializable]
    public enum modifierType {
        damageGiven, damageTaken, speed, maxHealth, stunsSelfChance, stunsTargetChance, chanceToBeAttacked, enemyDropChance, bluntDmgGiven,
        summonExpMod, getStunnedMod, allWeaponExpMod, bluntExpMod, edgedExpMod
    }

    [System.Serializable]
    public enum conditionType {
        naked
    }

    [System.Serializable]
    public struct traitInfo {
        public modifierType modType;
        public conditionType condition;
        public float modAmount;
    }




    public string t_name;
    public List<traitInfo> t_infos = new List<traitInfo>();
    public bool t_isGood;





    //  Do trait things here
    //  value based mods
    public float getDamageTakenMod() {
        float temp = 0.0f;
        foreach(var i in t_infos) {
            if(i.modType == modifierType.damageGiven)
                temp += i.modAmount;
        }

        return temp;
    }
    public float getDamageGivenMod(Weapon.attackType type) {
        float temp = 0.0f;
        foreach(var i in t_infos) {
            if(i.modType == modifierType.damageTaken)
                temp += i.modAmount;
            else if(i.modType == modifierType.bluntDmgGiven && type == Weapon.attackType.Blunt)
                temp += i.modAmount;
        }

        return temp;
    }
    public float getSpeedMod() {
        float temp = 0.0f;
        foreach(var i in t_infos) {
            if(i.modType == modifierType.speed)
                temp += i.modAmount;
        }

        return temp;
    }
    public float getMaxHealthMod() {
        float temp = 0.0f;
        foreach(var i in t_infos) {
            if(i.modType == modifierType.maxHealth)
                temp += i.modAmount;
        }

        return temp;
    }
    public float getChanceToBeAttackedMod() {
        float temp = 0.0f;
        foreach(var i in t_infos) {
            if(i.modType == modifierType.chanceToBeAttacked)
                temp += i.modAmount;
        }

        return temp;
    }
    public float getGetStunnedMod() {
        float temp = 1.0f;  //  starts at 1 (to set to zero, modAmount needs to be -1)
        foreach(var i in t_infos) {
            if(i.modType == modifierType.getStunnedMod)
                temp += i.modAmount;
        }

        return temp;
    }

    public float getSummonExpMod() {
        float temp = 1.0f;
        foreach(var i in t_infos) {
            if(i.modType == modifierType.summonExpMod)
                temp += i.modAmount;
            else if(i.modType == modifierType.allWeaponExpMod)
                temp += i.modAmount;
        }
        return temp;
    }
    public float getBluntExpMod() {
        float temp = 1.0f;
        foreach(var i in t_infos) {
            if(i.modType == modifierType.bluntExpMod)
                temp += i.modAmount;
            else if(i.modType == modifierType.allWeaponExpMod)
                temp += i.modAmount;
        }
        return temp;
    }
    public float getEdgedExpMod() {
        float temp = 1.0f;
        foreach(var i in t_infos) {
            if(i.modType == modifierType.edgedExpMod)
                temp += i.modAmount;
            else if(i.modType == modifierType.allWeaponExpMod)
                temp += i.modAmount;
        }
        return temp;
    }

    public int getEnemyDropChanceMod() {
        int temp = 0;
        foreach(var i in t_infos) {
            if(i.modType == modifierType.enemyDropChance)
                temp += (int)i.modAmount;
        }

        return temp;
    }

    //  non-value based mods
    public bool shouldStunSelf() {
        foreach(var i in t_infos) {
            if(i.modType == modifierType.stunsSelfChance && GameVariables.chanceOutOfHundred(Mathf.FloorToInt(i.modAmount)))
                return true;
        }
        return false;
    }
    public bool shouldStunTarget() {
        foreach(var i in t_infos) {
            if(i.modType == modifierType.stunsTargetChance && GameVariables.chanceOutOfHundred(Mathf.FloorToInt(i.modAmount)))
                return true;
        }
        return false;
    }


    public bool isTheSameTypeAs(UnitTrait other) {
        return t_name == other.t_name && t_isGood == other.t_isGood;
    }
}

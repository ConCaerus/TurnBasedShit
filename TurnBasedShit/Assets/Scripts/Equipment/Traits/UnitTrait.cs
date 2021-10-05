using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitTrait {

    [System.Serializable]
    public enum modifierType {
        damageGiven, damageTaken, speed, maxHealth, stunsSelf, stunsTarget, chanceToBeAttacked, enemyDropChance
    }

    [System.Serializable]
    public struct TraitInfo {
        public modifierType modType;
        public float modAmount;
    }




    public string t_name;
    public List<TraitInfo> t_infos = new List<TraitInfo>();
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
    public float getDamageGivenMod() {
        float temp = 0.0f;
        foreach(var i in t_infos) {
            if(i.modType == modifierType.damageTaken)
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
            if(i.modType == modifierType.stunsSelf && GameVariables.chanceOutOfHundred(Mathf.FloorToInt(i.modAmount)))
                return true;
        }
        return false;
    }
    public bool shouldStunTarget() {
        foreach(var i in t_infos) {
            if(i.modType == modifierType.stunsTarget && GameVariables.chanceOutOfHundred(Mathf.FloorToInt(i.modAmount)))
                return true;
        }
        return false;
    }
}

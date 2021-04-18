using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitTrait {

    [System.Serializable]
    public enum modifierType {
        damageGiven, damageTaken, speed, maxHealth, inventoryAfterAnchorSet
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
}

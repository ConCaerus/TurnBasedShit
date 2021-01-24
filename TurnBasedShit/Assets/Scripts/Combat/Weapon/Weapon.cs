using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon {
    public enum attributes {
        power, poison, healing
    }


    public List<attributes> w_attributes = new List<attributes>();

    public float w_power;
    public float w_speedMod;



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
    }
}

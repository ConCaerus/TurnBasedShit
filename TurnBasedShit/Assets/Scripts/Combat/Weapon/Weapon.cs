using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Weapon {
    public enum attributes {
        power, poison, healing
    }


    public List<attributes> w_attributes = new List<attributes>();

    public float w_power;
    public float w_speedMod;

    public SpriteLocation w_sprite;



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


    public void setToPreset(WeaponPreset preset) {
        var temp = preset.preset;
        w_power = temp.w_power;
        w_speedMod = temp.w_speedMod;
        w_attributes = temp.w_attributes;
        w_sprite = temp.w_sprite;
        w_sprite.setLocation();
    }


    public WeaponPreset weaponToPreset() {
        WeaponPreset preset = new WeaponPreset();
        preset.preset = this;
        return preset;
    }
}

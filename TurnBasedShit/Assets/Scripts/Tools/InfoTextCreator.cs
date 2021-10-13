﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InfoTextCreator {

    public static string createForUnitClass(UnitClass unit) {
        string temp = "<u><b>";
        temp += unit.stats.u_name + "\n";
        temp += "</b></u>hp: " + unit.stats.u_health.ToString("0.0") + " / " + unit.stats.getModifiedMaxHealth().ToString("0.0");
        temp += "\n   pow: " + unit.stats.u_power.ToString("0.0");
        if(unit.tempPowerMod > 1.0f)
            temp += " * " + unit.tempPowerMod.ToString("0.0");

        temp += "\n   def: " + unit.stats.u_defence.ToString("0.0");
        if(unit.tempDefenceMod > 0.0f)
            temp += " * " + unit.tempDefenceMod.ToString("0.0");

        temp += "\n   spd: " + unit.stats.u_speed.ToString("0.0");
        if(unit.tempSpeedMod > 1.0f)
            temp += " + " + unit.tempSpeedMod.ToString("0.0");

        return temp;
    }
    public static string createForUnitStats(UnitStats stats) {
        string temp = "<u><b>";
        temp += stats.u_name + "\n";
        temp += "</b></u>hp: " + stats.u_health.ToString("0.0") + " / " + stats.getModifiedMaxHealth().ToString("0.0");
        temp += "\n   pow: " + stats.u_power.ToString("0.0");

        temp += "\n   def: " + stats.u_defence.ToString("0.0");

        temp += "\n   spd: " + stats.u_speed.ToString("0.0");

        return temp;
    }

    public static string createForUnitTrait(UnitTrait trait) {
        string temp = "";
        foreach(var i in trait.t_infos) {
            switch(i.modType) {
                case UnitTrait.modifierType.damageGiven:
                    if(i.modAmount > 0.0f)
                        temp += "+" + (i.modAmount * 100.0f).ToString("0.0") + "% Damage Delt";
                    else
                        temp += "-" + Mathf.Abs(i.modAmount * 100.0f).ToString("0.0") + "% Damage Delt";
                    break;

                case UnitTrait.modifierType.damageTaken:
                    if(i.modAmount > 0.0f)
                        temp += "-" + (i.modAmount * 100.0f).ToString("0.0") + "% Damage Recieved";
                    else
                        temp += "+" + Mathf.Abs(i.modAmount * 100.0f).ToString("0.0") + "% Damage Recieved";
                    break;

                case UnitTrait.modifierType.speed:
                    if(i.modAmount > 0.0f)
                        temp += "+" + i.modAmount.ToString("0.0") + " Speed";
                    else
                        temp += "-" + Mathf.Abs(i.modAmount).ToString("0.0") + " Speed";
                    break;

                case UnitTrait.modifierType.maxHealth:
                    if(i.modAmount > 0.0f)
                        temp += "+" + i.modAmount.ToString("0.0") + " Max Health";
                    else
                        temp += "-" + Mathf.Abs(i.modAmount).ToString("0.0") + "Max Health";
                    break;

                case UnitTrait.modifierType.stunsSelf:
                    if(i.modAmount > 0.0f)
                        temp += "+" + i.modAmount.ToString("0.0") + "% Self Stun Chance";
                    else
                        temp += "-" + Mathf.Abs(i.modAmount).ToString("0.0") + "% Self Stun Chance";
                    break;

                case UnitTrait.modifierType.stunsTarget:
                    if(i.modAmount > 0.0f)
                        temp += "+" + i.modAmount.ToString("0.0") + "% Target Stun Chance";
                    else
                        temp += "-" + Mathf.Abs(i.modAmount).ToString("0.0") + "% Target Stun Chance";
                    break;

                case UnitTrait.modifierType.chanceToBeAttacked:
                    if(i.modAmount > 0.0f)
                        temp += "+" + (i.modAmount * 100.0f).ToString("0.0") + "% Targeted Chance";
                    else
                        temp += "-" + Mathf.Abs(i.modAmount * 100.0f).ToString("0.0") + "% Targeted Chance";
                    break;

                case UnitTrait.modifierType.enemyDropChance:
                    if(i.modAmount > 0.0f)
                        temp += "+" + i.modAmount.ToString("0.0") + "% Drop Chance";
                    else
                        temp += "-" + Mathf.Abs(i.modAmount).ToString("0.0") + "% Drop Chance";
                    break;
            }
            temp += "\n";
        }
        return temp;
    }


    public static string createForWeapon(Weapon we) {
        return "<b><u>" + we.w_name;
    }
    public static string createForArmor(Armor ar) {
        return "<b><u>" + ar.a_name;
    }
    public static string createForItem(Item it) {
        return "<b><u>" + it.i_name;
    }
    public static string createForConsumable(Consumable con) {
        return "<b><u>" + con.c_name;
    }
}

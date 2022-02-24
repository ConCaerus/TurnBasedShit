using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InfoTextCreator {

    public static string createForUnitClass(UnitClass unit) {
        string temp = "";
        temp += unit.stats.u_name + "\n";
        temp += "</b></u>hp: " + unit.stats.u_health.ToString("0.0") + " / " + unit.stats.getModifiedMaxHealth().ToString("0.0");
        temp += "\n   pow: " + unit.stats.u_power.ToString("0.0");
        if(unit.combatStats.tempPowerMod > 1.0f)
            temp += " * " + unit.combatStats.tempPowerMod.ToString("0.0");

        temp += "\n   def: " + unit.stats.u_defence.ToString("0.0");
        if(unit.combatStats.tempDefenceMod > 0.0f)
            temp += " * " + unit.combatStats.tempDefenceMod.ToString("0.0");

        temp += "\n   spd: " + unit.stats.u_speed.ToString("0.0");
        if(unit.combatStats.tempSpeedMod > 1.0f)
            temp += " + " + unit.combatStats.tempSpeedMod.ToString("0.0");

        return temp;
    }
    public static string createForUnitStats(UnitStats stats) {
        string temp = "";
        temp += stats.u_name + "\n";
        temp += "</b></u>hp: " + stats.u_health.ToString("0.0") + " / " + stats.getModifiedMaxHealth().ToString("0.0");
        temp += "\n   pow: " + stats.u_power.ToString("0.0");

        temp += "\n   def: " + stats.u_defence.ToString("0.0");

        temp += "\n   spd: " + stats.u_speed.ToString("0.0");

        return temp;
    }

    public static string createForCombatCardWeapon(Weapon we) {
        return we.name + ": " + (we.power >= 0f ? "+" : "-") + we.power.ToString("0.0");
    }
    public static string createForCombatCardArmor(Armor ar) {
        return ar.name  + (ar.getPowerAttCount() > 0f ? ": x" + ar.getPowerAttCount().ToString() : "");
    }
    public static string createForCombatCardItem(Item it) {
        return it.name;
    }

    public static string createForUnitTrait(UnitTrait trait) {
        string temp = "";
        /*  TODO: fuck bitches
        foreach(var i in trait.passiveMods) {
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

                case UnitTrait.modifierType.stunsSelfChance:
                    if(i.modAmount > 0.0f)
                        temp += "+" + i.modAmount.ToString("0.0") + "% Self Stun Chance";
                    else
                        temp += "-" + Mathf.Abs(i.modAmount).ToString("0.0") + "% Self Stun Chance";
                    break;

                case UnitTrait.modifierType.stunsTargetChance:
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
        }*/
        return temp;
    }


    public static string createForCollectable(Collectable col) {
        if(col == null || col.isEmpty())
            return "Empty";
        return col.name;
    }
}

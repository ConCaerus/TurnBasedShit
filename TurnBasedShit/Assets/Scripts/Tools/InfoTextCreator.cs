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
        return ar.name  + (ar.getAttCount(Armor.attribute.Power) > 0f ? ": x" + ar.getAttCount(Armor.attribute.Power).ToString() : "");
    }
    public static string createForCombatCardItemPassive(Item it, UnitStats stats, StatModifier.passiveModifierType type) {
        var val = it.getPassiveMod(type, stats, false);
        return val == 0f ? "" : it.name + createTextForPassiveMod(type, val);
    }
    public static string createForTraitPassive(UnitTrait t, UnitStats stats, StatModifier.passiveModifierType type) {
        var val = t.getPassiveMod(type, stats, false);
        return val == 0f ? "" : t.t_name + createTextForPassiveMod(type, val);
    }

    public static string createTextForPassiveMod(StatModifier.passiveModifierType type, float val) {
        if(val == 0f)
            return "";
        switch(type) {
            case StatModifier.passiveModifierType.modPower:             return "Pow x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.modDefence:           return "Def x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.modSpeed:             return "Spd x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.modMaxHealth:         return "Max Hp x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.modSummonPower:       return "Summon's Pow x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.modSummonDefence:     return "Summon's Def x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.modSummonSpeed:       return "Summon's Spd x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.modSummonMaxHealth:   return "Summon's Max Hp x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.modEdgedPower:        return "Edged Pow x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.modBluntPower:        return "Blunt Pow x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.modEdgedDefence:      return "Edged Def x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.modBluntDefence:      return "Blunt Def x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.modAttackedChance:    return "Chance to be Attacked x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.addExtraTurn:         return val.ToString("0.0") + " Extra turn" + (val > 1 ? "s" : "");
            case StatModifier.passiveModifierType.missChance:           return "Miss Chance x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.healInsteadOfDamage:  return "Heals Instead of Dealing Damage";
            case StatModifier.passiveModifierType.modEnemyMissChance:   return "Target's Miss Chance x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.modHealthGiven:       return "Healing Given x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.stunSelfChance:       return "Stuns Self" + val.ToString("0.0") + "%";
            case StatModifier.passiveModifierType.stunTargetChance:     return "Stuns Target" + val.ToString("0.0") + "%";  
            case StatModifier.passiveModifierType.modEnemyDropChance:   return "Enemy Drop Chance x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.allWeaponExpMod:      return "Weapon Exp x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.bluntExpMod:          return "Blunt Exp x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.edgedExpMod:          return "Edged Exp x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.summonExpMod:         return "Summon Exp x" + val.ToString("0.0");
            case StatModifier.passiveModifierType.addChargedPower:      return "Charged Pow +" + (val * 100.0f).ToString("0.0") + "%";
            default:    return "";
        }
    }

    public static string createForUnitTrait(UnitTrait tr) {
        return tr.t_name;
    }


    public static string createForCollectable(Collectable col) {
        if(col == null || col.isEmpty())
            return "Empty";
        return col.name;
    }

    public static string createForWeaponAtt(Weapon we, Weapon.attribute att) {
        var count = we.getAttCount(att);
        if(count == 0)
            return "";

        return we.name + ": " + (att == Weapon.attribute.LifeSteal ? "Life Steal" : att.ToString()) + " " + count;
    }
    public static string createForArmorAtt(Armor ar, Armor.attribute att) {
        var count = ar.getAttCount(att);
        if(count == 0)
            return "";

        return ar.name + ": " + att.ToString() + " " + count;
    }
}

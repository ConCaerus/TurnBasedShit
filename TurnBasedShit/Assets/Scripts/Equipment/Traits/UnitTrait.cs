using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitTrait {

    public string t_name;
    public List<StatModifier.passiveMod> passiveMods = new List<StatModifier.passiveMod>();
    public List<StatModifier.timedMod> timedMods = new List<StatModifier.timedMod>();
    public bool t_isGood;



    public float getPassiveMod(StatModifier.passiveModifierType mod, UnitStats user, bool multing) {
        float temp = 0.0f;

        bool noMatches = true;
        foreach(var i in passiveMods) {
            if(i.type == mod) {
                noMatches = false;
                temp += i.getMod(mod, user, false);
            }
        }

        if(noMatches)
            return multing ? 1.0f : 0.0f;
        return temp;
    }

    public float getTimedMod(StatModifier.timedModifierType mod, UnitClass user, bool multing, StatModifier.useTimeType time) {
        float temp = 0.0f;

        bool noMatches = true;
        foreach(var i in timedMods) {
            if(i.type == mod) {
                foreach(var t in i.useTimes) {
                    if(time == t) {
                        noMatches = false;
                        temp += i.getMod(mod, user, multing);
                        break;
                    }
                }
            }
        }

        if(noMatches)
            return multing ? 1.0f : 0.0f;
        return temp;
    }


    public bool isTheSameTypeAs(UnitTrait other) {
        return t_name == other.t_name && t_isGood == other.t_isGood;
    }
}

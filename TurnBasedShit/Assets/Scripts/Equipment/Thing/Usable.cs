﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Usable : Collectable {
    const int numOfEffects = 4;
    public enum effectType {
        heal, cureBleed, powerBuff, speedBuff, defenceBuff
    }


    public int maxStackCount = 1;
    [SerializeField] UsableSpriteHolder sprite;

    public effectType effect;
    public float effectAmount;

    //  returns true if the effect was applied
    public bool applyStatsEffect(UnitStats stats, PartyObject po) {
        if(po != null)
            stats = po.getInstantiatedMember(stats).GetComponent<UnitClass>().stats;

        switch(effect) {
            case effectType.heal:
                if(stats.u_health == stats.getModifiedMaxHealth())
                    return false;
                stats.u_health = Mathf.Clamp(stats.u_health + effectAmount, -10.0f, stats.getModifiedMaxHealth());
                break;

            case effectType.cureBleed:
                if(stats.u_bleedCount == 0)
                    return false;
                stats.u_bleedCount = 0;
                break;
        }

        if(po != null)
            po.resaveInstantiatedUnit(stats);
        Party.overrideUnit(stats);

        return true;
    }

    public UnitStats applyEffect(GameObject thing) {
        var uc = thing.GetComponent<UnitClass>();
        switch(effect) {
            case effectType.heal:
                uc.addHealth(effectAmount);
                break;

            case effectType.cureBleed:
                uc.stats.u_bleedCount = 0;
                break;

            case effectType.powerBuff:
                uc.tempPowerMod += effectAmount;
                break;

            case effectType.defenceBuff:
                uc.tempDefenceMod += effectAmount;
                break;

            case effectType.speedBuff:
                uc.tempSpeedMod += effectAmount;
                break;
        }

        Party.overrideUnit(uc.stats);
        return uc.stats;
    }

    public override void setEqualTo(Collectable col, bool takeInstanceID) {
        if(col.type != collectableType.usable || col.isEmpty())
            return;

        var other = (Usable)col;
        if(other == null || other.isEmpty())
            return;
        matchParentValues(col, takeInstanceID);
        maxStackCount = other.maxStackCount;
        effect = other.effect;
        effectAmount = other.effectAmount;
        sprite = other.sprite;
    }


    public UsableSpriteHolder getSpriteHolder() {
        return sprite;
    }
}


[System.Serializable]
public class UsableSpriteHolder {
    public Sprite sprite;
}
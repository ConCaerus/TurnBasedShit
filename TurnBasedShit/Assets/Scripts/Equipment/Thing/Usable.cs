using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Usable : Collectable {
    const int numOfEffects = 4;
    public enum effectType {
        heal, cureBleed, powerBuff, speedBuff, defenceBuff
    }


    [SerializeField] UsableSpriteHolder sprite;

    public effectType effect;
    public float effectAmount;
    public int foodBiteCount = 0;

    //  returns true if the effect was applied
    public bool applyStatsEffect(UnitStats stats, PartyObject po) {
        UnitClass obj = null;
        if(po != null) {
            obj = po.getInstantiatedMember(stats).GetComponent<UnitClass>();
            stats = obj.stats;
        }

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

            case effectType.powerBuff:
                if(po == null)
                    break;
                obj.tempPowerMod += effectAmount;
                break;

            case effectType.defenceBuff:
                if(po == null)
                    break;
                obj.tempDefenceMod += effectAmount;
                break;

            case effectType.speedBuff:
                if(po == null)
                    break;
                obj.tempSpeedMod += effectAmount;
                break;
        }

        if(po != null)
            po.resaveInstantiatedUnit(stats);
        Party.overrideUnit(stats);

        return true;
    }

    public override void setEqualTo(Collectable col, bool takeInstanceID) {
        if(col.type != collectableType.usable)
            return;

        var other = (Usable)col;
        if(other == null)
            return;
        matchParentValues(col, takeInstanceID);
        effect = other.effect;
        effectAmount = other.effectAmount;
        sprite = other.sprite;
        foodBiteCount = other.foodBiteCount;
    }


    public UsableSpriteHolder getSpriteHolder() {
        return sprite;
    }
}


[System.Serializable]
public class UsableSpriteHolder {
    public Sprite sprite;
}
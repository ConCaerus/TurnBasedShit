using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class Item : Collectable {
    //  times at which the item is used
    [System.Serializable]
    public enum useTimes {
        beforeEachTurn, afterEachTurn, beforeTurn, afterTurn, afterRound, beforeDefending, beforeAttacking, afterKill
    }
    //  effects the item has
    [System.Serializable]
    public enum passiveEffectTypes {
        modPower, modSpeed, modDefence, modHealGiven, modSummonDamageGiven, modEdgedDamageGiven, modBluntDamageGiven, 
        modChanceToBeAttacked, extraTurn, modMissChance, healInsteadOfDamage, modEnemyChanceToMiss
    }

    [System.Serializable]
    public enum timedEffectTypes { 
        healSelf, addTempSpeed, addTempPower, addTempDefence, chanceEnemyTurnsIntoSummon
    }


    [System.Serializable]
    public struct timedEffects {
        public timedEffectTypes effect;
        public useTimes time;
        public float effectAmount;
    }

    [System.Serializable]
    public struct passiveEffects {
        public passiveEffectTypes effect;
        public float effectAmount;
    }


    public List<timedEffects> tEffects = new List<timedEffects>();
    public List<passiveEffects> pEffects = new List<passiveEffects>();

    [SerializeField] ItemSpriteHolder sprite;

    //  passive shit
    public float getPassiveMod(passiveEffectTypes type) {
        float temp = 0.0f;
        foreach(var i in pEffects) {
            if(i.effect == type)
                temp += i.effectAmount;
        }
        return temp;
    }

    //  timed shit
    public float getTimedMod(timedEffectTypes type) {
        float temp = 0.0f;
        foreach(var i in tEffects) {
            if(i.effect == type)
                temp += i.effectAmount;
        }
        return temp;
    }


    public void triggerUseTime(UnitClass unit, useTimes time) {
        foreach(var i in tEffects) {
            if(i.time == time) {
                switch(i.effect) {
                    case timedEffectTypes.healSelf:
                        var healAmount = unit.stats.getModifiedMaxHealth() * i.effectAmount;
                        unit.addHealth(healAmount);
                        break;

                    case timedEffectTypes.addTempSpeed:
                        unit.combatStats.tempSpeedMod += i.effectAmount;
                        break;

                    case timedEffectTypes.addTempPower:
                        unit.combatStats.tempPowerMod += i.effectAmount;
                        break;

                    case timedEffectTypes.addTempDefence:
                        unit.combatStats.tempDefenceMod += i.effectAmount;
                        break;

                    case timedEffectTypes.chanceEnemyTurnsIntoSummon:
                        //  not a valid unit type
                        if(unit.GetComponent<UnitClass>().combatStats.attackingTarget == null || unit.GetComponent<UnitClass>().combatStats.attackingTarget.GetComponent<UnitClass>().combatStats.isPlayerUnit)
                            break;

                        //  cant take another summon
                        if(!unit.GetComponent<PlayerUnitInstance>().roomToSummon())
                            break;

                        //  takes the chance
                        if(GameVariables.chanceOutOfHundred((int)getTimedMod(timedEffectTypes.chanceEnemyTurnsIntoSummon))) {
                            var enemy = unit.GetComponent<UnitClass>().combatStats.attackingTarget;
                            //  creates a summon class for the enemy
                            enemy.AddComponent<SummonedUnitInstance>();
                            enemy.GetComponent<SummonedUnitInstance>().summoner = unit.stats;
                            enemy.GetComponent<SummonedUnitInstance>().stats = enemy.GetComponent<EnemyUnitInstance>().stats;
                            enemy.GetComponent<SummonedUnitInstance>().combatStats = enemy.GetComponent<EnemyUnitInstance>().combatStats;
                            enemy.GetComponent<SummonedUnitInstance>().combatStats.isPlayerUnit = true;

                            enemy.GetComponent<EnemyUnitInstance>().selfDestruct();

                            enemy.GetComponent<UnitClass>().combatStats.attackingTarget = null;

                            var spot = unit.GetComponent<PlayerUnitInstance>().getNextSummonSpotForUnit();
                            enemy.transform.SetParent(spot.transform);
                            if(enemy.GetComponent<UnitClass>().stats.u_type != GameInfo.combatUnitType.deadUnit)
                                enemy.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                            else
                                enemy.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                            enemy.transform.localScale = enemy.GetComponent<UnitClass>().combatStats.normalSize;
                            enemy.transform.DOLocalMove(enemy.GetComponent<UnitClass>().combatStats.normalPos, .15f);
                            spot.GetComponent<CombatSpot>().setColor();
                        }
                        break;
                }
            }
        }
    }

    public override void setEqualTo(Collectable col, bool takeID) {
        if(col.type != collectableType.Item || col == null)
            return;

        var other = (Item)col;

        if(other == null)
            return;

        matchParentValues(col, takeID);

        pEffects.Clear();
        for(int i = 0; i < other.pEffects.Count; i++)
            pEffects.Add(other.pEffects[i]);

        tEffects.Clear();
        for(int i = 0; i < other.tEffects.Count; i++)
            tEffects.Add(other.tEffects[i]);
    }

    public ItemSpriteHolder getSpriteHolder() {
        return sprite;
    }
}


[System.Serializable]
public class ItemSpriteHolder {
    public Sprite sprite;
}
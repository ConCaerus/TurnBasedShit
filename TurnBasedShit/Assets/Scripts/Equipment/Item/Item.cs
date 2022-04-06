using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class Item : Collectable {

    public List<StatModifier.passiveMod> pMods = new List<StatModifier.passiveMod>();
    public List<StatModifier.timedMod> tMods = new List<StatModifier.timedMod>();

    [SerializeField] ItemSpriteHolder sprite;


    public void triggerUseTime(UnitClass unit, StatModifier.useTimeType time) {
        foreach(var i in tMods) {
            foreach(var t in i.useTimes) {
                if(t == time) {
                    switch(i.type) {
                        case StatModifier.timedModifierType.healSelf:
                            var healAmount = unit.stats.getModifiedMaxHealth() * i.getMod(StatModifier.timedModifierType.healSelf, unit, false);
                            unit.addHealth(healAmount);
                            break;

                        case StatModifier.timedModifierType.addSpeed:
                            unit.combatStats.tempSpeedMod += i.getMod(StatModifier.timedModifierType.addSpeed, unit, false);
                            break;

                        case StatModifier.timedModifierType.addPower:
                            unit.combatStats.tempPowerMod += i.getMod(StatModifier.timedModifierType.addPower, unit, false);
                            break;

                        case StatModifier.timedModifierType.addDefence:
                            unit.combatStats.tempDefenceMod += i.getMod(StatModifier.timedModifierType.addDefence, unit, false);
                            break;

                        case StatModifier.timedModifierType.chanceEnemyTurnsIntoSummon:
                            //  not a valid unit type
                            if(unit.GetComponent<UnitClass>().combatStats.attackingTarget == null || unit.GetComponent<UnitClass>().combatStats.attackingTarget.GetComponent<UnitClass>().combatStats.isPlayerUnit)
                                break;

                            //  cant take another summon
                            if(!unit.GetComponent<PlayerUnitInstance>().roomToSummon())
                                break;

                            //  takes the chance
                            if(GameVariables.chanceOutOfHundred((int)i.getMod(StatModifier.timedModifierType.chanceEnemyTurnsIntoSummon, unit, false))) {
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
    }

    public float getPassiveMod(StatModifier.passiveModifierType type, UnitStats unit, bool multing) {
        var temp = 0.0f;
        if(pMods.Count == 0)
            return multing ? 1.0f : 0.0f;

        foreach(var i in pMods)
            temp += i.getMod(type, unit, multing);
        return temp;
    }
    public float getTimedMod(StatModifier.timedModifierType type, UnitClass unit, bool multing) {
        var temp = 0.0f;
        if(tMods.Count == 0)
            return multing ? 1.0f : 0.0f;

        foreach(var i in tMods)
            temp += i.getMod(type, unit, multing);
        return temp;
    }

    public override void setEqualTo(Collectable col, bool takeID) {
        if(col.type != collectableType.Item || col == null)
            return;

        var other = (Item)col;

        if(other == null)
            return;

        matchParentValues(col, takeID);

        pMods = new List<StatModifier.passiveMod>();
        tMods = new List<StatModifier.timedMod>();

        pMods = other.pMods;
        tMods = other.tMods;
    }

    public ItemSpriteHolder getSpriteHolder() {
        return sprite;
    }
}


[System.Serializable]
public class ItemSpriteHolder {
    public Sprite sprite;
}
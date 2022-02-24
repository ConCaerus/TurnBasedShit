using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

[System.Serializable]
public class Weapon : Collectable {
    public const int attributeCount = 4;
    public enum attribute {
        Power, Bleed, Healing, Stun
    }

    public enum attackType {
        Blunt, Edged
    }
    public enum specialUsage {
        healing, summoning, convertTarget
    }
    public enum specialUsageCostType {
        health, stun, bleed
    }

    [System.Serializable]
    public struct specialUsageCost {
        public specialUsageCostType costType;
        public float costAmount;
    }

    //  if your add to this remember to add it to the setEqualsTo func
    public GameInfo.wornState wornAmount = GameInfo.wornState.Perfect;
    public List<attribute> attributes = new List<attribute>();
    public specialUsage sUsage = (specialUsage)(-1);
    public List<specialUsageCost> sCosts = new List<specialUsageCost>();
    public float sUsageAmount = 0.0f;
    public float sUsageChance = 0.0f;

    public attackType aType = (attackType)(-1);
    public float power = 0.0f;
    public float speedMod = 0.0f;

    public WeaponSpriteHolder sprite;




    public void applyAttributes(GameObject weilder, GameObject attackedUnit) {
        foreach(var i in attributes) {
            if(i == attribute.Bleed) {
                if(!GameVariables.chanceBleed())
                    continue;
                attackedUnit.GetComponent<UnitClass>().stats.u_bleedCount++;
            }

            else if(i == attribute.Healing) {
                weilder.GetComponent<UnitClass>().addHealth(weilder.GetComponent<UnitClass>().stats.getModifiedMaxHealth() * 0.05f);
            }

            else if(i == attribute.Stun) {
                if(attackedUnit.GetComponent<UnitClass>().isStunned())
                    continue;
                attackedUnit.GetComponent<UnitClass>().chanceGettingStunned(GameVariables.getStunChance());
            }
        }
    }

    public bool applySpecailUsage(UnitClass weilder, UnitClass affectedObject) {
        bool succeeded = GameVariables.chanceOutOfHundred((int)sUsageChance);
        //  usage
        switch(sUsage) {
            case specialUsage.healing:
                if(succeeded) {
                    var healAmount = sUsageAmount * weilder.stats.getCritMult();

                    //  apply item effects
                    if(weilder.stats.item != null && !weilder.stats.item.isEmpty()) {
                        healAmount += healAmount * weilder.stats.item.getPassiveMod(StatModifier.passiveModifierType.modHealthGiven, weilder.stats, false);
                    }
                    affectedObject.addHealth(healAmount);
                }
                break;

            case specialUsage.convertTarget:
                if(succeeded) {
                    //  not a valid unit type
                    if(affectedObject == null || affectedObject.combatStats.isPlayerUnit)
                        break;

                    //  cant take another summon
                    if(!weilder.GetComponent<PlayerUnitInstance>().roomToSummon())
                        break;

                    //  takes the chance
                    var enemy = affectedObject.gameObject;
                    //  creates a summon class for the enemy
                    enemy.AddComponent<SummonedUnitInstance>();
                    enemy.GetComponent<SummonedUnitInstance>().summoner = weilder.stats;
                    enemy.GetComponent<SummonedUnitInstance>().stats = enemy.GetComponent<EnemyUnitInstance>().stats;
                    enemy.GetComponent<SummonedUnitInstance>().combatStats = enemy.GetComponent<EnemyUnitInstance>().combatStats;
                    enemy.GetComponent<SummonedUnitInstance>().combatStats.isPlayerUnit = true;

                    enemy.GetComponent<EnemyUnitInstance>().selfDestruct();

                    enemy.GetComponent<UnitClass>().combatStats.attackingTarget = null;

                    var spot = weilder.GetComponent<PlayerUnitInstance>().getNextSummonSpotForUnit();
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

        //  cost
        if(succeeded) {
            foreach(var i in sCosts) {
                switch(i.costType) {
                    case specialUsageCostType.health:
                        weilder.addHealth(-i.costAmount);
                        break;

                    case specialUsageCostType.stun:
                        weilder.chanceGettingStunned(100.0f);
                        break;

                    case specialUsageCostType.bleed:
                        weilder.stats.u_bleedCount += (int)i.costAmount;
                        break;
                }
            }
        }

        return succeeded;
    }

    public int getPowerAttCount() {
        int count = 0;
        foreach(var i in attributes) {
            if(i == attribute.Power)
                count++;
        }

        return count;
    }

    public override void setEqualTo(Collectable col, bool takeID) {
        if(col == null || col.type != collectableType.Weapon)   //  don't check if isEmpty
            return;
        var other = (Weapon)col;
        if(other == null)
            return;
        matchParentValues(col, takeID);
        power = other.power;
        speedMod = other.speedMod;
        attributes = other.attributes;
        sprite = other.sprite;
        sUsage = other.sUsage;
        sCosts = other.sCosts;
        sUsageAmount = other.sUsageAmount;
        sUsageChance = other.sUsageChance;
        wornAmount = other.wornAmount;
        aType = other.aType;

    }


    public int howManyOfAttribute(attribute a) {
        var count = 0;
        foreach(var i in attributes) {
            if(i == a)
                count++;
        }
        return count;
    }


    public attribute getRandAttribute() {
        var rand = Random.Range(0, 101);
        float step = 100.0f / (float)attributeCount;

        int index = 0;
        while(rand >= step) {
            rand -= (int)step;
            index++;
        }

        return (attribute)index;
    }
}


[System.Serializable]
public class WeaponSpriteHolder {
    public Sprite sprite;

    [SerializeField] public Vector2 pos;
    [SerializeField] public Vector2 size;
    [SerializeField] public float rot;
}
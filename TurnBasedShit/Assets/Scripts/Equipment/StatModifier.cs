using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class StatModifier {

    /*          NOTE: for fuck's sake, when making these things, if it asks for a modifier's amount anything below 1.0f is negitive impact.
     *                  Ex. modPower by 0.05f = user.power *= 0.05f. So if user.power was 100 it's now 5. put 1.05f instead.
     *              This goes both ways. If you want a negative impact on a value, put something just below 1.0f.
     *                  Ex. modPower by -.15f = user.power *= 0.05f. So if user.power was 100 it's now -15f. Put .85f instead.
     * 
     * 
     *          Everything with "mod" in the name should range from 0.0f - 2.0f
     *              and should be multiplied to an amount
     *              Ex. ModPower by 1.05 = user.power *= 1.05f;
     *              
     *          Everything with "chance" in the name (and not mod) should be added to a sum
     *              and shouldn't have a set range
     *              Ex. stun target chance by 5 = GameVariables.chanceStun(user.stunChance + 5);
     *              
     *          All of the timed effects don't have the word "mod" because they are being added to a mod.
     *              Ex. addPower by .05 = unit.tempPower += .05
     *              
     *              
     *          Weird shit:
     *              addExtraTurn's value is the number of extra turns that they take
     *                  Ex. amount = 1 - 1 extra turn, amount = 2 - 2 extra turns
     *              healInsteadOfDamage's value is a moded amount that gets applied before attacking
     *                  Ex. amount = 2.0f - double attacking (healing) power, amount = 0.5f - half attacking power
     *              healSelf's value should be the percentage of base max health that should be added.
     *                  Ex. amount = 0.15f - unit get's 15% of base max health back. This is the exception to all mods should be around 1.0f
     *          
     */
    [System.Serializable]
    public enum passiveModifierType {
        //  base stats
        modPower, modDefence, modSpeed, modMaxHealth, modSummonPower, modSummonDefence, modSummonSpeed, modSummonMaxHealth,
        modEdgedPower, modEdgedDefence, modBluntPower, modBluntDefence, modAttackedChance, addExtraTurn, missChance, healInsteadOfDamage, 
        modEnemyMissChance, modHealthGiven, stunSelfChance, stunTargetChance, modEnemyDropChance, allWeaponExpMod, 
        bluntExpMod, edgedExpMod, summonExpMod, addChargedPower, takeFirstTurn, takeLastTurn, equipmentNeverTatters
    }



    [System.Serializable]
    public enum timedModifierType {
        healSelf, addSpeed, addPower, addDefence, chanceEnemyTurnsIntoSummon
    }


    [System.Serializable]
    public enum useTimeType {
        beforeEveryTurn, afterEveryTurn, beforeTurn, afterTurn, afterRound, beforeDefending, beforeAttacking, afterKill, beforeDying
    }



    [System.Serializable]
    public enum conditionType {
        naked, clothed
    }


    [System.Serializable]
    public class mod {
        public List<conditionType> conditions = new List<conditionType>();
        [SerializeField] protected float amount = 1.0f;
        public bool good;


        public bool areConditionsMet(UnitStats user) {
            foreach(var i in conditions) {
                switch(i) {
                    case conditionType.naked:
                        if(user.armor != null || !user.armor.isEmpty())
                            return false;
                        break;
                }
            }

            //  only returns true after all of the conditions were able to pass
            return true;
        }
    }

    
    //  Passive mods happen all the time, so their functions need a UnitStats reference
    [System.Serializable]
    public class passiveMod : mod {
        public passiveModifierType type = (passiveModifierType)(-1);

        public float getMod(passiveModifierType t, UnitStats user, bool multing) {
            //  cannot be used
            if(t != type || !areConditionsMet(user))
                return multing ? 1.0f : 0.0f;

            //  can be used
            return amount;
        }
    }


    //  Timed mods happen during combat, so their functions need a UnitClass reference
    [System.Serializable]
    public class timedMod : mod {
        public timedModifierType type = (timedModifierType)(-1);
        public List<useTimeType> useTimes = new List<useTimeType>();

        public float getMod(timedModifierType t, UnitClass user, bool multing) {
            //  cannot be used
            if(t != type || !areConditionsMet(user.stats))
                return multing ? 1.0f : 0.0f;

            //  can be used
            return amount;
        }


        public void triggerUseTime(UnitClass user, useTimeType time) {
            //  checks if not the right time
            bool rightTime = false;
            foreach(var i in useTimes) {
                if(time == i) {
                    rightTime = true;
                    break;
                }
            }
            if(!rightTime)
                return;


            //  runs logic
            switch(type) {
                case timedModifierType.healSelf:
                    var healAmount = user.stats.getModifiedMaxHealth() * getMod(timedModifierType.healSelf, user, true);
                    user.addHealth(healAmount);
                    break;

                case timedModifierType.addSpeed:
                    user.combatStats.tempSpeedMod += getMod(timedModifierType.addSpeed, user, false);
                    break;

                case timedModifierType.addPower:
                    user.combatStats.tempPowerMod += getMod(timedModifierType.addPower, user, false);
                    break;

                case timedModifierType.addDefence:
                    user.combatStats.tempDefenceMod += getMod(timedModifierType.addDefence, user, false);
                    break;

                case timedModifierType.chanceEnemyTurnsIntoSummon:
                    //  not a valid unit type
                    if(user.GetComponent<UnitClass>().combatStats.attackingTarget == null || user.GetComponent<UnitClass>().combatStats.attackingTarget.GetComponent<UnitClass>().combatStats.isPlayerUnit)
                        break;

                    //  cant take another summon
                    if(!user.GetComponent<PlayerUnitInstance>().roomToSummon())
                        break;

                    //  takes the chance
                    if(GameVariables.chanceOutOfHundred((int)getMod(timedModifierType.chanceEnemyTurnsIntoSummon, user, false))) {
                        var enemy = user.GetComponent<UnitClass>().combatStats.attackingTarget;
                        //  creates a summon class for the enemy
                        enemy.AddComponent<SummonedUnitInstance>();
                        enemy.GetComponent<SummonedUnitInstance>().summoner = user.stats;
                        enemy.GetComponent<SummonedUnitInstance>().stats = enemy.GetComponent<EnemyUnitInstance>().stats;
                        enemy.GetComponent<SummonedUnitInstance>().combatStats = enemy.GetComponent<EnemyUnitInstance>().combatStats;
                        enemy.GetComponent<SummonedUnitInstance>().combatStats.isPlayerUnit = true;

                        enemy.GetComponent<EnemyUnitInstance>().selfDestruct();

                        enemy.GetComponent<UnitClass>().combatStats.attackingTarget = null;

                        var spot = user.GetComponent<PlayerUnitInstance>().getNextSummonSpotForUnit();
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

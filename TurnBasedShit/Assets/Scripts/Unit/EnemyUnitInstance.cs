using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyUnitInstance : UnitClass {
    public Weapon.attackType weakTo = (Weapon.attackType)(-1);
    [SerializeField] AnimationClip idle;

    public WeaponPreset weaponDrop;
    public int chanceToDropWeapon;
    public ArmorPreset armorDrop;
    public int chanceToDropArmor;
    public ItemPreset itemDrop;
    public int chanceToDropItem;
    public UsablePreset usableDrop;
    public int chanceToDropUsable;
    public UnusablePreset unusableDrop;
    public int chanceToDropUnusable;


    private void Awake() {
        combatStats.isPlayerUnit = false;
        if(GetComponentInChildren<UnitSpriteHandler>() != null)
            GetComponentInChildren<UnitSpriteHandler>().setReference(stats, true);
    }

    public override void setAttackingAnim() {
        if(stats.u_type == GameInfo.combatUnitType.deadUnit) {
            GetComponentInChildren<UnitSpriteHandler>().setAnimSpeed(1.0f);
            GetComponentInChildren<UnitSpriteHandler>().triggerAttackAnim();
        }
        else if(GetComponent<Animator>() != null) {
            GetComponent<Animator>().speed = 1.0f;
            GetComponent<Animator>().SetTrigger("attack");
        }
    }
    public override void setDefendingAnim() {
        if(stats.u_type == GameInfo.combatUnitType.deadUnit) {
            GetComponentInChildren<UnitSpriteHandler>().setAnimSpeed(1.0f);
            GetComponentInChildren<UnitSpriteHandler>().triggerDefendAnim();
        }
        else if(GetComponent<Animator>() != null) {
            GetComponent<Animator>().speed = 1.0f;
            GetComponent<Animator>().SetTrigger("defend");
        }
    }

    public bool isIdle() {
        if(stats.u_type == GameInfo.combatUnitType.deadUnit)
            return GetComponentInChildren<UnitSpriteHandler>().isAnimIdle();
        return GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip == idle;
    }

    public IEnumerator combatTurn() {
        if(gameObject != FindObjectOfType<TurnOrderSorter>().playingUnit)
            yield return 0;
        if(combatStats.attackingTarget == null)
            calcNextAttackingTarget();

        yield return new WaitForSeconds(1f);

        if(gameObject == FindObjectOfType<TurnOrderSorter>().playingUnit)
            attack(combatStats.attackingTarget);
    }


    void calcNextAttackingTarget() {
        int attackableCount = Party.getHolder().getObjectCount<UnitStats>() + FindObjectsOfType<SummonedUnitInstance>().Length;
        float defaultChancePerUnit = 100.0f / attackableCount;
        float total = 0.0f;


        for(int i = 0; i < attackableCount; i++) {
            if(i < Party.getHolder().getObjectCount<UnitStats>()) {
                total += calcChanceToAttack(attackableCount, Party.getHolder().getObject<UnitStats>(i));
            }
            else
                total += defaultChancePerUnit;
        }

        float rand = Random.Range(0.0f, total);
        for(int i = 0; i < attackableCount; i++) {
            if(i < Party.getHolder().getObjectCount<UnitStats>()) {
                var chance = calcChanceToAttack(attackableCount, Party.getHolder().getObject<UnitStats>(i));
                if(rand < chance) {
                    combatStats.attackingTarget = FindObjectOfType<PartyObject>().getInstantiatedMember(Party.getHolder().getObject<UnitStats>(i)).gameObject;
                    return;
                }
                rand -= chance;
            }

            else {
                if(rand < defaultChancePerUnit) {
                    combatStats.attackingTarget = FindObjectsOfType<SummonedUnitInstance>()[i - Party.getHolder().getObjectCount<UnitStats>()].gameObject;
                    return;
                }
                rand -= defaultChancePerUnit;
            }
        }
    }

    float calcChanceToAttack(int attackableCount, UnitStats stats) {
        float defaultChancePerUnit = 100.0f / attackableCount;
        float moddedChance = defaultChancePerUnit;

        if(stats.isTheSameInstanceAs(Party.getLeaderStats()))   //  +15% if leader
            moddedChance += 0.15f;

        foreach(var t in stats.u_traits)  //  apply traits n' shit
            moddedChance += t.getChanceToBeAttackedMod() * 100.0f;


        if(stats.item != null && !stats.item.isEmpty()) //  item shit
            moddedChance += stats.item.getPassiveMod(Item.passiveEffectTypes.modChanceToBeAttacked) * defaultChancePerUnit;

        return moddedChance;
    }


    public void chanceWeaponDrop(int bonusChance) {
        if(weaponDrop == null)
            return;
        if(GameVariables.chanceOutOfHundred(chanceToDropWeapon + bonusChance)) {
            var loc = GameInfo.getCombatDetails();
            loc.spoils.addObject<Collectable>(FindObjectOfType<PresetLibrary>().getWeapon(weaponDrop.preset));
            GameInfo.setCombatDetails(loc);
        }
    }
    public void chanceArmorDrop(int bonusChance) {
        if(armorDrop == null)
            return;
        if(GameVariables.chanceOutOfHundred(chanceToDropArmor + bonusChance)) {
            var loc = GameInfo.getCombatDetails();
            loc.spoils.addObject<Collectable>(FindObjectOfType<PresetLibrary>().getArmor(armorDrop.preset));
            GameInfo.setCombatDetails(loc);
        }
    }
    public void chanceItemDrop(int bonusChance) {
        if(itemDrop == null)
            return;
        if(GameVariables.chanceOutOfHundred(chanceToDropItem + bonusChance)) {
            var loc = GameInfo.getCombatDetails();
            loc.spoils.addObject<Collectable>(FindObjectOfType<PresetLibrary>().getItem(itemDrop.preset));
            GameInfo.setCombatDetails(loc);
        }
    }
    public void chanceUsableDrop(int bonusChance) {
        if(usableDrop == null)
            return;
        if(GameVariables.chanceOutOfHundred(chanceToDropUsable + bonusChance)) {
            var loc = GameInfo.getCombatDetails();
            loc.spoils.addObject<Collectable>(FindObjectOfType<PresetLibrary>().getUsable(usableDrop.preset));
            GameInfo.setCombatDetails(loc);
        }
    }
    public void chanceUnusableDrop(int bonusChance) {
        if(unusableDrop == null)
            return;
        if(GameVariables.chanceOutOfHundred(chanceToDropUnusable + bonusChance)) {
            var loc = GameInfo.getCombatDetails();
            loc.spoils.addObject<Collectable>(FindObjectOfType<PresetLibrary>().getUnusable(unusableDrop.preset));
            GameInfo.setCombatDetails(loc);
        }
    }
}

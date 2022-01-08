using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyUnitInstance : UnitClass {
    public Weapon.attackType weakTo;
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


    public type enemyType;

    [System.Serializable]
    public enum type {
        slime, groundBird, stumpSpider, rockCrawler
    }


    private void Awake() {
        isPlayerUnit = false;
    }

    public override void setAttackingAnim() {
        if(GetComponent<Animator>() != null) {
            GetComponent<Animator>().speed = 1.0f;
            GetComponent<Animator>().SetTrigger("attack");
        }
    }
    public override void setDefendingAnim() {
        if(GetComponent<Animator>() != null) {
            GetComponent<Animator>().speed = 1.0f;
            GetComponent<Animator>().SetTrigger("defend");
        }
    }

    public bool isIdle() {
        return GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip == idle;
    }

    public IEnumerator combatTurn() {
        if(attackingTarget == null)
            calcNextAttackingTarget();

        yield return new WaitForSeconds(1f);

        attack(attackingTarget);
    }


    void calcNextAttackingTarget() {
        int attackableCount = Party.getMemberCount() + FindObjectsOfType<SummonedUnitInstance>().Length;
        float defaultChancePerUnit = 100.0f / attackableCount;
        float total = 0.0f;



        for(int i = 0; i < attackableCount; i++) {
            if(i < Party.getMemberCount()) {
                total += calcChanceToAttack(attackableCount, Party.getMemberStats(i));
            }
            else
                total += defaultChancePerUnit;
        }

        float rand = Random.Range(0.0f, total);
        for(int i = 0; i < attackableCount; i++) {
            if(i < Party.getMemberCount()) {
                var chance = calcChanceToAttack(attackableCount, Party.getMemberStats(i));
                if(rand < chance) {
                    attackingTarget = FindObjectOfType<PartyObject>().getInstantiatedMember(Party.getMemberStats(i)).gameObject;
                    return;
                }
                rand -= chance;
            }

            else {
                if(rand < defaultChancePerUnit) {
                    attackingTarget = FindObjectsOfType<SummonedUnitInstance>()[i - Party.getMemberCount()].gameObject;
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

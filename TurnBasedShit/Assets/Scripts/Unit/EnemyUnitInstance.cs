using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyUnitInstance : UnitClass {
    public GameInfo.diffLvl enemyDiff = 0;
    public Weapon.attackType weakTo;

    public WeaponPreset weaponDrop;
    public int chanceToDropWeapon;
    public ArmorPreset armorDrop;
    public int chanceToDropArmor;
    public ItemPreset itemDrop;
    public int chanceToDropItem;
    public ConsumablePreset consumableDrop;
    public int chanceToDropConsumable;

    public type enemyType;

    Coroutine idler = null;

    [System.Serializable]
    public enum type {
        slime, groundBird
    }


    private void Awake() {
        isPlayerUnit = false;
    }

    public override void setAttackingAnim() {
        if(GetComponent<Animator>() != null) {
            GetComponent<Animator>().speed = 1.0f;
            GetComponent<Animator>().SetInteger("state", 2);
            if(idler != null)
                StopCoroutine(idler);
            idler = StartCoroutine(returnToIdle());
        }
    }
    public override void setDefendingAnim() {
        if(GetComponent<Animator>() != null) {
            GetComponent<Animator>().speed = 1.0f;
            GetComponent<Animator>().SetInteger("state", 1);
            if(idler != null)
                StopCoroutine(idler);
            idler = StartCoroutine(returnToIdle());
        }
    }

    public IEnumerator combatTurn() {
        if(FindObjectOfType<TurnOrderSorter>().playingUnit != gameObject)
            yield return 0;

        if(attackingTarget == null)
            calcNextAttackingTarget();

        yield return new WaitForSeconds(0.5f);

        if(attackingTarget != null) {
            yield return new WaitForSeconds(0.5f);
            attack(attackingTarget);
        }
    }


    void calcNextAttackingTarget() {
        int attackableCount = Party.getMemberCount() + FindObjectsOfType<SummonedUnitInstance>().Length;
        float defaultChancePerUnit = 100.0f / attackableCount;
        float rand = Random.Range(0, 101);

        

        for(int i = 0; i < attackableCount; i++) {
            float moddedChance = 0.0f;

            if(i < Party.getMemberCount()) {
                if(Party.getMemberStats(i).isEqualTo(Party.getLeaderStats()))   //  +15% if leader
                    moddedChance += 0.15f;

                foreach(var t in Party.getMemberStats(i).u_traits)  //  apply traits n' shit
                    moddedChance += t.getChanceToBeAttackedMod() * 100.0f;
            }

            if(rand < (defaultChancePerUnit * (i + 1)) + moddedChance) {
                if(i < Party.getMemberCount())
                    attackingTarget = FindObjectOfType<PartyObject>().getInstantiatedMember(Party.getMemberStats(i));
                else {
                    int index = attackableCount - i - 1;
                    attackingTarget = FindObjectsOfType<SummonedUnitInstance>()[index].gameObject;
                }
                return;
            }
        }

        //  gone through the loop and didn't get a target
        attackingTarget = FindObjectOfType<PartyObject>().getInstantiatedMember(Party.getMemberStats(Random.Range(0, Party.getMemberCount())));
    }



    IEnumerator returnToIdle() {
        yield return new WaitForEndOfFrame();

        if(isDone()) {
            GetComponent<Animator>().SetInteger("state", 0);
        }
        else
            idler = StartCoroutine(returnToIdle());
    }

    bool isDone() {
        if(GetComponent<Animator>() == null || GetComponent<Animator>().GetInteger("state") == 0)
            return false;
        return GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !GetComponent<Animator>().IsInTransition(0);
    }


    public void chanceWeaponDrop(int bonusChance) {
        if(weaponDrop == null)
            return;
        if(GameVariables.chanceOutOfHundred(chanceToDropWeapon + bonusChance)) {
            var loc = GameInfo.getCombatDetails();
            loc.weapons.Add(weaponDrop.preset);
            GameInfo.setCombatDetails(loc);
        }
    }
    public void chanceArmorDrop(int bonusChance) {
        if(armorDrop == null)
            return;
        if(GameVariables.chanceOutOfHundred(chanceToDropArmor + bonusChance)) {
            var loc = GameInfo.getCombatDetails();
            loc.armor.Add(armorDrop.preset);
            GameInfo.setCombatDetails(loc);
        }
    }
    public void chanceItemDrop(int bonusChance) {
        if(itemDrop == null)
            return;
        if(GameVariables.chanceOutOfHundred(chanceToDropItem + bonusChance)) {
            var loc = GameInfo.getCombatDetails();
            loc.items.Add(itemDrop.preset);
            GameInfo.setCombatDetails(loc);
        }
    }
    public void chanceConsumableDrop(int bonusChance) {
        if(consumableDrop == null)
            return;
        if(GameVariables.chanceOutOfHundred(chanceToDropConsumable + bonusChance)) {
            var loc = GameInfo.getCombatDetails();
            loc.consumables.Add(consumableDrop.preset);
            GameInfo.setCombatDetails(loc);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyUnitInstance : UnitClass {
    public GameInfo.diffLvl enemyDiff = 0;
    public Weapon.attackType weakTo;

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
        yield return new WaitForSeconds(0.5f);

        if(stunned) {
            stunned = false;
            yield return 0;
        }
        if(this != null && FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject && attackingTarget == null) {
            calcNextAttackingTarget();

            yield return new WaitForSeconds(0.5f);

            if(this != null && FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject) {
                attack(attackingTarget);
            }
        }
    }


    void calcNextAttackingTarget() {
        float defaultChancePerUnit = 100.0f / Party.getMemberCount();
        float rand = Random.Range(0, 101);

        for(int i = 0; i < Party.getMemberCount(); i++) {
            float moddedChance = 0.0f;

            if(Party.getMemberStats(i).isEqualTo(Party.getLeaderStats()))   //  +15% if leader
                moddedChance += 0.15f;

            foreach(var t in Party.getMemberStats(i).u_traits)  //  apply traits n' shit
                moddedChance += t.getChanceToBeAttackedMod() * 100.0f;

            if(rand < (defaultChancePerUnit * (i + 1)) + moddedChance) {
                attackingTarget = FindObjectOfType<PartyObject>().getInstantiatedMember(Party.getMemberStats(i));
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
}

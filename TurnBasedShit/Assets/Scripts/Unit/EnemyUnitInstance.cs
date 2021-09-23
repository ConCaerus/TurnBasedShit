using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitInstance : UnitClass {
    public GameInfo.diffLvl enemyDiff = 0;
    public Weapon.attackType weakTo;

    public type enemyType;

    Coroutine idler = null;

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
        if(this != null && FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject && attackingTarget == null) {
            setRandomAttackingTarget();

            yield return new WaitForSeconds(0.5f);

            if(this != null && FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject) {
                attack(attackingTarget);
            }
        }
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

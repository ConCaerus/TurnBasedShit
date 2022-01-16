using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SummonedUnitInstance : UnitClass {
    public UnitStats summoner;

    Coroutine idler = null;

    private void Awake() {
        if(combatStats == null)
            combatStats = new UnitCombatStats();
        combatStats.isPlayerUnit = true;
    }

    private void Start() {
        float scale = transform.localScale.x;
        float time = 0.15f;
        transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        transform.DOScale(scale, time);
    }


    public override void setAttackingAnim() {
        if(GetComponent<Animator>() != null) {
            GetComponent<Animator>().speed = 1.0f;
            GetComponent<Animator>().SetTrigger("attack");
            if(idler != null)
                StopCoroutine(idler);
            idler = StartCoroutine(returnToIdle());
        }
    }
    public override void setDefendingAnim() {
        if(GetComponent<Animator>() != null) {
            GetComponent<Animator>().speed = 1.0f;
            GetComponent<Animator>().SetTrigger("defend");
            if(idler != null)
                StopCoroutine(idler);
            idler = StartCoroutine(returnToIdle());
        }
    }

    public IEnumerator combatTurn() {
        if(FindObjectOfType<TurnOrderSorter>().playingUnit != gameObject || GetComponents<EnemyUnitInstance>().Length == 0)
            yield return 0;

        if(combatStats.attackingTarget == null)
            combatStats.attackingTarget = FindObjectsOfType<EnemyUnitInstance>()[Random.Range(0, FindObjectsOfType<EnemyUnitInstance>().Length)].gameObject;

        yield return new WaitForSeconds(0.5f);

        if(combatStats.attackingTarget != null) {
            yield return new WaitForSeconds(0.5f);
            attack(combatStats.attackingTarget);
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

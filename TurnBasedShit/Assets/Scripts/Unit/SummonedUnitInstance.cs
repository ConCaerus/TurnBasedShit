using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SummonedUnitInstance : UnitClass {
    public UnitStats summoner;

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

    public IEnumerator combatTurn() {
        if(FindObjectOfType<TurnOrderSorter>().playingUnit != gameObject || GetComponents<EnemyUnitInstance>().Length == 0)
            yield return 0;

        if(combatStats.attackingTarget == null)
            combatStats.attackingTarget = FindObjectsOfType<EnemyUnitInstance>()[Random.Range(0, FindObjectsOfType<EnemyUnitInstance>().Length)].gameObject;

        yield return new WaitForSeconds(0.5f);

        if(combatStats.attackingTarget != null && FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject) {
            yield return new WaitForSeconds(0.5f);
            attack(combatStats.attackingTarget);
        }
    }
}

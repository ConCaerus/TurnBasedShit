using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapEnemyMovement : MonoBehaviour {
    Vector2 target;
    Vector2 patrolAreaMidPoint;
    public GameInfo.diffLvl currentDiff;

    float travelTime = 1.0f;

    float distToTarget = 3.5f, distToAttack = 0.25f, wanderDist = 7.5f;

    Coroutine waiter = null;


    private void Start() {
        patrolAreaMidPoint = transform.position;
        currentDiff = Map.getDiffForX(transform.position.x);
        setNewTarget();
    }

    private void Update() {
        if(Vector2.Distance(transform.position, FindObjectOfType<MapMovement>().transform.position) < distToAttack)
            FindObjectOfType<MapEventsHandler>().triggerEncounter();
        else if(Vector2.Distance(transform.position, FindObjectOfType<MapMovement>().transform.position) < distToTarget) {
            target = FindObjectOfType<MapMovement>().transform.position;
            travelTime -= Time.deltaTime;
        }
        else if(Vector2.Distance(transform.position, target) < 0.01f && waiter == null)
            waiter = StartCoroutine(animateSetNewTarget());

        if(waiter == null)
            transform.DOMove(target, Vector2.Distance(transform.position, target) * travelTime);
    }

    void setNewTarget() {
        target = Map.getRandomPosInRegion((int)currentDiff);
        while(Vector2.Distance(target, patrolAreaMidPoint) > wanderDist)
            target = Map.getRandomPosInRegion((int)currentDiff);
    }


    IEnumerator animateSetNewTarget() {
        yield return new WaitForSeconds(0.25f);

        setNewTarget();
        waiter = null;
    }
}

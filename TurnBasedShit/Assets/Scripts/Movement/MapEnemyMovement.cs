using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapEnemyMovement : MonoBehaviour {
    Vector2 target;
    Vector2 patrolAreaMidPoint;
    public GameInfo.diffLvl currentDiff;

    float travelTime = 1.0f, incTravelTime = 0.0f;
    bool movingRight = true;

    float distToTarget = 5.0f, distToAttack = 0.25f, wanderDist = 7.5f;

    Coroutine waiter = null, irrelevantWaiter = null;


    private void Start() {
        patrolAreaMidPoint = transform.position;
        currentDiff = Map.getDiffForX(transform.position.x);
        show();
        setNewTarget();
    }

    private void Update() {
        if(Vector2.Distance(transform.position, FindObjectOfType<MapMovement>().transform.position) < distToAttack) {
            FindObjectOfType<MapEventsHandler>().triggerEncounter();
        }

        if(transform.position.x > target.x && movingRight)
            flip();
        else if(transform.position.x < target.x && !movingRight)
            flip();

        if(currentDiff != GameInfo.getCurrentDiff())
            createNewRelevantInstance();
        else if(Vector2.Distance(transform.position, FindObjectOfType<MapMovement>().transform.position) < distToTarget) {
            GetComponent<Animator>().SetInteger("state", 1);
            target = FindObjectOfType<MapMovement>().transform.position;
            incTravelTime += Time.deltaTime * 1.0f;
            GetComponent<Animator>().speed = incTravelTime;

            if(irrelevantWaiter != null)
                StopCoroutine(irrelevantWaiter);
            if(waiter != null)
                StopCoroutine(waiter);
            irrelevantWaiter = null;
            waiter = null;
        }
        else {
            incTravelTime = 0.0f;
            if(irrelevantWaiter == null)
                irrelevantWaiter = StartCoroutine(waitUntillIrrelevant());

            if(Vector2.Distance(transform.position, target) < 0.01f && waiter == null) {
                waiter = StartCoroutine(animateSetNewTarget());
                GetComponent<Animator>().SetInteger("state", 0);
            }
        }
        if(waiter == null) {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, target, (travelTime + incTravelTime) * Time.deltaTime);
        }
    }


    public void flip() {
        float flipSpeed = 0.25f;
        if(movingRight) {
            transform.DORotate(new Vector3(0.0f, 180.0f, 0.0f), flipSpeed);
        }
        else if(!movingRight) {
            transform.DORotate(new Vector3(0.0f, 0.0f, 0.0f), flipSpeed);
        }
        movingRight = !movingRight;
    }

    void setNewTarget() {
        target = Map.getRandomPosInRegion((int)currentDiff);
        while(Vector2.Distance(target, patrolAreaMidPoint) > wanderDist)
            target = Map.getRandomPosInRegion((int)currentDiff);
    }


    void show() {
        transform.localScale = new Vector3(0.0f, 0.0f);
        transform.DOScale(0.5f, 0.15f);
    }
    void hide() {
        transform.DOScale(0.0f, 0.25f);
        Destroy(gameObject, 0.25f);
        enabled = false;
    }

    void createNewRelevantInstance() {
        hide();
        FindObjectOfType<MapEnemySpawner>().createEnemyAroundPlayer();
    }


    IEnumerator waitUntillIrrelevant() {
        yield return new WaitForSeconds(15.0f);

        createNewRelevantInstance();
    }

    IEnumerator animateSetNewTarget() {
        yield return new WaitForSeconds(0.25f);

        setNewTarget();
        waiter = null;
    }
}

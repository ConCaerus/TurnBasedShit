using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapEnemyMovement : MonoBehaviour {
    [SerializeField] GameObject alertIcon;
    Vector2 target;
    Vector2 patrolAreaMidPoint;

    float speed;
    float maxSpeed = 5.0f, chaseSpeed = 3.0f, minSpeed = 1.0f;
    bool movingRight = true;

    float distToTarget = 5.0f, distToAttack = 0.25f, wanderDist = 7.5f;

    Coroutine waiter = null, irrelevantWaiter = null, chaseEventsWaiter = null;


    private void Start() {
        patrolAreaMidPoint = transform.position;
        alertIcon.transform.localPosition = Vector3.zero;
        alertIcon.transform.localScale = Vector3.zero;
        show();
        setNewTarget();
        speed = minSpeed;
    }

    private void Update() {
        if(Vector2.Distance(transform.position, FindObjectOfType<MapMovement>().transform.position) < distToAttack) {
            FindObjectOfType<MapEventsHandler>().triggerEncounter();
        }

        if(transform.position.x > target.x && movingRight)
            flip();
        else if(transform.position.x < target.x && !movingRight)
            flip();

        move();
    }


    void move() {
        var player = FindObjectOfType<MapMovement>();
        if(Vector2.Distance(transform.position, player.transform.position) < distToTarget) {   //  start chasing player
            GetComponent<Animator>().SetBool("chasing", true);
            target = player.transform.position;

            if(chaseEventsWaiter == null)
                chaseEventsWaiter = StartCoroutine(chaseTimedEvents());

            if(irrelevantWaiter != null)
                StopCoroutine(irrelevantWaiter);
            if(waiter != null)
                StopCoroutine(waiter);
            irrelevantWaiter = null;
            waiter = null;
        }
        else {   //  stop chasing player
            if(irrelevantWaiter == null)
                irrelevantWaiter = StartCoroutine(waitUntillIrrelevant());
            if(chaseEventsWaiter != null) {
                StopCoroutine(chaseEventsWaiter);
                alertIcon.transform.DOComplete();

                alertIcon.transform.DOScale(new Vector3(0.0f, 0.0f), 0.25f);
                alertIcon.transform.DOLocalMoveY(0.0f, .25f);
            }
            chaseEventsWaiter = null;


            if(Vector2.Distance(transform.position, target) < 0.01f && waiter == null) {
                waiter = StartCoroutine(animateSetNewTarget());
                GetComponent<Animator>().SetBool("chasing", false);
            }
        }
        if(waiter == null) {
            GetComponent<Animator>().speed = speed;
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, target, Time.deltaTime * speed);
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
        target = Map.getRandPos();
        while(Vector2.Distance(target, patrolAreaMidPoint) > wanderDist)
            target = Map.getRandPos();
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
        irrelevantWaiter = null;
    }

    IEnumerator animateSetNewTarget() {
        speed = 0.0f;
        yield return new WaitForSeconds(0.25f);

        setNewTarget();
        waiter = null;
    }

    IEnumerator chaseTimedEvents() {
        //  pause before rushing at the player
        alertIcon.transform.DOComplete();
        alertIcon.transform.DOScale(new Vector3(2.0f, 1.5f), 0.15f);
        alertIcon.transform.DOLocalMoveY(1.5f, .15f);
        speed = 0.0f;
        yield return new WaitForSeconds(0.5f);

        alertIcon.transform.DOScale(new Vector3(0.0f, 0.0f), 0.25f);
        alertIcon.transform.DOLocalMoveY(0.0f, .25f);

        //  when first sees player, gets a burst of speed
        speed = maxSpeed;
        yield return new WaitForSeconds(1.0f);

        //  sets speed to a little less than the player's speed
        speed = chaseSpeed;
        yield return new WaitForSeconds(3.0f);

        //  starts decreasing speed when tired 
        float dec = 0.05f;
        while(Vector2.Distance(transform.position, FindObjectOfType<MapMovement>().transform.position) < distToTarget && speed > minSpeed) {
            speed -= dec;
            yield return new WaitForEndOfFrame();
        }

        //  when speed is at the lowest it can be, wait for the player to leave sight before ending chase
        while(Vector2.Distance(transform.position, FindObjectOfType<MapMovement>().transform.position) < distToTarget)
            yield return new WaitForEndOfFrame();
        
        //  cleanup
        waiter = StartCoroutine(animateSetNewTarget());
        GetComponent<Animator>().SetBool("chasing", false);
        chaseEventsWaiter = null;
    }
}

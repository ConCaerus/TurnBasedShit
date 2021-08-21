using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TownMemberMovement : MonoBehaviour {
    public GameObject unit;
    public GameObject rotationObj;

    public AudioClip walkSound;


    float moveSpeed = 1.0f, flipSpeed = 0.15f, pauseTime = 0.5f, distToStop = 3.5f;

    public bool movingDir = true;
    public Vector2 moveDir = Vector2.right;
    bool canMove = true;

    float targetX = 0.0f;

    Coroutine anim = null;

    private void Awake() {
        DOTween.Init();
        setNewTargetX();
    }

    private void Start() {
        StartCoroutine(FindObjectOfType<TransitionCanvas>().runAfterLoading(setAnimSpeed));
    }

    void setAnimSpeed() {
        unit.GetComponent<UnitSpriteHandler>().setAnimSpeed(0.75f);
    }

    private void Update() {
        if(FindObjectOfType<TransitionCanvas>().loaded && canMove) {
            move();
            unit.GetComponent<UnitSpriteHandler>().setAnimState(3);
        }
        else {
            unit.GetComponent<UnitSpriteHandler>().setAnimState(0);
        }
    }


    void setNewTargetX() {
        targetX = FindObjectOfType<BuildingSpawner>().getXThatIsntInfrontOfADoor();
    }

    public void move() {
        if(anim == null)
            anim = StartCoroutine(walkAnim());

        if(!canMove)
            return;
        //  right
        if(targetX - transform.position.x > 0.0f) {
            if(!movingDir)
                flip();
        }

        //  left
        else {
            if(movingDir)
                flip();
        }

        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

    }

    public void flip() {
        if(movingDir) {
            rotationObj.transform.DORotate(new Vector3(0.0f, 180.0f, 0.0f), flipSpeed);
        }
        else if(!movingDir) {
            rotationObj.transform.DORotate(new Vector3(0.0f, 360.0f, 0.0f), flipSpeed, RotateMode.FastBeyond360);
        }
        movingDir = !movingDir;

        if(movingDir)
            moveDir = Vector2.right;
        else
            moveDir = Vector2.left;
    }


    IEnumerator walkAnim(bool jumpDir = true) {
        float time = 0.25f;
        unit.transform.DOPunchPosition(new Vector3(0.0f, 0.25f, 0.0f), time);
        FindObjectOfType<AudioManager>().playSound(walkSound);
        if(jumpDir)
            unit.transform.DOPunchRotation(new Vector3(0.0f, 0.0f, Random.Range(5.0f, 20.0f)), time);
        else
            unit.transform.DOPunchRotation(new Vector3(0.0f, 0.0f, Random.Range(-20.0f, -5.0f)), time);

        yield return new WaitForSeconds(time);

        //  stop moving if interacting with player
        while(GetComponentInChildren<TownMemberInstance>().interacting) {
            canMove = false;
            if(movingDir && transform.position.x > FindObjectOfType<LocationMovement>().transform.position.x)
                flip();
            else if(!movingDir && transform.position.x < FindObjectOfType<LocationMovement>().transform.position.x)
                flip();

            yield return new WaitForEndOfFrame();
        }
        canMove = true;

        //  stop moving if looking at player
        bool interactingWithAnother = false;
        foreach(var i in FindObjectsOfType<TownMemberInstance>()) {
            if(i.interacting) {
                interactingWithAnother = true;
                break;
            }
        }
        if(!interactingWithAnother) {
            float offset = FindObjectOfType<LocationMovement>().transform.position.x - transform.position.x;
            while(Mathf.Abs(offset) < distToStop && (offset == 0.0f || (offset < 0.0f && FindObjectOfType<LocationMovement>().movingDir) || (offset > 0.0f && !FindObjectOfType<LocationMovement>().movingDir))) {
                canMove = false;


                foreach(var i in FindObjectsOfType<TownMemberInstance>()) {
                    if(i.interacting) {
                        interactingWithAnother = true;
                        break;
                    }
                }

                if(interactingWithAnother)
                    break;

                if(movingDir == FindObjectOfType<LocationMovement>().movingDir)
                    flip();

                yield return new WaitForEndOfFrame();
                offset = FindObjectOfType<LocationMovement>().transform.position.x - transform.position.x;
            }
            canMove = true;
        }

        if(Mathf.Abs(transform.position.x - targetX) > 0.1f)
            anim = StartCoroutine(walkAnim(!jumpDir));
        else {
            canMove = false;
            yield return new WaitForSeconds(pauseTime);

            setNewTargetX();
            anim = null;
            canMove = true;
        }
    }
}

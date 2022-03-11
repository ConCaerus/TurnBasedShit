using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public abstract class InteractiveMovement : UnitMovement {
    public abstract bool canMoveAlongY();

    public abstract bool shouldInteract();
    public abstract bool shouldDeinteract();
    public abstract void interact();
    public abstract void deinteract();

    public float interactDist;

    private void Update() {
        if(FindObjectOfType<MenuCanvas>().isOpen())
            return;

        if(shouldInteract() && FindObjectOfType<TransitionCanvas>().loaded)
            interact();
        else if(shouldDeinteract() && FindObjectOfType<TransitionCanvas>().loaded)
            deinteract();

        if(unit.GetComponent<UnitSpriteHandler>().initialized) {
            if(shouldMove() && canMove && FindObjectOfType<TransitionCanvas>().loaded) {
                isMoving = true;
                move();
                unit.GetComponent<UnitSpriteHandler>().setWalkingAnim(true);
            }
            else {
                isMoving = false;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, GetComponent<Rigidbody2D>().velocity.y);
                unit.GetComponent<UnitSpriteHandler>().setWalkingAnim(false);
            }
        }
        else
            unit.GetComponent<UnitSpriteHandler>().setReference(Party.getLeaderStats(), true);

        if(!movingDown && !shouldMove() && canMoveAlongY()) {
            flip(true);
            movingDown = true;
        }
    }


    bool shouldMove() {
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || (canMoveAlongY() && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)));
    }

    public void move() {
        var sp = (moveSpeed + (leaderSpeed / 10.0f)) * 100.0f;
        Vector2 target = Vector2.zero;

        //  X AXIS
        //  right
        if(Input.GetKey(KeyCode.D)) {
            if(!movingRight)
                flip(true);

            target = Vector2.right;
        }

        //  left
        else if(Input.GetKey(KeyCode.A)) {
            if(movingRight)
                flip(true);

            //target = Vector2.right;
            target = Vector2.left;
        }

        //  Y AXIS
        if(canMoveAlongY()) {
            //  up
            if(Input.GetKey(KeyCode.W)) {
                if(movingDown)
                    flip(false);
                movingDown = false;
                target += Vector2.up;
            }

            //  down
            else if(Input.GetKey(KeyCode.S)) {
                if(!movingDown || !GetComponentInChildren<UnitSpriteHandler>().isFaceShown())
                    flip(true);
                movingDown = true;
                target += Vector2.down;
            }
        }

        var rb = GetComponent<Rigidbody2D>();

        rb.velocity = target * sp * Time.fixedDeltaTime;
        //transform.Translate(target * sp * Time.deltaTime);

        //transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);

        if(anim == null)
            anim = StartCoroutine(walkAnim());
    }
}

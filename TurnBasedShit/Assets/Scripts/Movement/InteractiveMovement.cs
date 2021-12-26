using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public abstract class InteractiveMovement : UnitMovement {
    public abstract bool outOfBounds();
    public abstract bool canMoveAlongY();

    public abstract bool shouldInteract();
    public abstract bool shouldDeinteract();
    public abstract void interact();
    public abstract void deinteract();

    public int interactDist;

    bool showingInteractText = false;
    public string interactString = "W";
    public TextMeshProUGUI interactText;

    private void Update() {
        if(FindObjectOfType<MenuCanvas>().isOpen())
            return;

        if(shouldInteract() && FindObjectOfType<TransitionCanvas>().loaded)
            interact();
        else if(shouldDeinteract() && FindObjectOfType<TransitionCanvas>().loaded)
            deinteract();

        if(shouldMove() && canMove && FindObjectOfType<TransitionCanvas>().loaded) {
            isMoving = true;
            move();
            unit.GetComponent<UnitSpriteHandler>().setWalkingAnim(true);
        }
        else {
            isMoving = false;
            unit.GetComponent<UnitSpriteHandler>().setWalkingAnim(false);
        }

        if(!movingDown && !shouldMove() && canMoveAlongY()) {
            flip(true);
            movingDown = true;
        }
    }


    bool shouldMove() {
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || (canMoveAlongY() && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)));
    }

    public void move() {
        var sp = moveSpeed + (Party.getLeaderStats().u_speed / 10.0f);
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

            target = Vector2.right;
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

        transform.Translate(target * sp * Time.deltaTime);
        if(outOfBounds())
            transform.Translate(-target * sp * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);

        if(anim == null)
            anim = StartCoroutine(walkAnim());
    }


    public void showInteractText() {
        if(showingInteractText)
            return;
        showingInteractText = true;
        float speed = 0.15f;
        float offset = 50f;

        interactText.text = interactString;
        interactText.gameObject.transform.localPosition = Vector3.zero;
        interactText.gameObject.transform.localScale = Vector3.zero;
        interactText.transform.DOComplete();
        interactText.gameObject.transform.DOScale(1.0f, speed);
        interactText.gameObject.transform.DOLocalMoveY(offset, speed);
    }
    public void hideInteractText() {
        if(!showingInteractText)
            return;
        showingInteractText = false;
        float speed = 0.25f;

        interactText.text = interactString;
        interactText.transform.DOComplete();
        interactText.gameObject.transform.DOScale(0.0f, speed);
        interactText.gameObject.transform.DOLocalMoveY(0.0f, speed);
    }
}

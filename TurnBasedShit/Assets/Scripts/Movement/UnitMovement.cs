﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class UnitMovement : MonoBehaviour {
    public GameObject unit;

    public AudioClip walkSound;


    public float moveSpeed = 10.0f;
    protected float flipSpeed = 0.15f;
    protected float leaderSpeed;

    public bool movingRight = true;
    public bool movingDown = true;
    public bool canMove = true;
    public bool isMoving = false;

    protected Coroutine anim = null;

    private void Start() {
        DOTween.Init();
        canMove = true;
        leaderSpeed = Party.getLeaderStats().u_speed;
        if(GetComponent<MapSideUnitMovement>() == null)
            unit.GetComponent<UnitSpriteHandler>().setReference(Party.getLeaderStats(), true);
        setVisuals();
    }

    public void setVisuals() {
        if(GetComponent<MapSideUnitMovement>() == null)
            unit.GetComponent<UnitSpriteHandler>().setReference(Party.getLeaderStats(), true);
    }

    public abstract bool showWeapon();

    public void flip(bool setFaceShown) {
        if(movingRight) {
            transform.DORotate(new Vector3(0.0f, 180.0f, 0.0f), flipSpeed);
        }
        else if(!movingRight) {
            transform.DORotate(new Vector3(0.0f, 0.0f, 0.0f), flipSpeed);
        }
        movingRight = !movingRight;

        if(setFaceShown != GetComponentInChildren<UnitSpriteHandler>().isFaceShown())
            StartCoroutine(toggleFaceDuringFlip(setFaceShown));

    }

    protected IEnumerator toggleFaceDuringFlip(bool state) {
        yield return new WaitForSeconds(flipSpeed / 2.0f);
        GetComponentInChildren<UnitSpriteHandler>().setShowingFace(state);
    }


    protected IEnumerator walkAnim(bool jumpDir = true) {
        float time = 0.2f;
        unit.transform.DOPunchPosition(new Vector3(0.0f, 0.7f, 0.0f), time);
        FindObjectOfType<AudioManager>().playSound(walkSound);
        if(jumpDir)
            unit.transform.DOPunchRotation(new Vector3(0.0f, 0.0f, Random.Range(5.0f, 25.0f)), time);
        else
            unit.transform.DOPunchRotation(new Vector3(0.0f, 0.0f, Random.Range(-25.0f, -5.0f)), time);

        yield return new WaitForSeconds(time);

        while(FindObjectOfType<MenuCanvas>().isOpen())
            yield return new WaitForEndOfFrame();


        if(isMoving)
            anim = StartCoroutine(walkAnim(!jumpDir));
        else
            anim = null;
    }
}

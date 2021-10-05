using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CombatCameraController : MonoBehaviour {
    float maxRandAmount = 0.15f;
    float moveSpeed = 0.75f;
    float buffer = 4.0f;

    GameObject lookingAtObj = null;

    private void Start() {
        moveToMiddle();
    }

    private void Update() {
        if(FindObjectOfType<TurnOrderSorter>().playingUnit == null && lookingAtObj != null) {
            lookingAtObj = null;
            moveToMiddle();
            return;
        }
        else if(FindObjectOfType<TurnOrderSorter>().playingUnit != null && lookingAtObj != FindObjectOfType<TurnOrderSorter>().playingUnit && FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().attackingTarget == null) {
            lookingAtObj = FindObjectOfType<TurnOrderSorter>().playingUnit;
            moveToPlayingUnit();
            return;
        }
        else if(FindObjectOfType<TurnOrderSorter>().playingUnit != null && FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().attackingTarget != null && lookingAtObj != FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().attackingTarget) {
            lookingAtObj = FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().attackingTarget;
            moveToAttackTarget();
            return;
        }
    }


    void moveToPlayingUnit() {
        var unitPos = FindObjectOfType<TurnOrderSorter>().playingUnit.transform.position;
        var randX = Random.Range(-maxRandAmount, maxRandAmount);
        var randY = Random.Range(-maxRandAmount, maxRandAmount);
        var target = ((Vector2)unitPos / buffer) + new Vector2(randX, randY);

        Camera.main.transform.DOMove(new Vector3(target.x, target.y, Camera.main.transform.position.z), moveSpeed);
    }
    void moveToAttackTarget() {
        var unitPos = (FindObjectOfType<TurnOrderSorter>().playingUnit.transform.position + (FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().attackingTarget.transform.position * 2.0f)) / 3.0f;
        var randX = Random.Range(-maxRandAmount, maxRandAmount);
        var randY = Random.Range(-maxRandAmount, maxRandAmount);
        var target = ((Vector2)unitPos / buffer) + new Vector2(randX, randY);

        Camera.main.transform.DOMove(new Vector3(target.x, target.y, Camera.main.transform.position.z), moveSpeed);
    }
    void moveToMiddle() {
        var randX = Random.Range(-maxRandAmount, maxRandAmount);
        var randY = Random.Range(-maxRandAmount, maxRandAmount);
        var target = new Vector2(randX, randY);

        Camera.main.transform.DOMove(new Vector3(target.x, target.y, Camera.main.transform.position.z), moveSpeed);
    }

    public void resetLookingAtObj() {
        lookingAtObj = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CombatCameraController : MonoBehaviour {
    float maxRandAmount = 0.15f;
    float moveSpeed = 0.5f;
    float buffer = 4.0f;
    public float maxX, minX, maxY, minY;

    GameObject lookingAtObj = null;

    private void Awake() {
        //moveToMiddle();
    }

    private void Update() {
        /*
        //  nothing to look at
        if(FindObjectOfType<TurnOrderSorter>().playingUnit == null && lookingAtObj != null) {
            lookingAtObj = null;
            moveToMiddle();
        }
        //  move to playing unit that doesn't have an attacking target
        else if(FindObjectOfType<TurnOrderSorter>().playingUnit != null && lookingAtObj != FindObjectOfType<TurnOrderSorter>().playingUnit && FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().combatStats.attackingTarget == null) {
            lookingAtObj = FindObjectOfType<TurnOrderSorter>().playingUnit;
            moveToPlayingUnit();
        }
        //  move to playing unit that has an attacking target
        else if(FindObjectOfType<TurnOrderSorter>().playingUnit != null && FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().combatStats.attackingTarget != null && lookingAtObj != FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().combatStats.attackingTarget) {
            lookingAtObj = FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().combatStats.attackingTarget;
            moveToAttackTarget();
        }*/
        if(FindObjectOfType<TransitionCanvas>().loaded && !FindObjectOfType<InfoCanvas>().isOpen() && !FindObjectOfType<MenuCanvas>().isOpen() && overDeadZone()) {
            Vector3 target = Vector2.Lerp(transform.position, GameInfo.getMousePos(), moveSpeed * Time.deltaTime);
            target.x = Mathf.Clamp(target.x, minX, maxX);
            target.y = Mathf.Clamp(target.y, minY, maxY);
            target.z = transform.position.z;
            if(transform.position.x - target.x == 0f)
                return;
            FindObjectOfType<EnvironmentHandler>().moveParallaxObjs(transform.position.x - target.x);
            //transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(target.x, target.y, transform.position.z);
        }
    }


    bool overDeadZone() {
        float deadZoneX = 2f, deadZoneY = 3.5f;
        var mPos = GameInfo.getMousePos();
        var cPos = Camera.main.transform.position;
        return Mathf.Abs(mPos.x - cPos.x) > deadZoneX || Mathf.Abs(mPos.y - cPos.y) > deadZoneY;
    }

    void moveToPlayingUnit() {
        Camera.main.transform.DOKill();
        var unitPos = FindObjectOfType<TurnOrderSorter>().playingUnit.transform.position;
        var randX = Random.Range(-maxRandAmount, maxRandAmount);
        var randY = Random.Range(-maxRandAmount, maxRandAmount);
        var target = ((Vector2)unitPos / buffer) + new Vector2(randX, randY);
        target.x = Mathf.Clamp(target.x, minX, maxX);

        FindObjectOfType<EnvironmentHandler>().moveParallaxObjs(transform.position.x - target.x);

        Camera.main.transform.DOMove(new Vector3(target.x, target.y, Camera.main.transform.position.z), moveSpeed);
    }
    void moveToAttackTarget() {
        Camera.main.transform.DOKill();
        var unitPos = (FindObjectOfType<TurnOrderSorter>().playingUnit.transform.position + (FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().combatStats.attackingTarget.transform.position * 2.0f)) / 3.0f;
        var randX = Random.Range(-maxRandAmount, maxRandAmount);
        var randY = Random.Range(-maxRandAmount, maxRandAmount);
        var target = ((Vector2)unitPos / buffer) + new Vector2(randX, randY);
        target.x = Mathf.Clamp(target.x, minX, maxX);

        FindObjectOfType<EnvironmentHandler>().moveParallaxObjs(transform.position.x  - target.x);

        Camera.main.transform.DOMove(new Vector3(target.x, target.y, Camera.main.transform.position.z), moveSpeed);
    }
    void moveToMiddle() {
        Camera.main.transform.DOKill();
        var randX = Random.Range(-maxRandAmount, maxRandAmount);
        var randY = Random.Range(-maxRandAmount, maxRandAmount);
        var target = new Vector2(randX, randY);
        target.x = Mathf.Clamp(target.x, minX, maxX);

        FindObjectOfType<EnvironmentHandler>().moveParallaxObjs(transform.position.x - target.x);

        Camera.main.transform.DOMove(new Vector3(target.x, target.y, Camera.main.transform.position.z), moveSpeed);
    }
}

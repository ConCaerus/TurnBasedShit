using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CombatCameraController : MonoBehaviour {
    [SerializeField] float maxRandAmount = 0.15f;
    [SerializeField] float moveSpeed = 0.1f;
    [SerializeField] float buffer = 4.0f;

    //  player = -1, middle = 0, enemy = 1
    int currentSide = 0;

    private void Start() {
        moveToMiddle();
    }


    public void moveToPlayingUnit() {
        var unitPos = FindObjectOfType<TurnOrderSorter>().playingUnit.transform.position;
        var randX = Random.Range(-maxRandAmount, maxRandAmount);
        var randY = Random.Range(-maxRandAmount, maxRandAmount);
        var target = ((Vector2)unitPos / buffer) + new Vector2(randX, randY);

        Camera.main.transform.DOMove(new Vector3(target.x, target.y, Camera.main.transform.position.z), moveSpeed);
    }
    public void moveToMiddle() {
        var randX = Random.Range(-maxRandAmount, maxRandAmount);
        var randY = Random.Range(-maxRandAmount, maxRandAmount);
        var target = new Vector2(randX, randY);

        Camera.main.transform.DOMove(new Vector3(target.x, target.y, Camera.main.transform.position.z), moveSpeed);
    }
}

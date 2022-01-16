using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CombatCameraController : MonoBehaviour {
    float maxRandAmount = 0.15f;
    float moveSpeed = 0.75f;
    float buffer = 4.0f;
    [SerializeField] float parallaxAmount;
    [SerializeField] float maxX, minX;

    GameObject lookingAtObj = null;

    //  first is close, fifth is far
    List<List<GameObject>> parallaxObs = new List<List<GameObject>>();

    [SerializeField] GameObject grasslandObjects, forestObjects, swampObjects, mountainObjects, hellObjects;

    private void Awake() {
        setEnvironmentObjects();
        populateParallaxObjs();

        moveToMiddle();
    }

    private void Update() {
        if(FindObjectOfType<TurnOrderSorter>().playingUnit == null && lookingAtObj != null) {
            lookingAtObj = null;
            moveToMiddle();
            return;
        }
        else if(FindObjectOfType<TurnOrderSorter>().playingUnit != null && lookingAtObj != FindObjectOfType<TurnOrderSorter>().playingUnit && FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().combatStats.attackingTarget == null) {
            lookingAtObj = FindObjectOfType<TurnOrderSorter>().playingUnit;
            moveToPlayingUnit();
            return;
        }
        else if(FindObjectOfType<TurnOrderSorter>().playingUnit != null && FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().combatStats.attackingTarget != null && lookingAtObj != FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().combatStats.attackingTarget) {
            lookingAtObj = FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().combatStats.attackingTarget;
            moveToAttackTarget();
            return;
        }
    }



    void setEnvironmentObjects() {
        grasslandObjects.SetActive(false);
        forestObjects.SetActive(false);
        swampObjects.SetActive(false);
        mountainObjects.SetActive(false);
        hellObjects.SetActive(false);

        if(GameInfo.getCurrentRegion() == GameInfo.region.grassland)
            grasslandObjects.SetActive(true);
        else if(GameInfo.getCurrentRegion() == GameInfo.region.forest)
            forestObjects.SetActive(true);
        else if(GameInfo.getCurrentRegion() == GameInfo.region.swamp)
            swampObjects.SetActive(true);
        else if(GameInfo.getCurrentRegion() == GameInfo.region.mountains)
            mountainObjects.SetActive(true);
        else if(GameInfo.getCurrentRegion() == GameInfo.region.hell)
            hellObjects.SetActive(true);

        Debug.Log("Loaded: " + GameInfo.getCurrentRegion());
    }

    void populateParallaxObjs() {
        if(parallaxObs.Count == 0) {
            parallaxObs.Add(new List<GameObject>());
            parallaxObs.Add(new List<GameObject>());
            parallaxObs.Add(new List<GameObject>());
            parallaxObs.Add(new List<GameObject>());
            parallaxObs.Add(new List<GameObject>());
            parallaxObs.Add(new List<GameObject>());
        }

        foreach(var i in GameObject.FindGameObjectsWithTag("FirstParallax")) 
            parallaxObs[0].Add(i.gameObject);
        foreach(var i in GameObject.FindGameObjectsWithTag("SecondParallax")) 
            parallaxObs[1].Add(i.gameObject);
        foreach(var i in GameObject.FindGameObjectsWithTag("ThirdParallax")) 
            parallaxObs[2].Add(i.gameObject);
        foreach(var i in GameObject.FindGameObjectsWithTag("FourthParallax")) 
            parallaxObs[3].Add(i.gameObject);
        foreach(var i in GameObject.FindGameObjectsWithTag("FifthParallax")) 
            parallaxObs[4].Add(i.gameObject);
        foreach(var i in GameObject.FindGameObjectsWithTag("SixthParallax"))
            parallaxObs[5].Add(i.gameObject);
    }


    void moveToPlayingUnit() {
        var unitPos = FindObjectOfType<TurnOrderSorter>().playingUnit.transform.position;
        var randX = Random.Range(-maxRandAmount, maxRandAmount);
        var randY = Random.Range(-maxRandAmount, maxRandAmount);
        var target = ((Vector2)unitPos / buffer) + new Vector2(randX, randY);
        target.x = Mathf.Clamp(target.x, minX, maxX);

        moveParallaxObjs(transform.position.x - target.x);

        Camera.main.transform.DOMove(new Vector3(target.x, target.y, Camera.main.transform.position.z), moveSpeed);
    }
    void moveToAttackTarget() {
        var unitPos = (FindObjectOfType<TurnOrderSorter>().playingUnit.transform.position + (FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().combatStats.attackingTarget.transform.position * 2.0f)) / 3.0f;
        var randX = Random.Range(-maxRandAmount, maxRandAmount);
        var randY = Random.Range(-maxRandAmount, maxRandAmount);
        var target = ((Vector2)unitPos / buffer) + new Vector2(randX, randY);
        target.x = Mathf.Clamp(target.x, minX, maxX);

        moveParallaxObjs(transform.position.x  - target.x);

        Camera.main.transform.DOMove(new Vector3(target.x, target.y, Camera.main.transform.position.z), moveSpeed);
    }
    void moveToMiddle() {
        var randX = Random.Range(-maxRandAmount, maxRandAmount);
        var randY = Random.Range(-maxRandAmount, maxRandAmount);
        var target = new Vector2(randX, randY);
        target.x = Mathf.Clamp(target.x, minX, maxX);

        moveParallaxObjs(transform.position.x - target.x);

        Camera.main.transform.DOMove(new Vector3(target.x, target.y, Camera.main.transform.position.z), moveSpeed);
    }


    void moveParallaxObjs(float movedX) {
        float useMod = parallaxAmount * parallaxObs.Count;
        foreach(var p in parallaxObs) {
            foreach(var i in p) {
                i.transform.DOComplete();
                i.transform.DOMove(new Vector3(i.transform.position.x + movedX * useMod, i.transform.position.y), moveSpeed);
            }
            useMod -= parallaxAmount;
        }
    }

    public GameObject getEnivironmentHolder() {
        switch(GameInfo.getCurrentRegion()) {
            case GameInfo.region.grassland: return grasslandObjects;
            case GameInfo.region.forest: return forestObjects;
            case GameInfo.region.swamp: return swampObjects;
            case GameInfo.region.mountains: return mountainObjects;
            case GameInfo.region.hell: return hellObjects;
            default: return null;
        }
    }
}

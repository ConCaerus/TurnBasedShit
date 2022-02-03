using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnvironmentHandler : MonoBehaviour {
    [SerializeField] GameObject grasslandObjects, forestObjects, swampObjects, mountainObjects, hellObjects;
    [SerializeField] GameObject cloudObjects;
    float moveSpeed = 0.75f;
    float parallaxAmount = .1f;

    //  first is close, fifth is far
    List<List<GameObject>> parallaxObjs = new List<List<GameObject>>();


    private void Start() {
        DOTween.Init();
        setEnvironmentObjects();
        if(cloudObjects != null) {
            foreach(var i in cloudObjects.GetComponentsInChildren<SpriteRenderer>())
                StartCoroutine(cloudMover(i.gameObject, i.transform.localPosition.x));
        }

        sortParallaxObjs();
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
    }
    void sortParallaxObjs() {
        //  populates the list with enough lists
        //  seventh list will hold duds like holders, these don't get moved in the mover
        while(parallaxObjs.Count < 7)
            parallaxObjs.Add(new List<GameObject>());

        //  sorts the fuckers
        foreach(var i in getEnivironmentHolder().GetComponentsInChildren<Transform>()) {
            parallaxObjs[   i.gameObject.CompareTag("FirstParallax")  ?  0 : //  fun
                            i.gameObject.CompareTag("SecondParallax") ?  1 :
                            i.gameObject.CompareTag("ThirdParallax")  ?  2 :
                            i.gameObject.CompareTag("FourthParallax") ?  3 :
                            i.gameObject.CompareTag("FifthParallax")  ?  4 :
                            i.gameObject.CompareTag("SixthParallax")  ?  5 : 6
            ].Add(i.gameObject);
        }
    }

    //  called from camera movement scripts, so it doesn't have to run every frame
    public void moveParallaxObjs(float movedX) {
        float useMod = parallaxAmount * parallaxObjs.Count;
        foreach(var p in parallaxObjs) {
            foreach(var i in p) {
                i.transform.DOKill();
                i.transform.DOMove(new Vector3(i.transform.position.x + movedX * useMod, i.transform.position.y), moveSpeed);
            }
            useMod -= parallaxAmount;
        }
    }

    IEnumerator cloudMover(GameObject c, float origin) {
        float max = 0.75f;
        var speed = Random.Range(.5f, .75f);
        if(c.tag == "FourthParallax") {
            max /= 2.0f;
            speed /= 2.0f;
        }
        else if(c.tag == "FifthParallax") {
            max /= 4.0f;
            speed /= 4.0f;
        }

        var x = Random.Range(-max, max);
        var xPos = origin + x;

        while(Mathf.Abs(c.transform.localPosition.x - xPos) > 0.05f) {
            c.transform.localPosition = Vector2.Lerp(c.transform.localPosition, new Vector2(xPos, c.transform.localPosition.y), speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(cloudMover(c, origin));
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

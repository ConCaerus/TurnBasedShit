using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public abstract class MapIcon : MonoBehaviour {
    protected Coroutine idler = null;
    public bool isMouseOver = false;

    UnityEngine.Experimental.Rendering.Universal.Light2D lightObj;

    float interactDist = 0.75f;
    protected float enterSpeed = 0.15f, exitSpeed = 0.25f;

    private void Start() {
        lightObj = GetComponentInChildren<UnityEngine.Experimental.Rendering.Universal.Light2D>();

        idler = StartCoroutine(startIdlingAtRandomTime());
        DOTween.To(() => lightObj.pointLightOuterRadius, x => lightObj.pointLightOuterRadius = x, 0.0f, exitSpeed);
    }

    public abstract void mouseOver();
    public abstract void mouseExit();
    public abstract void resetIconVisuals();
    public abstract IEnumerator idleAnim();


    private void Update() {
        //  moused over
        if(Vector2.Distance(GameInfo.getMousePos(), transform.position) < interactDist && !isMouseOver) {
            lightUp(.3f);
            mouseOver();
            return;
        }
        else if(Vector2.Distance(GameInfo.getMousePos(), transform.position) > interactDist && isMouseOver) {
            lightDown();
            isMouseOver = false;
            mouseExit();

            idler = StartCoroutine(waitToIdle());
            return;
        }

        if(idler == null && !isMouseOver)
            idler = StartCoroutine(idleAnim());
    }

    public void lightUp(float val) {
        DOTween.To(() => lightObj.pointLightOuterRadius, x => lightObj.pointLightOuterRadius = x, val, exitSpeed);
    }

    public void lightDown() {
        DOTween.To(() => lightObj.pointLightOuterRadius, x => lightObj.pointLightOuterRadius = x, 0.0f, exitSpeed);
    }


    IEnumerator startIdlingAtRandomTime() {
        yield return new WaitForSeconds(Random.Range(0.0f, 0.5f));

        idler = StartCoroutine(idleAnim());
    }

    IEnumerator waitToIdle() {
        yield return new WaitForSeconds(exitSpeed);

        if(isMouseOver)
            idler = null;
        else
            idler = StartCoroutine(idleAnim());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SpinWheelObject : MonoBehaviour {
    bool showing = false;

    Coroutine shower = null, hider = null, rotator = null, resizer = null;

    public void startShowing() {
        if(shower != null)
            return;
        if(hider != null)
            StopCoroutine(hider);
        hider = null;
        shower = StartCoroutine(show());
    }
    public void startHiding() {
        if(hider != null)
            return;
        if(shower != null)
            StopCoroutine(shower);
        if(resizer != null)
            StopCoroutine(resizer);
        resizer = null;
        shower = null;
        hider = StartCoroutine(hide());
    }


    IEnumerator show() {
        float time = 0.25f;

        showing = true;
        if(rotator == null)
            rotator = StartCoroutine(rotate());
        if(resizer != null)
            StopCoroutine(resizer);
        resizer = null;

        GetComponent<RectTransform>().DOKill();
        GetComponent<RectTransform>().DOScale(1.0f, time);
        yield return new WaitForSeconds(time);

        resizer = StartCoroutine(resizeOverTime(true));
        shower = null;
    }
    IEnumerator hide() {
        float time = 0.35f;

        GetComponent<RectTransform>().DOKill();
        GetComponent<RectTransform>().DOScale(0.0f, time);

        yield return new WaitForSeconds(time);
        showing = false;
        hider = null;
    }

    IEnumerator rotate() {
        GetComponent<RectTransform>().Rotate(new Vector3(0.0f, 0.0f, 250.0f * Time.deltaTime));

        yield return new WaitForEndOfFrame();

        if(showing)
            rotator = StartCoroutine(rotate());
        else {
            GetComponent<RectTransform>().rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            rotator = null;
        }
    }
    IEnumerator resizeOverTime(bool inflate) {
        float time = 0.75f;

        if(inflate)
            GetComponent<RectTransform>().DOScale(1.25f, time);
        else
            GetComponent<RectTransform>().DOScale(0.85f, time);

        yield return new WaitForSeconds(.25f);

        if(showing)
            resizer = StartCoroutine(resizeOverTime(!inflate));
        else
            resizer = null;
    }
}

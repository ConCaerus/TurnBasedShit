using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CombatHighlightObject : MonoBehaviour {
    float startAnimTime = 0.15f;
    float startScale = 0.5f;
    public bool finishedAnim = false;

    float endAnimTime = 0.15f;
    float endScale = 0.75f;

    Coroutine starter = null, ender = null;

    private void Start() {
        starter = StartCoroutine(startAnim());
    }


    public void setColor(Color c) {
        foreach(var i in GetComponentsInChildren<SpriteRenderer>())
            i.color = c;
    }
    public Color getColor() {
        return GetComponentInChildren<SpriteRenderer>().color;
    }



    IEnumerator startAnim() {
        finishedAnim = false;
        for(int i = 0; i < GetComponentsInChildren<SpriteRenderer>().Length; i++) {
            var temp = GetComponentsInChildren<SpriteRenderer>()[i].gameObject.transform.localPosition;
            GetComponentsInChildren<SpriteRenderer>()[i].gameObject.transform.localPosition = Vector2.zero;
            GetComponentsInChildren<SpriteRenderer>()[i].gameObject.transform.DOLocalMove(temp, startAnimTime);
        }
        transform.DOScale(startScale, startAnimTime * 0.75f);

        yield return new WaitForSeconds(startAnimTime * 0.75f);

        transform.DOScale(0.5f, startAnimTime * 0.25f);

        yield return new WaitForSeconds(startAnimTime * 0.25f);

        finishedAnim = true;
    }

    public void startEndAnim(bool destory) {
        if(ender != null)
            return;
        ender = StartCoroutine(endAnim(destory));
    }

    IEnumerator endAnim(bool destroy) {
        if(starter != null) {
            StopCoroutine(starter);
            starter = null;
        }
        transform.DOScale(endScale, endAnimTime * 0.25f);

        yield return new WaitForSeconds(endAnimTime * 0.25f);

        for(int i = 0; i < GetComponentsInChildren<SpriteRenderer>().Length; i++) {
            GetComponentsInChildren<SpriteRenderer>()[i].gameObject.transform.DOLocalMove(Vector3.zero, endAnimTime * 0.75f);
        }

        transform.DOScale(0.0f, endAnimTime * 0.75f);

        yield return new WaitForSeconds(startAnimTime * 0.75f);

        if(destroy && gameObject != null)
            Destroy(gameObject);
    }
}

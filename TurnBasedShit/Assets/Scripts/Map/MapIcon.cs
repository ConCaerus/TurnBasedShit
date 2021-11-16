using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class MapIcon : MonoBehaviour {
    UnityEngine.Experimental.Rendering.Universal.Light2D lightObj;

    protected float enterSpeed = 0.15f, exitSpeed = 0.25f;

    private void Start() {
        lightObj = GetComponentInChildren<UnityEngine.Experimental.Rendering.Universal.Light2D>(); 
        GetComponent<Animator>().ForceStateNormalizedTime(Random.Range(0.0f, 10.0f));

        DOTween.To(() => lightObj.pointLightOuterRadius, x => lightObj.pointLightOuterRadius = x, 0.0f, exitSpeed);
    }

    private void OnMouseEnter() {
        animate();
    }

    private void OnMouseExit() {
        deanimate();
    }

    public void lightUp(float val) {
        DOTween.To(() => lightObj.pointLightOuterRadius, x => lightObj.pointLightOuterRadius = x, val, exitSpeed);
    }

    public void lightDown() {
        DOTween.To(() => lightObj.pointLightOuterRadius, x => lightObj.pointLightOuterRadius = x, 0.0f, exitSpeed);
    }

    public void animate() {
        lightUp(.3f);
        GetComponent<Animator>().SetBool("mousedOver", true);
    }
    public void deanimate() {
        lightDown();
        GetComponent<Animator>().SetBool("mousedOver", false);
    }
}

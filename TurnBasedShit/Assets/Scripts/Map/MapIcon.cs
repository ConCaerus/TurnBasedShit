using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class MapIcon : MonoBehaviour {
    UnityEngine.Experimental.Rendering.Universal.Light2D lightObj;

    protected float enterSpeed = 0.15f, exitSpeed = 0.25f;

    [SerializeField] bool animated = true;


    private void OnTriggerEnter2D(Collider2D collision) {
        FindObjectOfType<InteractionCanvas>().show(transform.position);
        FindObjectOfType<MapMovement>().closestIcon = gameObject;
    }
    private void OnTriggerExit2D(Collider2D collision) {
        FindObjectOfType<InteractionCanvas>().hide();
        FindObjectOfType<MapMovement>().closestIcon = null;
    }

    private void Start() {
        if(!animated)
            return;
        lightObj = GetComponentInChildren<UnityEngine.Experimental.Rendering.Universal.Light2D>(); 
        GetComponent<Animator>().ForceStateNormalizedTime(Random.Range(0.0f, 10.0f));

        DOTween.To(() => lightObj.pointLightOuterRadius, x => lightObj.pointLightOuterRadius = x, 0.0f, exitSpeed);
    }

    private void OnMouseEnter() {
        if(!animated)
            return;
        if(FindObjectOfType<MapFogTexture>().isPositionCleared(transform.position))
            animate();
    }

    private void OnMouseExit() {
        if(!animated)
            return;
        deanimate();
    }

    void lightUp(float val) {
        if(!animated)
            return;
        DOTween.To(() => lightObj.pointLightOuterRadius, x => lightObj.pointLightOuterRadius = x, val, exitSpeed);
    }
    
    void lightDown() {
        if(!animated)
            return;
        DOTween.To(() => lightObj.pointLightOuterRadius, x => lightObj.pointLightOuterRadius = x, 0.0f, exitSpeed);
    }

    void animate() {
        if(!animated)
            return;
        lightUp(.3f);
        GetComponent<Animator>().SetBool("mousedOver", true);
    }
    void deanimate() {
        if(!animated)
            return;
        lightDown();
        GetComponent<Animator>().SetBool("mousedOver", false);
    }
}

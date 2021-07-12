using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowObject : MonoBehaviour {
    private void Start() {
        GetComponent<SpriteRenderer>().sprite = transform.GetComponentInParent<SpriteRenderer>().sprite;
        GetComponent<SpriteRenderer>().sortingOrder = transform.GetComponentInParent<SpriteRenderer>().sortingOrder - 1;
    }

    public void updateSprite(Sprite s) {
        GetComponent<SpriteRenderer>().sprite = s;
    }
}
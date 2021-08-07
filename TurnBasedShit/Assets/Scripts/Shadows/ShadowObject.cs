using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowObject : MonoBehaviour {
    private void Start() {
        GetComponent<SpriteRenderer>().sprite = transform.parent.GetComponent<SpriteRenderer>().sprite;
        GetComponent<SpriteRenderer>().sortingOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder - 1;
        GetComponent<SpriteRenderer>().sortingLayerID = transform.parent.GetComponent<SpriteRenderer>().sortingLayerID;
    }

    public void updateSprite() {
        GetComponent<SpriteRenderer>().sprite = transform.parent.GetComponent<SpriteRenderer>().sprite;
    }
}
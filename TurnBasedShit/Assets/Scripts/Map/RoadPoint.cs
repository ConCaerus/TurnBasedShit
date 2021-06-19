using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPoint : MonoBehaviour {
    public bool mouseOver = false;

    [SerializeField] Color normColor, overColor;
    [SerializeField] float normSize, overSize;


    private void OnMouseEnter() {
        mouseOver = true;
        GetComponent<SpriteRenderer>().color = overColor;
        transform.localScale = new Vector3(overSize, overSize, 0.0f);
    }

    private void OnMouseExit() {
        mouseOver = false;
        GetComponent<SpriteRenderer>().color = normColor;
        transform.localScale = new Vector3(normSize, normSize, 0.0f);
    }
}

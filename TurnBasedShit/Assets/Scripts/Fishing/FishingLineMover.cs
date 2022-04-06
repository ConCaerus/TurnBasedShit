using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FishingLineMover : MonoBehaviour {
    LineRenderer ln;

    public float minXArea, maxXArea, yPos;

    private void Awake() {
        ln = GetComponent<LineRenderer>();
        ln.positionCount = 2;
    }

    private void Update() {
        moveLine();
    }


    void moveLine() {
        ln.SetPosition(0, transform.GetChild(0).GetChild(0).transform.position);

        float area = maxXArea - minXArea;
        float percentage = minXArea + area * FindObjectOfType<FishingCanvas>().fishSlider.value;

        //  position bobber
        if(FindObjectOfType<FishingCanvas>().fishing) {
            FindObjectOfType<Bobber>().transform.position = new Vector3(percentage, FindObjectOfType<Bobber>().transform.position.y, 0.0f);
        }
        ln.SetPosition(1, FindObjectOfType<Bobber>().transform.GetChild(0).position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoOptions : MonoBehaviour {
    public bool mouseOver { get; private set; } = false;

    public Collectable referenceCol;

    private void OnMouseEnter() {
        mouseOver = true;
    }
    private void OnMouseExit() {
        mouseOver = false;
    }
}

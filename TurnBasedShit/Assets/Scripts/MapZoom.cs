using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapZoom : MonoBehaviour {
    const float zoomSpeed = 1.0f;
    const float maxZoom = 100.0f, minZoom = 0.5f;

    private void Update() {
        float scroll = Input.mouseScrollDelta.y;

        if(scroll != 0)
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + zoomSpeed * -scroll, minZoom, maxZoom);
    }
}

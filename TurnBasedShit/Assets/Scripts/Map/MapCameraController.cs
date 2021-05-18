using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraController : MonoBehaviour {
    [SerializeField] float zoomSpeed = 1.0f;
    [SerializeField] float maxZoom = 100.0f, minZoom = 0.5f;
    [SerializeField] float maxIconSize = 0.7f, minIconSize = 0.1f;

    Vector2 moveAnchorPoint;

    private void Start() {
        var target = FindObjectOfType<MapTrailRenderer>().getPartyAnchorPos();
        Camera.main.transform.position = new Vector3(target.x, target.y, Camera.main.transform.position.z);
    }

    private void Update() {
        if(Input.GetMouseButtonDown(1))
            moveAnchorPoint = GameInfo.getMousePos();
        else if(Input.GetMouseButtonUp(1))
            moveAnchorPoint = Vector2.zero;
        if(Input.GetMouseButton(1))
            move(1.0f);
        zoom();
    }


    void zoom() {
        float scroll = Input.mouseScrollDelta.y;

        //  there was a change in zoom
        if(scroll != 0) {
            //  zoom / movement
            moveAnchorPoint = GameInfo.getMousePos();
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + zoomSpeed * -scroll, minZoom, maxZoom);
            move(scroll * 0.75f);

            //  scaling of images
            foreach(var i in FindObjectOfType<MapLocationSpawner>().getShownLocationObjects()) {
                float scaleAmount = Mathf.Clamp(0.25f * (Camera.main.orthographicSize - 5.0f) / 4.0f, minIconSize, maxIconSize);
                i.transform.localScale = new Vector3(scaleAmount, scaleAmount, 0.0f);
            }
        }
    }


    void move(float percentage) {
        Vector2 offset = (moveAnchorPoint - GameInfo.getMousePos()) * percentage;
        var target = (Vector2)Camera.main.transform.position + offset;

        Camera.main.transform.position = new Vector3(target.x, target.y, Camera.main.transform.position.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapCameraController : MonoBehaviour {
    [SerializeField] Vector2 offset;
    [SerializeField] float zoomSpeed = 1.0f, moveSpeed = 3.0f;
    [SerializeField] float maxZoom = 100.0f, minZoom = 0.5f;
    [SerializeField] float maxIconSize = 0.7f, minIconSize = 0.1f;

    bool moveState = false;
    public bool canZoom = true;

    Vector2 moveAnchorPoint;

    private void Start() {
        var target = FindObjectOfType<MapMovement>().transform.position;
        Camera.main.transform.position = new Vector3(target.x, target.y, Camera.main.transform.position.z);
        transform.GetChild(0).localPosition = new Vector3(0.0f, 0.0f, Mathf.Abs(Camera.main.transform.position.z) + 1f);
        moveToPartyObject();
        moveState = true;
    }

    private void Update() {
        if(Input.GetMouseButtonDown(1))
            moveAnchorPoint = GameInfo.getMousePos();
        else if(Input.GetMouseButtonUp(1))
            moveAnchorPoint = Vector2.zero;
        if(Input.GetMouseButton(1)) {
            move(1.0f);
            moveState = false;
        }
        else if(moveState || FindObjectOfType<MapMovement>().isMoving) {
            moveToPartyObject();
            moveState = true;
        }
        if(canZoom && !FindObjectOfType<MapQuestMenu>().shown && !FindObjectOfType<MenuCanvas>().isOpen() && Input.mouseScrollDelta.y != 0) {
            zoom();
        }
    }


    void zoom() {
        float scroll = Input.mouseScrollDelta.y;

        //  there was a change in zoom
        if(scroll != 0) {
            //  zoom / movement
            moveAnchorPoint = GameInfo.getMousePos();
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Mathf.Clamp(Camera.main.transform.position.z + zoomSpeed * scroll * Time.deltaTime, minZoom, maxZoom));
            //Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + zoomSpeed * -scroll, minZoom, maxZoom);

            transform.GetChild(0).localPosition = new Vector3(0.0f, 0.0f, Mathf.Abs(Camera.main.transform.position.z) + 1f);

            //  scaling of images
            /*
            foreach(var i in FindObjectOfType<MapLocationSpawner>().getCurrentObjects()) {
                float scaleAmount = Mathf.Clamp(0.25f * (Camera.main.orthographicSize - 5.0f) / 4.0f, minIconSize, maxIconSize);
                i.transform.localScale = new Vector3(scaleAmount, scaleAmount, 0.0f);
            }
            */
        }
    }


    public void moveToPos(Vector2 pos) {
        if(FindObjectOfType<MapMovement>().isMoving)
            return;
        moveState = false;
        var target = new Vector3(pos.x, pos.y, Camera.main.transform.position.z);
        Camera.main.transform.position = target + (Vector3)offset;
    }

    void move(float percentage) {
        Vector2 o = (moveAnchorPoint - GameInfo.getMousePos()) * percentage;
        var target = (Vector2)Camera.main.transform.position + o;

        Camera.main.transform.position = new Vector3(target.x, target.y, Camera.main.transform.position.z);
    }

    void moveToPartyObject() {
        var target = FindObjectOfType<MapMovement>().transform.position;
        target = new Vector3(target.x, target.y, Camera.main.transform.position.z) + (Vector3)offset;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, target, moveSpeed * Time.deltaTime);
    }
}

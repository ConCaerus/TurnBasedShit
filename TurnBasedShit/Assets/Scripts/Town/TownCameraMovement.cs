using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownCameraMovement : MonoBehaviour {
    float maxXDiff = 3.5f;
    float speed = 5.0f;

    bool zoomedIn = false;
    Coroutine zoom = null;
    GameObject target;

    private void Awake() {
        target = FindObjectOfType<LocationMovement>().gameObject;
    }


    private void LateUpdate() {
        if(FindObjectOfType<TransitionCanvas>().loaded && zoom == null)
            moveToTarget();
    }


    void moveToTarget() {
        if(target.GetComponent<LocationMovement>().movingRight) {
            var p = new Vector3(target.transform.position.x + maxXDiff, 0.0f, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, p, speed * Time.deltaTime);
        }

        else {
            var p = new Vector3(target.transform.position.x - maxXDiff / 2.0f, 0.0f, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, p, speed * Time.deltaTime);
        }
    }

    public void hardMove() {
        var p = new Vector3(FindObjectOfType<LocationMovement>().transform.position.x, transform.position.y, transform.position.z);
        transform.position = p;
    }

    public void zoomIn(float y = 0.0f) {
        if(!zoomedIn) {
            Camera.main.orthographicSize = 5.0f;
            if(zoom != null)
                StopCoroutine(zoom);

            zoom = StartCoroutine(zoomAnim(4.0f, new Vector3(FindObjectOfType<LocationMovement>().transform.position.x, FindObjectOfType<LocationMovement>().transform.position.y + y, Camera.main.transform.position.z), false));
            zoomedIn = true;
        }
    }

    public void zoomOut() {
        if(zoomedIn) {
            Camera.main.orthographicSize = 4.0f;
            if(zoom != null)
                StopCoroutine(zoom);

            zoom = StartCoroutine(zoomAnim(5.0f, new Vector3(FindObjectOfType<LocationMovement>().transform.position.x, 0.0f, Camera.main.transform.position.z), true));
            zoomedIn = false;
        }
    }

    IEnumerator zoomAnim(float endVal, Vector3 targetPos, bool stopWhenDone) {
        while(Mathf.Abs(Camera.main.orthographicSize - endVal) > 0.005f) {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, endVal, speed * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPos, speed * 2.0f * Time.deltaTime);

            //  camera is too far from player
            if(Mathf.Abs(transform.position.x - FindObjectOfType<LocationMovement>().transform.position.x) > 7.0f) {
                break;
            }

            yield return new WaitForEndOfFrame();
        }
        Camera.main.orthographicSize = endVal;

        if(stopWhenDone)
            zoom = null;
    }
}

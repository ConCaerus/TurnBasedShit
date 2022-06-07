using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownCameraMovement : MonoBehaviour {
    float maxXDiff = 3.5f;
    float speed = 5.0f;
    Vector2 startingOffset;
    Vector2 offsetFromPlayer;

    bool zoomedIn = false;
    Coroutine zoom = null;
    GameObject target;

    private void Awake() {
        target = FindObjectOfType<LocationMovement>().gameObject;
        startingOffset = new Vector2(maxXDiff, transform.position.y - target.transform.position.y);
        offsetFromPlayer = startingOffset;
        moveToTarget(true);
    }


    private void LateUpdate() {
        if(target != null)
            moveToTarget();
    }


    void moveToTarget(bool snap = false) {
        float prev = transform.position.x;
        if(target.GetComponent<LocationMovement>().movingRight) {
            var p = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z) + (Vector3)offsetFromPlayer;
            transform.position = snap ? p : Vector3.Lerp(transform.position, p, speed * Time.deltaTime);
            //FindObjectOfType<EnvironmentHandler>().moveParallaxObjs(transform.position.x - prev);
        }

        else {
            var temp = new Vector2(-offsetFromPlayer.x, offsetFromPlayer.y);
            var p = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z) + (Vector3)temp;
            transform.position = snap ? p : Vector3.Lerp(transform.position, p, speed * Time.deltaTime);
            //FindObjectOfType<EnvironmentHandler>().moveParallaxObjs(transform.position.x - prev);
        }
    }

    public void hardMove() {
        var p = new Vector3(FindObjectOfType<LocationMovement>().transform.position.x, 0.0f, transform.position.z) + (Vector3)offsetFromPlayer;
        transform.position = p;
    }

    public void zoomIn(Vector2 offset) {
        if(!zoomedIn) {
            Camera.main.orthographicSize = 5.0f;
            if(zoom != null)
                StopCoroutine(zoom);

            zoom = StartCoroutine(zoomAnim(4.0f, offset, false));
            zoomedIn = true;
        }
    }

    public void zoomOut() {
        if(zoomedIn) {
            Camera.main.orthographicSize = 4.0f;
            if(zoom != null)
                StopCoroutine(zoom);

            zoom = StartCoroutine(zoomAnim(5.0f, startingOffset, true));
            zoomedIn = false;
        }
    }

    IEnumerator zoomAnim(float endVal, Vector2 offset, bool stopWhenDone) {
        offsetFromPlayer = offset;
        while(Mathf.Abs(Camera.main.orthographicSize - endVal) > 0.005f) {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, endVal, speed * Time.deltaTime);

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

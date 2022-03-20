using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMerchant : MonoBehaviour {
    Rigidbody2D rb;

    Vector3 target;

    public bool moving = false;

    MerchantInfo reference;

    private void OnTriggerEnter2D(Collider2D col) {
        if(moving && col.gameObject.tag == "Player") {
            moving = false;
            rb.velocity = Vector3.zero;
            FindObjectOfType<InteractionCanvas>().show(transform.position);
            FindObjectOfType<MapMovement>().closestIcon = gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D col) {
        if(col.gameObject.tag == "Player") {
            moving = true;
            FindObjectOfType<MapMovement>().closestIcon = null;
        }
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        target = Map.getRandPos();
        moving = true;
    }


    private void Update() {
        if(moving && FindObjectOfType<MapCameraController>().canZoom) {
            move();
        }
    }


    public void setReference(MerchantInfo info) {
        reference = info;
    }

    public void showCanvas() {
        FindObjectOfType<MapMerchantCanvas>().show(reference.inv);
    }


    void move() {
        float speed = 50.0f;

        var offset = target - transform.position;
        rb.velocity = new Vector2(Mathf.Clamp(offset.x, -1.0f, 1.0f), Mathf.Clamp(offset.y, -1.0f, 1.0f)) * speed * Time.fixedDeltaTime;

        if(Vector2.Distance(transform.position, target) < 1.0f) {
            StartCoroutine(findNewTarget());
            moving = false;
            rb.velocity = Vector2.zero;
        }
    }


    IEnumerator findNewTarget() {
        yield return new WaitForSeconds(.5f);

        target = Map.getRandPos();
        moving = true;
    }
}

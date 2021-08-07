using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShrineObject : MonoBehaviour {
    public float distToInteract = 1.5f;
    public GameObject shrineCanvas;

    float showTime = 0.25f;


    public void showCanvas() {
        shrineCanvas.SetActive(true);
        shrineCanvas.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), showTime);
        FindObjectOfType<LocationMovement>().flip();
        FindObjectOfType<LocationMovement>().canMove = false;
    }

    public void hideCanvas() {
        StartCoroutine(hider());
    }

    IEnumerator hider() {
        shrineCanvas.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), showTime);

        yield return new WaitForSeconds(showTime);

        shrineCanvas.SetActive(false);
        FindObjectOfType<LocationMovement>().flip();
        FindObjectOfType<LocationMovement>().canMove = true;
    }

    public bool isInMenu() {
        return shrineCanvas.activeInHierarchy;
    }
}

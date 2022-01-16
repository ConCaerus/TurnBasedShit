using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MapPartyInfoCanvas : MonoBehaviour {
    [SerializeField] GameObject closeButton, slots;

    bool shown = false;
    Coroutine shower = null;


    private void Start() {
        for(int i = 0; i < slots.transform.childCount; i++) {
            slots.transform.GetChild(i).GetComponent<Slot>().updateInfo(i);
        }
        transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = ">";
    }

    IEnumerator show() {
        float speed = 0.15f;

        transform.GetChild(1).GetComponent<RectTransform>().DOAnchorPosX(transform.GetChild(0).GetComponent<RectTransform>().rect.width, speed / 2.0f);
        transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "<";
        yield return new WaitForSeconds(speed / 2.0f);

        transform.GetChild(0).GetComponent<RectTransform>().DOScaleY(1.0f, speed);
        yield return new WaitForSeconds(speed);

        shown = true;
        shower = null;
    }

    IEnumerator hide() {
        float speed = 0.25f;

        transform.GetChild(0).GetComponent<RectTransform>().DOScaleY(0.0f, speed);
        yield return new WaitForSeconds(speed);

        transform.GetChild(1).GetComponent<RectTransform>().DOAnchorPosX(0.0f, speed / 2.0f);
        transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = ">";

        yield return new WaitForSeconds(speed / 2.0f);

        shown = false;
        shower = null;
    }

    //  Buttons
    public void toggleShown() {
        if(shower != null)
            return;
        if(shown)
            shower = StartCoroutine(hide());
        else
            shower = StartCoroutine(show());
    }
}

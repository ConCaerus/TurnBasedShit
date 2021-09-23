using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class InfoCanvas : MonoBehaviour {
    bool shown = false;

    Coroutine shower = null;

    public enum infoType {
        mapLocation
    };


    private void Awake() {
        DOTween.Init();
        if(transform.GetChild(0).GetComponent<RectTransform>().localScale.y > 0.0f)
            shown = true;
        hide();
    }

    private void Update() {
        positionInfoBox();


        foreach(var i in FindObjectsOfType<InfoBearer>()) {
            if(Vector2.Distance(GameInfo.getMousePos(), i.transform.position) < i.distToShowInfo) {
                if(shower == null)
                    shower = StartCoroutine(showInfo(i));
                return;
            }
        }

        if(shower != null)
            StopCoroutine(shower);
        shower = null;
        hide();
    }

    void positionInfoBox() {
        var offset = (transform.GetChild(0).GetComponent<RectTransform>().sizeDelta / 2.0f) * new Vector2(-1.0f, -1.0f);
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponentInParent<Canvas>().transform as RectTransform, Input.mousePosition, GetComponentInParent<Canvas>().worldCamera, out pos);
        transform.position = GetComponentInParent<Canvas>().transform.TransformPoint(pos + offset);
    }


    IEnumerator showInfo(InfoBearer b) {
        yield return new WaitForSeconds(b.waitTime);
        show();
        transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = b.titleText;
        transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = b.firstText;
        transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = b.secondText;

        shower = null;
    }



    public void show() {
        if(shown)
            return;
        transform.GetChild(0).GetComponent<Image>().DOColor(Color.black, 0.05f);
        transform.GetChild(0).GetComponent<RectTransform>().localRotation = Quaternion.Euler(0.0f, 0.0f, -25.0f);
        transform.GetChild(0).GetComponent<RectTransform>().DOScaleY(1.0f, 0.05f);
        transform.GetChild(0).GetComponent<RectTransform>().DOLocalRotate(new Vector3(0.0f, 0.0f, 0.0f), 0.05f);
        shown = true;
    }
    public void hide() {
        if(!shown)
            return;
        transform.GetChild(0).GetComponent<RectTransform>().DOScaleY(0.0f, 0.1f);
        transform.GetChild(0).GetComponent<RectTransform>().DOLocalRotate(new Vector3(0.0f, 0.0f, -25.0f), 0.1f);
        transform.GetChild(0).GetComponent<Image>().DOColor(Color.clear, 0.1f);
        shown = false;
    }


    public bool isOpen() {
        return shown;
    }
}

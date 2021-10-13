using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class InfoCanvas : MonoBehaviour {
    bool shown = false;

    float waitTimeForInfo = 0.5f;

    GameObject background;
    TextMeshProUGUI infoText;

    Coroutine shower = null;


    private void Awake() {
        background = transform.GetChild(0).gameObject;
        infoText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        startHiding();
    }


    private void Update() {
        if(shown) {
            positionInfoBox();
        }
    }

    void positionInfoBox() {
        var offset = (background.GetComponent<RectTransform>().sizeDelta / 2.0f) * new Vector2(1.0f, -1.0f);
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<Canvas>().transform as RectTransform, Input.mousePosition, GetComponent<Canvas>().worldCamera, out pos);
        background.transform.position = GetComponent<Canvas>().transform.TransformPoint(pos + offset);
    }



    public void showOneSentenceInfo(string info) {
        positionInfoBox();
        infoText.text = info;
    }

    public void startShowing(string info) {
        if(shower != null)
            StopCoroutine(shower);


        background.GetComponent<Image>().DOComplete();
        infoText.GetComponent<TextMeshProUGUI>().DOComplete();

        shower = StartCoroutine(waitToShow(info));
    }

    public void startHiding() {
        if(shower != null)
            StopCoroutine(shower);
        shower = null;



        background.GetComponent<Image>().DOComplete();
        infoText.GetComponent<TextMeshProUGUI>().DOComplete();

        background.GetComponent<Image>().DOColor(Color.clear, 0.15f);
        infoText.GetComponent<TextMeshProUGUI>().DOColor(Color.clear, 0.15f);
        shown = false;
    }



    IEnumerator waitToShow(string info) {
        infoText.GetComponent<LayoutElement>().preferredWidth = 200.0f;
        showOneSentenceInfo(info);
        yield return new WaitForSeconds(waitTimeForInfo);

        if(infoText.textBounds.size.x < 200.0f && infoText.textInfo.lineCount == 1)
            infoText.GetComponent<LayoutElement>().preferredWidth = infoText.textBounds.size.x;
        shown = true;

        background.GetComponent<Image>().color = Color.clear;
        infoText.GetComponent<TextMeshProUGUI>().color = Color.clear;
        background.GetComponent<Image>().DOColor(Color.black, 0.1f);
        infoText.GetComponent<TextMeshProUGUI>().DOColor(Color.white, 0.1f);

        yield return new WaitForSeconds(0.1f);

        shower = null;
    }


    public bool isOpen() {
        return shown;
    }
}

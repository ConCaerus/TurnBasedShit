using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class WaitingButton : MonoBehaviour {
    GameObject fill;
    TextMeshProUGUI text;
    GameObject button;

    public float fillThickness;
    public float timeToComplete = 2.0f;
    public float fillGrowMod = 1.005f;
    public float speedMod = 10.0f;

    public Color normalColor, highlightColor, selectedColor;

    public UnityEvent clickEvent;

    Coroutine waiter = null, returner = null;


    bool clicked = false;



    private void OnMouseDown() {
        clicked = true;
        button.GetComponent<Image>().color = selectedColor;
        startWaiting();
    }

    private void OnMouseEnter() {
        button.GetComponent<Image>().color = highlightColor;
    }

    private void OnMouseUp() {
        clicked = false;
        button.GetComponent<Image>().color = highlightColor;
        startReturning();
    }

    private void OnMouseExit() {
        clicked = false;
        button.GetComponent<Image>().color = normalColor;
        startReturning();
    }

    private void Start() {
        fill = transform.GetChild(0).gameObject;
        text = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        button = transform.GetChild(1).gameObject;

        //  resizes the fill
        fill.transform.GetComponent<RectTransform>().offsetMax = new Vector2(fillThickness, fillThickness);
        fill.transform.GetComponent<RectTransform>().offsetMin = new Vector2(-fillThickness, -fillThickness);

        GetComponent<BoxCollider2D>().size = GetComponent<RectTransform>().sizeDelta;

        setValue(0.0f);

        button.GetComponent<Image>().color = normalColor;
    }


    void setValue(float f) {
        fill.GetComponent<Image>().fillAmount = f;
    }

    public void setText(string t) {
        text.text = t;
    }


    public void startWaiting() {
        if(returner != null)
            StopCoroutine(returner);
        returner = null;

        if(waiter == null) {
            waiter = StartCoroutine(waitForCompletion());
        }
    }
    void startReturning() {
        if(waiter != null)
            StopCoroutine(waiter);
        waiter = null;

        if(returner == null)
            returner = StartCoroutine(returnToStart());
    }


    IEnumerator waitForCompletion() {
        float neededTime = timeToComplete - timeToComplete * fill.GetComponent<Image>().fillAmount;

        float a = fill.GetComponent<Image>().fillAmount;  // start
        float b = 1.0f;  // end
        for(float f = 0; f <= neededTime; f += Time.deltaTime * Mathf.Clamp((speedMod * (1.0f - ((f + 0.01f) / neededTime))), 0.1f, 1.0f)) {
            fill.GetComponent<Image>().fillAmount = Mathf.Lerp(a, b, f / neededTime);

            float minX = Mathf.Lerp(fill.GetComponent<RectTransform>().offsetMin.x, fill.GetComponent<RectTransform>().offsetMin.x * fillGrowMod, f / neededTime);
            float minY = Mathf.Lerp(fill.GetComponent<RectTransform>().offsetMin.y, fill.GetComponent<RectTransform>().offsetMin.y * fillGrowMod, f / neededTime);
            float maxX = Mathf.Lerp(fill.GetComponent<RectTransform>().offsetMax.x, fill.GetComponent<RectTransform>().offsetMax.x * fillGrowMod, f / neededTime);
            float maxY = Mathf.Lerp(fill.GetComponent<RectTransform>().offsetMax.y, fill.GetComponent<RectTransform>().offsetMax.y * fillGrowMod, f / neededTime);
            fill.GetComponent<RectTransform>().offsetMin = new Vector2(minX, minY);
            fill.GetComponent<RectTransform>().offsetMax = new Vector2(maxX, maxY);
            yield return null;
        }

        setValue(1.0f);

        if(clickEvent.GetPersistentEventCount() > 0)
            clickEvent.Invoke();

        waiter = null;
    }

    IEnumerator returnToStart() {
        float timeToReturn = timeToComplete / 4.0f;
        float neededTime = timeToReturn - timeToReturn * (1 - fill.GetComponent<Image>().fillAmount);

        float a = fill.GetComponent<Image>().fillAmount;  // start
        float b = 0.0f;  // end
        for(float f = 0; f <= neededTime; f += Time.deltaTime) {
            fill.GetComponent<Image>().fillAmount = Mathf.Lerp(a, b, f / neededTime);

            float minX = Mathf.Lerp(fill.GetComponent<RectTransform>().offsetMin.x, -fillThickness, f / neededTime);
            float minY = Mathf.Lerp(fill.GetComponent<RectTransform>().offsetMin.y, -fillThickness, f / neededTime);
            float maxX = Mathf.Lerp(fill.GetComponent<RectTransform>().offsetMax.x, fillThickness, f / neededTime);
            float maxY = Mathf.Lerp(fill.GetComponent<RectTransform>().offsetMax.y, fillThickness, f / neededTime);
            fill.GetComponent<RectTransform>().offsetMin = new Vector2(minX, minY);
            fill.GetComponent<RectTransform>().offsetMax = new Vector2(maxX, maxY);
            yield return null;
        }

        setValue(0.0f);
        returner = null;
    }
}

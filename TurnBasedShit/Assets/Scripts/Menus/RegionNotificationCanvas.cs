using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class RegionNotificationCanvas : MonoBehaviour {
    [SerializeField] TextMeshProUGUI regionText;
    float startingX;

    Coroutine anim = null;


    private void Start() {
        DOTween.Init();
        startingX = regionText.GetComponent<RectTransform>().localPosition.x;
        StartCoroutine(FindObjectOfType<TransitionCanvas>().runAfterLoading(delegate { anim = StartCoroutine(animateNameText()); }));
    }

    public void startShowing() {
        if(anim != null)
            return;

        anim = StartCoroutine(animateNameText());
    }

    public IEnumerator animateNameText() {
        float speed = 0.5f, waitTimeBtw = .75f;

        regionText.text = "Region #" + ((int)GameInfo.getCurrentRegion()).ToString();
        regionText.GetComponent<RectTransform>().DOLocalMoveX(0.0f, speed);

        yield return new WaitForSeconds(speed + waitTimeBtw);

        regionText.GetComponent<RectTransform>().DOLocalMoveX(Mathf.Abs(startingX), speed);

        yield return new WaitForSeconds(speed);

        regionText.GetComponent<RectTransform>().localPosition = new Vector3(startingX, regionText.GetComponent<RectTransform>().localPosition.y, regionText.GetComponent<RectTransform>().localPosition.z);
        anim = null;
    }


    public bool canAnimate() {
        return anim == null;
    }
}

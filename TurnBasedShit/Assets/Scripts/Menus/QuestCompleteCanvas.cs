using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuestCompleteCanvas : MonoBehaviour {
    Coroutine shower = null;

    [SerializeField] GameObject image;

    private void Start() {
        image.SetActive(true);
        GetComponent<Canvas>().worldCamera = Camera.main;
        image.GetComponent<Image>().color = Color.clear;
        image.transform.localScale = new Vector3(0.0f, 0.0f);
        DOTween.Init();
    }


    public void showCanvas() {
        if(shower != null)
            return;
        shower = StartCoroutine(animateShowing());
    }

    IEnumerator animateShowing() {
        image.transform.DOKill();
        image.transform.DOScale(1.0f, .15f);
        image.GetComponent<Image>().DOColor(Color.white, .15f);
        image.transform.DOShakeRotation(.35f, 60f);

        yield return new WaitForSeconds(2f);

        image.GetComponent<Image>().DOColor(Color.clear, .25f);
        image.transform.DOPunchScale(new Vector3(1.0001f, 1.0001f), .15f);

        yield return new WaitForSeconds(.15f);
        image.transform.DOScale(0.0f, .15f);
        shower = null;
    }
}

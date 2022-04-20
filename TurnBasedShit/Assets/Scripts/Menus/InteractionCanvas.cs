using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class InteractionCanvas : MonoBehaviour {
    public string interactTag;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] float offset = 100f;

    private void Start() {
        hardHide();
    }


    public void show(Vector2 interactPoint) {
        text.transform.DOKill();
        text.text = interactTag;
        transform.position = interactPoint;
        text.transform.DOLocalMoveY(offset, .15f);
        text.transform.DOScale(1f, .15f);
    }
    public void hide() {
        text.transform.DOKill();
        text.transform.DOLocalMoveY(0f, .15f);
        text.transform.DOScale(0f, .15f);
    }

    public void hardHide() {
        text.transform.DOKill();
        text.transform.localPosition = Vector3.zero;
        text.transform.localScale = Vector3.zero;
    }
}

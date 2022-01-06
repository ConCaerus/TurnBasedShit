using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class InteractionCanvas : MonoBehaviour {
    [SerializeField] string interactTag;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] float offset = 100f;

    private void Start() {
        hide();
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
        text.transform.DOLocalMoveY(0f, .25f);
        text.transform.DOScale(0f, .25f);
    }
}

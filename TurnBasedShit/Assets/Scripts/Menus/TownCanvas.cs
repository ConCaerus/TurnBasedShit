using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TownCanvas : MonoBehaviour {
    [SerializeField] TextMeshProUGUI nameText;
    Vector2 startPos;


    private void Start() {
        DOTween.Init();
        startPos = nameText.transform.position;
        StartCoroutine(FindObjectOfType<TransitionCanvas>().runAfterLoading(startNameAnim));
    }


    public void startNameAnim() {
        StartCoroutine(animateNameText());
    }

    public IEnumerator animateNameText() {
        float height = 150.0f;
        float nameTime = 2.0f;
        float percentage = 5.0f;

        nameText.text = GameInfo.getCurrentLocationAsTown().town.t_name;
        nameText.transform.DOMoveY(startPos.y - height, nameTime / percentage);

        yield return new WaitForSeconds(nameTime);

        nameText.transform.DOMoveY(startPos.y + height, nameTime / percentage);
    }
}

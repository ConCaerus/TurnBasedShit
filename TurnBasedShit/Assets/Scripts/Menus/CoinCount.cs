using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CoinCount : MonoBehaviour {
    public TextMeshProUGUI coinCount;

    [SerializeField] Color posCoinColor, negCoinColor;

    private void Start() {
        DOTween.Init();
    }

    public void updateCount(bool smoothly) {
        if(smoothly)
            StartCoroutine(animateCountChange());
        else
            coinCount.text = Inventory.getCoinCount().ToString();
    }

    IEnumerator animateCountChange() {
        var count = int.Parse(coinCount.text);
        float waitTime = 0.015f;

        while(count != Inventory.getCoinCount()) {
            if(count > Inventory.getCoinCount())
                count--;
            else
                count++;

            coinCount.text = count.ToString();
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void createCoinChangeText(int change) {
        StartCoroutine(animateCoinChangeText(change));
    }

    //  if text is not moving down, make sure that the parent canvas is set to screen space - Camera
    IEnumerator animateCoinChangeText(int change) {
        var obj = Instantiate(coinCount.gameObject, transform);
        obj.transform.position = coinCount.gameObject.transform.position;
        obj.GetComponent<TextMeshProUGUI>().fontSize = coinCount.GetComponent<TextMeshProUGUI>().fontSize / 1.5f;

        if(change > 0) {
            obj.GetComponent<TextMeshProUGUI>().color = posCoinColor;
            obj.GetComponent<TextMeshProUGUI>().text = "+" + change.ToString();
        }
        else {
            obj.GetComponent<TextMeshProUGUI>().color = negCoinColor;
            obj.GetComponent<TextMeshProUGUI>().text = change.ToString();
        }


        float startSpeed = 0.5f;
        float endSpeed = 0.5f;
        obj.transform.DOMoveY(obj.transform.position.y - 1.0f, endSpeed + startSpeed);

        yield return new WaitForSeconds(startSpeed);

        obj.GetComponent<TextMeshProUGUI>().DOColor(Color.clear, endSpeed);
        Destroy(obj.gameObject, endSpeed);
    }
}

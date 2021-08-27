using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class CombatUnitUI : MonoBehaviour {
    public GameObject canvasPreset;
    GameObject uiObj;

    [SerializeField] float yOffset;
    public Vector2 highlightOffset;
    float sliderSpeed = 0.5f, moveSpeed = 18.0f, highlightedHeight = 0.25f;

    Coroutine lateMover = null;


    private void Start() {
        spawnCanvas();
        updateUIInfo();
    }


    private void LateUpdate() {
        positionUIObj();
        updateUIInfo();
    }



    void spawnCanvas() {
        var temp = Instantiate(canvasPreset.gameObject, transform);
        temp.GetComponent<Canvas>().worldCamera = Camera.main;

        uiObj = temp.transform.GetChild(0).gameObject;
    }


    void positionUIObj() {
        Vector2 target;
        if(FindObjectOfType<UnitCombatHighlighter>().isUnitInList(gameObject))
            target = gameObject.transform.position + new Vector3(0.0f, yOffset + highlightedHeight, 0.0f);
        else
            target = gameObject.transform.position + new Vector3(0.0f, yOffset, 0.0f);

        uiObj.transform.position = Vector2.Lerp(uiObj.transform.position, target, moveSpeed * Time.deltaTime);
    }


    void updateUIInfo() {
        uiObj.transform.GetChild(0).GetComponent<Slider>().maxValue = GetComponent<UnitClass>().stats.getModifiedMaxHealth();
        uiObj.transform.GetChild(0).GetComponent<Slider>().value = GetComponent<UnitClass>().stats.u_health;

        uiObj.transform.GetChild(2).GetComponent<Slider>().maxValue = GetComponent<UnitClass>().stats.getModifiedMaxHealth();
        if(uiObj.transform.GetChild(2).GetComponent<Slider>().value != uiObj.transform.GetChild(0).GetComponent<Slider>().value && lateMover == null)
            lateMover = StartCoroutine(moveLateSlider());



        uiObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = GetComponent<UnitClass>().stats.u_level.ToString();
    }


    IEnumerator moveLateSlider() {
        yield return new WaitForSeconds(0.15f);

        uiObj.transform.GetChild(2).GetComponent<Slider>().DOValue(GetComponent<UnitClass>().stats.u_health, sliderSpeed);
        lateMover = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class CombatUnitUI : MonoBehaviour {
    public GameObject canvasPreset, stunPreset, bleedPreset;
    GameObject uiObj, stunObj, bleedObj;

    [SerializeField] float yOffset;
    public Vector2 highlightOffset, stunOffset, bleedOffset;
    float sliderSpeed = 0.5f, moveSpeed = 18.0f, highlightedHeight = 0.25f;

    Coroutine lateMover = null, stunMover = null, bleedMover = null;


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
        //  sliders n' shit
        //  real health
        uiObj.transform.GetChild(0).GetComponent<Slider>().maxValue = GetComponent<UnitClass>().stats.getModifiedMaxHealth();
        uiObj.transform.GetChild(0).GetComponent<Slider>().value = GetComponent<UnitClass>().stats.u_health;

        //  late health
        uiObj.transform.GetChild(2).GetComponent<Slider>().maxValue = GetComponent<UnitClass>().stats.getModifiedMaxHealth();
        if(uiObj.transform.GetChild(2).GetComponent<Slider>().value != uiObj.transform.GetChild(0).GetComponent<Slider>().value && lateMover == null)
            lateMover = StartCoroutine(moveLateSlider());


        //  text n' shit
        uiObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = GetComponent<UnitClass>().stats.u_level.ToString();  //  level text

        //  stun shit
        if(GetComponent<UnitClass>().isStunned() && stunObj == null) {
            stunObj = Instantiate(stunPreset.gameObject, transform);
            stunObj.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            StartCoroutine(showStunObj());
        }
        else if(!GetComponent<UnitClass>().isStunned() && stunObj != null) {
            if(stunMover != null)
                StopCoroutine(stunMover);
            stunMover = null;

            float waitTime = 0.15f;
            stunObj.transform.DOLocalMoveY(0.0f, waitTime);
            stunObj.transform.DOScale(0.0f, waitTime);
            Destroy(stunObj.gameObject, waitTime);
        }

        //  bleed shit
        if(GetComponent<UnitClass>().stats.u_bleedCount > 0 && bleedObj == null) {
            bleedObj = Instantiate(bleedPreset.gameObject, transform);
            bleedObj.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            bleedObj.transform.DOScale(1.0f, 0.15f);
            bleedObj.transform.localPosition = bleedOffset;

            bleedMover = StartCoroutine(bleedAnim());
        }
        else if(GetComponent<UnitClass>().stats.u_bleedCount == 0 && bleedObj != null) {
            if(bleedMover != null)
                StopCoroutine(bleedMover);
            bleedMover = null;

            float waitTime = 0.15f;
            bleedObj.transform.DOScale(0.0f, waitTime);
            Destroy(bleedObj.gameObject, waitTime);
        }
    }

    public void removeUI() {
        uiObj.SetActive(false);
    }


    IEnumerator moveLateSlider() {
        yield return new WaitForSeconds(0.15f);

        uiObj.transform.GetChild(2).GetComponent<Slider>().DOValue(GetComponent<UnitClass>().stats.u_health, sliderSpeed);
        lateMover = null;
    }

    IEnumerator stunAnim() {
        float diff = 0.1f, waitTime = 0.25f;
        stunObj.transform.DOLocalMoveY(stunOffset.y + diff, waitTime);
        yield return new WaitForSeconds(waitTime);

        stunObj.transform.DOLocalMoveY(stunOffset.y - diff * 2.0f, waitTime);
        yield return new WaitForSeconds(waitTime);

        stunObj.transform.DOLocalMoveY(stunOffset.y + diff, waitTime);
        yield return new WaitForSeconds(waitTime);

        stunMover = StartCoroutine(stunAnim());
    }
    IEnumerator showStunObj() {
        float waitTime = 0.15f;

        stunObj.transform.DOLocalMove(stunOffset, waitTime);
        stunObj.transform.DOScale(1.0f, waitTime);

        yield return new WaitForSeconds(waitTime);

        stunMover = StartCoroutine(stunAnim());
    }

    IEnumerator bleedAnim() {
        float diff = 0.1f, waitTime = 0.25f;
        bleedObj.transform.DOLocalMove(bleedOffset + new Vector2(0.0f, diff), waitTime);
        yield return new WaitForSeconds(waitTime);

        bleedObj.transform.DOLocalMove(bleedOffset + new Vector2(0.0f, -diff * 2.0f), waitTime);
        yield return new WaitForSeconds(waitTime);

        bleedObj.transform.DOLocalMove(bleedOffset + new Vector2(0.0f, diff), waitTime);
        yield return new WaitForSeconds(waitTime);

        bleedMover = StartCoroutine(bleedAnim());
    }
}

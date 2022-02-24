using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class CombatUnitUI : MonoBehaviour {
    public GameObject canvasPreset, stunPreset, bleedPreset;
    GameObject[] equipmentSlots = new GameObject[3];
    Vector2[] equipmentSlotsPos = new Vector2[3];
    GameObject uiObj, stunObj, bleedObj;
    TextMeshProUGUI bleedCount;
    Vector2 bleedCountPos;

    public CombatCards combatCards;

    [SerializeField] Vector2 offset;
    public Vector2 highlightOffset, stunOffset, bleedOffset;
    float sliderSpeed = 0.5f, highlightedHeight = 0.25f;
    float[] moveSpeeds = new float[4];
    float maxMoveSpeed = 25.0f, minMoveSpeed = 10.0f;

    //  false for auto, true for modifying
    public bool showingWouldBeHealedValue = false;

    Coroutine lateMover = null, stunMover = null, bleedMover = null;


    private void Start() {
        for(int i = 0; i < moveSpeeds.Length; i++)
            moveSpeeds[i] = Random.Range(minMoveSpeed, maxMoveSpeed);

        spawnCanvas();
        hardSetHealthSliders();
        updateUIInfo();
    }


    private void LateUpdate() {
        positionUIObj();
        updateUIInfo();
    }

    private void Update() {
        if(equipmentSlots[0] != null) {
            if(Input.GetMouseButtonDown(1) && GetComponent<PlayerUnitInstance>() != null && GetComponent<PlayerUnitInstance>().isMouseOverUnit)
                showExtra();
            else if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                hideExtra();
        }
    }



    void spawnCanvas() {
        var temp = Instantiate(canvasPreset.gameObject, transform);
        temp.GetComponent<Canvas>().worldCamera = Camera.main;

        uiObj = temp.transform.GetChild(0).gameObject;

        equipmentSlots[0] = uiObj.transform.GetChild(3).gameObject;
        equipmentSlots[1] = uiObj.transform.GetChild(5).gameObject;
        equipmentSlots[2] = uiObj.transform.GetChild(4).gameObject;
        bleedCount = uiObj.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>();
        bleedCountPos = bleedCount.transform.localPosition;
        combatCards = uiObj.transform.GetChild(7).gameObject.GetComponent<CombatCards>();

        for(int i = 0; i < equipmentSlots.Length; i++) {
            equipmentSlotsPos[i] = equipmentSlots[i].transform.localPosition;
        }


        hideExtra();
    }

    public void showExtra() {
        for(int i = 0; i < equipmentSlots.Length; i++) {
            equipmentSlots[i].transform.DOLocalMove(equipmentSlotsPos[i], .05f);
            equipmentSlots[i].transform.DOScale(1.0f, 0.05f);
        }
        bleedCount.gameObject.transform.DOLocalMove(bleedCountPos, .05f);
        bleedCount.transform.DOScale(1.0f, 0.05f);
    }
    public void hideExtra() {
        foreach(var i in equipmentSlots) {
            i.transform.DOMove(transform.position, 0.1f);
            i.transform.DOScale(0.0f, 0.1f);
        }
        bleedCount.gameObject.transform.DOMove(transform.position, .1f);
        bleedCount.transform.DOScale(0.0f, 0.1f);
    }


    void positionUIObj() {
        Vector2 target;
        if(FindObjectOfType<UnitCombatHighlighter>().isUnitInList(gameObject))
            target = transform.position + new Vector3(offset.x, offset.y + highlightedHeight, 0.0f);
        else
            target = transform.position + new Vector3(offset.x, offset.y, 0.0f);
        uiObj.transform.position = Vector2.Lerp(uiObj.transform.position, target, moveSpeeds[0] * Time.deltaTime);
    }


    public void updateUIInfo() {
        //  sliders n' shit
        uiObj.transform.GetChild(0).GetComponent<Slider>().maxValue = GetComponent<UnitClass>().stats.getModifiedMaxHealth();
        uiObj.transform.GetChild(2).GetComponent<Slider>().maxValue = GetComponent<UnitClass>().stats.getModifiedMaxHealth();

        if(uiObj.transform.GetChild(0).GetComponent<Slider>().value > GetComponent<UnitClass>().stats.u_health) {
            //  thick health
            uiObj.transform.GetChild(0).GetComponent<Slider>().value = GetComponent<UnitClass>().stats.u_health;
            //  light health
            if(uiObj.transform.GetChild(2).GetComponent<Slider>().value != uiObj.transform.GetChild(0).GetComponent<Slider>().value && lateMover == null && !showingWouldBeHealedValue)
                lateMover = StartCoroutine(moveLateSlider(uiObj.transform.GetChild(2).GetComponent<Slider>()));
        }
        else if(uiObj.transform.GetChild(0).GetComponent<Slider>().value < GetComponent<UnitClass>().stats.u_health) {
            //  light health
            if(!showingWouldBeHealedValue)
                uiObj.transform.GetChild(2).GetComponent<Slider>().value = GetComponent<UnitClass>().stats.u_health;
            //  thick health
            if(uiObj.transform.GetChild(0).GetComponent<Slider>().value != uiObj.transform.GetChild(2).GetComponent<Slider>().value && lateMover == null)
                lateMover = StartCoroutine(moveLateSlider(uiObj.transform.GetChild(0).GetComponent<Slider>()));
        }
        else if(uiObj.transform.GetChild(0).GetComponent<Slider>().value != uiObj.transform.GetChild(2).GetComponent<Slider>().value && lateMover == null && !showingWouldBeHealedValue)
            lateMover = StartCoroutine(moveLateSlider(uiObj.transform.GetChild(2).GetComponent<Slider>()));


        //  text n' shit
        uiObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = GetComponent<UnitClass>().stats.u_level.ToString();  //  level text
        bleedCount.text = GetComponent<UnitClass>().stats.u_bleedCount.ToString("0");

        if(GetComponent<UnitClass>().stats.u_type != GameInfo.combatUnitType.player || !GetComponent<UnitClass>().combatStats.isPlayerUnit) {
            if(GetComponent<UnitClass>().stats.u_bleedCount > 0) {
                bleedCount.gameObject.transform.DOLocalMove(bleedCountPos, .05f);
                bleedCount.transform.DOScale(1.0f, 0.05f);
            }
            else {
                bleedCount.gameObject.transform.DOMove(transform.position, .1f);
                bleedCount.transform.DOScale(0.0f, 0.1f);
            }
        }

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

        //  equipment shit
        if(GetComponent<UnitClass>().stats.u_type == GameInfo.combatUnitType.player && equipmentSlots[0] != null) {
            if(GetComponent<UnitClass>().stats.weapon != null && !GetComponent<UnitClass>().stats.weapon.isEmpty()) {
                equipmentSlots[0].SetActive(true);
                equipmentSlots[0].GetComponent<SlotObject>().setImage(1, FindObjectOfType<PresetLibrary>().getWeaponSprite(GetComponent<UnitClass>().stats.weapon).sprite);
                equipmentSlots[0].GetComponent<SlotObject>().setImageColor(0, FindObjectOfType<PresetLibrary>().getRarityColor(GetComponent<UnitClass>().stats.weapon.rarity));
                equipmentSlots[0].GetComponent<SlotObject>().setInfo(GetComponent<UnitClass>().stats.weapon.name);
            }
            else
                equipmentSlots[0].SetActive(false);

            if(GetComponent<UnitClass>().stats.armor != null && !GetComponent<UnitClass>().stats.armor.isEmpty()) {
                equipmentSlots[1].SetActive(true);
                equipmentSlots[1].GetComponent<SlotObject>().setImage(1, FindObjectOfType<PresetLibrary>().getArmorSprite(GetComponent<UnitClass>().stats.armor).sprite);
                equipmentSlots[1].GetComponent<SlotObject>().setImageColor(0, FindObjectOfType<PresetLibrary>().getRarityColor(GetComponent<UnitClass>().stats.armor.rarity));
                equipmentSlots[1].GetComponent<SlotObject>().setInfo(GetComponent<UnitClass>().stats.armor.name);
            }
            else
                equipmentSlots[1].SetActive(false);

            if(GetComponent<UnitClass>().stats.item != null && !GetComponent<UnitClass>().stats.item.isEmpty()) {
                equipmentSlots[2].SetActive(true);
                equipmentSlots[2].GetComponent<SlotObject>().setImage(1, FindObjectOfType<PresetLibrary>().getItemSprite(GetComponent<UnitClass>().stats.item).sprite);
                equipmentSlots[2].GetComponent<SlotObject>().setImageColor(0, FindObjectOfType<PresetLibrary>().getRarityColor(GetComponent<UnitClass>().stats.item.rarity));
                equipmentSlots[2].GetComponent<SlotObject>().setInfo(GetComponent<UnitClass>().stats.item.name);
            }
            else
                equipmentSlots[2].SetActive(false);
        }
    }

    public void hardSetHealthSliders() {
        uiObj.transform.GetChild(0).GetComponent<Slider>().maxValue = GetComponent<UnitClass>().stats.getModifiedMaxHealth();
        uiObj.transform.GetChild(2).GetComponent<Slider>().maxValue = GetComponent<UnitClass>().stats.getModifiedMaxHealth();

        uiObj.transform.GetChild(0).GetComponent<Slider>().value = GetComponent<UnitClass>().stats.u_health;
        uiObj.transform.GetChild(2).GetComponent<Slider>().value = GetComponent<UnitClass>().stats.u_health;
    }

    public void moveLightHealthSliderToValue(float value) {
        uiObj.transform.GetChild(2).GetComponent<Slider>().DOValue(value, sliderSpeed);
        showingWouldBeHealedValue = true;
    }

    public void removeUI() {
        uiObj.SetActive(false);
    }


    IEnumerator moveLateSlider(Slider late) {
        yield return new WaitForSeconds(0.15f);

        late.DOValue(GetComponent<UnitClass>().stats.u_health, sliderSpeed);
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

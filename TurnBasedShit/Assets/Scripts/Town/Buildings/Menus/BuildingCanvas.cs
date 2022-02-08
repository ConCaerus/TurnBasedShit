using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

//  only used for building instances that require a unit to be selected
public abstract class BuildingCanvas : MonoBehaviour {
    public GameObject canvas;
    public GameObject mainSlot;
    public GameObject slots;
    GameObject lastMousedOverSlot = null;

    protected int selectedIndex = -1;

    float showTime = 0.15f;
    float animTime = 0.15f;

    private void Start() {
        DOTween.Init();
        updateInfo();
    }

    private void Update() {
        if(getMousedOverSlot() != null && getMousedOverSlot() != lastMousedOverSlot) {
            //  returns last used slot to normal
            if(lastMousedOverSlot != null) {
                lastMousedOverSlot.transform.GetChild(0).GetComponent<RectTransform>().DOKill();
                lastMousedOverSlot.transform.GetChild(1).GetComponent<RectTransform>().DOKill();
                lastMousedOverSlot.transform.GetChild(0).GetComponent<RectTransform>().DOScale(0.35f, animTime);
                lastMousedOverSlot.transform.GetChild(1).GetComponent<RectTransform>().DOScale(0.35f, animTime);
            }

            //  inflates new used slot
            lastMousedOverSlot = getMousedOverSlot();
            lastMousedOverSlot.transform.GetChild(0).GetComponent<RectTransform>().DOKill();
            lastMousedOverSlot.transform.GetChild(1).GetComponent<RectTransform>().DOKill();
            lastMousedOverSlot.transform.GetChild(0).GetComponent<RectTransform>().DOScale(0.5f, animTime);
            lastMousedOverSlot.transform.GetChild(1).GetComponent<RectTransform>().DOScale(0.55f, animTime);
        }
        if(Input.GetMouseButtonDown(0) && getMousedOverSlot() != null) {
            selectedIndex = getIndexForSlot(getMousedOverSlot());
            if(Party.getHolder().getObject<UnitStats>(selectedIndex) == null || Party.getHolder().getObject<UnitStats>(selectedIndex).isEmpty())
                selectedIndex = -1;
            updateInfo();
        }
    }

    public GameObject getMousedOverSlot() {
        foreach(var i in slots.GetComponentsInChildren<Button>()) {
            if(i.gameObject == EventSystem.current.currentSelectedGameObject)
                return i.gameObject;
        }
        return null;
    }
    public int getIndexForSlot(GameObject slot) {
        for(int i = 0; i < slots.transform.childCount; i++) {
            if(slot == slots.transform.GetChild(i).gameObject)
                return i;
        }
        return -1;
    }

    public UnitStats getSelectedUnit() {
        return Party.getHolder().getObject<UnitStats>(selectedIndex);
    }

    public void updateInfo() {
        //  slots
        for(int i = 0; i < slots.transform.childCount; i++) {
            slots.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = Color.white;
            slots.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            slots.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }
        for(int i = 0; i < Party.getHolder().getObjectCount<UnitStats>(); i++) {
            slots.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            slots.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnitHeadSprite(Party.getHolder().getObject<UnitStats>(i).u_sprite.headIndex);
            slots.transform.GetChild(i).GetChild(0).GetComponent<Image>().SetNativeSize();
            slots.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
            slots.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnitFace(Party.getHolder().getObject<UnitStats>(i).u_sprite.faceIndex);
            slots.transform.GetChild(i).GetChild(1).GetComponent<Image>().SetNativeSize();
            slots.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = Party.getHolder().getObject<UnitStats>(i).u_sprite.color;
        }

        //  main slot
        mainSlot.transform.GetChild(0).gameObject.SetActive(selectedIndex > -1);
        mainSlot.transform.GetChild(1).gameObject.SetActive(selectedIndex > -1);
        if(selectedIndex > -1) {
            mainSlot.transform.GetChild(0).GetComponent<Image>().color = Party.getHolder().getObject<UnitStats>(selectedIndex).u_sprite.color;
            mainSlot.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnitHeadSprite(Party.getHolder().getObject<UnitStats>(selectedIndex).u_sprite.headIndex);
            mainSlot.transform.GetChild(0).GetComponent<Image>().SetNativeSize();
            mainSlot.transform.GetChild(1).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnitFace(Party.getHolder().getObject<UnitStats>(selectedIndex).u_sprite.faceIndex);
            mainSlot.transform.GetChild(1).GetComponent<Image>().SetNativeSize();
        }

        updateCustomInfo();
    }

    public abstract void updateCustomInfo();

    public void exit() {
        FindObjectOfType<LocationMovement>().deinteract();
    }

    public void showCanvas() {
        canvas.SetActive(true);
        canvas.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        canvas.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), showTime);
        updateInfo();
        updateCustomInfo();
    }
    public void hideCanvas() {
        StartCoroutine(hider());
    }
    public IEnumerator hider() {
        canvas.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), showTime);
        yield return new WaitForSeconds(showTime);
        canvas.SetActive(false);
    }
}

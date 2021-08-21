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

    protected int selectedIndex = -1;

    float showTime = 0.15f;

    private void Start() {
        DOTween.Init();
        updateInfo();
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0) && getMousedOverSlot() != null) {
            selectedIndex = getIndexForSlot(getMousedOverSlot());
            if(Party.getMemberStats(selectedIndex) == null || Party.getMemberStats(selectedIndex).isEmpty())
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
        return Party.getMemberStats(selectedIndex);
    }

    public void updateInfo() {
        //  slots
        for(int i = 0; i < slots.transform.childCount; i++)
            slots.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        for(int i = 0; i < Party.getMemberCount(); i++) {
            slots.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            slots.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = Party.getMemberStats(i).u_sprite.color;
        }

        //  main slot
        mainSlot.transform.GetChild(0).gameObject.SetActive(false);
        if(selectedIndex > -1) {
            mainSlot.transform.GetChild(0).gameObject.SetActive(true);
            mainSlot.transform.GetChild(0).GetComponent<Image>().color = Party.getMemberStats(selectedIndex).u_sprite.color;
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

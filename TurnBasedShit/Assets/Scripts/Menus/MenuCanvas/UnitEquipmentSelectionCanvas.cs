using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public abstract class UnitEquipmentSelectionCanvas : MonoBehaviour {
    public abstract void rotateEquipment();
    public abstract void updateInfo();
    public abstract void populateSlots();
    public abstract int getState();
    public abstract void removeUnitEquipmentAndResetSelection();

    public Image shownSprite;
    public SlotMenu slot;

    bool shouldShowState = false;
    bool mouseOverX = false;

    public bool shown = false;

    private void Awake() {
        GetComponentInParent<InfoBearer>().runOnMouseOver(animateMouseOverSlot);
        GetComponentInParent<InfoBearer>().runOnMouseExit(animateMouseExitSlot);
    }

    private void Update() {
        if(slot.run())
            updateInfo();
        if(Input.GetMouseButtonDown(0) && !mouseOverX) {
            if(shouldShowState) {
                startShowing();
            }
            else
                startHiding();
        }
    }


    public void startShowing() {
        transform.parent.GetComponent<BoxCollider2D>().enabled = false;
        populateSlots();
        updateInfo();
        StartCoroutine(show());
    }

    public void startHiding() {
        transform.parent.GetComponent<BoxCollider2D>().enabled = true;
        if(slot.getSelectedSlotIndex() != -1)
            rotateEquipment();
        StartCoroutine(hide());
    }
    public void setMouseOverX(bool b) {
        mouseOverX = b;
    }
    public void setShownState(bool b) {
        shouldShowState = b;
    }

    public void animateMouseOverSlot() {
        transform.parent.DOScale(0.55f, 0.15f);
        shownSprite.transform.DOScale(0.85f, 0.15f);
    }
    public void animateMouseExitSlot() {
        transform.parent.DOScale(0.5f, 0.25f);
        shownSprite.transform.DOScale(0.75f, 0.25f);
    }

    IEnumerator show() {
        shown = true;

        //  close the image that shows the units current equippment
        if(getState() == 0)
            FindObjectOfType<UnitCanvas>().weaponImage.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime);
        if(getState() == 1)
            FindObjectOfType<UnitCanvas>().armorImage.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime);
        if(getState() == 2)
            FindObjectOfType<UnitCanvas>().itemImage.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime);


        yield return new WaitForSeconds(FindObjectOfType<UnitCanvas>().equippmentTransitionTime);

        yield return new WaitForSeconds(FindObjectOfType<UnitCanvas>().timeBtwTransition);

        transform.parent.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime / 2.0f);

        yield return new WaitForSeconds(FindObjectOfType<UnitCanvas>().equippmentTransitionTime / 2.0f);

        transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime / 2.0f);
    }

    IEnumerator hide() {
        if(!shown)
            yield return 0;

        shown = false;
        transform.parent.transform.DOScale(new Vector3(0.5f, 0.5f, 1.0f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime / 2.0f);

        yield return new WaitForSeconds(FindObjectOfType<UnitCanvas>().equippmentTransitionTime / 2.0f);

        transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime / 2.0f);

        yield return new WaitForSeconds(FindObjectOfType<UnitCanvas>().equippmentTransitionTime / 2.0f);

        yield return new WaitForSeconds(FindObjectOfType<UnitCanvas>().timeBtwTransition);

        if(getState() == 0)
            FindObjectOfType<UnitCanvas>().weaponImage.transform.DOScale(new Vector3(0.75f, 0.75f, 0.75f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime);
        if(getState() == 1)
            FindObjectOfType<UnitCanvas>().armorImage.transform.DOScale(new Vector3(0.75f, 0.75f, 0.75f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime);
        if(getState() == 2)
            FindObjectOfType<UnitCanvas>().itemImage.transform.DOScale(new Vector3(0.75f, 0.75f, 0.75f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime);

        yield return new WaitForSeconds(FindObjectOfType<UnitCanvas>().equippmentTransitionTime);
        populateSlots();
    }
}

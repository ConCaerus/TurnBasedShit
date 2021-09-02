using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;


public abstract class SelectionCanvas : MonoBehaviour {
    public Image shownImage;
    public TextMeshProUGUI nameText, firstText, secondText, firstAttributeText, secondAttributeText;
    public GameObject slotHolder;
    protected int selectedIndex = -1;

    protected bool shown = false;


    public GameObject getMousedOverSlot() {
        foreach(var i in slotHolder.GetComponentsInChildren<Button>()) {
            if(i.gameObject == EventSystem.current.currentSelectedGameObject)
                return i.gameObject;
        }
        return null;
    }


    private void Start() {
        populateSlots();
    }



    private void Update() {
        //  update info
        if(Input.GetMouseButtonDown(0) && getMousedOverSlot() != null) {
            selectedIndex = getIndexForSlot(getMousedOverSlot());
            updateInfo();
        }
        //  close window
        else if(Input.GetMouseButtonDown(0) && getMousedOverSlot() == null && shown) {
            if(selectedIndex != -1)
                rotateEquipment();
            StartCoroutine(hide());
        }
    }

    public void populateSlots() {
        int slotIndex = 0;
        selectedIndex = -1;
        foreach(var i in slotHolder.GetComponentsInChildren<Button>()) {
            i.transform.GetChild(0).GetComponent<Image>().enabled = false;
            i.GetComponent<Image>().color = Color.gray;
        }
        for(int i = -1; i < slotHolder.transform.childCount; i++) {
            switch(getState()) {
                case 0:
                    //  adds the unit's own equipment into the slots
                    if(i == -1) {
                        if(FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon == null || FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon.isEmpty())
                            continue;
                        slotHolder.transform.GetChild(slotIndex).GetChild(0).GetComponent<Image>().enabled = true;
                        slotHolder.transform.GetChild(slotIndex).GetComponent<Image>().color = FindObjectOfType<UnitCanvas>().getRarityColor(FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon.w_rarity) / 1.25f;
                        slotHolder.transform.GetChild(slotIndex).GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon).sprite;
                        slotIndex++;
                    }
                    else if(Inventory.getWeapon(i) != null && !Inventory.getWeapon(i).isEmpty()) {
                        slotHolder.transform.GetChild(slotIndex).GetChild(0).GetComponent<Image>().enabled = true;
                        slotHolder.transform.GetChild(slotIndex).GetComponent<Image>().color = FindObjectOfType<UnitCanvas>().getRarityColor(Inventory.getWeapon(i).w_rarity) / 1.25f;
                        slotHolder.transform.GetChild(slotIndex).GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(Inventory.getWeapon(i)).sprite;
                        slotIndex++;
                    }
                    break;

                case 1:
                    if(i == -1) {
                        if(FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor == null || FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor.isEmpty())
                            continue;
                        slotHolder.transform.GetChild(slotIndex).GetChild(0).GetComponent<Image>().enabled = true;
                        slotHolder.transform.GetChild(slotIndex).GetComponent<Image>().color = FindObjectOfType<UnitCanvas>().getRarityColor(FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor.a_rarity) / 1.25f;
                        slotHolder.transform.GetChild(slotIndex).GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor).sprite;
                        slotIndex++;
                    }
                    else if(Inventory.getArmor(i) != null && !Inventory.getArmor(i).isEmpty()) {
                        slotHolder.transform.GetChild(slotIndex).GetChild(0).GetComponent<Image>().enabled = true;
                        slotHolder.transform.GetChild(slotIndex).GetComponent<Image>().color = FindObjectOfType<UnitCanvas>().getRarityColor(Inventory.getArmor(i).a_rarity) / 1.25f;
                        slotHolder.transform.GetChild(slotIndex).GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(Inventory.getArmor(i)).sprite;
                        slotIndex++;
                    }
                    break;

                case 2:
                    if(i == -1) {
                        if(FindObjectOfType<UnitCanvas>().shownUnit.equippedItem == null || FindObjectOfType<UnitCanvas>().shownUnit.equippedItem.isEmpty())
                            continue;
                        slotHolder.transform.GetChild(slotIndex).GetChild(0).GetComponent<Image>().enabled = true;
                        slotHolder.transform.GetChild(slotIndex).GetComponent<Image>().color = FindObjectOfType<UnitCanvas>().getRarityColor(FindObjectOfType<UnitCanvas>().shownUnit.equippedItem.i_rarity) / 1.25f;
                        slotHolder.transform.GetChild(slotIndex).GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getItemSprite(FindObjectOfType<UnitCanvas>().shownUnit.equippedItem).sprite;
                        slotIndex++;
                    }
                    else if(Inventory.getItem(i) != null && !Inventory.getItem(i).isEmpty()) {
                        slotHolder.transform.GetChild(slotIndex).GetChild(0).GetComponent<Image>().enabled = true;
                        slotHolder.transform.GetChild(slotIndex).GetComponent<Image>().color = FindObjectOfType<UnitCanvas>().getRarityColor(Inventory.getItem(i).i_rarity) / 1.25f;
                        slotHolder.transform.GetChild(slotIndex).GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getItemSprite(Inventory.getItem(i)).sprite;
                        slotIndex++;
                    }
                    break;
            }
        }
    }

    public abstract void rotateEquipment();
    public abstract void updateInfo();

    //   0 - weapon, 1 - armor, 2 - item
    public abstract int getState();


    public int getIndexForSlot(GameObject slot) {
        for(int i = 0; i < slotHolder.transform.childCount; i++) {
            if(slot == slotHolder.transform.GetChild(i).gameObject)
                return i;
        }
        return -1;
    }


    public void startShowing() {
        selectedIndex = -1;
        updateInfo();
        StartCoroutine(show());
    }

    public IEnumerator show() {
        populateSlots();

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
        shown = true;
    }

    public IEnumerator hide() {
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

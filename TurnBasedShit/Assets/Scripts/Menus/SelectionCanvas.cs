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
        }
        if(Input.GetMouseButtonDown(0) && getMousedOverSlot() != null && isSlotPopulated(selectedIndex)) {
            updateInfo();
        }
        //  close window
        else if(Input.GetMouseButtonDown(0) && getMousedOverSlot() == null && shown) {
            if(isSlotPopulated(selectedIndex)) {
                rotateEquipment();
            }
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
        for(int i = 0; i < slotHolder.transform.childCount; i++) {

            switch(getState()) {
                case 0:
                    if(Inventory.getWeapon(i) != null && !Inventory.getWeapon(i).isEmpty()) {
                        slotHolder.transform.GetChild(slotIndex).GetChild(0).GetComponent<Image>().enabled = true;
                        slotHolder.transform.GetChild(slotIndex).GetComponent<Image>().color = FindObjectOfType<UnitCanvas>().getRarityColor(Inventory.getWeapon(i).w_rarity) / 1.25f;
                        slotHolder.transform.GetChild(slotIndex).GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(Inventory.getWeapon(i)).sprite;
                        slotIndex++;
                    }
                    break;

                case 1:
                    if(Inventory.getArmor(i) != null && !Inventory.getArmor(i).isEmpty()) {
                        slotHolder.transform.GetChild(slotIndex).GetChild(0).GetComponent<Image>().enabled = true;
                        slotHolder.transform.GetChild(slotIndex).GetComponent<Image>().color = FindObjectOfType<UnitCanvas>().getRarityColor(Inventory.getArmor(i).a_rarity) / 1.25f;
                        slotHolder.transform.GetChild(slotIndex).GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(Inventory.getArmor(i)).sprite;
                        slotIndex++;
                    }
                    break;

                case 2:
                    if(Inventory.getItem(i) != null && !Inventory.getItem(i).isEmpty()) {
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
    public abstract bool isSlotPopulated(int index);

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
        StartCoroutine(show());
    }

    public IEnumerator show() {
        populateSlots();
        if(getState() == 0)
            FindObjectOfType<UnitCanvas>().weaponImage.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime);
        if(getState() == 1)
            FindObjectOfType<UnitCanvas>().armorImage.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime);
        if(getState() == 2)
            FindObjectOfType<UnitCanvas>().itemImage.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime);
        yield return new WaitForSeconds(FindObjectOfType<UnitCanvas>().equippmentTransitionTime);

        yield return new WaitForSeconds(FindObjectOfType<UnitCanvas>().timeBtwTransition);

        transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime);

        yield return new WaitForSeconds(FindObjectOfType<UnitCanvas>().equippmentTransitionTime);
        shown = true;
    }

    public IEnumerator hide() {
        if(!shown)
            yield return 0;

        shown = false;
        transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime);
        yield return new WaitForSeconds(FindObjectOfType<UnitCanvas>().equippmentTransitionTime);

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

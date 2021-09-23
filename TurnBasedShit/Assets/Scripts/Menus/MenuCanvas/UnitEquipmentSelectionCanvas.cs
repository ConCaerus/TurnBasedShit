﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public abstract class UnitEquipmentSelectionCanvas : SelectionCanvas {
    public Image shownImage;
    public TextMeshProUGUI nameText, firstText, secondText, firstAttributeText, secondAttributeText;

    public abstract void rotateEquipment();
    public abstract void updateInfo();


    public override void populateSlots() {
        selectedIndex = -1;

        int slotIndex = 0;

        switch(getState()) {
            case 0:
                for(int i = -1; i < Inventory.getWeaponCount(); i++) {
                    //  adds the unit's own equipment into the slots
                    if(i == -1) {
                        if(FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon == null || FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon.isEmpty())
                            continue;

                        var obj = createSlot(slotIndex);
                        obj.transform.GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon.w_rarity);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon).sprite;
                        obj.transform.GetChild(1).gameObject.SetActive(false);
                        slotIndex++;
                    }

                    else if(Inventory.getWeapon(i) != null && !Inventory.getWeapon(i).isEmpty()) {
                        var obj = createSlot(slotIndex);
                        obj.transform.GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getWeapon(i).w_rarity);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(Inventory.getWeapon(i)).sprite;
                        obj.transform.GetChild(1).gameObject.SetActive(false);
                        slotIndex++;
                    }
                }
                break;

            case 1:
                for(int i = -1; i < Inventory.getArmorCount(); i++) {
                    if(i == -1) {
                        if(FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor == null || FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor.isEmpty())
                            continue;

                        var obj = createSlot(slotIndex);
                        obj.transform.GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor.a_rarity);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor).sprite;
                        obj.transform.GetChild(1).gameObject.SetActive(false);
                        slotIndex++;
                    }

                    else if(Inventory.getArmor(i) != null && !Inventory.getArmor(i).isEmpty()) {
                        var obj = createSlot(slotIndex);
                        obj.transform.GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getArmor(i).a_rarity);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(Inventory.getArmor(i)).sprite;
                        obj.transform.GetChild(1).gameObject.SetActive(false);
                        slotIndex++;
                    }
                }
                break;

            case 2:
                for(int i = 0; i < Inventory.getItemCount(); i++) {
                    if(i == -1) {
                        if(FindObjectOfType<UnitCanvas>().shownUnit.equippedItem == null || FindObjectOfType<UnitCanvas>().shownUnit.equippedItem.isEmpty())
                            continue;

                        var obj = createSlot(slotIndex);
                        obj.transform.GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(FindObjectOfType<UnitCanvas>().shownUnit.equippedItem.i_rarity);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getItemSprite(FindObjectOfType<UnitCanvas>().shownUnit.equippedItem).sprite;
                        obj.transform.GetChild(1).gameObject.SetActive(false);
                        slotIndex++;
                    }

                    else if(Inventory.getItem(i) != null && !Inventory.getItem(i).isEmpty()) {
                        var obj = createSlot(slotIndex);
                        obj.transform.GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getItem(i).i_rarity);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getItemSprite(Inventory.getItem(i)).sprite;
                        obj.transform.GetChild(1).gameObject.SetActive(false);
                        slotIndex++;
                    }
                }
                break;
        }

        /*
        //  destroy unused slots
        var temp = new List<GameObject>();
        for(int i = slotIndex; i < slotHolder.transform.childCount; i++)
            temp.Add(slotHolder.transform.GetChild(i).gameObject);
        foreach(var i in temp)
            Destroy(i.gameObject); */
    }


    GameObject createSlot(int index) {
        var slotScale = slotPreset.GetComponent<RectTransform>().rect.size.x;
        var startingPos = new Vector2(slotHolder.GetComponent<RectTransform>().rect.xMin + (slotScale / 2.0f), slotHolder.GetComponent<RectTransform>().rect.yMax - (slotScale / 2.0f));

        GameObject obj = null;
        if(slotHolder.transform.childCount > index && slotHolder.transform.GetChild(index) != null)
            obj = slotHolder.transform.GetChild(index).gameObject;
        else
            obj = Instantiate(slotPreset.gameObject, slotHolder.transform);

            int y = Mathf.FloorToInt(index / numOfSlotsPerLine);
        int x = index - (y * numOfSlotsPerLine);

        obj.transform.GetComponent<RectTransform>().localPosition = startingPos + new Vector2((slotScale + slotBuffer) * x, -((slotScale + slotBuffer) * y));
        obj.GetComponent<Button>().onClick.AddListener(delegate { setSelectedIndex(index); });
        return obj;
    }

    public void setSelectedIndex(int index) {
        selectedIndex = index;
        updateInfo();
    }

    public abstract void removeUnitEquipmentAndResetSelection();

    public void startShowing() {
        selectedIndex = -1;
        updateInfo();
        StartCoroutine(show());
    }

    public void startHiding() {
            if(selectedIndex != -1)
                rotateEquipment();
            StartCoroutine(hide());
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

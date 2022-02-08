using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public abstract class UnitEquipmentSelectionCanvas : MonoBehaviour {
    public abstract void updateInfo();

    public Image shownSprite;
    public SlotMenu slot;

    bool mouseOverX = false;
    protected float populatingWaitTime = 0.035f;

    public bool shown = false;
    bool mouseOver = false;

    private void Awake() {
        GetComponentInParent<InfoBearer>().runOnMouseOver(animateMouseOverSlot);
        GetComponentInParent<InfoBearer>().runOnMouseExit(animateMouseExitSlot);
        slot.setSelectedSlotIndex(0);
    }

    private void Update() {
        if(slot.run() && shown)
            updateInfo();
        if(Input.GetMouseButtonDown(0) && !mouseOverX) {
            if(mouseOver && !shown)
                startShowing();
            else if(shown && !mouseOver)
                startHiding();
        }
    }


    public IEnumerator populateSlots() {
        int slotIndex = 0;

        switch(getState()) {
            case 0:
                if(FindObjectOfType<UnitCanvas>().shownUnit.weapon != null && !FindObjectOfType<UnitCanvas>().shownUnit.weapon.isEmpty()) {
                    createSlot(slotIndex, FindObjectOfType<UnitCanvas>().shownUnit.weapon, true);
                    slotIndex++;
                    yield return new WaitForSeconds(populatingWaitTime);
                }

                for(int i = 0; i < Inventory.getHolder().getObjectCount<Weapon>(); i++) {
                    if(Inventory.getHolder().getObject<Weapon>(i) == null || Inventory.getHolder().getObject<Weapon>(i).isEmpty())
                        continue;
                    createSlot(slotIndex, Inventory.getHolder().getObject<Weapon>(i), false);
                    slotIndex++;
                    yield return new WaitForSeconds(populatingWaitTime);
                }
                break;

            case 1:
                if(FindObjectOfType<UnitCanvas>().shownUnit.armor != null && !FindObjectOfType<UnitCanvas>().shownUnit.armor.isEmpty()) {
                    createSlot(slotIndex, FindObjectOfType<UnitCanvas>().shownUnit.armor, true);
                    slotIndex++;
                    yield return new WaitForSeconds(populatingWaitTime);
                }

                for(int i = 0; i < Inventory.getHolder().getObjectCount<Armor>(); i++) {
                    if(Inventory.getHolder().getObject<Armor>(i) == null || Inventory.getHolder().getObject<Armor>(i).isEmpty())
                        continue;
                    createSlot(slotIndex, Inventory.getHolder().getObject<Armor>(i), false);
                    slotIndex++;
                    yield return new WaitForSeconds(populatingWaitTime);
                }
                break;

            case 2:
                if(FindObjectOfType<UnitCanvas>().shownUnit.item != null && !FindObjectOfType<UnitCanvas>().shownUnit.item.isEmpty()) {
                    createSlot(slotIndex, FindObjectOfType<UnitCanvas>().shownUnit.item, true);
                    slotIndex++;
                    yield return new WaitForSeconds(populatingWaitTime);
                }

                for(int i = 0; i < Inventory.getHolder().getObjectCount<Item>(); i++) {
                    if(Inventory.getHolder().getObject<Item>(i) == null || Inventory.getHolder().getObject<Item>(i).isEmpty())
                        continue;
                    createSlot(slotIndex, Inventory.getHolder().getObject<Item>(i), false);
                    slotIndex++;
                    yield return new WaitForSeconds(populatingWaitTime);
                }
                break;
        }

        slot.deleteSlotsAfterIndex(slotIndex);
    }

    public GameObject createSlot(int slotIndex, Collectable col, bool unitCol) {
        var obj = slot.createSlot(slotIndex, FindObjectOfType<PresetLibrary>().getRarityColor(col.rarity), InfoTextCreator.createForCollectable(col));
        if(unitCol)
            obj.GetComponent<SlotObject>().setImageColor(0, FindObjectOfType<UnitCanvas>().shownUnit.u_sprite.color);
        obj.GetComponent<SlotObject>().setInfo(col.name);
        obj.GetComponent<SlotObject>().setImage(1, FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(col));

        float prevScale = obj.transform.localScale.x;
        obj.transform.localScale = Vector3.zero;
        obj.transform.DOScale(prevScale, populatingWaitTime);
        return obj;
    }

    public Collectable getCollectableInSlot(int index) {
        if(index == -1)
            return null;

        switch(getState()) {
            case 0:
                if(index >= 0) {
                    if(FindObjectOfType<UnitCanvas>().shownUnit.weapon == null || FindObjectOfType<UnitCanvas>().shownUnit.weapon.isEmpty())
                        return Inventory.getHolder().getObject<Weapon>(index);
                    return Inventory.getHolder().getObject<Weapon>(index - 1);
                }
                else if(FindObjectOfType<UnitCanvas>().shownUnit.weapon != null && !FindObjectOfType<UnitCanvas>().shownUnit.weapon.isEmpty())
                    return FindObjectOfType<UnitCanvas>().shownUnit.weapon;
                break;

            case 1:
                if(index >= 0) {
                    if(FindObjectOfType<UnitCanvas>().shownUnit.armor == null || FindObjectOfType<UnitCanvas>().shownUnit.armor.isEmpty())
                        return Inventory.getHolder().getObject<Armor>(index);
                    return Inventory.getHolder().getObject<Armor>(index - 1);
                }
                else if(FindObjectOfType<UnitCanvas>().shownUnit.armor != null && !FindObjectOfType<UnitCanvas>().shownUnit.armor.isEmpty())
                    return FindObjectOfType<UnitCanvas>().shownUnit.armor;
                break;

            case 2:
                if(index >= 0) {
                    if(FindObjectOfType<UnitCanvas>().shownUnit.item == null || FindObjectOfType<UnitCanvas>().shownUnit.item.isEmpty())
                        return Inventory.getHolder().getObject<Item>(index);
                    return Inventory.getHolder().getObject<Item>(index - 1);
                }
                else if(FindObjectOfType<UnitCanvas>().shownUnit.item != null && !FindObjectOfType<UnitCanvas>().shownUnit.item.isEmpty())
                    return FindObjectOfType<UnitCanvas>().shownUnit.item;
                break;
        }
        return null;
    }


    public void startShowing() {
        transform.parent.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(show());
    }

    public void startHiding() {
        transform.parent.GetComponent<BoxCollider2D>().enabled = true;
        if(slot.getSelectedSlotIndex() != -1) {
            rotateEquipment();
        }
        StartCoroutine(hide());
    }
    public void setMouseOverX(bool b) {
        mouseOverX = b;
    }
    public void setMouseOver(bool b) {
        mouseOver = b;
    }

    public void animateMouseOverSlot() {
        transform.parent.DOScale(0.55f, 0.15f);
        shownSprite.transform.DOScale(0.85f, 0.15f);

        GameObject l = FindObjectOfType<UnitCanvas>().lockImages[getState()];

        l.transform.DOScale(0.45f, .15f);
    }
    public void animateMouseExitSlot() {
        transform.parent.DOScale(0.5f, 0.25f);
        shownSprite.transform.DOScale(0.75f, 0.25f);

        GameObject l = FindObjectOfType<UnitCanvas>().lockImages[getState()];

        l.transform.DOScale(0.35f, .25f);
    }

    IEnumerator show() {
        slot.setSelectedSlotIndex(0);

        //  close the image that shows the units current equippment
        if(getState() == 0)
            FindObjectOfType<UnitCanvas>().weaponImage.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime);
        else if(getState() == 1)
            FindObjectOfType<UnitCanvas>().armorImage.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime);
        else if(getState() == 2)
            FindObjectOfType<UnitCanvas>().itemImage.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime);


        yield return new WaitForSeconds(FindObjectOfType<UnitCanvas>().equippmentTransitionTime);

        yield return new WaitForSeconds(FindObjectOfType<UnitCanvas>().timeBtwTransition);

        transform.parent.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime / 2.0f);

        yield return new WaitForSeconds(FindObjectOfType<UnitCanvas>().equippmentTransitionTime / 2.0f);

        transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime / 2.0f);

        yield return new WaitForSeconds(FindObjectOfType<UnitCanvas>().equippmentTransitionTime / 2.0f);

        StartCoroutine(populateSlots());
        shown = true;
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
        else if(getState() == 1)
            FindObjectOfType<UnitCanvas>().armorImage.transform.DOScale(new Vector3(0.75f, 0.75f, 0.75f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime);
        else if(getState() == 2)
            FindObjectOfType<UnitCanvas>().itemImage.transform.DOScale(new Vector3(0.75f, 0.75f, 0.75f), FindObjectOfType<UnitCanvas>().equippmentTransitionTime);

        yield return new WaitForSeconds(FindObjectOfType<UnitCanvas>().equippmentTransitionTime);
        slot.destroySlots();
    }


    public void rotateEquipment() {
        Collectable col = getCollectableInSlot(slot.getSelectedSlotIndex());
        if(col == null || col.isEmpty())
            return;

        updateInfo();

        switch(getState()) {
            case 0:
                if(FindObjectOfType<UnitCanvas>().shownUnit.weapon != null && !FindObjectOfType<UnitCanvas>().shownUnit.weapon.isEmpty())
                    Inventory.addSingleCollectable(FindObjectOfType<UnitCanvas>().shownUnit.weapon, FindObjectOfType<PresetLibrary>(), FindObjectOfType<FullInventoryCanvas>());
                break;

            case 1:
                if(FindObjectOfType<UnitCanvas>().shownUnit.armor != null && !FindObjectOfType<UnitCanvas>().shownUnit.armor.isEmpty())
                    Inventory.addSingleCollectable(FindObjectOfType<UnitCanvas>().shownUnit.armor, FindObjectOfType<PresetLibrary>(), FindObjectOfType<FullInventoryCanvas>());
                break;

            case 2:
                if(FindObjectOfType<UnitCanvas>().shownUnit.item != null && !FindObjectOfType<UnitCanvas>().shownUnit.item.isEmpty())
                    Inventory.addSingleCollectable(FindObjectOfType<UnitCanvas>().shownUnit.item, FindObjectOfType<PresetLibrary>(), FindObjectOfType<FullInventoryCanvas>());
                break;
        }

        FindObjectOfType<UnitCanvas>().setUnitEquipment(col);
        Inventory.removeCollectable(col);
    }

    public void removeUnitEquipmentAndResetSelection() {
        if(shown) {
            updateInfo();
        }

        switch(getState()) {
            case 0:
                var unitWeapon = FindObjectOfType<UnitCanvas>().shownUnit.weapon;
                if(unitWeapon == null || unitWeapon.isEmpty())
                    return;

                Inventory.addSingleCollectable(unitWeapon, FindObjectOfType<PresetLibrary>(), FindObjectOfType<FullInventoryCanvas>());
                FindObjectOfType<UnitCanvas>().setUnitWeapon(null);
                break;

            case 1:
                var unitArmor = FindObjectOfType<UnitCanvas>().shownUnit.armor;
                if(unitArmor == null || unitArmor.isEmpty())
                    return;

                Inventory.addSingleCollectable(unitArmor, FindObjectOfType<PresetLibrary>(), FindObjectOfType<FullInventoryCanvas>());
                FindObjectOfType<UnitCanvas>().setUnitArmor(null);
                break;

            case 2:
                var unitItem = FindObjectOfType<UnitCanvas>().shownUnit.item;
                if(unitItem == null || unitItem.isEmpty())
                    return;

                Inventory.addSingleCollectable(unitItem, FindObjectOfType<PresetLibrary>(), FindObjectOfType<FullInventoryCanvas>());
                FindObjectOfType<UnitCanvas>().setUnitItem(null);
                break;
        }
    }

    public abstract int getState();
}

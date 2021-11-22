using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;


public class ItemSelectionCanvas : UnitEquipmentSelectionCanvas {
    [SerializeField] TextMeshProUGUI nameText;

    public Item getItemInSlot(int index) {
        if(FindObjectOfType<UnitCanvas>().shownUnit.item == null || FindObjectOfType<UnitCanvas>().shownUnit.item.isEmpty())
            return Inventory.getItem(index);
        else if(index > 0)
            return Inventory.getItem(index - 1);
        return FindObjectOfType<UnitCanvas>().shownUnit.item;
    }


    public override void populateSlots() {
        int slotIndex = 0;
        if(FindObjectOfType<UnitCanvas>().shownUnit.item != null && !FindObjectOfType<UnitCanvas>().shownUnit.item.isEmpty()) {
            var obj = slot.createSlot(slotIndex, FindObjectOfType<UnitCanvas>().shownUnit.u_sprite.color, InfoTextCreator.createForCollectable(FindObjectOfType<UnitCanvas>().shownUnit.item));
            obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getItemSprite(FindObjectOfType<UnitCanvas>().shownUnit.item).sprite;
            slotIndex++;
        }

        for(int i = 0; i < Inventory.getItemCount(); i++) {
            if(Inventory.getItem(i) == null || Inventory.getItem(i).isEmpty())
                continue;
            var obj = slot.createSlot(slotIndex, FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getItem(i).rarity), InfoTextCreator.createForCollectable(Inventory.getItem(i)));
            obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getItemSprite(Inventory.getItem(i)).sprite;
            slotIndex++;
        }

        slot.deleteSlotsAfterIndex(slotIndex);
    }

    public override void updateInfo() {
        //                          Item shit
        if(getItemInSlot(slot.getSelectedSlotIndex()) == null || getItemInSlot(slot.getSelectedSlotIndex()).isEmpty()) {
            shownSprite.sprite = null;
            shownSprite.transform.parent.GetChild(0).GetComponent<Image>().color = Color.gray;

            nameText.text = "";
            return;
        }
        var shownItem = getItemInSlot(slot.getSelectedSlotIndex());
        shownSprite.sprite = FindObjectOfType<PresetLibrary>().getItemSprite(shownItem).sprite;
        shownSprite.transform.parent.GetChild(0).GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(shownItem.rarity);


        //  stats
        nameText.text = shownItem.name;
    }

    public override void rotateEquipment() {
        if(FindObjectOfType<UnitCanvas>().shownUnit.item != null && !FindObjectOfType<UnitCanvas>().shownUnit.item.isEmpty()) {
            var slottedItem = getItemInSlot(slot.getSelectedSlotIndex());
            Inventory.addItem(FindObjectOfType<UnitCanvas>().shownUnit.item);
            FindObjectOfType<UnitCanvas>().setUnitItem(slottedItem);
            Inventory.removeItem(slottedItem);
        }

        //  unit just takes
        else {
            var slottedItem = getItemInSlot(slot.getSelectedSlotIndex());
            FindObjectOfType<UnitCanvas>().setUnitItem(slottedItem);

            Inventory.removeItem(slottedItem);
        }
    }


    public override void removeUnitEquipmentAndResetSelection() {
        if(shown) {
            updateInfo();
        }
        var unitItem = FindObjectOfType<UnitCanvas>().shownUnit.item;
        if(unitItem == null || unitItem.isEmpty())
            return;

        Inventory.addItem(unitItem);
        FindObjectOfType<UnitCanvas>().setUnitItem(null);
    }

    public override int getState() {
        return 2;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;


public class ItemSelectionCanvas : SelectionCanvas {

    public Item getItemInSlot(int wIndex) {
        int slotIndex = -1;
        for(int i = 0; i < slotHolder.transform.childCount; i++) {
            if(Inventory.getItem(i) != null && !Inventory.getItem(i).isEmpty()) {
                slotIndex++;
                if(slotIndex == wIndex)
                    return Inventory.getItem(i);
            }
        }

        return null;
    }

    public override void updateInfo() {
        //                          Item shit
        if(selectedIndex < 0)
            selectedIndex = 0;
        var shownItem = getItemInSlot(selectedIndex);
        shownImage.sprite = FindObjectOfType<PresetLibrary>().getItemSprite(shownItem).sprite;
        shownImage.transform.parent.GetChild(0).GetComponent<Image>().color = FindObjectOfType<UnitCanvas>().getRarityColor(shownItem.i_rarity);
        //  elementImage shit here

        //  stats
        nameText.text = shownItem.i_name;
    }

    public override void rotateEquipment() {
        if(FindObjectOfType<UnitCanvas>().shownUnit.equippedItem != null && !FindObjectOfType<UnitCanvas>().shownUnit.equippedItem.isEmpty()) {
            Inventory.addItem(FindObjectOfType<UnitCanvas>().shownUnit.equippedItem);
            FindObjectOfType<UnitCanvas>().setUnitItem(getItemInSlot(selectedIndex));

            Inventory.removeItem(getItemInSlot(selectedIndex));
        }

        //  unit just takes
        else {
            FindObjectOfType<UnitCanvas>().setUnitItem(getItemInSlot(selectedIndex));

            Inventory.removeItem(getItemInSlot(selectedIndex));
        }
        selectedIndex = -1;
    }

    public override bool isSlotPopulated(int index) {
        return getItemInSlot(selectedIndex) != null;
    }

    public override int getState() {
        return 2;
    }
}

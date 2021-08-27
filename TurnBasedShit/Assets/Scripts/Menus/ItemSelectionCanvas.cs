using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;


public class ItemSelectionCanvas : SelectionCanvas {

    public Item getItemInSlot(int index) {
        if(index == 0) {
            if(FindObjectOfType<UnitCanvas>().shownUnit.equippedItem == null || FindObjectOfType<UnitCanvas>().shownUnit.equippedItem.isEmpty())
                return Inventory.getItem(0);
            else
                return FindObjectOfType<UnitCanvas>().shownUnit.equippedItem;
        }
        else {
            if(FindObjectOfType<UnitCanvas>().shownUnit.equippedItem == null || FindObjectOfType<UnitCanvas>().shownUnit.equippedItem.isEmpty())
                return Inventory.getItem(index);
            else
                return Inventory.getItem(index - 1);
        }
    }

    public override void updateInfo() {
        //                          Item shit
        if(getItemInSlot(selectedIndex) == null || getItemInSlot(selectedIndex).isEmpty()) {
            shownImage.sprite = null;
            shownImage.transform.parent.GetChild(0).GetComponent<Image>().color = Color.gray;

            nameText.text = "";
            return;
        }
        var shownItem = getItemInSlot(selectedIndex);
        shownImage.sprite = FindObjectOfType<PresetLibrary>().getItemSprite(shownItem).sprite;
        shownImage.transform.parent.GetChild(0).GetComponent<Image>().color = FindObjectOfType<UnitCanvas>().getRarityColor(shownItem.i_rarity);
        //  elementImage shit here

        //  stats
        nameText.text = shownItem.i_name;
    }

    public override void rotateEquipment() {
        if(FindObjectOfType<UnitCanvas>().shownUnit.equippedItem != null && !FindObjectOfType<UnitCanvas>().shownUnit.equippedItem.isEmpty()) {
            var slottedItem = getItemInSlot(selectedIndex);
            Inventory.addItem(FindObjectOfType<UnitCanvas>().shownUnit.equippedItem);
            FindObjectOfType<UnitCanvas>().setUnitItem(slottedItem);
            Inventory.removeItem(slottedItem);
        }

        //  unit just takes
        else {
            FindObjectOfType<UnitCanvas>().setUnitItem(getItemInSlot(selectedIndex));

            Inventory.removeItem(getItemInSlot(selectedIndex));
        }
        selectedIndex = -1;
    }

    public override int getState() {
        return 2;
    }
}

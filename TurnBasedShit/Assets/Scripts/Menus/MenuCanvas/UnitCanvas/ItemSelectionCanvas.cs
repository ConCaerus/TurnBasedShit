using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;


public class ItemSelectionCanvas : UnitEquipmentSelectionCanvas {
    public Item getItemInSlot(int index) {
        if(FindObjectOfType<UnitCanvas>().shownUnit.equippedItem == null || FindObjectOfType<UnitCanvas>().shownUnit.equippedItem.isEmpty())
            return Inventory.getItem(index);
        else if(index > 0)
            return Inventory.getItem(index - 1);
        return FindObjectOfType<UnitCanvas>().shownUnit.equippedItem;
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
        shownImage.transform.parent.GetChild(0).GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(shownItem.i_rarity);
        shownImage.material = FindObjectOfType<PresetLibrary>().getRarityMaterial(shownItem.i_rarity);
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
            var slottedItem = getItemInSlot(selectedIndex);
            FindObjectOfType<UnitCanvas>().setUnitItem(slottedItem);

            Inventory.removeItem(slottedItem);
        }
        selectedIndex = -1;
    }


    public override void removeUnitEquipmentAndResetSelection() {
        selectedIndex = -1;
        if(shown) {
            updateInfo();
        }
        var unitItem = FindObjectOfType<UnitCanvas>().shownUnit.equippedItem;
        if(unitItem == null || unitItem.isEmpty())
            return;

        Inventory.addItem(unitItem);
        FindObjectOfType<UnitCanvas>().setUnitItem(null);
    }

    public override int getState() {
        return 2;
    }
}

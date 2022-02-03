using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;


public class ItemSelectionCanvas : UnitEquipmentSelectionCanvas {
    [SerializeField] TextMeshProUGUI nameText;

    public override void updateInfo() {
        //                          Item shit
        if(getCollectableInSlot(slot.getSelectedSlotIndex()) == null || getCollectableInSlot(slot.getSelectedSlotIndex()).isEmpty()) {
            shownSprite.sprite = null;
            shownSprite.transform.parent.GetChild(0).GetComponent<Image>().color = Color.gray;

            nameText.text = "";
            return;
        }
        var shownItem = (Item)getCollectableInSlot(slot.getSelectedSlotIndex());
        shownSprite.sprite = FindObjectOfType<PresetLibrary>().getItemSprite(shownItem).sprite;
        shownSprite.transform.parent.GetChild(0).GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(shownItem.rarity);


        //  stats
        nameText.text = shownItem.name;
    }

    public override int getState() {
        return 2;
    }
}

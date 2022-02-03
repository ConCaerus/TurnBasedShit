using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CollectionCanvas : MonoBehaviour {
    [SerializeField] GameObject mainSlot;
    [SerializeField] TextMeshProUGUI itemNameText, flavorText, countText;
    [SerializeField] SlotMenu slot;

    private void Update() {
        if(slot.run()) {
            updateInfo();
        }
    }

    public void populateSlots() {
        slot.destroySlots();

        int index = 0;
        var list = FindObjectOfType<PresetLibrary>().getAllCollectables();
        foreach(var i in list) {
            var obj = makeSlot(index, i);
            obj.GetComponent<SlotObject>().setText(0, "");
            index++;
        }

        var slots = slot.getSlots();
        foreach(var i in Collection.getHolder().getObject<CollectionInfo>(0).indexes) {
            slots[i].GetComponent<SlotObject>().setImage(1, FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(list[i]));
            slots[i].GetComponent<SlotObject>().setImageColor(1, Color.white);
            slots[i].GetComponent<SlotObject>().setImageColor(0, FindObjectOfType<PresetLibrary>().getRarityColor(list[i].rarity));
            slots[i].GetComponent<SlotObject>().setInfo(list[i].name);
        }

        slot.resetScrollValue();
        updateInfo();
    }



    void updateInfo() {
        countText.text = Collection.getHolder().getObject<CollectionInfo>(0).numberCollected.ToString() + " / " + FindObjectOfType<PresetLibrary>().getAllCollectables().Count.ToString();

        if(slot.getSelectedSlot() == null || slot.getSelectedSlot().GetComponent<SlotObject>().images[0].color.a < 1.0f) {
            mainSlot.GetComponent<SlotObject>().setImageColor(1, Color.clear);
            mainSlot.GetComponent<SlotObject>().setImage(1, null);
            mainSlot.GetComponent<SlotObject>().setImageColor(0, Color.gray);
            mainSlot.GetComponent<SlotObject>().setInfo("");
            itemNameText.text = "";
            flavorText.text = "";
        }
        else {
            mainSlot.GetComponent<SlotObject>().setImageColor(1, Color.white);
            mainSlot.GetComponent<SlotObject>().setImage(1, slot.getSelectedSlot().GetComponent<SlotObject>().images[1].sprite);
            mainSlot.GetComponent<SlotObject>().setImageColor(0, slot.getSelectedSlot().GetComponent<SlotObject>().images[0].color);
            mainSlot.GetComponent<SlotObject>().setInfo(slot.getSelectedSlot().GetComponent<SlotObject>().info.getInfo());
            itemNameText.text = getSelectedCollectable().name;
            flavorText.text = getSelectedCollectable().flavor;
        }
    }

    GameObject makeSlot(int i, Collectable c) {
        var obj = slot.createSlot(i, Color.white);
        obj.GetComponent<SlotObject>().setImage(1, null);
        obj.GetComponent<SlotObject>().setImageColor(1, Color.clear);
        obj.GetComponent<SlotObject>().setImageColor(0, FindObjectOfType<PresetLibrary>().getRarityColor(c.rarity) / 2f);
        obj.GetComponent<SlotObject>().setInfo("???");

        return obj;
    }

    Collectable getSelectedCollectable() {
        if(slot.getSelectedSlotIndex() == -1)
            return null;

        return FindObjectOfType<PresetLibrary>().getAllCollectables()[slot.getSelectedSlotIndex()];
    }
}

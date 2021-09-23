using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class InventorySelectionCanvas : SelectionCanvas {
    public int state;

    public override void populateSlots() {
        selectedIndex = -1;

        var slotScale = slotPreset.GetComponent<RectTransform>().rect.size.x;
        var startingPos = new Vector2(slotHolder.GetComponent<RectTransform>().rect.xMin + (slotScale / 2.0f), slotHolder.GetComponent<RectTransform>().rect.yMax - (slotScale / 2.0f));

        switch(state) {
            case 0:
                for(int i = 0; i < Inventory.getWeaponCount(); i++) {
                    var obj = createSlot(i);
                    obj.transform.GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getWeapon(i).w_rarity);
                    obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(Inventory.getWeapon(i)).sprite;
                    obj.transform.GetChild(1).gameObject.SetActive(false);
                }
                break;

            case 1:
                for(int i = 0; i < Inventory.getArmorCount(); i++) {
                    var obj = createSlot(i);
                    obj.transform.GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getArmor(i).a_rarity);
                    obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(Inventory.getArmor(i)).sprite;
                    obj.transform.GetChild(1).gameObject.SetActive(false);
                }
                break;

            case 2:
                for(int i = 0; i < Inventory.getItemCount(); i++) {
                    var obj = createSlot(i);
                    obj.transform.GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getItem(i).i_rarity);
                    obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getItemSprite(Inventory.getItem(i)).sprite;
                    obj.transform.GetChild(1).gameObject.SetActive(false);
                }
                break;

            case 3:
                for(int i = 0; i < Inventory.getUniqueConsumableCount(); i++) {
                    var obj = createSlot(i);
                    obj.transform.GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getUniqueConsumable(i).c_rarity);
                    obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getConsumableSprite(Inventory.getUniqueConsumable(i)).sprite;
                    obj.transform.GetChild(1).gameObject.SetActive(true);
                    obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Inventory.getConsumableTypeCount(Inventory.getUniqueConsumable(i)).ToString();
                }
                break;
        }
    }


    GameObject createSlot(int index) {
        var slotScale = slotPreset.GetComponent<RectTransform>().rect.size.x;
        var startingPos = new Vector2(slotHolder.GetComponent<RectTransform>().rect.xMin + (slotScale / 2.0f), slotHolder.GetComponent<RectTransform>().rect.yMax - (slotScale / 2.0f));

        var obj = Instantiate(slotPreset.gameObject, slotHolder.transform);
        int y = Mathf.FloorToInt(index / numOfSlotsPerLine);
        int x = index - (y * numOfSlotsPerLine);

        obj.transform.GetComponent<RectTransform>().localPosition = startingPos + new Vector2((slotScale + slotBuffer) * x, -((slotScale + slotBuffer) * y));
        return obj;
    }

    public override int getState() {
        return state;
    }
}

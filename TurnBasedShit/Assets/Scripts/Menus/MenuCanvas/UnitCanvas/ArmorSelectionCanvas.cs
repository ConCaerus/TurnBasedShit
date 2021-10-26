using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;

public class ArmorSelectionCanvas : UnitEquipmentSelectionCanvas {
    [SerializeField] Image armorImage;
    [SerializeField] TextMeshProUGUI nameText, defText, spdText, firstAttributeText, secondAttributeText;

    public Armor getArmorInSlot(int index) {
        if(FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor == null || FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor.isEmpty())
            return Inventory.getArmor(index);
        else if(index > 0)
            return Inventory.getArmor(index - 1);
        return FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor;
    }


    public override void populateSlots() {
        int slotIndex = 0;
        if(FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor != null && !FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor.isEmpty()) {
            var obj = slot.createSlot(slotIndex, FindObjectOfType<UnitCanvas>().shownUnit.u_sprite.color);
            obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor).sprite;
            slotIndex++;
        }

        for(int i = 0; i < Inventory.getArmorCount(); i++) {
            if(Inventory.getArmor(i) == null || Inventory.getArmor(i).isEmpty())
                continue;
            var obj = slot.createSlot(slotIndex, FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getArmor(i).a_rarity));
            obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(Inventory.getArmor(i)).sprite;
            slotIndex++;
        }
    }

    public override void updateInfo() {
        //                          Armor shit
        if(getArmorInSlot(slot.getSelectedSlotIndex()) == null || getArmorInSlot(slot.getSelectedSlotIndex()).isEmpty()) {
            armorImage.sprite = null;
            armorImage.transform.parent.GetChild(0).GetComponent<Image>().color = Color.gray;

            nameText.text = "";
            defText.text = "0.0";
            spdText.text = "0.0";
            firstAttributeText.text = "";
            secondAttributeText.text = "";
            return;
        }
        var shownArmor = getArmorInSlot(slot.getSelectedSlotIndex());
        armorImage.sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(shownArmor).sprite;
        armorImage.transform.parent.GetChild(0).GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(shownArmor.a_rarity);


        //  stats
        nameText.text = shownArmor.a_name;
        defText.text = shownArmor.a_defence.ToString("0.0");
        spdText.text = shownArmor.a_speedMod.ToString("0.0");

        //  attributes
        List<Armor.attribute> usedAtts = new List<Armor.attribute>();
        usedAtts.Clear();
        int usedIndex = 0;
        firstAttributeText.text = "";
        secondAttributeText.text = "";
        Vector2 prevPos = secondAttributeText.transform.position;
        foreach(var i in shownArmor.a_attributes) {
            bool useable = true;
            foreach(var u in usedAtts) {
                if(u == i) {
                    useable = false;
                    break;
                }
            }

            if(!useable)
                continue;

            usedAtts.Add(i);

            if(usedIndex == 0)
                firstAttributeText.text = i.ToString() + " " + shownArmor.howManyOfAttribute(i).ToString();
            else if(usedIndex == 1)
                secondAttributeText.text = i.ToString() + " " + shownArmor.howManyOfAttribute(i).ToString();

            else {
                float yOff = firstAttributeText.transform.position.y - secondAttributeText.transform.position.y;
                var t = Instantiate(firstAttributeText, firstAttributeText.transform.parent);
                t.transform.position = prevPos - new Vector2(0.0f, yOff);
                t.text = i.ToString() + " " + shownArmor.howManyOfAttribute(i).ToString();

                prevPos = t.transform.position;
            }

            usedIndex++;
        }
    }

    public override void rotateEquipment() {
        if(FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor != null && !FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor.isEmpty()) {
            var slottedArmor = getArmorInSlot(slot.getSelectedSlotIndex());
            Inventory.addArmor(FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor);
            FindObjectOfType<UnitCanvas>().setUnitArmor(slottedArmor);
            Inventory.removeArmor(slottedArmor);
        }

        //  unit just takes
        else {
            var slottedArmor = getArmorInSlot(slot.getSelectedSlotIndex());
            FindObjectOfType<UnitCanvas>().setUnitArmor(slottedArmor);

            Inventory.removeArmor(slottedArmor);
        }
    }


    public override void removeUnitEquipmentAndResetSelection() {
        if(shown) {
            updateInfo();
        }
        var unitArmor = FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor;
        if(unitArmor == null || unitArmor.isEmpty())
            return;

        Inventory.addArmor(unitArmor);
        FindObjectOfType<UnitCanvas>().setUnitArmor(null);
    }

    public override int getState() {
        return 1;
    }
}

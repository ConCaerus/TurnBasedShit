using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;

public class ArmorSelectionCanvas : SelectionCanvas {

    public Armor getArmorInSlot(int wIndex) {
        int slotIndex = -1;
        for(int i = 0; i < slotHolder.transform.childCount; i++) {
            if(Inventory.getArmor(i) != null && !Inventory.getArmor(i).isEmpty()) {
                slotIndex++;
                if(slotIndex == wIndex)
                    return Inventory.getArmor(i);
            }
        }

        return null;
    }

    public override void updateInfo() {
        //                          Armor shit
        if(selectedIndex < 0)
            selectedIndex = 0;
        var shownArmor = getArmorInSlot(selectedIndex);
        shownImage.sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(shownArmor).sprite;
        shownImage.transform.parent.GetChild(0).GetComponent<Image>().color = FindObjectOfType<UnitCanvas>().getRarityColor(shownArmor.a_rarity);


        //  stats
        nameText.text = shownArmor.a_name;
        firstText.text = shownArmor.a_defence.ToString("0.0");
        secondText.text = shownArmor.a_speedMod.ToString("0.0");

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
        if(FindObjectOfType<UnitCanvas>().shownUnit.equippedItem != null && !FindObjectOfType<UnitCanvas>().shownUnit.equippedItem.isEmpty()) {
            Inventory.addArmor(FindObjectOfType<UnitCanvas>().shownUnit.equippedArmor);
            FindObjectOfType<UnitCanvas>().setUnitArmor(getArmorInSlot(selectedIndex));

            Inventory.removeArmor(getArmorInSlot(selectedIndex));
        }

        //  unit just takes
        else {
            FindObjectOfType<UnitCanvas>().setUnitArmor(getArmorInSlot(selectedIndex));

            Inventory.removeArmor(getArmorInSlot(selectedIndex));
        }
        selectedIndex = -1;
    }

    public override bool isSlotPopulated(int index) {
        return getArmorInSlot(selectedIndex) != null;
    }

    public override int getState() {
        return 1;
    }
}

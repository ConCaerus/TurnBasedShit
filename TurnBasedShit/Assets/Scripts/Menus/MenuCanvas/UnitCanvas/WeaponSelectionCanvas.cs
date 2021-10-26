using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class WeaponSelectionCanvas : UnitEquipmentSelectionCanvas {
    [SerializeField] Image weaponImage;
    [SerializeField] TextMeshProUGUI nameText, powText, spdText, firstAttributeText, secondAttributeText;

    public Weapon getWeaponInSlot(int index) {
        if(FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon == null || FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon.isEmpty())
            return Inventory.getWeapon(index);
        else if(index > 0)
            return Inventory.getWeapon(index - 1);
        return FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon;
    }


    public override void populateSlots() {
        int slotIndex = 0;
        if(FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon != null && !FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon.isEmpty()) {
            var obj = slot.createSlot(slotIndex, FindObjectOfType<UnitCanvas>().shownUnit.u_sprite.color);
            obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon).sprite;
            slotIndex++;
        }

        for(int i = 0; i < Inventory.getWeaponCount(); i++) {
            if(Inventory.getWeapon(i) == null || Inventory.getWeapon(i).isEmpty())
                continue;
            var obj = slot.createSlot(slotIndex, FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getWeapon(i).w_rarity));
            obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(Inventory.getWeapon(i)).sprite;
            slotIndex++;
        }
    }

    public override void updateInfo() {
        //                          Weapon shit
        if(getWeaponInSlot(slot.getSelectedSlotIndex()) == null || getWeaponInSlot(slot.getSelectedSlotIndex()).isEmpty()) {
            weaponImage.sprite = null;
            weaponImage.transform.parent.GetChild(0).GetComponent<Image>().color = Color.gray;

            nameText.text = "";
            powText.text = "0.0";
            spdText.text = "0.0";
            firstAttributeText.text = "";
            secondAttributeText.text = "";
            return;
        }
        var shownWeapon = getWeaponInSlot(slot.getSelectedSlotIndex());
        weaponImage.sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(shownWeapon).sprite;
        weaponImage.transform.parent.GetChild(0).GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(shownWeapon.w_rarity);


        //  stats
        nameText.text = shownWeapon.w_name;
        powText.text = shownWeapon.w_power.ToString("0.0");
        spdText.text = shownWeapon.w_speedMod.ToString("0.0");

        //  attributes
        List<Weapon.attribute> usedAtts = new List<Weapon.attribute>();
        usedAtts.Clear();
        int usedIndex = 0;
        firstAttributeText.text = "";
        secondAttributeText.text = "";
        Vector2 prevPos = secondAttributeText.transform.position;
        foreach(var i in shownWeapon.w_attributes) {
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
                firstAttributeText.text = i.ToString() + " " + shownWeapon.howManyOfAttribute(i).ToString();
            else if(usedIndex == 1)
                secondAttributeText.text = i.ToString() + " " + shownWeapon.howManyOfAttribute(i).ToString();

            else {
                float yOff = firstAttributeText.transform.position.y - secondAttributeText.transform.position.y;
                var t = Instantiate(firstAttributeText, firstAttributeText.transform.parent);
                t.transform.position = prevPos - new Vector2(0.0f, yOff);
                t.text = i.ToString() + " " + shownWeapon.howManyOfAttribute(i).ToString();

                prevPos = t.transform.position;
            }

            usedIndex++;
        }
    }

    public override void rotateEquipment() {
        if(FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon != null && !FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon.isEmpty()) {
            var slottedWeapon = getWeaponInSlot(slot.getSelectedSlotIndex());
            Inventory.addWeapon(FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon);
            FindObjectOfType<UnitCanvas>().setUnitWeapon(slottedWeapon);
            Inventory.removeWeapon(slottedWeapon);
        }

        //  unit just takes
        else {
            var slottedWeapon = getWeaponInSlot(slot.getSelectedSlotIndex());
            FindObjectOfType<UnitCanvas>().setUnitWeapon(slottedWeapon);

            Inventory.removeWeapon(slottedWeapon);
        }
    }


    public override void removeUnitEquipmentAndResetSelection() {
        if(shown) {
            updateInfo();
        }
        var unitWeapon = FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon;
        if(unitWeapon == null || unitWeapon.isEmpty())
            return;

        Inventory.addWeapon(unitWeapon);
        FindObjectOfType<UnitCanvas>().setUnitWeapon(null);
    }

    public override int getState() {
        return 0;
    }
}

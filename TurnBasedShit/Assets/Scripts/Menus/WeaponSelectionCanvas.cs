using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class WeaponSelectionCanvas : SelectionCanvas {

    public Weapon getWeaponInSlot(int index) {
        if(index == 0) {
            if(FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon == null || FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon.isEmpty())
                return Inventory.getWeapon(0);
            else
                return FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon;
        }
        else {
            if(FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon == null || FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon.isEmpty())
                return Inventory.getWeapon(index);
            else
                return Inventory.getWeapon(index - 1);
        }
    }

    public override void updateInfo() {
        //                          Weapon shit
        if(getWeaponInSlot(selectedIndex) == null || getWeaponInSlot(selectedIndex).isEmpty()) {
            shownImage.sprite = null;
            shownImage.transform.parent.GetChild(0).GetComponent<Image>().color = Color.gray;

            nameText.text = "";
            firstText.text = "0.0";
            secondText.text = "0.0";
            firstAttributeText.text = "";
            secondAttributeText.text = "";
            return;
        }
        var shownWeapon = getWeaponInSlot(selectedIndex);
        shownImage.sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(shownWeapon).sprite;
        shownImage.transform.parent.GetChild(0).GetComponent<Image>().color = FindObjectOfType<UnitCanvas>().getRarityColor(shownWeapon.w_rarity);


        //  stats
        nameText.text = shownWeapon.w_name;
        firstText.text = shownWeapon.w_power.ToString("0.0");
        secondText.text = shownWeapon.w_speedMod.ToString("0.0");

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
            var slottedWeapon = getWeaponInSlot(selectedIndex);
            Inventory.addWeapon(FindObjectOfType<UnitCanvas>().shownUnit.equippedWeapon);
            FindObjectOfType<UnitCanvas>().setUnitWeapon(slottedWeapon);
            Inventory.removeWeapon(slottedWeapon);
        }

        //  unit just takes
        else {
            FindObjectOfType<UnitCanvas>().setUnitWeapon(getWeaponInSlot(selectedIndex));

            Inventory.removeWeapon(getWeaponInSlot(selectedIndex));
        }
        selectedIndex = -1;
    }

    public override int getState() {
        return 0;
    }
}

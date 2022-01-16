using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class WeaponSelectionCanvas : UnitEquipmentSelectionCanvas {
    [SerializeField] TextMeshProUGUI nameText, powText, spdText, firstAttributeText, secondAttributeText;

    public Weapon getWeaponInSlot(int index) {
        if(FindObjectOfType<UnitCanvas>().shownUnit.weapon == null || FindObjectOfType<UnitCanvas>().shownUnit.weapon.isEmpty())
            return Inventory.getHolder().getObject<Weapon>(index);
        else if(index > 0)
            return Inventory.getHolder().getObject<Weapon>(index - 1);
        return FindObjectOfType<UnitCanvas>().shownUnit.weapon;
    }


    public override IEnumerator populateSlots() {
        int slotIndex = 0;
        if(FindObjectOfType<UnitCanvas>().shownUnit.weapon != null && !FindObjectOfType<UnitCanvas>().shownUnit.weapon.isEmpty()) {
            var obj = slot.createSlot(slotIndex, FindObjectOfType<UnitCanvas>().shownUnit.u_sprite.color, InfoTextCreator.createForCollectable(FindObjectOfType<UnitCanvas>().shownUnit.weapon));
            obj.GetComponent<SlotObject>().setInfo(FindObjectOfType<UnitCanvas>().shownUnit.weapon.name);
            obj.GetComponent<SlotObject>().setMainImage(FindObjectOfType<PresetLibrary>().getWeaponSprite(FindObjectOfType<UnitCanvas>().shownUnit.weapon).sprite);
            slotIndex++;
        }

        for(int i = 0; i < Inventory.getHolder().getObjectCount<Weapon>(); i++) {
            if(Inventory.getHolder().getObject<Weapon>(i) == null || Inventory.getHolder().getObject<Weapon>(i).isEmpty())
                continue;
            var obj = slot.createSlot(slotIndex, FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getHolder().getObject<Weapon>(i).rarity), InfoTextCreator.createForCollectable(Inventory.getHolder().getObject<Weapon>(i)));
            obj.GetComponent<SlotObject>().setInfo(Inventory.getHolder().getObject<Weapon>(i).name);
            obj.GetComponent<SlotObject>().setMainImage(FindObjectOfType<PresetLibrary>().getWeaponSprite(Inventory.getHolder().getObject<Weapon>(i)).sprite);
            float prevScale = obj.transform.localScale.x;
            obj.transform.localScale = Vector3.zero;
            obj.transform.DOScale(prevScale, populatingWaitTime);
            slotIndex++;
            yield return new WaitForSeconds(populatingWaitTime);
        }

        slot.deleteSlotsAfterIndex(slotIndex);
    }

    public override void updateInfo() {
        //                          Weapon shit
        if(getWeaponInSlot(slot.getSelectedSlotIndex()) == null || getWeaponInSlot(slot.getSelectedSlotIndex()).isEmpty()) {
            shownSprite.sprite = null;
            shownSprite.transform.parent.GetChild(0).GetComponent<Image>().color = Color.gray;

            nameText.text = "";
            powText.text = "0.0";
            spdText.text = "0.0";
            firstAttributeText.text = "";
            secondAttributeText.text = "";
            return;
        }
        var shownWeapon = getWeaponInSlot(slot.getSelectedSlotIndex());
        shownSprite.sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(shownWeapon).sprite;
        shownSprite.transform.parent.GetChild(0).GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(shownWeapon.rarity);


        //  stats
        nameText.text = shownWeapon.name;
        powText.text = shownWeapon.power.ToString("0.0");
        spdText.text = shownWeapon.speedMod.ToString("0.0");

        //  attributes
        List<Weapon.attribute> usedAtts = new List<Weapon.attribute>();
        usedAtts.Clear();
        int usedIndex = 0;
        firstAttributeText.text = "";
        secondAttributeText.text = "";
        Vector2 prevPos = secondAttributeText.transform.position;
        foreach(var i in shownWeapon.attributes) {
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
        if(FindObjectOfType<UnitCanvas>().shownUnit.weapon != null && !FindObjectOfType<UnitCanvas>().shownUnit.weapon.isEmpty()) {
            var slottedWeapon = getWeaponInSlot(slot.getSelectedSlotIndex());
            Inventory.addCollectable(FindObjectOfType<UnitCanvas>().shownUnit.weapon);
            FindObjectOfType<UnitCanvas>().setUnitWeapon(slottedWeapon);
            Inventory.removeCollectable(slottedWeapon);
        }

        //  unit just takes
        else {
            var slottedWeapon = getWeaponInSlot(slot.getSelectedSlotIndex());
            FindObjectOfType<UnitCanvas>().setUnitWeapon(slottedWeapon);

            Inventory.removeCollectable(slottedWeapon);
        }
    }


    public override void removeUnitEquipmentAndResetSelection() {
        if(shown) {
            updateInfo();
        }
        var unitWeapon = FindObjectOfType<UnitCanvas>().shownUnit.weapon;
        if(unitWeapon == null || unitWeapon.isEmpty())
            return;

        Inventory.addCollectable(unitWeapon);
        FindObjectOfType<UnitCanvas>().setUnitWeapon(null);
    }

    public override int getState() {
        return 0;
    }
}

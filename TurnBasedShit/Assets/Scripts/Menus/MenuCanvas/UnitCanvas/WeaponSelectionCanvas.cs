using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class WeaponSelectionCanvas : UnitEquipmentSelectionCanvas {
    [SerializeField] TextMeshProUGUI nameText, powText, spdText, firstAttributeText, secondAttributeText;

    public override void updateInfo() {
        //                          Weapon shit
        if(getCollectableInSlot(slot.getSelectedSlotIndex()) == null || getCollectableInSlot(slot.getSelectedSlotIndex()).isEmpty()) {
            shownSprite.sprite = null;
            shownSprite.transform.parent.GetChild(0).GetComponent<Image>().color = Color.gray;

            nameText.text = "";
            powText.text = "0.0";
            spdText.text = "0.0";
            firstAttributeText.text = "";
            secondAttributeText.text = "";
            return;
        }
        var shownWeapon = (Weapon)getCollectableInSlot(slot.getSelectedSlotIndex());
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

    public override int getState() {
        return 0;
    }
}

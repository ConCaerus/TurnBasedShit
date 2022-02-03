using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;

public class ArmorSelectionCanvas : UnitEquipmentSelectionCanvas {
    [SerializeField] TextMeshProUGUI nameText, defText, spdText, firstAttributeText, secondAttributeText;

    public override void updateInfo() {
        //                          Armor shit
        if(getCollectableInSlot(slot.getSelectedSlotIndex()) == null || getCollectableInSlot(slot.getSelectedSlotIndex()).isEmpty()) {
            shownSprite.sprite = null;
            shownSprite.transform.parent.GetChild(0).GetComponent<Image>().color = Color.gray;

            nameText.text = "";
            defText.text = "0.0";
            spdText.text = "0.0";
            firstAttributeText.text = "";
            secondAttributeText.text = "";
            return;
        }
        var shownArmor = (Armor)getCollectableInSlot(slot.getSelectedSlotIndex());
        shownSprite.sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(shownArmor).sprite;
        shownSprite.transform.parent.GetChild(0).GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(shownArmor.rarity);


        //  stats
        nameText.text = shownArmor.name;
        defText.text = shownArmor.defence.ToString("0.0");
        spdText.text = shownArmor.speedMod.ToString("0.0");

        //  attributes
        List<Armor.attribute> usedAtts = new List<Armor.attribute>();
        usedAtts.Clear();
        int usedIndex = 0;
        firstAttributeText.text = "";
        secondAttributeText.text = "";
        Vector2 prevPos = secondAttributeText.transform.position;
        foreach(var i in shownArmor.attributes) {
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

    public override int getState() {
        return 1;
    }
}

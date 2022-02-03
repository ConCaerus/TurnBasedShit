using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapPartyInfoSlot : SlotObject {
    [SerializeField] Slider healthSlider, expSlider;


    public void updateInfo(int slotIndex) {
        if(slotIndex >= Party.getHolder().getObjectCount<UnitStats>()) {
            texts[0].gameObject.SetActive(false);
            texts[1].gameObject.SetActive(false);
            healthSlider.gameObject.SetActive(false);
            expSlider.gameObject.SetActive(false);
            images[0].gameObject.SetActive(false);
            images[1].gameObject.SetActive(false);
            return;
        }

        var unit = Party.getHolder().getObject<UnitStats>(slotIndex);

        texts[1].text = unit.u_name;
        texts[0].text = unit.u_level.ToString();

        healthSlider.maxValue = unit.getModifiedMaxHealth();
        healthSlider.value = unit.u_health;
        expSlider.maxValue = unit.u_expCap;
        expSlider.value = unit.u_exp;

        images[0].sprite = FindObjectOfType<PresetLibrary>().getUnitHeadSprite(unit.u_sprite.headIndex);
        images[0].color = unit.u_sprite.color;
        images[0].SetNativeSize();
        images[0].transform.localScale = new Vector3(.35f, .35f);
        images[1].sprite = FindObjectOfType<PresetLibrary>().getUnitFace(unit.u_sprite.faceIndex);
        images[1].SetNativeSize();
        images[1].transform.localScale = Vector3.one;
    }
}

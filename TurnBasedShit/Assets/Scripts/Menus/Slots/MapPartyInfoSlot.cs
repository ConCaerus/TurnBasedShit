using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapPartyInfoSlot : Slot {
    [SerializeField] TextMeshProUGUI levelText, nameText;
    [SerializeField] Slider healthSlider, expSlider;
    [SerializeField] Image headImage, faceImage;


    public override void updateInfo(int slotIndex) {
        if(slotIndex >= Party.getHolder().getObjectCount<UnitStats>()) {
            levelText.gameObject.SetActive(false);
            nameText.gameObject.SetActive(false);
            healthSlider.gameObject.SetActive(false);
            expSlider.gameObject.SetActive(false);
            headImage.gameObject.SetActive(false);
            faceImage.gameObject.SetActive(false);
            return;
        }

        var unit = Party.getHolder().getObject<UnitStats>(slotIndex);

        nameText.text = unit.u_name;
        levelText.text = unit.u_level.ToString();

        healthSlider.maxValue = unit.getModifiedMaxHealth();
        healthSlider.value = unit.u_health;
        expSlider.maxValue = unit.u_expCap;
        expSlider.value = unit.u_exp;

        headImage.sprite = FindObjectOfType<PresetLibrary>().getUnitHeadSprite(unit.u_sprite.headIndex);
        headImage.color = unit.u_sprite.color;
        headImage.SetNativeSize();
        headImage.transform.localScale = new Vector3(.35f, .35f);
        faceImage.sprite = FindObjectOfType<PresetLibrary>().getUnitFace(unit.u_sprite.faceIndex);
        faceImage.SetNativeSize();
        faceImage.transform.localScale = Vector3.one;
    }
}

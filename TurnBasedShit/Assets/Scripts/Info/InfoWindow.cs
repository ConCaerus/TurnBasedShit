using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoWindow : MonoBehaviour {
    [Header("Non Equipment Variables")]
    [SerializeField] GameObject nHolder;
    [SerializeField] SlotObject nMainSlot;
    [SerializeField] TextMeshProUGUI nNameText, nDescText, nTypeText, nCostText;

    [Header("Equipment Variables")]
    [SerializeField] GameObject eHolder;
    [SerializeField] SlotObject eMainSlot;
    [SerializeField] TextMeshProUGUI eNameText, eDescText, eTypeText, eCostText, powText, spdText, wornText, attTypeText;
    [SerializeField] GameObject attsHolder;

    private void Update() {
        if((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && FindObjectOfType<InfoCanvas>().infoWindowShown) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if(hit.collider == null || (hit.collider.gameObject != eHolder && hit.collider.gameObject != nHolder)) {
                FindObjectOfType<InfoCanvas>().hideInfoWindow();
            }
        }
    }


    public void updateInfo(Collectable col) {
        if(col.type != Collectable.collectableType.Weapon && col.type != Collectable.collectableType.Armor)
            updateNonEquipmentInfo(col);
        else
            updateEquipmentInfo(col);
    }

    void updateNonEquipmentInfo(Collectable col) {
        nHolder.SetActive(true);
        eHolder.SetActive(false);
        nMainSlot.setImage(1, FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(col));
        nMainSlot.setImageColor(0, FindObjectOfType<PresetLibrary>().getRarityColor(col.rarity));
        nMainSlot.setInfo(col.name);

        nNameText.text = col.name;
        nDescText.text = col.description;
        nCostText.text = col.coinCost.ToString() + "c";

        nTypeText.text = ((GameInfo.rarity)col.rarity).ToString() + " " + col.type.ToString();
    }
    void updateEquipmentInfo(Collectable col) {
        nHolder.SetActive(false);
        eHolder.SetActive(true);
        eMainSlot.setImage(1, FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(col));
        eMainSlot.setImageColor(0, FindObjectOfType<PresetLibrary>().getRarityColor(col.rarity));
        eMainSlot.setInfo(col.name);
        
        eNameText.text = col.name;
        eDescText.text = col.description;
        eCostText.text = col.coinCost.ToString() + "c";
        
        eTypeText.text = ((GameInfo.rarity)col.rarity).ToString() + " " + col.type.ToString();

        if(col.type == Collectable.collectableType.Weapon) {
            var we = (Weapon)col;

            for(int i = 0; i < attsHolder.transform.childCount; i++) {
                if(we.attributes.Count <= i)
                    attsHolder.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = "";
                else
                    attsHolder.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = we.attributes[i].ToString();
            }

            powText.text = "POW: " + we.power.ToString("0.0");
            spdText.text = "SPD: " + we.speedMod.ToString("0.0");
            wornText.text = "State: " + we.wornAmount.ToString();
            attTypeText.text = we.aType.ToString();
        }

        else {
            var ar = (Armor) col;

            for(int i = 0; i < attsHolder.transform.childCount; i++) {
                if(ar.attributes.Count <= i)
                    attsHolder.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = "";
                else
                    attsHolder.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = ar.attributes[i].ToString();
            }

            powText.text = "DEF: " + ar.defence.ToString("0.0");
            spdText.text = "SPD: " + ar.speedMod.ToString("0.0");
            wornText.text = "State: " + ar.wornAmount.ToString();
            attTypeText.text = "";
        }
    }
}

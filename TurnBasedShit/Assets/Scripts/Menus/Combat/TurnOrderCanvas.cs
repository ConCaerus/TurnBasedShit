using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TurnOrderCanvas : MonoBehaviour {
    List<GameObject> slots = new List<GameObject>();
    GameObject selectedSlot = null;


    private void Start() {
        foreach(var i in GetComponentsInChildren<InfoBearer>())
            slots.Add(i.gameObject);
        updateInfomation();
    }


    public void updateInfomation(bool bufferTime = false) {
        StartCoroutine(animateTurnAdvance(bufferTime));
    }


    IEnumerator animateTurnAdvance(bool bufferTime = false) {
        float speed = 0.1f;
        if(!bufferTime)
            speed = 0.0f;

        var units = GetComponent<TurnOrderSorter>().getNextPlayers(slots.Count);
        for(int i = 0; i < slots.Count; i++) {
            slots[i].GetComponent<RectTransform>().DOPunchAnchorPos(new Vector3(0.0f, 25.0f, 0.0f), speed);
            if(units[i] == null)
                continue;

            var newUnit = units[i].GetComponent<UnitClass>();



            yield return new WaitForSeconds(speed);

            var prev = slots[i];
            slots[i] = Instantiate(FindObjectOfType<PresetLibrary>().getProfileForUnit(newUnit).gameObject, slots[i].transform.parent);
            Destroy(prev.gameObject);

            if(newUnit.isPlayerUnit) {
                slots[i].transform.GetChild(0).GetComponent<Image>().color = newUnit.stats.u_sprite.color;
                var backgroundColor = newUnit.stats.u_sprite.color * 2.0f;
                slots[i].GetComponent<Image>().color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 1.0f);
            }

            slots[i].GetComponent<InfoBearer>().setInfo(newUnit.stats.u_name);
            slots[i].GetComponent<InfoBearer>().runOnMouseOver(upscaleSlotObj);
            slots[i].GetComponent<InfoBearer>().runOnMouseExit(downscaleSlotObj);
        }
    }

    public void upscaleSlotObj() {
        var slot = FindObjectOfType<InfoCanvas>().shownInfo.gameObject;
        slot.transform.GetChild(0).DOKill();
        if(slot.transform.childCount == 2) {
            slot.transform.GetChild(1).DOKill();
            slot.transform.GetChild(0).DOScale(.4f, 0.15f);
            slot.transform.GetChild(1).DOScale(.45f, 0.15f);
        }
        else
            slot.transform.GetChild(0).DOScale(1.125f, 0.15f);

        var pad = slot.GetComponent<RectMask2D>().padding;
        float val = 3.5f;
        slot.GetComponent<RectMask2D>().padding = new Vector4(pad.x - val, pad.y - val, pad.z - val, pad.w - val);
        selectedSlot = slot;
    }
    public void downscaleSlotObj() {
        if(selectedSlot == null)
            return;
        selectedSlot.transform.GetChild(0).DOKill();
        if(selectedSlot.transform.childCount == 2) {
            selectedSlot.transform.GetChild(1).DOKill();
            selectedSlot.transform.GetChild(0).DOScale(.35f, 0.25f);
            selectedSlot.transform.GetChild(1).DOScale(.35f, 0.25f);
        }
        else
            selectedSlot.transform.GetChild(0).DOScale(1.0f, 0.25f);

        selectedSlot.GetComponent<RectMask2D>().padding = FindObjectOfType<PresetLibrary>().getPlayerUnitProfile().GetComponent<RectMask2D>().padding;
        selectedSlot = null;
    }
}

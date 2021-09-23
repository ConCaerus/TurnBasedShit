using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MapQuestMenu : MonoBehaviour {
    public bool shown;

    [SerializeField] GameObject slotPreset;
    [SerializeField] SlotMenu slot;

    [SerializeField] Color bossColor, delColor, pickColor;

    Coroutine shower = null;


    private void Awake() {
        DOTween.Init();

        createSlots();
        shower = StartCoroutine(hide());
    }

    private void Update() {
        if(shown && slot.run())
            moveCamToSelected();
    }



    void createSlots() {
        int slotIndex = 0;
        foreach(var i in ActiveQuests.getAllBossFightQuests()) {
            slot.createNewSlot(slotIndex, slotPreset, slot.gameObject.transform, bossColor);
            slotIndex++;
        }
        foreach(var i in ActiveQuests.getAllDeliveryQuests()) {
            slot.createNewSlot(slotIndex, slotPreset, slot.gameObject.transform, delColor);
            slotIndex++;
        }
        foreach(var i in ActiveQuests.getAllPickupQuests()) {
            slot.createNewSlot(slotIndex, slotPreset, slot.gameObject.transform, pickColor);
            slotIndex++;
        }
    }


    void moveCamToSelected() {
        if(slot.getEventSelectedSlot() == null)
            return;
        FindObjectOfType<MapCameraController>().moveToPos(getSelectedQuestPos());
    }


    Vector2 getSelectedQuestPos() {
        if(slot.getSelectedSlotIndex() == -1)
            return transform.position;
        int slotIndex = 0;
        foreach(var i in ActiveQuests.getAllBossFightQuests()) {
            if(slotIndex == slot.getSelectedSlotIndex()) {
                for(int b = 0; b < MapLocationHolder.getBossCount(); b++) {
                    if(MapLocationHolder.getBossLocation(b).attachedQuest.isEqualTo(i)) { 
                        return MapLocationHolder.getBossLocation(b).pos;
                    }
                }
            }
            slotIndex++;
        }
        foreach(var i in ActiveQuests.getAllDeliveryQuests()) {
            if(slotIndex == slot.getSelectedSlotIndex()) {
                for(int t = 0; t < MapLocationHolder.getTownCount(); t++) {
                    if(MapLocationHolder.getTownLocation(t).town.isEqualTo(i.deliveryLocation))
                        return MapLocationHolder.getTownLocation(t).pos;
                }
            }
            slotIndex++;
        }
        foreach(var i in ActiveQuests.getAllPickupQuests()) {
            if(slotIndex == slot.getSelectedSlotIndex()) {
                for(int p = 0; p < MapLocationHolder.getPickupCount(); p++) {
                    if(MapLocationHolder.getPickupLocation(p).attachedQuest.isEqualTo(i))
                        return MapLocationHolder.getPickupLocation(p).pos;
                }
            }
            slotIndex++;
        }

        return transform.position;
    }


    IEnumerator show() {
        float speed = 0.15f;

        transform.GetChild(1).GetComponent<RectTransform>().DOAnchorPosX(-transform.GetChild(0).GetComponent<RectTransform>().rect.width, speed / 2.0f);
        transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = ">";
        yield return new WaitForSeconds(speed / 2.0f);

        transform.GetChild(0).GetComponent<RectTransform>().DOScaleY(1.0f, speed);
        yield return new WaitForSeconds(speed);

        shown = true;
        shower = null;
    }

    IEnumerator hide() {
        float speed = 0.25f;

        transform.GetChild(0).GetComponent<RectTransform>().DOScaleY(0.0f, speed);
        yield return new WaitForSeconds(speed);

        transform.GetChild(1).GetComponent<RectTransform>().DOAnchorPosX(0.0f, speed / 2.0f);
        transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "<";

        yield return new WaitForSeconds(speed / 2.0f);

        shown = false;
        shower = null;
    }

    //  Buttons
    public void toggleShown() {
        if(shower != null)
            return;
        if(shown)
            shower = StartCoroutine(hide());
        else
            shower = StartCoroutine(show());
    }
}

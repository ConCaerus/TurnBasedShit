using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MapQuestMenu : MonoBehaviour { //  only for quests with a location
    public bool shown;

    [SerializeField] SlotMenu slot;

    [SerializeField] Color bossColor, delColor, pickColor, rescueColor;

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
        foreach(var i in ActiveQuests.getHolder().getObjects<BossFightQuest>()) {
            if(i.completed)
                continue;
            slot.createSlot(slotIndex, bossColor);
            slotIndex++;
        }
        foreach(var i in ActiveQuests.getHolder().getObjects<DeliveryQuest>()) {
            if(i.completed)
                continue;
            slot.createSlot(slotIndex, delColor);
            slotIndex++;
        }
        foreach(var i in ActiveQuests.getHolder().getObjects<PickupQuest>()) {
            if(i.completed)
                continue;
            slot.createSlot(slotIndex, pickColor);
            slotIndex++;
        }
        foreach(var i in ActiveQuests.getHolder().getObjects<RescueQuest>()) {
            if(i.completed)
                continue;
            slot.createSlot(slotIndex, rescueColor);
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
        foreach(var i in ActiveQuests.getHolder().getObjects<BossFightQuest>()) {
            if(i.completed)
                continue;
            if(slotIndex == slot.getSelectedSlotIndex()) {
                return i.location.pos;
            }
            slotIndex++;
        }
        foreach(var i in ActiveQuests.getHolder().getObjects<DeliveryQuest>()) {
            if(i.completed)
                continue;
            if(slotIndex == slot.getSelectedSlotIndex()) {
                return i.deliveryLocation.pos;
            }
            slotIndex++;
        }
        foreach(var i in ActiveQuests.getHolder().getObjects<PickupQuest>()) {
            if(i.completed)
                continue;
            if(slotIndex == slot.getSelectedSlotIndex()) {
                return i.location.pos;
            }
            slotIndex++;
        }
        foreach(var i in ActiveQuests.getHolder().getObjects<RescueQuest>()) {
            if(i.completed)
                continue;
            if(slotIndex == slot.getSelectedSlotIndex()) {
                return i.location.pos;
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

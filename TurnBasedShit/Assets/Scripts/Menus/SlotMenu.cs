using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SlotMenu : MonoBehaviour {
    float slotTopY = 120, slotBotY = -120;
    float slotBuffer = 10.0f;
    float scrollSpeed = 35.0f;

    float scrollPos = 0.0f;

    List<GameObject> slots = new List<GameObject>();
    int selectedSlotIndex = -1;




    //  update func
    //  returns true when a change in the selected slot could have occured
    public bool run() {
        scrollThoughList();
        if(Input.GetMouseButtonDown(0)) {
            for(int i = 0; i < slots.Count; i++) {
                if(slots[i].gameObject == EventSystem.current.currentSelectedGameObject) {
                    selectedSlotIndex = i;
                }
            }
            return true;
        }
        return false;
    }

    public int getSelectedSlotIndex() {
        return selectedSlotIndex;
    }
    public GameObject getSelectedSlot() {
        if(getSelectedSlotIndex() < 0 || getSelectedSlotIndex() > slots.Count - 1)
            return null;
        return slots[selectedSlotIndex];
    }
    public GameObject getEventSelectedSlot() {
        for(int i = 0; i < slots.Count; i++) {
            if(slots[i].gameObject == EventSystem.current.currentSelectedGameObject) {
                return slots[i].gameObject;
            }
        }

        return null;
    }

    public GameObject createNewSlot(int index, GameObject slotPreset, Transform holder, Color slotColor) {
        GameObject obj;
        if(index >= slots.Count)
            obj = Instantiate(slotPreset, holder);
        else
            obj = slots[index];

        //  position and scale
        float step = slotPreset.GetComponent<RectTransform>().rect.height + slotBuffer;
        obj.transform.localPosition = new Vector3(0.0f, slotTopY - (step * index) + scrollPos, 0.0f);
        //obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        //  width and height
        var x = obj.GetComponent<RectTransform>().rect.width;
        var y = obj.GetComponent<RectTransform>().rect.height;

        //  text and images
        obj.GetComponent<Image>().color = slotColor;

        if(index >= slots.Count)
            slots.Add(obj);

        return obj;
    }


    public List<GameObject> createANumberOfSlots(int num, GameObject slotPreset, Transform holder, Color slotColor) {
        var temp = new List<GameObject>();

        //  delete unusable slots
        for(int i = 0; i < slots.Count; i++) {
            if(i >= num) {
                temp.Add(slots[i]);
                continue;
            }
            if(slots[i].transform.childCount != slotPreset.transform.childCount) {
                temp.Add(slots[i]);
            }
        }
        foreach(var i in temp) {
            slots.Remove(i.gameObject);
            Destroy(i.gameObject);
        }
        temp = new List<GameObject>();

        for(int i = 0; i < num; i++) {
            temp.Add(createNewSlot(i, slotPreset, holder, slotColor));
        }

        return temp;
    }

    public void deleteSlotAtIndex(int index) {
        //  destorys slot
        GameObject sl = slots[index];
        slots.RemoveAt(index);
        Destroy(sl.gameObject);

        //  reposition existing slots
        if(slots.Count == 0)
            return;
        float step = slots[0].GetComponent<RectTransform>().rect.height + slotBuffer;
        for(int i = 0; i < slots.Count; i++) {
            slots[i].transform.localPosition = new Vector3(0.0f, slotTopY - (step * i) + scrollPos, 0.0f);
        }

        clampScroll();
    }
    public void moveSlot(int slotIndex, int moveToIndex) {
        if(slotIndex >= slots.Count || moveToIndex >= slots.Count || slotIndex == moveToIndex)
            return;

        var slot = slots[slotIndex];
        slots.Insert(moveToIndex, slot);
        slots.RemoveAt(slotIndex);
    }
    public GameObject replaceSlot(int index, GameObject slotPreset, Transform holder, Color slotColor) {
        //  destroy old slot
        Destroy(slots[index].gameObject);

        var obj = Instantiate(slotPreset, holder);

        //  position and scale
        float step = slotPreset.GetComponent<RectTransform>().rect.height + slotBuffer;
        obj.transform.localPosition = new Vector3(0.0f, slotTopY - (step * index) + scrollPos, 0.0f);
        //obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        //  width and height
        var x = obj.GetComponent<RectTransform>().rect.width;
        var y = obj.GetComponent<RectTransform>().rect.height;
        obj.GetComponent<BoxCollider2D>().size = new Vector2(x, y);

        //  text and images
        obj.GetComponent<Image>().color = slotColor;

        slots[index] = obj;
        return obj;

    }

    public void scrollThoughList() {
        float scroll = Input.mouseScrollDelta.y;

        if(scroll == 0 || slots.Count == 0)
            return;
        scrollSlots(-scroll * scrollSpeed);

        clampScroll();
    }

    void clampScroll() {
        if(slots.Count == 0)
            return;
        //  Top bounds
        if(slots[0].transform.localPosition.y < slotTopY)
            scrollSlots(slotTopY - slots[0].transform.localPosition.y);

        //  Bot bounds
        else if(slots[slots.Count - 1].transform.localPosition.y > slotBotY)
            scrollSlots(slotBotY - slots[slots.Count - 1].transform.localPosition.y);
    }

    void scrollSlots(float val) {
        scrollPos = val;
        foreach(var i in slots) {
            i.transform.localPosition = new Vector3(0.0f, i.transform.localPosition.y + val, 0.0f);
        }
    }

    public void destroySlots() {
        foreach(var i in slots)
            Destroy(i.gameObject);
        slots.Clear();
    }
    public List<GameObject> getSlots() {
        return slots;
    }
    public void setSlots(List<GameObject> sl) {
        slots = sl;
    }
}

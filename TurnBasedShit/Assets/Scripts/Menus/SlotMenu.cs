using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]
public class SlotMenu : MonoBehaviour {
    public GameObject slotPreset;

    List<GameObject> slots = new List<GameObject>();
    int selectedSlotIndex = -1;



    public void init() {
        int count = GetComponentInChildren<GridLayoutGroup>().constraintCount;
        float xSize = 0.0f;
        if(count > 1) {
            float width = transform.GetChild(0).GetComponent<RectTransform>().rect.width - GetComponentInChildren<GridLayoutGroup>().padding.left - GetComponentInChildren<GridLayoutGroup>().padding.right;
            width -= GetComponentInChildren<GridLayoutGroup>().spacing.x / (count - 1);

            xSize = width / count;
        }
        else if(count == 1) {
            xSize = slotPreset.GetComponent<RectTransform>().rect.width + GetComponentInChildren<GridLayoutGroup>().padding.left + GetComponentInChildren<GridLayoutGroup>().padding.right;
        }
        else
            return;

        float ySize = (xSize / slotPreset.GetComponent<RectTransform>().rect.width) * slotPreset.GetComponent<RectTransform>().rect.height;
        GetComponentInChildren<GridLayoutGroup>().cellSize = new Vector2(xSize, ySize);
    }

    //  update func
    //  returns true when a change in the selected slot could have occured
    public bool run() {
        if(slots.Count == 0)
            return false;

        //  returns true if selected slot changed
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
    public void setSelectedSlotIndex(int i) {
        selectedSlotIndex = i;
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


    //  instantiates a new slot object at the end of the list
    public GameObject instantiateNewSlot(Color slotColor) {
        var obj = Instantiate(slotPreset, gameObject.transform.GetChild(0).transform);
        slots.Add(obj);

        float count = slots.Count / GetComponentInChildren<GridLayoutGroup>().constraintCount;
        if(slots.Count % GetComponentInChildren<GridLayoutGroup>().constraintCount != 0)
            count++;
        float size = count * GetComponentInChildren<GridLayoutGroup>().cellSize.y;
        size += GetComponentInChildren<GridLayoutGroup>().padding.top + GetComponentInChildren<GridLayoutGroup>().padding.bottom;
        if(count > 1)
            size += GetComponentInChildren<GridLayoutGroup>().spacing.y * (count - 1);

        transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x, size);

        //  color
        obj.GetComponent<Image>().color = slotColor;

        return obj;
    }


    //  recycles slot object at index, if no existing slot at the index, creats a new slot object
    public GameObject createSlot(int index, Color slotColor) {
        if(index >= slots.Count)
            return instantiateNewSlot(slotColor);
        GameObject obj;
        obj = slots[index];

        //  color
        obj.GetComponent<Image>().color = slotColor;

        return obj;
    }



    public List<GameObject> createANumberOfSlots(int num, Color slotColor) {
        var temp = new List<GameObject>();

        //  delete unusable slots
        if(num < slots.Count) {
            for(int i = num; i < slots.Count; i++) {
                temp.Add(slots[i]);
            }
            foreach(var i in temp) {
                slots.Remove(i.gameObject);
                Destroy(i.gameObject);
            }
            temp = new List<GameObject>();
        }

        for(int i = 0; i < num; i++) {
            temp.Add(createSlot(i, slotColor));
        }

        return temp;
    }

    public void deleteSlotAtIndex(int index) {
        //  destorys slot
        GameObject sl = slots[index];
        slots.RemoveAt(index);
        Destroy(sl.gameObject);
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

        //  width and height
        var x = obj.GetComponent<RectTransform>().rect.width;
        var y = obj.GetComponent<RectTransform>().rect.height;
        obj.GetComponent<BoxCollider2D>().size = new Vector2(x, y);

        //  text and images
        obj.GetComponent<Image>().color = slotColor;

        slots[index] = obj;
        return obj;

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

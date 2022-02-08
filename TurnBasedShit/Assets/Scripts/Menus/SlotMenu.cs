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
    List<int> selectedSlotIndexes = new List<int>();

    public bool canSelectMultiple = false;

    bool initialized = false;

    public delegate void funcWithIndex(int slotIndex, bool wasSelected);
    funcWithIndex runOnSelect = null;


    void init() {
        int count = GetComponentInChildren<GridLayoutGroup>().constraintCount;
        float xSize = 0.0f;
        if(count > 1) {
            float width = transform.GetChild(0).GetComponent<RectTransform>().rect.width - GetComponentInChildren<GridLayoutGroup>().padding.left - GetComponentInChildren<GridLayoutGroup>().padding.right;
            width -= GetComponentInChildren<GridLayoutGroup>().spacing.x / (count - 1);
            width -= (count - 1) * GetComponentInChildren<GridLayoutGroup>().spacing.x;

            xSize = width / count;
        }
        else if(count == 1) {
            xSize = slotPreset.GetComponent<RectTransform>().rect.width + GetComponentInChildren<GridLayoutGroup>().padding.left + GetComponentInChildren<GridLayoutGroup>().padding.right;
        }
        else
            return;

        float ySize = (xSize / slotPreset.GetComponent<RectTransform>().rect.width) * slotPreset.GetComponent<RectTransform>().rect.height;
        GetComponentInChildren<GridLayoutGroup>().cellSize = new Vector2(xSize, ySize);

        resetScrollValue();

        initialized = true;
    }

    //  update func
    //  returns true when a change in the selected slot could have occured
    public bool run() {
        if(slots.Count == 0 || !initialized)
            return false;

        //  returns true if selected slot changed
        if(Input.GetMouseButtonDown(0)) {
            for(int i = 0; i < slots.Count; i++) {
                bool selected = slots[i].gameObject == EventSystem.current.currentSelectedGameObject || 
                     (slots[i].GetComponent<SlotObject>() != null && slots[i].GetComponent<SlotObject>().button.gameObject == EventSystem.current.currentSelectedGameObject);
                if(selected) {
                    bool wasSelected = false;
                    if(!canSelectMultiple) {
                        if(selectedSlotIndexes.Count == 0)
                            selectedSlotIndexes.Add(0);
                        selectedSlotIndexes[0] = i;
                    }
                    else {
                        if(selectedSlotIndexes.Contains(i))
                            selectedSlotIndexes.Remove(i);
                        else {
                            selectedSlotIndexes.Add(i);
                            wasSelected = true;
                        }
                    }
                    if(runOnSelect != null)
                        runOnSelect(i, wasSelected);
                }
            }
            return true;
        }
        return false;
    }

    public int getSelectedSlotIndex() {
        if(selectedSlotIndexes == null || selectedSlotIndexes.Count == 0 || !initialized)
            return -1;
        return selectedSlotIndexes[0];
    }
    public List<int> getSelectedSlotIndexes() {
        selectedSlotIndexes.Sort();
        return selectedSlotIndexes;
    }
    public void setSelectedSlotIndex(int i) {
        if(selectedSlotIndexes.Count == 0)
            selectedSlotIndexes.Add(0);
        selectedSlotIndexes[0] = i;
    }
    public void resetSelectedSlotIndexes() {
        selectedSlotIndexes.Clear();
    }
    public void setAllSelectedSlotIndexes() {
        selectedSlotIndexes = new List<int>();
        for(int i = 0; i < slots.Count; i++)
            selectedSlotIndexes.Add(i);
    }
    public GameObject getSelectedSlot() {
        if(getSelectedSlotIndex() < 0 || getSelectedSlotIndex() > slots.Count - 1 || slots.Count <= selectedSlotIndexes[0])
            return null;
        return slots[selectedSlotIndexes[0]];
    }
    public List<GameObject> getSelectedSlots() {
        if(getSelectedSlotIndex() < 0 || getSelectedSlotIndex() > slots.Count - 1)
            return null;

        var temp = new List<GameObject>();
        for(int i = 0; i < selectedSlotIndexes.Count; i++)
            temp.Add(slots[selectedSlotIndexes[i]]);
        return temp;
    }
    public GameObject getEventSelectedSlot() {
        for(int i = 0; i < slots.Count; i++) {
            if(slots[i].gameObject == EventSystem.current.currentSelectedGameObject) {
                return slots[i].gameObject;
            }
        }

        return null;
    }


    public void setRunOnSelect(funcWithIndex f) {
        runOnSelect = f;
    }


    //  instantiates a new slot object at the end of the list
    public GameObject instantiateNewSlot(Color slotColor, string mousedOverInfo = "") {
        if(!initialized)
            init();

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

        //  collider
        if(obj.GetComponent<BoxCollider2D>() != null) {
            obj.GetComponent<BoxCollider2D>().offset = new Vector2(0.0f, 0.0f);
            obj.GetComponent<BoxCollider2D>().size = GetComponentInChildren<GridLayoutGroup>().cellSize;
        }

        //  color
        if(obj.GetComponent<SlotObject>() != null)
            obj.GetComponent<SlotObject>().setImageColor(0, slotColor);
        else
            obj.GetComponent<Image>().color = slotColor;

        if(obj.GetComponent<InfoBearer>() != null)
            obj.GetComponent<InfoBearer>().setInfo(mousedOverInfo);

        return obj;
    }


    //  recycles slot object at index, if no existing slot at the index, creats a new slot object
    public GameObject createSlot(int index, Color slotColor, string mousedOverInfo = "") {
        if(index >= slots.Count)
            return instantiateNewSlot(slotColor, mousedOverInfo);
        GameObject obj;
        obj = slots[index];

        //  color
        if(obj.GetComponent<SlotObject>() != null)
            obj.GetComponent<SlotObject>().setImageColor(0, slotColor);
        else
            obj.GetComponent<Image>().color = slotColor;

        if(obj.GetComponent<InfoBearer>() != null)
            obj.GetComponent<InfoBearer>().setInfo(mousedOverInfo);

        //  collider
        if(obj.GetComponent<BoxCollider2D>() != null) {
            obj.GetComponent<BoxCollider2D>().offset = new Vector2(0.0f, 0.0f);
            obj.GetComponent<BoxCollider2D>().size = GetComponentInChildren<GridLayoutGroup>().cellSize;
        }

        return obj;
    }


    public void deleteSlotsAfterIndex(int index) {
        for(int i = slots.Count - 1; i >= index; i--)
            deleteSlotAtIndex(i);
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

    public void resetScrollValue() {
        transform.GetChild(1).GetComponent<Scrollbar>().value = 1.0f;   //  for some reason, this is top
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

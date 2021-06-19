using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotMenu : MonoBehaviour {
    float slotTopY = 120, slotBotY = -120;
    float slotBuffer = 10.0f;
    float scrollSpeed = 35.0f;

    List<GameObject> slots = new List<GameObject>();
    int selectedSlotIndex = -1;


    public bool run() {
        scrollThoughList();
        if(getMousedOverSlot() != null && Input.GetMouseButtonDown(0)) {
            for(int i = 0; i < slots.Count; i++) {
                if(getMousedOverSlot() == slots[i]) {
                    selectedSlotIndex = i;
                    break;
                }
            }
            return true;
        }
        return false;
    }


    public GameObject getMousedOverSlot() {
        //  return if their are no active slots
        if(slots.Count == 0) {
            return null;
        }

        Ray ray;
        RaycastHit2D hit;

        List<Collider2D> unwantedHits = new List<Collider2D>();

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        //  return if the ray did not hit anything
        if(hit.collider == null)
            return null;

        while(true) {
            //  if the hit is hitting an inventory slot
            foreach(var i in slots) {
                if(hit.collider == i.GetComponent<Collider2D>()) {
                    foreach(var u in unwantedHits)
                        u.enabled = true;
                    return i.gameObject;
                }
            }

            //  hit is not a wanted object
            hit.collider.enabled = false;
            unwantedHits.Add(hit.collider);

            hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            //  hit has run out of hit objects
            if(hit.collider == null) {
                foreach(var i in unwantedHits)
                    i.enabled = true;
                return null;
            }
        }
    }

    public int getSelectedSlotIndex() {
        return selectedSlotIndex;
    }
    public GameObject getSelectedSlot() {
        if(getSelectedSlotIndex() < 0 || getSelectedSlotIndex() > slots.Count - 1)
            return null;
        return slots[selectedSlotIndex];
    }

    public GameObject createNewSlot(int index, GameObject slotPreset, Transform holder, Color spriteColor) {
        GameObject obj = Instantiate(slotPreset, holder);

        //  position and scale
        float step = slotPreset.GetComponent<RectTransform>().rect.height + slotBuffer;
        obj.transform.localPosition = new Vector3(0.0f, slotTopY - (step * index), 0.0f);
        obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        //  width and height
        var x = obj.GetComponent<RectTransform>().rect.width;
        var y = obj.GetComponent<RectTransform>().rect.height;
        obj.GetComponent<BoxCollider2D>().size = new Vector2(x, y);

        //  text and images
        obj.transform.GetChild(1).GetComponent<Image>().color = spriteColor;

        slots.Add(obj);
        return obj;
    }

    public void deleteSlotAtIndex(int index) {
        GameObject sl = slots[index];
        slots.RemoveAt(index);
        Destroy(sl.gameObject);
    }

    public void scrollThoughList() {
        float scroll = Input.mouseScrollDelta.y;

        if(scroll == 0)
            return;

        scrollSlots(-scroll * scrollSpeed);

        //  Top bounds
        if(slots[0].transform.localPosition.y < slotTopY)
            scrollSlots(slotTopY - slots[0].transform.localPosition.y);

        //  Bot bounds
        else if(slots[slots.Count - 1].transform.localPosition.y > slotBotY)
            scrollSlots(slotBotY - slots[slots.Count - 1].transform.localPosition.y);
    }

    void scrollSlots(float val) {
        foreach(var i in slots) {
            i.transform.localPosition = new Vector3(0.0f, i.transform.localPosition.y + val, 0.0f);
        }
    }

    public void clearSlots() {
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

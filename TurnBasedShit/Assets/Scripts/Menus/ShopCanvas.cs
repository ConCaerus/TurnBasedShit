using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopCanvas : MonoBehaviour {
    [SerializeField] TextMeshProUGUI coinCounter, nameText, toggleText, sellReductionText, transactionText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] GameObject slotHolder, slotObject;

    [SerializeField] float slotTopY, slotBotY;
    [SerializeField] float slotBuffer = 10.0f;
    [SerializeField] float scrollSpeed = 1.0f;
    [SerializeField] float sellReduction = 0.05f;

    List<GameObject> slots = new List<GameObject>();
    GameObject selectedSlot;

    //  0 - Buying, 1 - Selling
    int shopState = 0;

    //  0 - Weapon, 1 - armor, 2 - consumable
    int slotState = 0;

    private void Awake() {
        updateInfo();
        createSlots();
    }

    private void Update() {
        scrollThoughList();

        if(Input.GetMouseButtonDown(0) && getMousedOverSlot() != null) {
            selectedSlot = getMousedOverSlot();
            updateInfo();
        }
    }

    GameObject getMousedOverSlot() {
        //  return if their are no active slots
        if(slots.Count == 0)
            return null;

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

    int getSelectedSlotIndex() {
        if(selectedSlot == null || slots.Count == 0)
            return -1;
        for(int i = 0; i < slots.Count; i++) {
            if(slots[i] == selectedSlot)
                return i;
        }
        return -1;
    }

    void updateInfo() {
        coinCounter.text = Inventory.getCoinCount().ToString();
        if(shopState == 0) {
            sellReductionText.enabled = false;
            toggleText.text = "Sell";
            transactionText.text = "Buy";
        }
        else {
            sellReductionText.enabled = true;
            sellReductionText.text = "-" + (sellReduction * 100.0f).ToString() + "%";
            toggleText.text = "Buy";
            transactionText.text = "Sell";
        }

        foreach(var i in slots)
            i.GetComponent<Image>().color = Color.black;

        if(selectedSlot != null) {
            selectedSlot.GetComponent<Image>().color = Color.grey;

            switch(slotState) {
                case 0:
                    nameText.text = Inventory.getWeapon(getSelectedSlotIndex()).w_name;
                    costText.text = Inventory.getWeapon(getSelectedSlotIndex()).w_coinCost.ToString() + "c";
                    if(shopState == 1) {
                        costText.text = ((int)(Inventory.getWeapon(getSelectedSlotIndex()).w_coinCost - (Inventory.getWeapon(getSelectedSlotIndex()).w_coinCost * sellReduction))).ToString() + "c";
                    }
                    break;
            }
        }
    }


    void createSlots() {
        for(int i = 0; i < slots.Count; i++)
            Destroy(slots[i].gameObject);
        slots.Clear();
        //  Buy state
        if(shopState == 0) {

        }

        //  Sell state
        if(shopState == 1) {
            switch(slotState) {
                //  Weapons
                case 0:
                    float step = slotObject.GetComponent<RectTransform>().rect.height + slotBuffer;
                    for(int i = 0; i < Inventory.getWeaponCount(); i++) {
                        var obj = Instantiate(slotObject, slotHolder.transform);
                        obj.transform.localPosition = new Vector3(0.0f, slotTopY - (step * i), 0.0f);
                        obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                        obj.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
                        obj.transform.GetChild(1).GetComponent<Image>().sprite = Inventory.getWeapon(i).w_sprite.getSprite();

                        var x = obj.GetComponent<RectTransform>().rect.width;
                        var y = obj.GetComponent<RectTransform>().rect.height;
                        obj.GetComponent<BoxCollider2D>().size = new Vector2(x, y);

                        slots.Add(obj);
                    }
                    break;

                //  Armor
                case 1:
                    break;

                //  Consumables
                case 2:
                    break;
            }

            selectedSlot = slots[0];
        }

        scrollThoughList();
        updateInfo();
    }

    void scrollThoughList() {
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

    //  buttons 
    public void toggleState() {
        shopState = Mathf.Abs(shopState - 1);

        createSlots();
        updateInfo();
    }

    public void transaction() {
        if(selectedSlot != null) {
            if(shopState == 1) {
                switch(slotState) {
                    //  Weapon
                    case 0:
                        Inventory.addCoins((int)(Inventory.getWeapon(getSelectedSlotIndex()).w_coinCost - Inventory.getWeapon(getSelectedSlotIndex()).w_coinCost * sellReduction));
                        Inventory.removeWeapon(getSelectedSlotIndex());
                        break;
                }
            }
            createSlots();
            updateInfo();
        }
    }
}
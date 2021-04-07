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

    List<GameObject> slots = new List<GameObject>();
    GameObject selectedSlot;

    //  0 - Buying, 1 - Selling
    int shopState = 0;

    //  0 - Weapon, 1 - armor, 2 - consumable, 3 - item
    int slotState = 0;

    private void Start() {
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
        Town currentTown = FindObjectOfType<TownInstance>().town;
        coinCounter.text = Inventory.getCoinCount().ToString();
        if(shopState == 0) {
            sellReductionText.enabled = false;
            toggleText.text = "Sell";
            transactionText.text = "Buy";
        }
        else {
            sellReductionText.enabled = true;
            sellReductionText.text = "-" + (currentTown.shopSellReduction * 100.0f).ToString("0.0") + "%";
            toggleText.text = "Buy";
            transactionText.text = "Sell";
        }

        if(slots.Count > 0) {
            foreach(var i in slots)
                i.GetComponent<Image>().color = Color.black;
        }

        if(selectedSlot != null && getSelectedSlotIndex() > -1) {
            selectedSlot.GetComponent<Image>().color = Color.grey;

            switch(slotState) {
                //  Weapon
                case 0:
                    //  Buying 
                    if(shopState == 0) {
                        nameText.text = ((Weapon)ShopInventory.getObject(currentTown.t_index, getSelectedSlotIndex(), typeof(Weapon))).w_name;
                        costText.text = getBuyPrice(((Weapon)ShopInventory.getObject(currentTown.t_index, getSelectedSlotIndex(), typeof(Weapon))).w_coinCost).ToString() + "c";
                        break;
                    }

                    //  Selling
                    else if(shopState == 1) {
                        nameText.text = Inventory.getWeapon(getSelectedSlotIndex()).w_name;
                        costText.text = getSellPrice(Inventory.getWeapon(getSelectedSlotIndex()).w_coinCost).ToString() + "c";
                        break;
                    }
                    break;

                //  Armor
                case 1://  Buying 
                    if(shopState == 0) {
                        nameText.text = ((Armor)ShopInventory.getObject(currentTown.t_index, getSelectedSlotIndex(), typeof(Armor))).a_name;
                        costText.text = getBuyPrice(((Armor)ShopInventory.getObject(currentTown.t_index, getSelectedSlotIndex(), typeof(Armor))).a_coinCost).ToString() + "c";
                        break;
                    }

                    //  Selling
                    else if(shopState == 1) {
                        nameText.text = Inventory.getArmor(getSelectedSlotIndex()).a_name;
                        costText.text = getSellPrice(Inventory.getArmor(getSelectedSlotIndex()).a_coinCost).ToString() + "c";
                        break;
                    }
                    break;

                //  Consumable
                case 2:
                    //  Buying 
                    if(shopState == 0) {
                        nameText.text = ((Consumable)ShopInventory.getObject(currentTown.t_index, getSelectedSlotIndex(), typeof(Consumable))).c_name;
                        costText.text = getBuyPrice(((Consumable)ShopInventory.getObject(currentTown.t_index, getSelectedSlotIndex(), typeof(Consumable))).c_coinCost).ToString() + "c";
                        break;
                    }

                    //  Selling
                    else if(shopState == 1) {
                        nameText.text = (Inventory.getConsumable(getSelectedSlotIndex())).c_name;
                        costText.text = getSellPrice(Inventory.getConsumable(getSelectedSlotIndex()).c_coinCost).ToString() + "c";
                        break;
                    }
                    break;

                //  Item
                case 3:
                    //  Buying 
                    if(shopState == 0) {
                        nameText.text = ((Item)ShopInventory.getObject(currentTown.t_index, getSelectedSlotIndex(), typeof(Item))).i_name;
                        costText.text = getBuyPrice(((Item)ShopInventory.getObject(currentTown.t_index, getSelectedSlotIndex(), typeof(Item))).i_coinCost).ToString() + "c";
                        break;
                    }

                    //  Selling
                    else if(shopState == 1) {
                        nameText.text = Inventory.getItem(getSelectedSlotIndex()).i_name;
                        costText.text = getSellPrice(Inventory.getItem(getSelectedSlotIndex()).i_coinCost).ToString() + "c";
                        break;
                    }
                    break;
            }
        }
        else {
            //  no selected slot
            nameText.text = "";
            costText.text = "";
        }
    }


    void createSlots() {
        for(int i = 0; i < slots.Count; i++)
            Destroy(slots[i].gameObject);
        slots.Clear();

        Town currentTown = FindObjectOfType<TownInstance>().town;

        //  Buy state
        if(shopState == 0) {
            switch(slotState) {
                //  Weapons
                case 0:
                    int weaponCount = ShopInventory.getTypeCount(currentTown.t_index, typeof(Weapon));
                    for(int i = 0; i < weaponCount; i++)
                        createNewSlot(i, ((Weapon)ShopInventory.getObject(currentTown.t_index, i, typeof(Weapon))).w_sprite.getSprite());
                    break;

                //  Armor
                case 1:
                    int armorCount = ShopInventory.getTypeCount(currentTown.t_index, typeof(Armor));
                    for(int i = 0; i < armorCount; i++)
                        createNewSlot(i, ((Armor)ShopInventory.getObject(currentTown.t_index, i, typeof(Armor))).a_sprite.getSprite());
                    break;

                //  Consumable
                case 2:
                    int consumableCount = ShopInventory.getTypeCount(currentTown.t_index, typeof(Consumable));
                    for(int i = 0; i < consumableCount; i++)
                        createNewSlot(i, ((Consumable)ShopInventory.getObject(currentTown.t_index, i, typeof(Consumable))).c_sprite.getSprite());
                    break;

                //  Item
                case 3:
                    int itemCount = ShopInventory.getTypeCount(currentTown.t_index, typeof(Item));
                    for(int i = 0; i < itemCount; i++)
                        createNewSlot(i, ((Item)ShopInventory.getObject(currentTown.t_index, i, typeof(Item))).i_sprite.getSprite());
                    break;
            }
        }

        //  Sell state
        if(shopState == 1) {
            switch(slotState) {
                //  Weapons
                case 0:
                    if(Inventory.getTypeCount(typeof(Weapon)) == 0)
                        break;

                    for(int i = 0; i < Inventory.getTypeCount(typeof(Weapon)); i++)
                        createNewSlot(i, Inventory.getWeapon(i).w_sprite.getSprite());
                    break;

                //  Armor
                case 1:
                    if(Inventory.getTypeCount(typeof(Armor)) == 0)
                        break;

                    for(int i = 0; i < Inventory.getTypeCount(typeof(Armor)); i++)
                        createNewSlot(i, Inventory.getArmor(i).a_sprite.getSprite());
                    break;

                //  Consumable
                case 2:
                    if(Inventory.getTypeCount(typeof(Consumable)) == 0)
                        break;

                    for(int i = 0; i < Inventory.getTypeCount(typeof(Consumable)); i++)
                        createNewSlot(i, Inventory.getConsumable(i).c_sprite.getSprite());
                    break;

                //  Item
                case 3:
                    if(Inventory.getTypeCount(typeof(Item)) == 0)
                        break;

                    for(int i = 0; i < Inventory.getTypeCount(typeof(Item)); i++)
                        createNewSlot(i, Inventory.getItem(i).i_sprite.getSprite());
                    break;
            }

            selectedSlot = slots[0];
        }

        if(slots.Count > 0)
            scrollThoughList();
        updateInfo();
    }
    GameObject createNewSlot(int index, Sprite sp) {
        var obj = Instantiate(slotObject, slotHolder.transform);

        //  position and scale
        float step = slotObject.GetComponent<RectTransform>().rect.height + slotBuffer;
        obj.transform.localPosition = new Vector3(0.0f, slotTopY - (step * index), 0.0f);
        obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        //  width and height
        var x = obj.GetComponent<RectTransform>().rect.width;
        var y = obj.GetComponent<RectTransform>().rect.height;
        obj.GetComponent<BoxCollider2D>().size = new Vector2(x, y);

        //  text and images
        obj.GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();
        obj.transform.GetChild(1).GetComponent<Image>().sprite = sp;

        slots.Add(obj);
        return obj;
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

    public void setSlotState(int i) {
        slotState = i;
        createSlots();
        updateInfo();
    }

    public void transaction() {
        if(selectedSlot != null && getSelectedSlotIndex() > -1) {
            int townIndex = FindObjectOfType<TownInstance>().town.t_index;
            //  Buying
            if(shopState == 0) {
                switch(slotState) {
                    //  Weapon
                    case 0:
                        Weapon weaponInSlot = (Weapon)ShopInventory.getObject(townIndex, getSelectedSlotIndex(), typeof(Weapon));
                        var wPrice = getBuyPrice(weaponInSlot.w_coinCost);

                        if(Inventory.getCoinCount() >= wPrice) {
                            Inventory.removeCoins(wPrice);
                            Inventory.addWeapon(weaponInSlot);
                            ShopInventory.removeFromIndex(townIndex, getSelectedSlotIndex(), typeof(Weapon));
                        }
                        break;

                    //  Armor
                    case 1:
                        Armor armorInSlot = (Armor)ShopInventory.getObject(townIndex, getSelectedSlotIndex(), typeof(Armor));
                        var aPrice = getBuyPrice(armorInSlot.a_coinCost);

                        if(Inventory.getCoinCount() >= aPrice) {
                            Inventory.removeCoins(aPrice);
                            Inventory.addArmor(armorInSlot);
                            ShopInventory.removeFromIndex(townIndex, getSelectedSlotIndex(), typeof(Armor));
                        }
                        break;

                    //  Consumable
                    case 2:
                        Consumable consumableInSlot = (Consumable)ShopInventory.getObject(townIndex, getSelectedSlotIndex(), typeof(Consumable));
                        var cPrice = getBuyPrice(consumableInSlot.c_coinCost);

                        if(Inventory.getCoinCount() >= cPrice) {
                            Inventory.removeCoins(cPrice);
                            Inventory.addConsumable(consumableInSlot);
                            ShopInventory.removeFromIndex(townIndex, getSelectedSlotIndex(), typeof(Consumable));
                        }
                        break;

                    //  Item
                    case 3:
                        Item itemInSlot = (Item)ShopInventory.getObject(townIndex, getSelectedSlotIndex(), typeof(Item));
                        var iPrice = getBuyPrice(itemInSlot.i_coinCost);

                        if(Inventory.getCoinCount() >= iPrice) {
                            Inventory.removeCoins(iPrice);
                            Inventory.addItem(itemInSlot);
                            ShopInventory.removeFromIndex(townIndex, getSelectedSlotIndex(), typeof(Item));
                        }
                        break;
                }
            }

            //  Selling
            else if(shopState == 1) {
                switch(slotState) {
                    //  Weapon
                    case 0:
                        Inventory.addCoins(getSellPrice(Inventory.getWeapon(getSelectedSlotIndex()).w_coinCost));
                        Inventory.removeWeapon(getSelectedSlotIndex());
                        break;

                    //  Armor
                    case 1:
                        Inventory.addCoins(getSellPrice(Inventory.getArmor(getSelectedSlotIndex()).a_coinCost));
                        Inventory.removeArmor(getSelectedSlotIndex());
                        break;

                    //  Consumable
                    case 2:
                        Inventory.addCoins(getSellPrice(Inventory.getConsumable(getSelectedSlotIndex()).c_coinCost));
                        Inventory.removeConsumable(getSelectedSlotIndex());
                        break;

                    //  Item
                    case 3:
                        Inventory.addCoins(getSellPrice(Inventory.getItem(getSelectedSlotIndex()).i_coinCost));
                        Inventory.removeItem(getSelectedSlotIndex());
                        break;
                }
            }
            createSlots();
            updateInfo();
        }
    }

    public int getBuyPrice(float originalPrice) {
        return (int)Mathf.Clamp(originalPrice + (originalPrice * FindObjectOfType<TownInstance>().town.shopPriceMod), 1, Mathf.Infinity);
    }

    public int getSellPrice(float originalPrice) {
        return (int)Mathf.Clamp(originalPrice - (originalPrice * FindObjectOfType<TownInstance>().town.shopSellReduction), 1, Mathf.Infinity);
    }
}
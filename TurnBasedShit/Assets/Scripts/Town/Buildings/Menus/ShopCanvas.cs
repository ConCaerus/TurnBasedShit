using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ShopCanvas : MonoBehaviour {
    [SerializeField] GameObject canvas;
    [SerializeField] TextMeshProUGUI coinCounter, nameText, toggleText, sellReductionText, transactionText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] GameObject slotHolder, slotObject;

    [SerializeField] float slotTopY = 120, slotBotY = -120;
    [SerializeField] float slotBuffer = 10.0f;
    [SerializeField] float scrollSpeed = 35.0f;

    float showTime = 0.15f;

    List<GameObject> slots = new List<GameObject>();
    GameObject selectedSlot;

    Town currentTown;
    ShopBuilding currentShop;

    //  0 - Buying, 1 - Selling
    int shopState = 0;

    //  0 - Weapon, 1 - armor, 2 - consumable, 3 - item, 4 - slave
    int slotState = 0;

    private void Start() {
        DOTween.Init();
        currentTown = GameInfo.getCurrentLocationAsTown().town;
        currentShop = GameInfo.getCurrentLocationAsTown().town.getShop();
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

    public void updateInfo() {
        coinCounter.text = Inventory.getCoinCount().ToString();
        if(shopState == 0) {
            sellReductionText.enabled = false;
            toggleText.text = "Sell";
            transactionText.text = "Buy";
        }
        else {
            sellReductionText.enabled = true;
            sellReductionText.text = "-" + (currentShop.sellReduction * 100.0f).ToString("0.0") + "%";
            toggleText.text = "Buy";
            transactionText.text = "Sell";
        }

        if(slots.Count > 0) {
            foreach(var i in slots)
                i.GetComponent<Image>().color = Color.black;
        }

        if(selectedSlot != null && getSelectedSlotIndex() > -1 && slots.Count > 0) {
            selectedSlot.GetComponent<Image>().color = Color.grey;

            switch(slotState) {
                //  Weapon
                case 0:
                    //  Buying 
                    if(shopState == 0) {
                        Weapon w = ShopInventory.getWeapon(currentTown.t_instanceID, getSelectedSlotIndex());
                        nameText.text = w.w_name;
                        costText.text = getBuyPrice(w.w_coinCost).ToString() + "c";
                        break;
                    }

                    //  Selling
                    else if(shopState == 1) {
                        Weapon w = Inventory.getWeapon(getSelectedSlotIndex());
                        nameText.text = w.w_name;
                        costText.text = getSellPrice(w.w_coinCost).ToString() + "c";
                        break;
                    }
                    break;

                //  Armor
                case 1://  Buying 
                    if(shopState == 0) {
                        Armor a = ShopInventory.getArmor(currentTown.t_instanceID, getSelectedSlotIndex());
                        nameText.text = a.a_name;
                        costText.text = getBuyPrice(a.a_coinCost).ToString() + "c";
                        break;
                    }

                    //  Selling
                    else if(shopState == 1) {
                        Armor a = Inventory.getArmor(getSelectedSlotIndex());
                        nameText.text = a.a_name;
                        costText.text = getSellPrice(a.a_coinCost).ToString() + "c";
                        break;
                    }
                    break;

                //  Consumable
                case 2:
                    //  Buying 
                    if(shopState == 0) {
                        Consumable c = ShopInventory.getConsumable(currentTown.t_instanceID, getSelectedSlotIndex());
                        nameText.text = c.c_name;
                        costText.text = getBuyPrice(c.c_coinCost).ToString() + "c";
                        break;
                    }

                    //  Selling
                    else if(shopState == 1) {
                        Consumable c = Inventory.getConsumable(getSelectedSlotIndex());
                        nameText.text = c.c_name;
                        costText.text = getSellPrice(c.c_coinCost).ToString() + "c";
                        break;
                    }
                    break;

                //  Item
                case 3:
                    //  Buying 
                    if(shopState == 0) {
                        Item i = ShopInventory.getItem(currentTown.t_instanceID, getSelectedSlotIndex());
                        nameText.text = i.i_name;
                        costText.text = getBuyPrice(i.i_coinCost).ToString() + "c";
                        break;
                    }

                    //  Selling
                    else if(shopState == 1) {
                        Item i = Inventory.getItem(getSelectedSlotIndex());
                        nameText.text = i.i_name;
                        costText.text = getSellPrice(i.i_coinCost).ToString() + "c";
                        break;
                    }
                    break;

                //  Slave
                case 4:
                    //  Buying 
                    if(shopState == 0) {
                        UnitStats stats = ShopInventory.getSlave(currentTown.t_instanceID, getSelectedSlotIndex());
                        nameText.text = stats.u_name;
                        costText.text = getBuyPrice(stats.determineCost()).ToString() + "c";
                        break;
                    }

                    //  Selling
                    else if(shopState == 1) {
                        UnitStats stats = Party.getMemberStats(getSelectedSlotIndex());
                        nameText.text = stats.u_name;
                        costText.text = getSellPrice(stats.determineCost()).ToString() + "c";
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

        Town currentTown = GameInfo.getCurrentLocationAsTown().town;

        //  Buy state
        if(shopState == 0) {
            switch(slotState) {
                //  Weapons
                case 0:
                    int weaponCount = ShopInventory.getTypeCount(currentTown.t_instanceID, typeof(Weapon));
                    if(weaponCount <= 0)
                        break;
                    for(int i = 0; i < weaponCount; i++)
                        createNewSlot(i, FindObjectOfType<PresetLibrary>().getWeaponSprite(ShopInventory.getWeapon(currentTown.t_instanceID, i)).sprite, Color.white);
                    break;

                //  Armor
                case 1:
                    int armorCount = ShopInventory.getTypeCount(currentTown.t_instanceID, typeof(Armor));
                    if(armorCount <= 0)
                        break;
                    for(int i = 0; i < armorCount; i++)
                        createNewSlot(i, FindObjectOfType<PresetLibrary>().getArmorSprite(ShopInventory.getArmor(currentTown.t_instanceID, i)).sprite, Color.white);
                    break;

                //  Consumable
                case 2:
                    int consumableCount = ShopInventory.getTypeCount(currentTown.t_instanceID, typeof(Consumable));
                    if(consumableCount <= 0)
                        break;
                    for(int i = 0; i < consumableCount; i++)
                        createNewSlot(i, FindObjectOfType<PresetLibrary>().getConsumableSprite(ShopInventory.getConsumable(currentTown.t_instanceID, i)).sprite, Color.white);
                    break;

                //  Item
                case 3:
                    int itemCount = ShopInventory.getTypeCount(currentTown.t_instanceID, typeof(Item));
                    if(itemCount <= 0)
                        break;
                    for(int i = 0; i < itemCount; i++)
                        createNewSlot(i, FindObjectOfType<PresetLibrary>().getItemSprite(ShopInventory.getItem(currentTown.t_instanceID, i)).sprite, Color.white);
                    break;

                //  Slave
                case 4:
                    int slaveCount = ShopInventory.getTypeCount(currentTown.t_instanceID, typeof(UnitStats));
                    if(slaveCount <= 0)
                        break;
                    for(int i = 0; i < slaveCount; i++) {
                        //createNewSlot(i, FindObjectOfType<PresetLibrary>().getPlayerUnitSprite().sprite, ShopInventory.getSlave(currentTown.t_instanceID, i).u_color);
                    }
                    break;
            }
        }

        //  Sell state
        if(shopState == 1) {
            switch(slotState) {
                //  Weapons
                case 0:
                    if(Inventory.getWeaponCount() <= 0)
                        break;

                    for(int i = 0; i < Inventory.getWeaponCount(); i++)
                        createNewSlot(i, FindObjectOfType<PresetLibrary>().getWeaponSprite(Inventory.getWeapon(i)).sprite, Color.white);
                    break;

                //  Armor
                case 1:
                    if(Inventory.getArmorCount() <= 0)
                        break;

                    for(int i = 0; i < Inventory.getArmorCount(); i++)
                        createNewSlot(i, FindObjectOfType<PresetLibrary>().getArmorSprite(Inventory.getArmor(i)).sprite, Color.white);
                    break;

                //  Consumable
                case 2:
                    if(Inventory.getConsumabeCount() <= 0)
                        break;

                    for(int i = 0; i < Inventory.getConsumabeCount(); i++)
                        createNewSlot(i, FindObjectOfType<PresetLibrary>().getConsumableSprite(Inventory.getConsumable(i)).sprite, Color.white);
                    break;

                //  Item
                case 3:
                    if(Inventory.getItemCount() <= 0)
                        break;

                    for(int i = 0; i < Inventory.getItemCount(); i++)
                        createNewSlot(i, FindObjectOfType<PresetLibrary>().getItemSprite(Inventory.getItem(i)).sprite, Color.white);
                    break;

                //  Slave
                case 4:
                    if(Party.getMemberCount() <= 0)
                        break;

                    //for(int i = 0; i < Party.getMemberCount(); i++)
                        //createNewSlot(i, FindObjectOfType<PresetLibrary>().getPlayerUnitSprite().sprite, Party.getMemberStats(i).u_color);
                    break;
            }
        }

        if(slots.Count > 0)
            scrollThoughList();
        updateInfo();
    }
    GameObject createNewSlot(int index, Sprite sp, Color spriteColor) {
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
        obj.transform.GetChild(1).GetComponent<Image>().color = spriteColor;

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
            int townIndex = GameInfo.getCurrentLocationAsTown().town.t_instanceID;
            //  Buying
            if(shopState == 0) {
                switch(slotState) {
                    //  Weapon
                    case 0:
                        Weapon weaponInSlot = ShopInventory.getWeapon(townIndex, getSelectedSlotIndex());
                        var wPrice = getBuyPrice(weaponInSlot.w_coinCost);

                        if(Inventory.getCoinCount() >= wPrice) {
                            Inventory.removeCoins(wPrice);
                            Inventory.addWeapon(weaponInSlot);
                            ShopInventory.removeWeapon(townIndex, getSelectedSlotIndex());
                        }
                        break;

                    //  Armor
                    case 1:
                        Armor armorInSlot = ShopInventory.getArmor(townIndex, getSelectedSlotIndex());
                        var aPrice = getBuyPrice(armorInSlot.a_coinCost);

                        if(Inventory.getCoinCount() >= aPrice) {
                            Inventory.removeCoins(aPrice);
                            Inventory.addArmor(armorInSlot);
                            ShopInventory.removeArmor(townIndex, getSelectedSlotIndex());
                        }
                        break;

                    //  Consumable
                    case 2:
                        Consumable consumableInSlot = ShopInventory.getConsumable(townIndex, getSelectedSlotIndex());
                        var cPrice = getBuyPrice(consumableInSlot.c_coinCost);

                        if(Inventory.getCoinCount() >= cPrice) {
                            Inventory.removeCoins(cPrice);
                            Inventory.addConsumable(consumableInSlot);
                            ShopInventory.removeConsumable(townIndex, getSelectedSlotIndex());
                        }
                        break;

                    //  Item
                    case 3:
                        Item itemInSlot = ShopInventory.getItem(townIndex, getSelectedSlotIndex());
                        var iPrice = getBuyPrice(itemInSlot.i_coinCost);

                        if(Inventory.getCoinCount() >= iPrice) {
                            Inventory.removeCoins(iPrice);
                            Inventory.addItem(itemInSlot);
                            ShopInventory.removeItem(townIndex, getSelectedSlotIndex());
                        }
                        break;

                    //  Slaves
                    case 4:
                        UnitStats statsInSlot = ShopInventory.getSlave(townIndex, getSelectedSlotIndex());
                        var uPrice = statsInSlot.determineCost();

                        if(Inventory.getCoinCount() >= uPrice) {
                            Inventory.removeCoins(uPrice);
                            Party.addNewUnit(statsInSlot);
                            ShopInventory.removeSlave(townIndex, getSelectedSlotIndex());
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

                    //  Slave
                    case 4:
                        Inventory.addCoins(getSellPrice(Party.getMemberStats(getSelectedSlotIndex()).determineCost()));
                        Party.removeUnit(getSelectedSlotIndex());
                        break;
                }
            }
            selectedSlot = null;
            createSlots();
            updateInfo();
        }
    }

    public int getBuyPrice(float originalPrice) {
        return (int)Mathf.Clamp(originalPrice + (originalPrice * currentShop.priceMod), 1, Mathf.Infinity);
    }

    public int getSellPrice(float originalPrice) {
        return (int)Mathf.Clamp(originalPrice - (originalPrice * currentShop.sellReduction), 1, Mathf.Infinity);
    }

    public void resetShop() {
        ShopInventory.populateShop(GameInfo.getCurrentLocationAsTown().town.t_instanceID, GameInfo.getCurrentDiff(), FindObjectOfType<PresetLibrary>());

        createSlots();
        updateInfo();
    }

    public void exit() {
        FindObjectOfType<LocationMovement>().deinteract();
    }

    public void showCanvas() {
        canvas.SetActive(true);
        canvas.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        canvas.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), showTime);
    }
    public void hideCanvas() {
        StartCoroutine(hider());
    }
    public IEnumerator hider() {
        canvas.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), showTime);
        yield return new WaitForSeconds(showTime);
        canvas.SetActive(false);
    }
}
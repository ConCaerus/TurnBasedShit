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
    float showTime = 0.15f;

    [SerializeField] SlotMenu slot;

    Town currentTown;
    ShopBuilding currentShop;

    //  0 - Buying, 1 - Selling
    int shopState = 0;

    //  0 - Weapon, 1 - armor, 2 - consumable, 3 - item, 4 - slave
    int slotState = 0;

    private void Start() {
        DOTween.Init();
        slot.init();
        hideCanvas();
        currentTown = GameInfo.getCurrentLocationAsTown().town;
        currentShop = GameInfo.getCurrentLocationAsTown().town.getShop();
        updateInfo();
        createSlots();
    }

    private void Update() {
        if(slot.run())
            updateInfo();
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

        if(slot.getSlots().Count > 0) {
            foreach(var i in slot.getSlots())
                i.GetComponent<Image>().color = Color.black;
        }

        if(slot.getSelectedSlot() != null && slot.getSelectedSlotIndex() > -1) {
            switch(slotState) {
                //  Weapon
                case 0:
                    //  Buying 
                    if(shopState == 0) {
                        Weapon w = ShopInventory.getWeapon(currentTown.t_instanceID, slot.getSelectedSlotIndex());
                        nameText.text = w.w_name;
                        costText.text = getBuyPrice(w.w_coinCost).ToString() + "c";
                        break;
                    }

                    //  Selling
                    else if(shopState == 1) {
                        Weapon w = Inventory.getWeapon(slot.getSelectedSlotIndex());
                        nameText.text = w.w_name;
                        costText.text = getSellPrice(w.w_coinCost).ToString() + "c";
                        break;
                    }
                    break;

                //  Armor
                case 1://  Buying 
                    if(shopState == 0) {
                        Armor a = ShopInventory.getArmor(currentTown.t_instanceID, slot.getSelectedSlotIndex());
                        nameText.text = a.a_name;
                        costText.text = getBuyPrice(a.a_coinCost).ToString() + "c";
                        break;
                    }

                    //  Selling
                    else if(shopState == 1) {
                        Armor a = Inventory.getArmor(slot.getSelectedSlotIndex());
                        nameText.text = a.a_name;
                        costText.text = getSellPrice(a.a_coinCost).ToString() + "c";
                        break;
                    }
                    break;

                //  Consumable
                case 2:
                    //  Buying 
                    if(shopState == 0) {
                        Consumable c = ShopInventory.getConsumable(currentTown.t_instanceID, slot.getSelectedSlotIndex());
                        nameText.text = c.c_name;
                        costText.text = getBuyPrice(c.c_coinCost).ToString() + "c";
                        break;
                    }

                    //  Selling
                    else if(shopState == 1) {
                        Consumable c = Inventory.getConsumable(slot.getSelectedSlotIndex());
                        nameText.text = c.c_name;
                        costText.text = getSellPrice(c.c_coinCost).ToString() + "c";
                        break;
                    }
                    break;

                //  Item
                case 3:
                    //  Buying 
                    if(shopState == 0) {
                        Item i = ShopInventory.getItem(currentTown.t_instanceID, slot.getSelectedSlotIndex());
                        nameText.text = i.i_name;
                        costText.text = getBuyPrice(i.i_coinCost).ToString() + "c";
                        break;
                    }

                    //  Selling
                    else if(shopState == 1) {
                        Item i = Inventory.getItem(slot.getSelectedSlotIndex());
                        nameText.text = i.i_name;
                        costText.text = getSellPrice(i.i_coinCost).ToString() + "c";
                        break;
                    }
                    break;

                //  Slave
                case 4:
                    //  Buying 
                    if(shopState == 0) {
                        UnitStats stats = ShopInventory.getSlave(currentTown.t_instanceID, slot.getSelectedSlotIndex());
                        nameText.text = stats.u_name;
                        costText.text = getBuyPrice(stats.determineCost()).ToString() + "c";
                        break;
                    }

                    //  Selling
                    else if(shopState == 1) {
                        UnitStats stats = Party.getMemberStats(slot.getSelectedSlotIndex());
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
        slot.destroySlots();
        Town currentTown = GameInfo.getCurrentLocationAsTown().town;

        //  Buy state
        if(shopState == 0) {
            switch(slotState) {
                //  Weapons
                case 0:
                    int weaponCount = ShopInventory.getTypeCount(currentTown.t_instanceID, typeof(Weapon));
                    if(weaponCount <= 0)
                        break;
                    for(int i = 0; i < weaponCount; i++) {
                        var obj = slot.createSlot(i, Color.white);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(ShopInventory.getWeapon(currentTown.t_instanceID, i)).sprite;
                    }
                    break;

                //  Armor
                case 1:
                    int armorCount = ShopInventory.getTypeCount(currentTown.t_instanceID, typeof(Armor));
                    if(armorCount <= 0)
                        break;
                    for(int i = 0; i < armorCount; i++) {
                        var obj = slot.createSlot(i, Color.white);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(ShopInventory.getArmor(currentTown.t_instanceID, i)).sprite;
                    }
                    break;

                //  Consumable
                case 2:
                    int consumableCount = ShopInventory.getTypeCount(currentTown.t_instanceID, typeof(Consumable));
                    if(consumableCount <= 0)
                        break;
                    for(int i = 0; i < consumableCount; i++) {
                        var obj = slot.createSlot(i, Color.white);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getConsumableSprite(ShopInventory.getConsumable(currentTown.t_instanceID, i)).sprite;
                    }
                    break;

                //  Item
                case 3:
                    int itemCount = ShopInventory.getTypeCount(currentTown.t_instanceID, typeof(Item));
                    if(itemCount <= 0)
                        break;
                    for(int i = 0; i < itemCount; i++) {
                        var obj = slot.createSlot(i, Color.white);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getItemSprite(ShopInventory.getItem(currentTown.t_instanceID, i)).sprite;
                    }
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

                    for(int i = 0; i < Inventory.getWeaponCount(); i++) {
                        var obj = slot.createSlot(i, Color.white);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(Inventory.getWeapon(i)).sprite;
                    }
                    break;

                //  Armor
                case 1:
                    if(Inventory.getArmorCount() <= 0)
                        break;

                    for(int i = 0; i < Inventory.getArmorCount(); i++) {
                        var obj = slot.createSlot(i, Color.white);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(Inventory.getArmor(i)).sprite;
                    }
                    break;

                //  Consumable
                case 2:
                    if(Inventory.getConsumabeCount() <= 0)
                        break;

                    for(int i = 0; i < Inventory.getConsumabeCount(); i++) {
                        var obj = slot.createSlot(i, Color.white);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getConsumableSprite(Inventory.getConsumable(i)).sprite;
                    }
                    break;

                //  Item
                case 3:
                    if(Inventory.getItemCount() <= 0)
                        break;

                    for(int i = 0; i < Inventory.getItemCount(); i++) {
                        var obj = slot.createSlot(i, Color.white);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getItemSprite(Inventory.getItem(i)).sprite;
                    }
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
        updateInfo();
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
        if(slot.getSelectedSlot() != null && slot.getSelectedSlotIndex() > -1) {
            int townIndex = GameInfo.getCurrentLocationAsTown().town.t_instanceID;
            //  Buying
            if(shopState == 0) {
                switch(slotState) {
                    //  Weapon
                    case 0:
                        Weapon weaponInSlot = ShopInventory.getWeapon(townIndex, slot.getSelectedSlotIndex());
                        var wPrice = getBuyPrice(weaponInSlot.w_coinCost);

                        if(Inventory.getCoinCount() >= wPrice) {
                            Inventory.removeCoins(wPrice);
                            Inventory.addWeapon(weaponInSlot);
                            ShopInventory.removeWeapon(townIndex, slot.getSelectedSlotIndex());
                        }
                        break;

                    //  Armor
                    case 1:
                        Armor armorInSlot = ShopInventory.getArmor(townIndex, slot.getSelectedSlotIndex());
                        var aPrice = getBuyPrice(armorInSlot.a_coinCost);

                        if(Inventory.getCoinCount() >= aPrice) {
                            Inventory.removeCoins(aPrice);
                            Inventory.addArmor(armorInSlot);
                            ShopInventory.removeArmor(townIndex, slot.getSelectedSlotIndex());
                        }
                        break;

                    //  Consumable
                    case 2:
                        Consumable consumableInSlot = ShopInventory.getConsumable(townIndex, slot.getSelectedSlotIndex());
                        var cPrice = getBuyPrice(consumableInSlot.c_coinCost);

                        if(Inventory.getCoinCount() >= cPrice) {
                            Inventory.removeCoins(cPrice);
                            Inventory.addConsumable(consumableInSlot);
                            ShopInventory.removeConsumable(townIndex, slot.getSelectedSlotIndex());
                        }
                        break;

                    //  Item
                    case 3:
                        Item itemInSlot = ShopInventory.getItem(townIndex, slot.getSelectedSlotIndex());
                        var iPrice = getBuyPrice(itemInSlot.i_coinCost);

                        if(Inventory.getCoinCount() >= iPrice) {
                            Inventory.removeCoins(iPrice);
                            Inventory.addItem(itemInSlot);
                            ShopInventory.removeItem(townIndex, slot.getSelectedSlotIndex());
                        }
                        break;

                    //  Slaves
                    case 4:
                        UnitStats statsInSlot = ShopInventory.getSlave(townIndex, slot.getSelectedSlotIndex());
                        var uPrice = statsInSlot.determineCost();

                        if(Inventory.getCoinCount() >= uPrice) {
                            Inventory.removeCoins(uPrice);
                            Party.addUnit(statsInSlot);
                            ShopInventory.removeSlave(townIndex, slot.getSelectedSlotIndex());
                        }
                        break;

                }
            }

            //  Selling
            else if(shopState == 1) {
                switch(slotState) {
                    //  Weapon
                    case 0:
                        Inventory.addCoins(getSellPrice(Inventory.getWeapon(slot.getSelectedSlotIndex()).w_coinCost));
                        Inventory.removeWeapon(slot.getSelectedSlotIndex());
                        break;

                    //  Armor
                    case 1:
                        Inventory.addCoins(getSellPrice(Inventory.getArmor(slot.getSelectedSlotIndex()).a_coinCost));
                        Inventory.removeArmor(slot.getSelectedSlotIndex());
                        break;

                    //  Consumable
                    case 2:
                        Inventory.addCoins(getSellPrice(Inventory.getConsumable(slot.getSelectedSlotIndex()).c_coinCost));
                        Inventory.removeConsumable(slot.getSelectedSlotIndex());
                        break;

                    //  Item
                    case 3:
                        Inventory.addCoins(getSellPrice(Inventory.getItem(slot.getSelectedSlotIndex()).i_coinCost));
                        Inventory.removeItem(slot.getSelectedSlotIndex());
                        break;

                    //  Slave
                    case 4:
                        Inventory.addCoins(getSellPrice(Party.getMemberStats(slot.getSelectedSlotIndex()).determineCost()));
                        Party.removeUnit(slot.getSelectedSlotIndex());
                        break;
                }
            }
            slot.setSelectedSlotIndex(-1);
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
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

    //  0 - Weapon, 1 - armor, 2 - usable, 3 - unusable, 4 - item, 5 - slave
    int slotState = 0;

    private void Start() {
        DOTween.Init();
        hideCanvas();

        if(GameInfo.getCurrentLocationAsTown() == null || true) {
            var tempTown = Map.getRandomTownLocationInRegion(GameInfo.getCurrentRegion());
            tempTown.town.holder.overrideObject<ShopBuilding>(0, FindObjectOfType<PresetLibrary>().getBuilding(Building.type.Shop).GetComponent<ShopInstance>().reference);
            GameInfo.setCurrentLocationAsTown(tempTown);
        }
        currentTown = GameInfo.getCurrentLocationAsTown().town;
        currentShop = GameInfo.getCurrentLocationAsTown().town.holder.getObject<ShopBuilding>(0);
        updateInfo();
        createSlots();

        hideCanvas();
    }

    private void Update() {
        if(slot.run()) {
            updateInfo();
        }
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

        if(slot.getSelectedSlot() != null && slot.getSelectedSlotIndex() > -1) {
            Collectable currentCol = null;
            if(slotState == 0) {    //  Wepons
                if(shopState == 0)
                    currentCol = ShopInventory.getHolder(currentTown.t_instanceID).getObject<Weapon>(slot.getSelectedSlotIndex());
                else
                    currentCol = Inventory.getHolder().getObject<Weapon>(slot.getSelectedSlotIndex());
            }
            else if(slotState == 1) {   //  Amor
                if(shopState == 0)
                    currentCol = ShopInventory.getHolder(currentTown.t_instanceID).getObject<Armor>(slot.getSelectedSlotIndex());
                else
                    currentCol = Inventory.getHolder().getObject<Armor>(slot.getSelectedSlotIndex());
            }
            else if(slotState == 2) {   //  Usables
                if(shopState == 0)
                    currentCol = ShopInventory.getHolder(currentTown.t_instanceID).getObject<Usable>(slot.getSelectedSlotIndex());
                else
                    currentCol = Inventory.getHolder().getObject<Usable>(slot.getSelectedSlotIndex());
            }
            else if(slotState == 3) {   //  Unusables
                if(shopState == 0)
                    currentCol = ShopInventory.getHolder(currentTown.t_instanceID).getObject<Unusable>(slot.getSelectedSlotIndex());
                else
                    currentCol = Inventory.getHolder().getObject<Unusable>(slot.getSelectedSlotIndex());
            }
            else if(slotState == 4) {   //  Items
                if(shopState == 0)
                    currentCol = ShopInventory.getHolder(currentTown.t_instanceID).getObject<Item>(slot.getSelectedSlotIndex());
                else
                    currentCol = Inventory.getHolder().getObject<Item>(slot.getSelectedSlotIndex());
            }
            else if(slotState == 5) {   //  Slaves
                if(shopState == 0) {
                    UnitStats stats = ShopInventory.getHolder(currentTown.t_instanceID).getObject<UnitStats>(slot.getSelectedSlotIndex());
                    nameText.text = stats.u_name;
                    costText.text = getBuyPrice(stats.determineCost()).ToString() + "c";
                }
                else if(shopState == 1) {
                    UnitStats stats = Party.getHolder().getObject<UnitStats>(slot.getSelectedSlotIndex());
                    nameText.text = stats.u_name;
                    costText.text = getSellPrice(stats.determineCost()).ToString() + "c";
                }
                return;
            }


            if(currentCol != null && !currentCol.isEmpty()) {
                nameText.text = currentCol.name;
                costText.text = getBuyPrice(currentCol.coinCost).ToString() + "c";
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

        //  Buy state
        if(shopState == 0) {
            switch(slotState) {
                //  Weapons
                case 0:
                    int weaponCount = ShopInventory.getHolder(currentTown.t_instanceID).getObjectCount<Weapon>();
                    if(weaponCount <= 0)
                        break;
                    for(int i = 0; i < weaponCount; i++) {
                        var obj = slot.createSlot(i, Color.white, InfoTextCreator.createForCollectable(Inventory.getHolder().getObject<Weapon>(i)));
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(ShopInventory.getHolder(currentTown.t_instanceID).getObject<Weapon>(i)).sprite;
                    }
                    break;

                //  Armor
                case 1:
                    int armorCount = ShopInventory.getHolder(currentTown.t_instanceID).getObjectCount<Armor>();
                    if(armorCount <= 0)
                        break;
                    for(int i = 0; i < armorCount; i++) {
                        var obj = slot.createSlot(i, Color.white, InfoTextCreator.createForCollectable(Inventory.getHolder().getObject<Armor>(i)));
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(ShopInventory.getHolder(currentTown.t_instanceID).getObject<Armor>(i)).sprite;
                    }
                    break;

                //  Usable
                case 2:
                    int consumableCount = ShopInventory.getHolder(currentTown.t_instanceID).getObjectCount<Usable>();
                    if(consumableCount <= 0)
                        break;
                    for(int i = 0; i < consumableCount; i++) {
                        var obj = slot.createSlot(i, Color.white);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUsableSprite(ShopInventory.getHolder(currentTown.t_instanceID).getObject<Usable>(i)).sprite;
                    }
                    break;

                //  Unusable
                case 3:
                    int unusableCount = ShopInventory.getHolder(currentTown.t_instanceID).getObjectCount<Unusable>();
                    if(unusableCount <= 0)
                        break;
                    for(int i = 0; i < unusableCount; i++) {
                        var obj = slot.createSlot(i, Color.white);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnusableSprite(ShopInventory.getHolder(currentTown.t_instanceID).getObject<Unusable>(i)).sprite;
                    }
                    break;

                //  Item
                case 4:
                    int itemCount = ShopInventory.getHolder(currentTown.t_instanceID).getObjectCount<Item>();
                    if(itemCount <= 0)
                        break;
                    for(int i = 0; i < itemCount; i++) {
                        var obj = slot.createSlot(i, Color.white);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getItemSprite(ShopInventory.getHolder(currentTown.t_instanceID).getObject<Item>(i)).sprite;
                    }
                    break;

                //  Slave
                case 5:
                    int slaveCount = ShopInventory.getHolder(currentTown.t_instanceID).getObjectCount<UnitStats>();
                    if(slaveCount <= 0)
                        break;
                    for(int i = 0; i < slaveCount; i++) {
                        var obj = slot.createSlot(i, Color.white);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnitHeadSprite(ShopInventory.getHolder(currentTown.t_instanceID).getObject<UnitStats>(i).u_sprite.headIndex);
                        obj.transform.GetChild(0).GetComponent<Image>().color = ShopInventory.getHolder(currentTown.t_instanceID).getObject<UnitStats>(i).u_sprite.color;
                    }
                    break;
            }
        }

        //  Sell state
        if(shopState == 1) {
            switch(slotState) {
                //  Weapons
                case 0:
                    if(Inventory.getHolder().getObjectCount<Weapon>() <= 0)
                        break;

                    for(int i = 0; i < Inventory.getHolder().getObjectCount<Weapon>(); i++) {
                        var obj = slot.createSlot(i, Color.white, InfoTextCreator.createForCollectable(Inventory.getHolder().getObject<Weapon>(i)));
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(Inventory.getHolder().getObject<Weapon>(i)).sprite;
                    }
                    break;

                //  Armor
                case 1:
                    if(Inventory.getHolder().getObjectCount<Armor>() <= 0)
                        break;

                    for(int i = 0; i < Inventory.getHolder().getObjectCount<Armor>(); i++) {
                        var obj = slot.createSlot(i, Color.white, InfoTextCreator.createForCollectable(Inventory.getHolder().getObject<Armor>(i)));
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(Inventory.getHolder().getObject<Armor>(i)).sprite;
                    }
                    break;

                //  Consumable
                case 2:
                    if(Inventory.getHolder().getObjectCount<Usable>() <= 0)
                        break;

                    for(int i = 0; i < Inventory.getHolder().getObjectCount<Usable>(); i++) {
                        var obj = slot.createSlot(i, Color.white, InfoTextCreator.createForCollectable(Inventory.getHolder().getObject<Usable>(i)));
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUsableSprite(Inventory.getHolder().getObject<Usable>(i)).sprite;
                    }
                    break;

                //  Unusable
                case 3:
                    if(Inventory.getHolder().getObjectCount<Unusable>() <= 0)
                        break;

                    for(int i = 0; i < Inventory.getHolder().getObjectCount<Unusable>(); i++) {
                        var obj = slot.createSlot(i, Color.white, InfoTextCreator.createForCollectable(Inventory.getHolder().getObject<Unusable>(i)));
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnusableSprite(Inventory.getHolder().getObject<Unusable>(i)).sprite;
                    }
                    break;

                //  Item
                case 4:
                    if(Inventory.getHolder().getObjectCount<Item>() <= 0)
                        break;

                    for(int i = 0; i < Inventory.getHolder().getObjectCount<Item>(); i++) {
                        var obj = slot.createSlot(i, Color.white, InfoTextCreator.createForCollectable(Inventory.getHolder().getObject<Item>(i)));
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getItemSprite(Inventory.getHolder().getObject<Item>(i)).sprite;
                    }
                    break;

                //  Slave
                case 5:
                    if(Party.getHolder().getObjectCount<UnitStats>() <= 0)
                        break;

                    for(int i = 0; i < Party.getHolder().getObjectCount<UnitStats>(); i++) {
                        var obj = slot.createSlot(i, Color.white, InfoTextCreator.createForUnitStats(Party.getHolder().getObject<UnitStats>(i)));
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnitHeadSprite(Party.getHolder().getObject<UnitStats>(i).u_sprite.headIndex);
                        obj.transform.GetChild(0).GetComponent<Image>().color = Party.getHolder().getObject<UnitStats>(i).u_sprite.color;
                    }
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
            Collectable currentCol = null;

            //  Buying
            if(shopState == 0) {
                if(slotState == 0)
                    currentCol = ShopInventory.getHolder(townIndex).getObject<Weapon>(slot.getSelectedSlotIndex());
                else if(slotState == 1)
                    currentCol = ShopInventory.getHolder(townIndex).getObject<Armor>(slot.getSelectedSlotIndex());
                else if(slotState == 2)
                    currentCol = ShopInventory.getHolder(townIndex).getObject<Usable>(slot.getSelectedSlotIndex());
                else if(slotState == 3)
                    currentCol = ShopInventory.getHolder(townIndex).getObject<Unusable>(slot.getSelectedSlotIndex());
                else if(slotState == 4)
                    currentCol = ShopInventory.getHolder(townIndex).getObject<Item>(slot.getSelectedSlotIndex());

                else if(slotState == 5) {
                    UnitStats statsInSlot = ShopInventory.getHolder(townIndex).getObject<UnitStats>(slot.getSelectedSlotIndex());
                    var sPrice = statsInSlot.determineCost();

                    if(Inventory.getCoinCount() >= sPrice) {
                        Inventory.addCoins(-sPrice);
                        Party.addUnit(statsInSlot);
                        ShopInventory.removeUnit(townIndex, slot.getSelectedSlotIndex());
                    }
                }

                var cPrice = getBuyPrice(currentCol.coinCost);
                if(Inventory.getCoinCount() >= cPrice && currentCol != null && !currentCol.isEmpty()) {
                    Inventory.addCoins(-cPrice);
                    Inventory.addCollectable(currentCol, FindObjectOfType<PresetLibrary>());
                    ShopInventory.removeCollectable(townIndex, currentCol);
                }
            }

            //  Selling
            else {
                if(slotState == 0) {
                    currentCol = Inventory.getHolder().getObject<Weapon>(slot.getSelectedSlotIndex());
                }
                else if(slotState == 1) {
                    currentCol = Inventory.getHolder().getObject<Armor>(slot.getSelectedSlotIndex());
                }
                else if(slotState == 2) {
                    currentCol = Inventory.getHolder().getObject<Usable>(slot.getSelectedSlotIndex());
                }
                else if(slotState == 3) {
                    currentCol = Inventory.getHolder().getObject<Unusable>(slot.getSelectedSlotIndex());
                }
                else if(slotState == 4) {
                    currentCol = Inventory.getHolder().getObject<Item>(slot.getSelectedSlotIndex());
                }

                else if(slotState == 5) {
                    var stats = Party.getHolder().getObject<UnitStats>(slot.getSelectedSlotIndex());
                    Inventory.addCoins(getSellPrice(stats.determineCost()));
                    Party.removeUnit(slot.getSelectedSlotIndex());
                    ShopInventory.addUnit(townIndex, stats);
                }

                if(currentCol != null && !currentCol.isEmpty()) {
                    Inventory.addCoins(getSellPrice(currentCol.coinCost));
                    Inventory.removeCollectable(currentCol);
                    ShopInventory.addCollectable(townIndex, currentCol);
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
        ShopInventory.populateShop(GameInfo.getCurrentLocationAsTown().town.t_instanceID, GameInfo.getCurrentRegion(), FindObjectOfType<PresetLibrary>());

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
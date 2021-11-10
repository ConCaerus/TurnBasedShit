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
        slot.init();
        hideCanvas();

        if(GameInfo.getCurrentLocationAsTown() == null || true) {
            var tempTown = Map.getRandomTownLocationInRegion(GameInfo.getCurrentRegion());
            tempTown.town.addBuilding(FindObjectOfType<PresetLibrary>().getBuilding(Building.type.Shop).GetComponent<ShopInstance>().reference);
            GameInfo.setCurrentLocationAsTown(tempTown);
        }
        currentTown = GameInfo.getCurrentLocationAsTown().town;
        currentShop = GameInfo.getCurrentLocationAsTown().town.getShop();
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
                    currentCol = ShopInventory.getWeapon(currentTown.t_instanceID, slot.getSelectedSlotIndex());
                else
                    currentCol = Inventory.getWeapon(slot.getSelectedSlotIndex());
            }
            else if(slotState == 1) {   //  Amor
                if(shopState == 0)
                    currentCol = ShopInventory.getArmor(currentTown.t_instanceID, slot.getSelectedSlotIndex());
                else
                    currentCol = Inventory.getArmor(slot.getSelectedSlotIndex());
            }
            else if(slotState == 2) {   //  Usables
                if(shopState == 0)
                    currentCol = ShopInventory.getUsable(currentTown.t_instanceID, slot.getSelectedSlotIndex());
                else
                    currentCol = Inventory.getUsable(slot.getSelectedSlotIndex());
            }
            else if(slotState == 3) {   //  Unusables
                if(shopState == 0)
                    currentCol = ShopInventory.getUnusable(currentTown.t_instanceID, slot.getSelectedSlotIndex());
                else
                    currentCol = Inventory.getUnusable(slot.getSelectedSlotIndex());
            }
            else if(slotState == 4) {   //  Items
                if(shopState == 0)
                    currentCol = ShopInventory.getItem(currentTown.t_instanceID, slot.getSelectedSlotIndex());
                else
                    currentCol = Inventory.getItem(slot.getSelectedSlotIndex());
            }
            else if(slotState == 5) {   //  Slaves
                if(shopState == 0) {
                    UnitStats stats = ShopInventory.getSlave(currentTown.t_instanceID, slot.getSelectedSlotIndex());
                    nameText.text = stats.u_name;
                    costText.text = getBuyPrice(stats.determineCost()).ToString() + "c";
                }
                else if(shopState == 1) {
                    UnitStats stats = Party.getMemberStats(slot.getSelectedSlotIndex());
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
                    int weaponCount = ShopInventory.getTypeCount(currentTown.t_instanceID, typeof(Weapon));
                    if(weaponCount <= 0)
                        break;
                    for(int i = 0; i < weaponCount; i++) {
                        var obj = slot.createSlot(i, Color.white, InfoTextCreator.createForCollectable(Inventory.getWeapon(i)));
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(ShopInventory.getWeapon(currentTown.t_instanceID, i)).sprite;
                    }
                    break;

                //  Armor
                case 1:
                    int armorCount = ShopInventory.getTypeCount(currentTown.t_instanceID, typeof(Armor));
                    if(armorCount <= 0)
                        break;
                    for(int i = 0; i < armorCount; i++) {
                        var obj = slot.createSlot(i, Color.white, InfoTextCreator.createForCollectable(Inventory.getArmor(i)));
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(ShopInventory.getArmor(currentTown.t_instanceID, i)).sprite;
                    }
                    break;

                //  Usable
                case 2:
                    int consumableCount = ShopInventory.getTypeCount(currentTown.t_instanceID, typeof(Usable));
                    if(consumableCount <= 0)
                        break;
                    for(int i = 0; i < consumableCount; i++) {
                        var obj = slot.createSlot(i, Color.white);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUsableSprite(ShopInventory.getUsable(currentTown.t_instanceID, i)).sprite;
                    }
                    break;

                //  Unusable
                case 3:
                    int unusableCount = ShopInventory.getTypeCount(currentTown.t_instanceID, typeof(Unusable));
                    if(unusableCount <= 0)
                        break;
                    for(int i = 0; i < unusableCount; i++) {
                        var obj = slot.createSlot(i, Color.white);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnusableSprite(ShopInventory.getUnusable(currentTown.t_instanceID, i)).sprite;
                    }
                    break;

                //  Item
                case 4:
                    int itemCount = ShopInventory.getTypeCount(currentTown.t_instanceID, typeof(Item));
                    if(itemCount <= 0)
                        break;
                    for(int i = 0; i < itemCount; i++) {
                        var obj = slot.createSlot(i, Color.white);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getItemSprite(ShopInventory.getItem(currentTown.t_instanceID, i)).sprite;
                    }
                    break;

                //  Slave
                case 5:
                    int slaveCount = ShopInventory.getTypeCount(currentTown.t_instanceID, typeof(UnitStats));
                    if(slaveCount <= 0)
                        break;
                    for(int i = 0; i < slaveCount; i++) {
                        var obj = slot.createSlot(i, Color.white);
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnitHeadSprite(ShopInventory.getSlave(currentTown.t_instanceID, i).u_sprite.headIndex);
                        obj.transform.GetChild(0).GetComponent<Image>().color = ShopInventory.getSlave(currentTown.t_instanceID, i).u_sprite.color;
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
                        var obj = slot.createSlot(i, Color.white, InfoTextCreator.createForCollectable(Inventory.getWeapon(i)));
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(Inventory.getWeapon(i)).sprite;
                    }
                    break;

                //  Armor
                case 1:
                    if(Inventory.getArmorCount() <= 0)
                        break;

                    for(int i = 0; i < Inventory.getArmorCount(); i++) {
                        var obj = slot.createSlot(i, Color.white, InfoTextCreator.createForCollectable(Inventory.getArmor(i)));
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(Inventory.getArmor(i)).sprite;
                    }
                    break;

                //  Consumable
                case 2:
                    if(Inventory.getUsableCount() <= 0)
                        break;

                    for(int i = 0; i < Inventory.getUsableCount(); i++) {
                        var obj = slot.createSlot(i, Color.white, InfoTextCreator.createForCollectable(Inventory.getUsable(i)));
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUsableSprite(Inventory.getUsable(i)).sprite;
                    }
                    break;

                //  Unusable
                case 3:
                    if(Inventory.getUnusableCount() <= 0)
                        break;

                    for(int i = 0; i < Inventory.getUnusableCount(); i++) {
                        var obj = slot.createSlot(i, Color.white, InfoTextCreator.createForCollectable(Inventory.getUnusable(i)));
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnusableSprite(Inventory.getUnusable(i)).sprite;
                    }
                    break;

                //  Item
                case 4:
                    if(Inventory.getItemCount() <= 0)
                        break;

                    for(int i = 0; i < Inventory.getItemCount(); i++) {
                        var obj = slot.createSlot(i, Color.white, InfoTextCreator.createForCollectable(Inventory.getItem(i)));
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getItemSprite(Inventory.getItem(i)).sprite;
                    }
                    break;

                //  Slave
                case 5:
                    if(Party.getMemberCount() <= 0)
                        break;

                    for(int i = 0; i < Party.getMemberCount(); i++) {
                        var obj = slot.createSlot(i, Color.white, InfoTextCreator.createForUnitStats(Party.getMemberStats(i)));
                        obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnitHeadSprite(Party.getMemberStats(i).u_sprite.headIndex);
                        obj.transform.GetChild(0).GetComponent<Image>().color = Party.getMemberStats(i).u_sprite.color;
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
                    currentCol = ShopInventory.getWeapon(townIndex, slot.getSelectedSlotIndex());
                else if(slotState == 1)
                    currentCol = ShopInventory.getArmor(townIndex, slot.getSelectedSlotIndex());
                else if(slotState == 2)
                    currentCol = ShopInventory.getUsable(townIndex, slot.getSelectedSlotIndex());
                else if(slotState == 3)
                    currentCol = ShopInventory.getUnusable(townIndex, slot.getSelectedSlotIndex());
                else if(slotState == 4)
                    currentCol = ShopInventory.getItem(townIndex, slot.getSelectedSlotIndex());

                else if(slotState == 5) {
                    UnitStats statsInSlot = ShopInventory.getSlave(townIndex, slot.getSelectedSlotIndex());
                    var sPrice = statsInSlot.determineCost();

                    if(Inventory.getCoinCount() >= sPrice) {
                        Inventory.removeCoins(sPrice);
                        Party.addUnit(statsInSlot);
                        ShopInventory.removeSlave(townIndex, slot.getSelectedSlotIndex());
                    }
                }

                var cPrice = getBuyPrice(currentCol.coinCost);
                if(Inventory.getCoinCount() >= cPrice && currentCol != null && !currentCol.isEmpty()) {
                    Inventory.removeCoins(cPrice);
                    Inventory.addCollectable(currentCol);
                    ShopInventory.removeCollectable(townIndex, currentCol);
                }
            }

            //  Selling
            else {
                if(slotState == 0) {
                    currentCol = Inventory.getWeapon(slot.getSelectedSlotIndex());
                }
                else if(slotState == 1) {
                    currentCol = Inventory.getArmor(slot.getSelectedSlotIndex());
                }
                else if(slotState == 2) {
                    currentCol = Inventory.getUsable(slot.getSelectedSlotIndex());
                }
                else if(slotState == 3) {
                    currentCol = Inventory.getUnusable(slot.getSelectedSlotIndex());
                }
                else if(slotState == 4) {
                    currentCol = Inventory.getItem(slot.getSelectedSlotIndex());
                }

                else if(slotState == 5) {
                    var stats = Party.getMemberStats(slot.getSelectedSlotIndex());
                    Inventory.addCoins(getSellPrice(stats.determineCost()));
                    Party.removeUnit(slot.getSelectedSlotIndex());
                    ShopInventory.addSlave(townIndex, stats);
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
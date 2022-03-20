using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MapMerchantCanvas : MonoBehaviour {
    public CoinCount coinCount;
    public SlotMenu slot;

    public bool buying = true;
    int filterState = 0;    //  0:all, 1:weapon, 2:armor, 3:item, 4:usable, 5:unusable, 6:unit

    ObjectHolder inv = new ObjectHolder();


    private void Start() {
        hide();
        coinCount.updateCount(false);
    }

    private void Update() {
        if(transform.GetChild(0).localScale.x > 0.0f && slot.run())
            updateInfo();
    }


    public void show(ObjectHolder merchantInv) {
        //  logic that makes shit work
        filterState = 0;
        buying = true;
        inv = merchantInv;
        FindObjectOfType<MapCameraController>().canZoom = false;
        FindObjectOfType<MapMovement>().canMove = false;

        //  animation
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).DOScale(1.0f, .1f);

        //  setting up the canvas' information
        populateSlots();
        updateInfo();
    }
    public void hide() {
        //  logic shit
        FindObjectOfType<MapCameraController>().canZoom = true;
        FindObjectOfType<MapMovement>().canMove = true;

        //  animation
        transform.GetChild(0).DOScale(0.0f, .1f);
    }

    public bool isOpen() {
        return transform.GetChild(0).gameObject.activeInHierarchy && transform.GetChild(0).localScale.x > 0.0f;
    }


    void populateSlots() {
        if(buying) {
            int index = 0;
            if(filterState != 6) {  //  add collectables
                foreach(var i in inv.getCollectables()) {
                    if(shouldAddCol(i))
                        createSlot(index++, i);
                }
            }
            if(filterState == 6 || filterState == 0) {
                foreach(var i in inv.getObjects<UnitStats>()) {
                    createSlot(index++, i);
                }
            }
            slot.deleteSlotsAfterIndex(index);
        }

        else {
            int index = 0;
            if(filterState != 6) {
                foreach(var i in Inventory.getHolder().getCollectables()) {
                    if(shouldAddCol(i))
                        createSlot(index++, i);
                }
            }
            if(filterState == 6 || filterState == 0) {
                foreach(var i in Party.getHolder().getObjects<UnitStats>()) {
                    createSlot(index++, i);
                }
            }
            slot.deleteSlotsAfterIndex(index);
        }
    }

    GameObject createSlot(int index, Collectable col) {
        var obj = slot.createSlot(index, Color.white);
        obj.GetComponent<SlotObject>().setImage(1, FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(col));
        obj.GetComponent<SlotObject>().setImageColor(0, FindObjectOfType<PresetLibrary>().getRarityColor(col.rarity));
        return obj;
    }
    GameObject createSlot(int index, UnitStats stats) {
        var obj = slot.createSlot(index, Color.white);
        obj.GetComponent<SlotObject>().setImage(1, FindObjectOfType<PresetLibrary>().getUnitHeadSprite(stats.u_sprite.headIndex));
        obj.GetComponent<SlotObject>().setImageColor(0, stats.u_sprite.color);
        return obj;
    }

    bool shouldAddCol(Collectable col) {
        return filterState == 0 ? true : filterState == 1 ? col.type == Collectable.collectableType.Weapon : filterState == 2 ? col.type == Collectable.collectableType.Armor :
               filterState == 3 ? col.type == Collectable.collectableType.Item : filterState == 4 ? col.type == Collectable.collectableType.Usable :
               filterState == 5 ? col.type == Collectable.collectableType.Unusable : false;
    }

    void updateInfo() {
        coinCount.updateCount(false);
    }



    public void openBuyWindow() {
        if(buying)
            return;

        buying = true;
        populateSlots();
        slot.resetScrollValue();
    }
    public void openSellWindow() {
        if(!buying)
            return;

        buying = false;
        populateSlots();
        slot.resetScrollValue();
    }

    public void setFilter(int state) {  //  0: All, 1: Weapon, 2: Armor, 3: Item, 4: Usable, 5: Unusable
        if(state == filterState)
            return;
        filterState = state;
        populateSlots();
    }

    Collectable getSelectedCollectable() {
        int index = slot.getSelectedSlotIndex();
        if(index < 0 || index > slot.getSlots().Count || filterState == 6)
            return null;

        List<Collectable> cols = buying ? inv.getCollectables() : Inventory.getHolder().getCollectables();

        switch(filterState) {
            case 0: //  all
                return index >= cols.Count ? null : cols[index];
            case 1: //  weapons
                foreach(var i in cols) {
                    if(index == 0)
                        return i;
                    if(i.type == Collectable.collectableType.Weapon)
                        index--;
                }
                return null;
            case 2: //  armor
                foreach(var i in cols) {
                    if(index == 0)
                        return i;
                    if(i.type == Collectable.collectableType.Armor)
                        index--;
                }
                return null;
            case 3: //  items
                foreach(var i in cols) {
                    if(index == 0)
                        return i;
                    if(i.type == Collectable.collectableType.Item)
                        index--;
                }
                return null;
            case 4: //  usables
                foreach(var i in cols) {
                    if(index == 0)
                        return i;
                    if(i.type == Collectable.collectableType.Usable)
                        index--;
                }
                return null;
            case 5: //  unusables
                foreach(var i in cols) {
                    if(index == 0)
                        return i;
                    if(i.type == Collectable.collectableType.Unusable)
                        index--;
                }
                return null;
            default: return null;
        }
    }

    UnitStats getSelectedUnitStats() {
        int index = slot.getSelectedSlotIndex();
        if(index < 0 || index > slot.getSlots().Count || (filterState != 0 && filterState != 6))
            return null;

        return buying ? inv.getObject<UnitStats>(index) : Party.getHolder().getObject<UnitStats>(index);
    }

    public void transaction() {
        var col = getSelectedCollectable();
        if(col != null) {
            if(buying) {
                if(Inventory.getCoinCount() < col.coinCost)
                    return;
                inv.removeCollectable(col);
                Inventory.addSingleCollectable(col, FindObjectOfType<PresetLibrary>(), FindObjectOfType<FullInventoryCanvas>());
                Inventory.addCoins(-col.coinCost, coinCount, true);

                populateSlots();
                return;
            }
            else {
                Inventory.removeCollectable(col);
                inv.addObject<Collectable>(col);
                Inventory.addCoins(col.coinCost, coinCount, true);

                populateSlots();
                return;
            }
        }

        var unit = getSelectedUnitStats();
        if(unit != null) {
            if(buying) {
                if(Inventory.getCoinCount() < unit.determineCost())
                    return;
                inv.removeObject<UnitStats>(inv.getUnitStatsIndex(unit));
                Party.addUnit(unit);
                Inventory.addCoins(-unit.determineCost(), coinCount, true);

                populateSlots();
                return;
            }
            else {
                Party.removeUnit(unit);
                inv.addObject<UnitStats>(unit);
                Inventory.addCoins(unit.determineCost(), coinCount, true);

                populateSlots();
                return;
            }
        }
    }
}

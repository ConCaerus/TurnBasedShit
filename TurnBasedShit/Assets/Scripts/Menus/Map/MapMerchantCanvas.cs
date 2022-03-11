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

    List<Collectable> inv = new List<Collectable>();


    private void Start() {
        populateSlots();
        coinCount.updateCount(false);
    }

    private void Update() {
        if(transform.GetChild(0).localScale.x > 0.0f && slot.run())
            updateInfo();
    }


    public void show(List<Collectable> merchantInv) {
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
            foreach(var i in inv) {
                if(shouldAddCol(i))
                    createSlot(index++, i);
            }
            slot.deleteSlotsAfterIndex(index);
        }

        else {
            int index = 0;
            foreach(var i in Inventory.getHolder().getCollectables()) {
                if(shouldAddCol(i))
                    createSlot(index++, i);
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

    public void transaction() {
        int index = slot.getSelectedSlotIndex();
        if(index < 0 || index > slot.getSlots().Count || !Inventory.hasAvailableSpace())
            return;

        if(buying) {
            var col = inv[index];
            inv.RemoveAt(index);
            Inventory.addSingleCollectable(col, FindObjectOfType<PresetLibrary>(), FindObjectOfType<FullInventoryCanvas>());
            Inventory.addCoins(-col.coinCost, coinCount, true);
        }

        else {
            var col = Inventory.getHolder().getCollectables()[index];
            Inventory.removeCollectable(col);
            inv.Add(col);
            Inventory.addCoins(col.coinCost, coinCount, true);
        }

        populateSlots();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MapMerchantCanvas : MonoBehaviour {
    public CoinCount coinCount;
    public SlotMenu slot;

    public bool buying = true;

    List<Collectable> temp = new List<Collectable>();


    private void Start() {
        for(int i = 0; i < 50; i++) {
            temp.Add(FindObjectOfType<PresetLibrary>().getRandomCollectable());
        }
        populateSlots();
        coinCount.updateCount(false);
    }

    private void Update() {
        if(transform.GetChild(0).localScale.x > 0.0f && slot.run())
            updateInfo();
    }


    public void show() {
        //  logic that makes shit work
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
            slot.deleteSlotsAfterIndex(temp.Count);
            int index = 0;
            foreach(var i in temp) {
                createSlot(index++, i);
            }
        }

        else {
            slot.deleteSlotsAfterIndex(Inventory.getHolder().getCollectables().Count);
            int index = 0;
            foreach(var i in Inventory.getHolder().getCollectables()) {
                createSlot(index++, i);
            }
        }
    }

    GameObject createSlot(int index, Collectable col) {
        var obj = slot.createSlot(index, Color.white);
        obj.GetComponent<SlotObject>().setImage(1, FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(col));
        obj.GetComponent<SlotObject>().setImageColor(0, FindObjectOfType<PresetLibrary>().getRarityColor(col.rarity));
        return obj;
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

    public void transaction() {
        int index = slot.getSelectedSlotIndex();
        if(index < 0 || index > slot.getSlots().Count || !Inventory.hasAvailableSpace())
            return;

        if(buying) {
            var col = temp[index];
            temp.RemoveAt(index);
            Inventory.addSingleCollectable(col, FindObjectOfType<PresetLibrary>(), FindObjectOfType<FullInventoryCanvas>());
            Inventory.addCoins(-col.coinCost, coinCount, true);
        }

        else {
            var col = Inventory.getHolder().getCollectables()[index];
            Inventory.removeCollectable(col);
            temp.Add(col);
            Inventory.addCoins(col.coinCost, coinCount, true);
        }

        populateSlots();
    }
}

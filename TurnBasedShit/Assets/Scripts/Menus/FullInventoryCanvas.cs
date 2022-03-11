using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class FullInventoryCanvas : MonoBehaviour {
    [SerializeField] SlotMenu invSlot, newInvSlot;
    [SerializeField] TextMeshProUGUI countText;

    List<Collectable> newInvList = new List<Collectable>();

    private void Start() {
        DOTween.Init();
        hide();

        invSlot.canSelectMultiple = true;
        newInvSlot.canSelectMultiple = true;
        invSlot.setRunOnSelect(updateInvSlotColor);
        newInvSlot.setRunOnSelect(updateNewInvSlotColor);
    }


    private void Update() {
        invSlot.run();
        newInvSlot.run();
    }

    public void show(List<Collectable> newList) {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).transform.DOScale(1.0f, 0.15f);
        newInvList = newList;
        invSlot.resetSelectedSlotIndexes();
        newInvSlot.resetSelectedSlotIndexes();
        StartCoroutine(populateSlots());
    }

    public void hide() {
        transform.GetChild(0).transform.DOScale(0.0f, 0.25f);
    }

    public bool isOpen() {
        return transform.GetChild(0).gameObject.activeInHierarchy && transform.GetChild(0).localScale.x > 0.0f;
    }


    public void updateInvSlotColor(int index, bool wasSelected) {
        invSlot.getSlots()[index].GetComponent<SlotObject>().setImageColor(0, wasSelected ? Color.white : FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getHolder().getCollectables()[index].rarity));
    }
    public void updateNewInvSlotColor(int index, bool wasSelected) {
        newInvSlot.getSlots()[index].GetComponent<SlotObject>().setImageColor(0, wasSelected ? Color.white : FindObjectOfType<PresetLibrary>().getRarityColor(newInvList[index].rarity));
    }


    IEnumerator populateSlots() {
        invSlot.deleteSlotsAfterIndex(Inventory.getHolder().getCollectables().Count);
        newInvSlot.deleteSlotsAfterIndex(newInvList.Count);
        float waitTime = 0.05f, scaleTime = 0.1f;
        var invList = Inventory.getHolder().getCollectables();

        countText.text = Inventory.getHolder().getCollectables().Count.ToString() + "/" + Inventory.getMaxCapacity().ToString();

        for(int i = 0; i < invList.Count || i < newInvList.Count; i++) {
            bool did = false;
            if(i < newInvList.Count) {
                var obj = makeNewInvSlot(i, newInvList[i]);
                var prev = obj.transform.localScale.x;
                obj.transform.localScale = Vector3.zero;
                obj.transform.DOScale(prev, scaleTime);
                did = true;
            }

            if(i < invList.Count) {
                var obj = makeInvSlot(i, invList[i]);
                var prev = obj.transform.localScale.x;
                obj.transform.localScale = Vector3.zero;
                obj.transform.DOScale(prev, scaleTime);
                did = true;
            }

            if(!did)
                yield break;

            yield return new WaitForSeconds(waitTime);
        }
    }

    GameObject makeInvSlot(int i, Collectable c) {
        var obj = invSlot.createSlot(i, Color.white, InfoTextCreator.createForCollectable(c));
        obj.GetComponent<SlotObject>().setImage(1, FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(c));
        obj.GetComponent<SlotObject>().setImageColor(0, FindObjectOfType<PresetLibrary>().getRarityColor(c.rarity));

        return obj;
    }
    GameObject makeNewInvSlot(int i, Collectable c) {
        var obj = newInvSlot.createSlot(i, Color.white, InfoTextCreator.createForCollectable(c));
        obj.GetComponent<SlotObject>().setImage(1, FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(c));
        obj.GetComponent<SlotObject>().setImageColor(0, FindObjectOfType<PresetLibrary>().getRarityColor(c.rarity));

        return obj;
    }


    public void swapCollectables() {
        //  change inv things
        for(int i = invSlot.getSelectedSlotIndexes().Count - 1; i >= 0; i--) {
            //  add to newList
            newInvList.Add(Inventory.getHolder().getCollectables()[invSlot.getSelectedSlotIndexes()[i]]);
            //  remove from inventory
            Inventory.removeCollectable(Inventory.getHolder().getCollectables()[invSlot.getSelectedSlotIndexes()[i]]);
        }

        //  change newInv things
        for(int i = newInvSlot.getSelectedSlotIndexes().Count - 1; i >= 0; i--) {
            if(!Inventory.hasAvailableSpace())
                break;
            //  add to inventory
            Inventory.addSingleCollectable(newInvList[newInvSlot.getSelectedSlotIndexes()[i]], FindObjectOfType<PresetLibrary>(), FindObjectOfType<FullInventoryCanvas>());
            //  remove from newInv
            newInvList.Remove(newInvList[newInvSlot.getSelectedSlotIndexes()[i]]);
        }

        invSlot.resetSelectedSlotIndexes();
        newInvSlot.resetSelectedSlotIndexes();
        StartCoroutine(populateSlots());
    }

    public void done() {
        //  cleanup
        hide();
    }

    public void selectAll() {
        newInvSlot.setAllSelectedSlotIndexes();
        foreach(var i in newInvSlot.getSlots())
            i.GetComponent<SlotObject>().setImageColor(0, Color.white);
    }
}

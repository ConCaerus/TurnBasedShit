using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;


public abstract class SelectionCanvas : MonoBehaviour {
    public GameObject slotHolder;
    public GameObject slotPreset;
    protected int selectedIndex = -1;

    protected List<GameObject> slots = new List<GameObject>();

    public int numOfSlotsPerLine;
    public float slotBuffer;

    protected bool shown = false;


    private void Start() {
        populateSlots();
    }


    public abstract void populateSlots();

    //   0 - weapon, 1 - armor, 2 - item, 3 - consumables
    public abstract int getState();


    public int getIndexForSlot(GameObject slot) {
        for(int i = 0; i < slotHolder.transform.childCount; i++) {
            if(slot == slotHolder.transform.GetChild(i).gameObject)
                return i;
        }
        return -1;
    }
}

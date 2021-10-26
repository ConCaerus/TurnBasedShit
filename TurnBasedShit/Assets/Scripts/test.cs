using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
    [SerializeField] SlotMenu slot;

    private void Start() {
        slot.init();
        slot.createANumberOfSlots(10, Color.white);
    }


    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            for(int i = 0; i < 5; i++)
                slot.instantiateNewSlot(Color.white);
        }
        //Debug.Log(Input.mousePosition.y);
    }
}

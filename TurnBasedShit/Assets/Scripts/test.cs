using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour {

    private void Start() {
        Inventory.clearUsables();
        for(int i = 0; i < 12; i++) {
            var thing = FindObjectOfType<PresetLibrary>().getRandomUsable();
            Inventory.addUsable(thing);
            Debug.Log(thing);
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.V)) {
            for(int i = 0; i < Inventory.getUsableCount(); i++)
                Debug.Log(Inventory.getUsable(i).name);
        }

        else if(Input.GetKeyDown(KeyCode.Space))
            Inventory.removeUsable(0);
    }
}
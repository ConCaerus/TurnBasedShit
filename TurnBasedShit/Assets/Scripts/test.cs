using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour {

    private void Start() {
        GetComponent<InfoBearer>().optionsCollectableReference = FindObjectOfType<PresetLibrary>().getRandomCollectable();
        while(GetComponent<InfoBearer>().optionsCollectableReference.type == Collectable.collectableType.Weapon || 
            GetComponent<InfoBearer>().optionsCollectableReference.type == Collectable.collectableType.Armor)
            GetComponent<InfoBearer>().optionsCollectableReference = FindObjectOfType<PresetLibrary>().getRandomCollectable();
    }
}
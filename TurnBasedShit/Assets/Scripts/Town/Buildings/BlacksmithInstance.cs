using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithInstance : BuildingInstance {
    public BlacksmithBuilding reference;
    [SerializeField] Sprite emptySprite;


    private void Start() {
        if(!reference.hasHammer)
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = emptySprite;
    }
}

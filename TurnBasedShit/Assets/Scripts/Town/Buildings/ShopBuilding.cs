using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopBuilding : Building {
    public float sellReduction = 0.05f;
    public float priceMod = 0.0f;


    public ShopBuilding() {
        b_type = type.Shop;
    }


    public bool isEqualTo(ShopBuilding other) {
        return other.sellReduction == sellReduction && priceMod == other.priceMod;
    }
    public void setEqualTo(ShopBuilding other) {
        sellReduction = other.sellReduction;
        priceMod = other.priceMod;
        orderInTown = other.orderInTown;
    }
}

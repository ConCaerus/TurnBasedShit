using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChurchBuilding : Building {
    public int priceToRemove;
    public int priceToAdd;

    public ChurchBuilding() {
        b_type = type.Church;
    }


    public bool isEqualTo(ChurchBuilding other) {
        return other.b_type == type.Church && priceToRemove == other.priceToRemove && priceToAdd == other.priceToAdd;
    }
    public void setEqualTo(ChurchBuilding other) {
        orderInTown = other.orderInTown;
        priceToRemove = other.priceToRemove;
        priceToAdd = other.priceToAdd;
    }
}

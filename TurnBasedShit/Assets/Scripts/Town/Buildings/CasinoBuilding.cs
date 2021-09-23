using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CasinoBuilding : Building {

    public CasinoBuilding() {
        b_type = type.Casino;
    }


    public bool isEqualTo(CasinoBuilding other) {
        if(other == null)
            return false;
        return true;
    }
    public void setEqualTo(CasinoBuilding other) {
        orderInTown = other.orderInTown;
    }
}

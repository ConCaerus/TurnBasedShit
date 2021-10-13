using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlacksmithBuilding : Building {
    public int chargeRate = 1;


    public BlacksmithBuilding() {
        b_type = type.Blacksmith;
    }


    public void setEqualTo(BlacksmithBuilding other) {
        chargeRate = other.chargeRate;
    }
}

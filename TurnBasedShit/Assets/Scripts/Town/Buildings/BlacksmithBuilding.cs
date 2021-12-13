using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlacksmithBuilding : Building {
    public int chargeRate = 1;

    public bool hasHammer;


    public BlacksmithBuilding() {
        b_type = type.Blacksmith;
        hasHammer = true;
    }


    public void setEqualTo(BlacksmithBuilding other) {
        chargeRate = other.chargeRate;
        hasHammer = other.hasHammer;
    }
}

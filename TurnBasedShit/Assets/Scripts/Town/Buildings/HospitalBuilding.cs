using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HospitalBuilding : Building {
    public int freeHeals = 0;
    public int pricePerHeal = 0;


    public HospitalBuilding() {
        b_type = type.Hospital;
    }

    public bool isEqualTo(HospitalBuilding other) {
        return other.freeHeals == freeHeals && other.pricePerHeal == pricePerHeal;
    }
    public void setEqualTo(HospitalBuilding other) {
        freeHeals = other.freeHeals;
        pricePerHeal = other.pricePerHeal;
        orderInTown = other.orderInTown;
    }
}

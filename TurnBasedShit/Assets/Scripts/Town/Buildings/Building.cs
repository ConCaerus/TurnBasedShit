using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building {
    public const int buildingTypeCount = 4;
    public enum type {
        Hospital, Shop, Church, Casino
    }

    public int orderInTown = -1;

    public type b_type;
}
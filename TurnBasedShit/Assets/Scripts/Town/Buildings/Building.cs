using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building {
    public const int buildingTypeCount = 3;
    public enum type {
        Hospital, Shop, Church
    }

    public int orderInTown;

    public type b_type;
}
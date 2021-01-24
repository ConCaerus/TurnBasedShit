using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitClassStats {
    public string u_name = "";

    public float u_health = 100.0f, u_maxHealth = 100.0f;
    public float u_speed = 0.0f;
    public int u_poisonCount = 0;

    public int u_order = 0;

    public Weapon equippedWeapon;
    public Armor equippedArmor;
}

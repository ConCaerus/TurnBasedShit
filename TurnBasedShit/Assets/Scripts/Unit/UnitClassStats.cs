using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitClassStats {
    public string u_name = "";

    public float u_health = 100.0f, u_maxHealth = 100.0f;
    public float u_speed = 0.0f;
    public float u_power = 0.0f;
    public int u_poisonCount = 0;

    public int u_order = 0;

    public Weapon equippedWeapon;
    public Armor equippedArmor;
    public Item equippedItem;

    public SpriteLoader u_sprite;
    public Color u_color;

    public void setNewOrder(int i) {
        u_order = i;
    }

    public bool isEmpty() {
        return u_name == "" && u_order == 0;
    }

    public bool equals(UnitClassStats other) {
        bool names = u_name == other.u_name;
        bool health = u_health == other.u_health;
        bool maxHealth = u_maxHealth == other.u_maxHealth;
        bool speed = u_speed == other.u_speed;
        bool power = u_power == other.u_power;
        bool order = u_order == other.u_order;
        bool weapon = equippedWeapon.isEqualTo(other.equippedWeapon);
        bool armor = equippedArmor.isEqualTo(other.equippedArmor);
        bool item = equippedItem.isEqualTo(other.equippedItem);
        bool color = u_color == other.u_color;

        return names && health && maxHealth && speed && power && order && weapon && armor && color;
    }
}

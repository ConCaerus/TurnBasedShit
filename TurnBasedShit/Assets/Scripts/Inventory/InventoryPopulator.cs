using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPopulator : MonoBehaviour {
    [SerializeField] WeaponPreset weaponToAdd;
    [SerializeField] ArmorPreset armorToAdd;


    public void addAndResetEquippmentToAdd() {
        if(weaponToAdd != null) {
            Weapon w = weaponToAdd.preset;
            Inventory.addWeaponToInventory(w);
        }

        if(armorToAdd != null) {
            Armor a = armorToAdd.preset;
            Inventory.addArmorToInventory(a);
        }

        weaponToAdd = null;
        armorToAdd = null;

        Inventory.saveAllEquippment();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPopulator : MonoBehaviour {
    [SerializeField] WeaponPreset weaponToAdd;
    [SerializeField] ArmorPreset armorToAdd;
    [SerializeField] ConsumablePreset itemToAdd;


    public void resetInventory() {
        Inventory.clearInventory();
    }

    public void addAndResetEquippmentToAdd(int count = 0) {
        for(int j = 0; j < count; j++) {
            if(weaponToAdd != null) {
                Weapon w = weaponToAdd.preset;
                Inventory.addNewWeapon(w);
            }

            if(armorToAdd != null) {
                Armor a = armorToAdd.preset;
                Inventory.addNewArmor(a);
            }

            if(itemToAdd != null) {
                Consumable i = itemToAdd.preset;
                Inventory.addNewConsumable(i);
            }
        }

        weaponToAdd = null;
        armorToAdd = null;
        itemToAdd = null;
    }
}

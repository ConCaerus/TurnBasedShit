using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPopulator : MonoBehaviour {
    [SerializeField] WeaponPreset weaponToAdd;
    [SerializeField] ArmorPreset armorToAdd;
    [SerializeField] ConsumablePreset consumableToAdd;
    [SerializeField] ItemPreset itemToAdd;


    public void resetInventory() {
        Inventory.clearInventory();
    }

    public void addAndResetEquippmentToAdd(int count = 0) {
        for(int j = 0; j < count; j++) {
            if(weaponToAdd != null) {
                Weapon w = weaponToAdd.preset;
                Inventory.addWeapon(w);
            }

            if(armorToAdd != null) {
                Armor a = armorToAdd.preset;
                Inventory.addArmor(a);
            }

            if(consumableToAdd != null) {
                Consumable i = consumableToAdd.preset;
                Inventory.addConsumable(i);
            }

            if(itemToAdd != null) {
                Item i = itemToAdd.preset;
                Inventory.addItem(i);
            }
        }

        weaponToAdd = null;
        armorToAdd = null;
        consumableToAdd = null;
        itemToAdd = null;
    }
}

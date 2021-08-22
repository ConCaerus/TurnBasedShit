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
                Weapon w = FindObjectOfType<PresetLibrary>().getWeapon(weaponToAdd.preset);
                Inventory.addWeapon(w);
            }

            if(armorToAdd != null) {
                Armor a = FindObjectOfType<PresetLibrary>().getArmor(armorToAdd.preset);
                Inventory.addArmor(a);
            }

            if(consumableToAdd != null) {
                Consumable c = FindObjectOfType<PresetLibrary>().getConsumable(consumableToAdd.preset);
                Inventory.addConsumable(c);
            }

            if(itemToAdd != null) {
                Item i = FindObjectOfType<PresetLibrary>().getItem(itemToAdd.preset);
                Inventory.addItem(i);
            }
        }

        weaponToAdd = null;
        armorToAdd = null;
        consumableToAdd = null;
        itemToAdd = null;
    }
}

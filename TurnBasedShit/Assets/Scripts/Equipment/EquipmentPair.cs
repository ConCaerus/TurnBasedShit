using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentPair", menuName = "Presets/EquipmentPair")]
public class EquipmentPair : ScriptableObject {
    public WeaponPreset[] pairedWeapons;
    public ArmorPreset[] pairedArmors;
    public ItemPreset[] pairedItems;

    public float powerMod = 1.5f, defenceMod = 0.1f, speedMod = 1.5f;

    public bool checkIfApplys(UnitStats stats) {
        //  returns false if stats has nothing equipped
        if((stats.equippedWeapon == null || stats.equippedWeapon.isEmpty()) && (stats.equippedArmor == null || stats.equippedArmor.isEmpty()) && (stats.equippedItem == null || stats.equippedArmor.isEmpty()))
            return false;
        //  check if doesn't have while stats has
        if(pairedWeapons.Length == 0 && stats.equippedWeapon != null && !stats.equippedWeapon.isEmpty())
            return false;
        if(pairedArmors.Length == 0 && stats.equippedArmor != null && !stats.equippedArmor.isEmpty())
            return false;
        if(pairedItems.Length == 0 && stats.equippedItem != null && !stats.equippedItem.isEmpty())
            return false;

        //  checks if any missmatches
        bool temp = false;
        if(pairedWeapons.Length > 0) {
            foreach(var i in pairedWeapons) {
                if(i.preset.isTheSameTypeAs(stats.equippedWeapon)) {
                    temp = true;
                    break;
                }
            }
            if(!temp)
                return false;
        }

        if(pairedArmors.Length > 0) {
            temp = false;
            foreach(var i in pairedArmors) {
                if(i.preset.isTheSameTypeAs(stats.equippedArmor)) {
                    temp = true;
                    break;
                }
            }
            if(!temp)
                return false;
        }

        if(pairedItems.Length > 0) {
            temp = false;
            foreach(var i in pairedItems) {
                if(i.preset.isTheSameTypeAs(stats.equippedItem)) {
                    temp = true;
                    break;
                }
            }
            if(!temp)
                return false;
        }
        return true;
    }
}

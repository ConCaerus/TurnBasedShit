using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetLibrary : MonoBehaviour {
    //  Equipment
    [SerializeField] WeaponPreset[] weapons;
    [SerializeField] ArmorPreset[] armor;
    [SerializeField] ConsumablePreset[] consumables;

    //  Map
    [SerializeField] BuildingPreset[] buildings;
    [SerializeField] CombatLocationPreset[] combatLocations;

    //  Equipment
    public Weapon getWeapon(string name) {
        foreach(var i in weapons) {
            if(i.preset.w_name == name)
                return i.preset;
        }
        return null;
    }
    public Weapon getRandomWeapon() {
        return weapons[Random.Range(0, weapons.Length)].preset;
    }

    public Armor getArmor(string name) {
        foreach(var i in armor) {
            if(i.preset.a_name == name)
                return i.preset;
        }
        return null;
    }
    public Armor getRandomArmor() {
        return armor[Random.Range(0, armor.Length)].preset;
    }

    public Consumable getConsumable(string name) {
        foreach(var i in consumables) {
            if(i.preset.c_name == name)
                return i.preset;
        }
        return null;
    }
    public Consumable getRandomConsumable() {
        return consumables[Random.Range(0, consumables.Length)].preset;
    }

    //  Map
    public Building getBuilding(Building.type t) {
        foreach(var i in buildings) {
            if(i.preset.b_type == t)
                return i.preset;
        }
        return null;
    }
    public Building getRandomBuilding() {
        return buildings[Random.Range(0, buildings.Length)].preset;
    }

    public CombatLocation getCombatLocation(CombatLocation.diffLevel lvl) {
        List<CombatLocation> temps = new List<CombatLocation>();

        foreach(var i in combatLocations) {
            if(i.preset.difficulty == lvl)
                temps.Add(i.preset);
        }
        return temps[Random.Range(0, temps.Count)];
    }
    public CombatLocation getRandomCombatLocation() {
        return combatLocations[Random.Range(0, combatLocations.Length)].preset;
    }
}

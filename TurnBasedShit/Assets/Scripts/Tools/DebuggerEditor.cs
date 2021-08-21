using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(DebuggerObj))]
public class DebuggerEditor : Editor {
    string invCount = "0";


    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var temp = (DebuggerObj)target;


        //  Party
        if(GUILayout.Button("Clear Party")) {
            Party.clearParty();
        }
        if(GUILayout.Button("Clear Party Equipment")) {
            Party.clearPartyEquipment();
        }
        if(GUILayout.Button("Add Unit")) {
            var thing = FindObjectOfType<PresetLibrary>().createRandomPlayerUnitStats();
            Debug.Log("Added " + thing.u_name);
            Party.addNewUnit(thing);
        }

        //  Inventory
        if(GUILayout.Button("Clear Inventory")) {
            Inventory.clearInventory();
        }

        GUILayout.BeginHorizontal();

        invCount = GUILayout.TextField(invCount, 25);

        if(GUILayout.Button("Send Shit To Inventory")) {
            addAndResetEquippmentToAdd(int.Parse(invCount), temp);
            invCount = "0";
        }

        GUILayout.EndHorizontal();
        if(GUILayout.Button("Reset Inventory Instance Queue"))
            GameInfo.clearInventoryInstanceIDQueue();

        //  Quests
        if(GUILayout.Button("Clear Active Quests")) {
            ActiveQuests.clear();
        }
        if(GUILayout.Button("Reset Quest Instance Queue")) {
            GameInfo.clearQuestInstanceIDQueue();
        }

        //  MapLocations
        if(GUILayout.Button("Populate Towns"))
            Map.populateTowns(FindObjectOfType<PresetLibrary>());
        if(GUILayout.Button("Clear Towns"))
            MapLocationHolder.clearTownLocations();
        if(GUILayout.Button("Reset Town Instance Queue"))
            GameInfo.clearTownInstanceIDQueue();
    }




    public void addAndResetEquippmentToAdd(int count = 0, DebuggerObj temp = null) {
        for(int j = 0; j < count; j++) {
            if(temp.weaponToAdd != null) {
                Weapon w = FindObjectOfType<PresetLibrary>().getWeapon(temp.weaponToAdd.preset);
                Inventory.addWeapon(w);
            }

            if(temp.armorToAdd != null) {
                Armor a = FindObjectOfType<PresetLibrary>().getArmor(temp.armorToAdd.preset);
                Inventory.addArmor(a);
            }

            if(temp.consumableToAdd != null) {
                Consumable c = FindObjectOfType<PresetLibrary>().getConsumable(temp.consumableToAdd.preset);
                Inventory.addConsumable(c);
            }

            if(temp.itemToAdd != null) {
                Item i = FindObjectOfType<PresetLibrary>().getItem(temp.itemToAdd.preset);
                Inventory.addItem(i);
            }
        }

        temp.weaponToAdd = null;
        temp.armorToAdd = null;
        temp.consumableToAdd = null;
        temp.itemToAdd = null;
    }
}
#endif
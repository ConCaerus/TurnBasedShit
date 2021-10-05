using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class DebuggerWindow : EditorWindow {
    int invCount = 0;
    [SerializeField] WeaponPreset weaponToAdd;
    [SerializeField] ArmorPreset armorToAdd;
    [SerializeField] ConsumablePreset consumableToAdd;
    [SerializeField] ItemPreset itemToAdd;

    [MenuItem("Window/Debugger")]
    public static void showWindow() {
        GetWindow<DebuggerWindow>("Debugger");
    }


    private void OnGUI() {
        //  Inventory
        GUILayout.Label("Inventory", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        weaponToAdd = (WeaponPreset)EditorGUILayout.ObjectField(weaponToAdd, typeof(WeaponPreset), true);
        armorToAdd = (ArmorPreset)EditorGUILayout.ObjectField(armorToAdd, typeof(ArmorPreset), true);
        consumableToAdd = (ConsumablePreset)EditorGUILayout.ObjectField(consumableToAdd, typeof(ConsumablePreset), true);
        itemToAdd = (ItemPreset)EditorGUILayout.ObjectField(itemToAdd, typeof(ItemPreset), true);

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        GUILayout.Label(invCount.ToString(), GUILayout.Width(30f));
        invCount = (int)GUILayout.HorizontalSlider(invCount, 0f, 100f);


        if(GUILayout.Button("Send Shit To Inventory")) {
            addAndResetEquippmentToAdd(invCount);
            invCount = 0;
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        if(GUILayout.Button("+25c"))
            Inventory.addCoins(25);
        if(GUILayout.Button("+50c"))
            Inventory.addCoins(50);
        if(GUILayout.Button("+100c"))
            Inventory.addCoins(100);
        if(GUILayout.Button("+500c"))
            Inventory.addCoins(500);
        if(GUILayout.Button("+1000c"))
            Inventory.addCoins(1000);

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        if(GUILayout.Button("Clear Inventory")) {
            Inventory.clearInventory(true);
        }

        GUILayout.EndHorizontal();

        //  Party
        GUILayout.Label("Party", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Clear Party")) {
            Party.clearParty(true);
        }
        if(GUILayout.Button("Clear Party Equipment")) {
            Party.clearPartyEquipment();
        }
        if(GUILayout.Button("Add Unit")) {
            var thing = FindObjectOfType<PresetLibrary>().createRandomPlayerUnitStats(true);
            Debug.Log("Added " + thing.u_name);
            Party.addUnit(thing);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Clear Graveyard"))
            Graveyard.clearGraveyard();
        GUILayout.EndHorizontal();

        //  Quests
        GUILayout.Label("Quests", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Clear Active Quests")) {
            ActiveQuests.clear(true);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("+Boss"))
            ActiveQuests.addQuest(FindObjectOfType<PresetLibrary>().createRandomBossFightQuest(true));
        if(GUILayout.Button("+Delivery"))
            ActiveQuests.addQuest(FindObjectOfType<PresetLibrary>().createRandomDeliveryQuest(true));
        if(GUILayout.Button("+Kill"))
            ActiveQuests.addQuest(FindObjectOfType<PresetLibrary>().createRandomKillQuest(true));
        if(GUILayout.Button("+Pickup"))
            ActiveQuests.addQuest(FindObjectOfType<PresetLibrary>().createRandomPickupQuest(true));
        GUILayout.EndHorizontal();

        //  MapLocations
        GUILayout.Label("Map", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Populate Towns"))
            Map.populateTowns(FindObjectOfType<PresetLibrary>());
        if(GUILayout.Button("Clear Towns"))
            MapLocationHolder.clearTownLocations();

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        if(GUILayout.Button("Clear Locations"))
            MapLocationHolder.clear();
        if(GUILayout.Button("+Upgrade"))
            MapLocationHolder.addLocation(FindObjectOfType<PresetLibrary>().createRandomUpgradeLocation());

        GUILayout.EndHorizontal();

        //  Save Data
        GUILayout.Label("Save Data", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Clear Save"))
            SaveData.deleteCurrentSave();
        if(GUILayout.Button("Clear Everything"))
            PlayerPrefs.DeleteAll();

        GUILayout.EndHorizontal();
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

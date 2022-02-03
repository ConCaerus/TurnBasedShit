using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;


public class DebuggerWindow : EditorWindow {
    int invCount = 0;
    [SerializeField] WeaponPreset weaponToAdd;
    [SerializeField] ArmorPreset armorToAdd;
    [SerializeField] ItemPreset itemToAdd;
    [SerializeField] UsablePreset usableToAdd;
    [SerializeField] UnusablePreset unusableToAdd;

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
        usableToAdd = (UsablePreset)EditorGUILayout.ObjectField(usableToAdd, typeof(UsablePreset), true);
        unusableToAdd = (UnusablePreset)EditorGUILayout.ObjectField(unusableToAdd, typeof(UnusablePreset), true);
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
            Inventory.clear(true);
        }

        GUILayout.EndHorizontal();

        //  Party
        GUILayout.Label("Party", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Clear Party")) {
            Party.clear(true);
        }
        if(GUILayout.Button("Add Unit")) {
            var thing = FindObjectOfType<PresetLibrary>().createRandomPlayerUnitStats(true);
            Debug.Log("Added " + thing.u_name);
            Party.addUnit(thing);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Clear Graveyard"))
            Graveyard.clear();
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
            ActiveQuests.addQuest(FindObjectOfType<PresetLibrary>().createRandomBossFightQuest(true, true, GameInfo.getCurrentRegion()));
        if(GUILayout.Button("+Delivery"))
            ActiveQuests.addQuest(FindObjectOfType<PresetLibrary>().createRandomDeliveryQuest(true, GameInfo.getCurrentRegion()));
        if(GUILayout.Button("+Kill"))
            ActiveQuests.addQuest(FindObjectOfType<PresetLibrary>().createRandomKillQuest(true, GameInfo.getCurrentRegion()));
        if(GUILayout.Button("+Pickup"))
            ActiveQuests.addQuest(FindObjectOfType<PresetLibrary>().createRandomPickupQuest(true, true, GameInfo.getCurrentRegion()));
        if(GUILayout.Button("+Rescue"))
            ActiveQuests.addQuest(FindObjectOfType<PresetLibrary>().createRandomRescueQuest(true, true, GameInfo.getCurrentRegion()));
        if(GUILayout.Button("+Fishing"))
            ActiveQuests.addQuest(FindObjectOfType<PresetLibrary>().createRandomFishingQuest(true, GameInfo.getCurrentRegion()));
        GUILayout.EndHorizontal();

        //  MapLocations
        GUILayout.Label("Map", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Set Grassland"))
            GameInfo.setCurrentRegion(GameInfo.region.grassland);
        if(GUILayout.Button("Set Forest"))
            GameInfo.setCurrentRegion(GameInfo.region.forest);
        if(GUILayout.Button("Set Swamp"))
            GameInfo.setCurrentRegion(GameInfo.region.swamp);
        if(GUILayout.Button("Set Mountains"))
            GameInfo.setCurrentRegion(GameInfo.region.mountains);
        if(GUILayout.Button("Set Hell"))
            GameInfo.setCurrentRegion(GameInfo.region.hell);

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        if(GUILayout.Button("Clear Locations"))
            MapLocationHolder.clear();
        if(GUILayout.Button("+Upgrade"))
            MapLocationHolder.addLocation(FindObjectOfType<PresetLibrary>().createUpgradeLocation(GameInfo.getCurrentRegion()));

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        if(GUILayout.Button("Clear Fog Data"))
            Map.clearFogTexture();

        GUILayout.EndHorizontal();

        //  Save Data
        GUILayout.Label("Save Data", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Create Save Data")) {
            SaveData.deleteCurrentSave();
            SaveData.createSaveDataForCurrentSave(FindObjectOfType<PresetLibrary>(), FindObjectOfType<TransitionCanvas>());
        }
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
                Inventory.addCollectable(w, FindObjectOfType<PresetLibrary>());
            }

            if(armorToAdd != null) {
                Armor a = FindObjectOfType<PresetLibrary>().getArmor(armorToAdd.preset);
                Inventory.addCollectable(a, FindObjectOfType<PresetLibrary>());
            }

            if(usableToAdd != null) {
                Usable c = FindObjectOfType<PresetLibrary>().getUsable(usableToAdd.preset);
                Inventory.addCollectable(c, FindObjectOfType<PresetLibrary>());
            }

            if(unusableToAdd != null) {
                Unusable c = FindObjectOfType<PresetLibrary>().getUnusable(unusableToAdd.preset);
                Inventory.addCollectable(c, FindObjectOfType<PresetLibrary>());
            }

            if(itemToAdd != null) {
                Item i = FindObjectOfType<PresetLibrary>().getItem(itemToAdd.preset);
                Inventory.addCollectable(i, FindObjectOfType<PresetLibrary>());
            }
        }

        weaponToAdd = null;
        armorToAdd = null;
        usableToAdd = null;
        unusableToAdd = null;
        itemToAdd = null;
    }
}

#endif
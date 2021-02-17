using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InventoryPopulator))]
public class InventoryPopulatorEditor : Editor {

    string count = "0";

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        InventoryPopulator ip = (InventoryPopulator)target;

        if(GUILayout.Button("Reset Inventory")) {
            ip.resetInventory();
        }

        GUILayout.BeginHorizontal();

        count = GUILayout.TextField(count, 25);

        if(GUILayout.Button("Send Shit To Inventory")) {
            ip.addAndResetEquippmentToAdd(int.Parse(count));
            count = "0";
        }

        GUILayout.EndHorizontal();
    }
}
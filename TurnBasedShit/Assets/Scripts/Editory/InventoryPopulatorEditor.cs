using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InventoryPopulator))]
public class InventoryPopulatorEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        InventoryPopulator ip = (InventoryPopulator)target;

        if(GUILayout.Button("Send Equippment To Inventory")) {
            ip.addAndResetEquippmentToAdd();
        }
    }
}
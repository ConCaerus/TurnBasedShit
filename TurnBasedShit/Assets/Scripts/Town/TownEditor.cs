using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(TownInstance))]
public class TownEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        TownInstance ti = (TownInstance)target;

        if(GUILayout.Button("Add Rand Buildings")) {
            ti.town.addRandomBuildings(ti.buildingCount);
        }
        if(GUILayout.Button("Add Empty Buildings")) {
            ti.town.addEmptyBuildings(ti.buildingCount);
        }
        if(GUILayout.Button("Instantiate Buildings")) {
            foreach(var i in FindObjectsOfType<BuildingInstance>())
                DestroyImmediate(i.gameObject);
            ti.instantiateBuildings();
        }
    }
}
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(EquipmentTransformSetter))]
public class EquipmentPresetEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        var eq = (EquipmentTransformSetter)target;

        GUILayout.Label("Save info");
        if(GUILayout.Button("Set Transform"))
            eq.setTransform();

        GUILayout.Label("Sprites");
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Apply Sprites"))
            eq.applySprites();
        if(GUILayout.Button("View For All"))
            eq.veiwForAll();
        if(GUILayout.Button("Clear Sprites"))
            eq.clearSprites();
        GUILayout.EndHorizontal();

        GUILayout.Label("Unit");
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Apply Body") && eq.body != null) 
            eq.applyBody();
        if(GUILayout.Button("Apply Head") && eq.head != null)
            eq.applyHead();
        GUILayout.EndHorizontal();

    }
}

#endif

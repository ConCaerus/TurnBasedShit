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
        if(GUILayout.Button("Set Transform")) {
            if(eq.wePreset != null && eq.arPreset == null) {
                var existingSO = AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(eq.wePreset.GetInstanceID()), typeof(ScriptableObject));
                Debug.Log(AssetDatabase.GetAssetPath(eq.wePreset.GetInstanceID()));

                //  make changes
                //  returns out if an error did happen
                if(!eq.setTransform())
                    return;

                //  override and save changes
                EditorUtility.SetDirty(existingSO);
            }
            else if(eq.wePreset == null && eq.arPreset != null) {
                var existingSO = AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(eq.arPreset.GetInstanceID()), typeof(ScriptableObject));
                Debug.Log(AssetDatabase.GetAssetPath(eq.arPreset.GetInstanceID()));

                //  make changes
                //  returns out if an error did happen
                if(!eq.setTransform())
                    return;

                //  override and save changes
                EditorUtility.SetDirty(existingSO);
            }
            else {
                Debug.Log("Only change one preset's transform at a time");
                return;
            }
            AssetDatabase.SaveAssets();

            //  resets all variables
            eq.arPreset = null;
            eq.wePreset = null;
        }

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

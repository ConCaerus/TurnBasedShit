using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(BuildingPreset))]
public class BuildingPresetEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        BuildingPreset bp = (BuildingPreset)target;

        if(GUILayout.Button("Set Sprite")) {
            bp.preset.b_sprite.setSprite();
        }
    }
}
#endif
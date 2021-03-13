using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ArmorPreset))]
public class ArmorPresetEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var t = (ArmorPreset)target;

        if(GUILayout.Button("Set Sprite Location")) {
            t.preset.a_sprite.setSprite();
        }
    }
}
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemPreset))]
public class ItemPresetEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var t = (ItemPreset)target;

        if(GUILayout.Button("Set Sprite Location")) {
            t.preset.i_sprite.setLocation();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(WeaponPreset))]
public class WeaponPresetEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var t = (WeaponPreset)target;

        if(GUILayout.Button("Set Sprite Location")) {
            t.preset.w_sprite.setSprite();
        }
    }
}
#endif
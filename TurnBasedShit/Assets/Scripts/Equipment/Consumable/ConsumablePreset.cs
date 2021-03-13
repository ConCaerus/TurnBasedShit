using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "ConsumablePreset", menuName = "Presets/ConsumablePreset")]
public class ConsumablePreset : ScriptableObject {
    public Consumable preset;
}

#if UNITY_EDITOR
[CustomEditor(typeof(ConsumablePreset))]
public class ConsumablePresetEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var t = (ConsumablePreset)target;

        if(GUILayout.Button("Set Sprite Location")) {
            t.preset.c_sprite.setSprite();
        }
    }
}
#endif
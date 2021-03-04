using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "ConsumablePreset", menuName = "Presets/ConsumablePreset")]
public class ConsumablePreset : ScriptableObject {
    public Consumable preset;
}

[CustomEditor(typeof(ConsumablePreset))]
public class ConsumablePresetEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var t = (ConsumablePreset)target;

        if(GUILayout.Button("Set Sprite Location")) {
            t.preset.i_sprite.setLocation();
        }
    }
}

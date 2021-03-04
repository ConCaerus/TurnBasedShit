using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "CombatLocation", menuName = "Presets/CombatLocation")]
public class CombatLocationPreset : ScriptableObject {
    public CombatLocation preset;

    public List<GameObject> enemies = new List<GameObject>();
}

[CustomEditor(typeof(CombatLocationPreset))]
public class CombatLocationEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        var t = (CombatLocationPreset)target;

        if(GUILayout.Button("Set Enemies")) {
            t.preset.enemies.Clear();

            foreach(var i in t.enemies)
                t.preset.enemies.Add(i.GetComponent<UnitClass>().stats);

            t.enemies.Clear();
        }
    }
}

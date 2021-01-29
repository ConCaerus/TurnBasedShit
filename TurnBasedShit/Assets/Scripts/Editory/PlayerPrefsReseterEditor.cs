using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerPrefsReseter))]
public class PlayerPrefsReseterEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        PlayerPrefsReseter temp = (PlayerPrefsReseter)target;

        if(GUILayout.Button("Reset Player Prefs"))
            temp.resetPlayerPrefs();
        if(GUILayout.Button("Reset Player Unit Equippment"))
            temp.resetPlayerUnitEquippment();
    }
}

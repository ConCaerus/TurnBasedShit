using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ShadowObject))]
public class ShadowObjectEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        ShadowObject ip = (ShadowObject)target;

        if(GUILayout.Button("Set Sprite")) {
            ip.updateSprite();
        }
    }
}
#endif
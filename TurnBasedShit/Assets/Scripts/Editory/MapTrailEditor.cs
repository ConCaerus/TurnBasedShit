using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapTrail))]
public class MapTrailEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var t = (MapTrail)target;

        if(GUILayout.Button("Reset Saved Trail")) {
            PlayerPrefs.DeleteKey("Trail Anchors");
            PlayerPrefs.DeleteKey("Map Locations");
        }
    }
}

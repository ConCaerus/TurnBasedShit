using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(PartyPopulator))]
public class PartyEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var temp = (PartyPopulator)target;

        if(GUILayout.Button("Clear Party")) {
            temp.clearParty();
        }

        if(GUILayout.Button("Clear Party Equipment")) {
            temp.clearPartyEquippment();
        }

        if(GUILayout.Button("Add Unit")) {
            temp.addUnit();
        }
    }
}
#endif
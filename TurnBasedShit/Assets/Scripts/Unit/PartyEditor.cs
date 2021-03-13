using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(PartyObject))]
public class PartyEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var temp = (PartyObject)target;

        if(GUILayout.Button("Reset Party")) {
            Party.clearParty();
        }

        if(GUILayout.Button("Reset Party Equipment")) {
            Party.clearPartyEquipment();
        }

        if(GUILayout.Button("Add Unit To Party")) {
            temp.addUnitToAdd();
        }
    }
}
#endif
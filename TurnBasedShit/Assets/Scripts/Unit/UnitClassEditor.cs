using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//  didnt want to change the name of the actual script
[CustomEditor(typeof(PlayerUnitInstance))]
public class UnitClassEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        PlayerUnitInstance unit = (PlayerUnitInstance)target;

        if(GUILayout.Button("Reset Unit Saved Equippment")) {
            unit.resetSavedEquippment();
        }

        if(GUILayout.Button("Set Equipment")) {
            unit.setEquipment();
        }

        if(GUILayout.Button("Set Sprite")) {
            unit.resetSpriteAndColor();
        }
    }
}

[CustomEditor(typeof(EnemyUnitInstance))]
public class EnemyUnitClassEditory : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EnemyUnitInstance unit = (EnemyUnitInstance)target;

        if(GUILayout.Button("Reset Unit Saved Equippment")) {
            unit.resetSavedEquippment();
        }

        if(GUILayout.Button("Set Equipment")) {
            unit.setEquipment();
        }

        if(GUILayout.Button("Set Sprite")) {
            unit.resetSpriteAndColor();
        }
    }
}

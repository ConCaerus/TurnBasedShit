using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestStorySystem))]
public class ASDFASDFASDFE : Editor {

    string choice = "0";

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();


        TestStorySystem tss = (TestStorySystem)target;


        GUILayout.BeginHorizontal();

        choice = GUILayout.TextField(choice, 25);

        if(GUILayout.Button("Select Choice")) {
            tss.advanceStory(int.Parse(choice));
            choice = "0";
        }

        GUILayout.EndHorizontal();

        if(GUILayout.Button("Reset Story")) {
            tss.resetStory();
        }
    }
}

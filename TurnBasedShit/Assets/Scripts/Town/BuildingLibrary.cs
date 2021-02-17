using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class BuildingLibrary {


    public static Building getBuildingOfType(Building.type type) {
        var buildings = AssetDatabase.FindAssets("t:BuildingPreset", null);
        foreach(var i in buildings) {
            var g = AssetDatabase.GUIDToAssetPath(i);
            BuildingPreset b = (BuildingPreset)AssetDatabase.LoadAssetAtPath(g, typeof(BuildingPreset));
            if(b.preset.b_type == type)
                return b.preset;
        }
        return null;
    }

    public static Building getBuildingFromIndex(int type) {
        var buildings = AssetDatabase.FindAssets("t:BuildingPreset", null);
        var g = AssetDatabase.GUIDToAssetPath(buildings[type]);
        BuildingPreset b = (BuildingPreset)AssetDatabase.LoadAssetAtPath(g, typeof(BuildingPreset));
        return b.preset;
    }
}

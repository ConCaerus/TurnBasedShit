using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class SpriteLocation {
    [SerializeField] Sprite startingSprite = null;
    public string s_location = "", s_name = "";

    public Sprite getSprite() {
        if(startingSprite != null)
            return startingSprite;
        var sprites = AssetDatabase.LoadAllAssetsAtPath(s_location);

        foreach(var i in sprites) {
            if(i.name == s_name && i.GetType() == typeof(Sprite)) {
                return (Sprite)i;
            }
        }

        Debug.Log("Could not load sprite for some reason");
        return null;
    }

    public void setLocation(Sprite s = null) {
        if(s == null && startingSprite != null) {
            s = startingSprite;
            s.name = startingSprite.name;
        }
        if(s == null) return;
        startingSprite = s;
        s_name = s.name;
        startingSprite.name = s_name;

        s_location = AssetDatabase.GetAssetPath(s);
    }

    public void clear() {
        s_location = "";
        s_name = "";
        startingSprite = null;
    }
}

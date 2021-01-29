using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class SpriteLocation {
    [SerializeField] Sprite startingSprite;
    [SerializeField] Texture2D weirdSprite;
    public string s_location, s_name;

    public Sprite getSprite() {
        var sprites = AssetDatabase.LoadAllAssetsAtPath(s_location);

        foreach(var i in sprites) {
            if(i.name == s_name) {
                if(i.GetType() == typeof(Texture2D)) {
                    var tex = (Texture2D)i;
                    var rect = new Rect(0, 0, tex.width, tex.height);
                    Sprite temp = Sprite.Create(tex, rect, Vector2.zero);
                    weirdSprite = tex;
                    startingSprite = (Sprite)temp;
                    return (Sprite)temp;
                }
                startingSprite = (Sprite)i;
                return (Sprite)i;
            }
        }
        return null;
    }

    public void setLocation(Sprite s = null) {
        if(s == null && startingSprite != null) s = startingSprite;
        if(s == null && weirdSprite != null) {
            setLocation(weirdSprite);
            return;
        }
        if(s == null) return;
        startingSprite = s;
        s_name = s.name;

        s_location = AssetDatabase.GetAssetPath(s);
        if(string.IsNullOrEmpty(s_location)) {
            setLocation(s.texture);
            return;
        }
    }

    public void setLocation(Texture2D s) {
        if(s == null && weirdSprite != null) s = weirdSprite;
        if(s == null && startingSprite != null) {
            setLocation(startingSprite);
            return;
        }
        if(s == null) return;

        var rect = new Rect(0, 0, s.width, s.height);
        Sprite temp = Sprite.Create(s, rect, Vector2.zero);
        startingSprite = temp;
        startingSprite.name = s.name;
        s_name = s.name;

        s_location = AssetDatabase.GetAssetPath(s);
        if(string.IsNullOrEmpty(s_location)) {
            setLocation(startingSprite);
            return;
        }
    }

    public void clear() {
        s_location = "";
        s_name = "";
        startingSprite = null;
        weirdSprite = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteLoader {
    [SerializeField] Sprite sprite;
    [SerializeField] string sInfo;


    public void setSprite(Sprite s = null) {
        if(s == null && sprite != null) {
            s = sprite;
        }
        else if(s == null && !string.IsNullOrEmpty(sInfo)) {
            Texture2D t = new Texture2D(2, 2);

            if(t.LoadImage(getInfoFromString())) {
                s = texToSprite(t);
            }
        }
        if(s == null) {
            Debug.LogError("Tried to save null sprite");
            return;
        }

        sprite = s;
        var tex = spriteToTex(s);
        setInfoString(tex.EncodeToPNG());
    }
    public void setTexture(Texture2D tex) {
        setInfoString(tex.EncodeToPNG());
        sprite = texToSprite(tex);
    }

    Texture2D spriteToTex(Sprite s) {
        return s.texture;
    }

    Sprite texToSprite(Texture2D tex) {
        return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    }


    public Sprite getSpriteFromInfo() {
        if(!string.IsNullOrEmpty(sInfo)) {
            Texture2D tex = new Texture2D(2, 2);

            if(tex.LoadImage(getInfoFromString())) {
                return texToSprite(tex);
            }
        }
        Debug.LogError("Tried to get sprite from no info");
        return null;
    }


    public string getInfoString() {
        return sInfo;
    }
    public byte[] getInfoFromString() {
        return System.Convert.FromBase64String(sInfo);
    }
    public void setInfoString(byte[] info) {
        sInfo = System.Convert.ToBase64String(info);
    }
    public Sprite getSprite() {
        if(!string.IsNullOrEmpty(sInfo)) {
            return getSpriteFromInfo();
        }
        if(sprite != null) {
            return sprite;
        }
        Debug.LogError("Tried to get null sprite");
        return null;
    }

    public void clear() {
        sprite = null;
        sInfo = string.Empty;
    }

    public bool hasSprite() {
        if(sprite == null && string.IsNullOrEmpty(getInfoString())) return false;
        return true;
    }
}

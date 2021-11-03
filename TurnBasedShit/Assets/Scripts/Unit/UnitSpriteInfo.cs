using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitSpriteInfo {
    public int headIndex = 0, faceIndex = 0, bodyIndex = 0;
    public int layerOffset = 0;
    public Color color = Color.white;

    public void randomize(PresetLibrary lib) {
        headIndex = Random.Range(0, lib.getHeadCount());
        faceIndex = Random.Range(0, lib.getFaceCount());
        bodyIndex = Random.Range(0, lib.getBodyCount());
        color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
    }

    public void setEqualTo(UnitSpriteInfo other) {
        if(other == null)
            return;
        headIndex = other.headIndex;
        faceIndex = other.faceIndex;
        bodyIndex = other.bodyIndex;
        color = other.color;
    }

    public bool isEqualTo(UnitSpriteInfo other) {
        if(other == null)
            return false;

        return headIndex == other.headIndex && faceIndex == other.faceIndex && bodyIndex == other.bodyIndex && color == other.color;
    }
}

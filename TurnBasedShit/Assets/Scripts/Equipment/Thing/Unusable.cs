using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Unusable : Collectable {

    [SerializeField] UnusableSpriteHolder sprite;
    public int maxStackCount = 1;

    public override void setEqualTo(Collectable col, bool takeInstanceID) {
        if(col.type != collectableType.unusable || col == null || col.isEmpty())
            return;

        var other = (Unusable)col;
        if(other == null || other.isEmpty())
            return;
        matchParentValues(col, takeInstanceID);
        maxStackCount = other.maxStackCount;
        sprite = other.sprite;
    }


    public UnusableSpriteHolder getSpriteHolder() {
        return sprite;
    }
}


[System.Serializable]
public class UnusableSpriteHolder {
    public Sprite sprite;
}
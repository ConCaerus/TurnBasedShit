using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Unusable : Collectable {

    [SerializeField] UnusableSpriteHolder sprite;

    public override void setEqualTo(Collectable col, bool takeInstanceID) {
        if(col.type != collectableType.Unusable || col == null)
            return;

        var other = (Unusable)col;
        if(other == null)
            return;
        matchParentValues(col, takeInstanceID);
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
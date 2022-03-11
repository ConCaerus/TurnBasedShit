using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EyeLocation : MapLocation {

    public EyeLocation(Vector2 p, GameInfo.region reg) {
        type = locationType.eye;
        region = reg;
        pos = p;
    }

    public override void enterLocation(TransitionCanvas tc) {
    }
}

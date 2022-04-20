using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BridgeLocation : MapLocation {
    public bool advancing; // if the bridge is leading to the next region and not returning to a previos region

    public BridgeLocation(float yPos, bool advancingBridge, GameInfo.region reg) {
        region = reg;
        type = locationType.bridge;
        advancing = advancingBridge;
        pos = (advancing) ? new Vector2(Map.rightBound(), yPos) : new Vector2(Map.leftBound(), yPos);
    }



    public override void enterLocation(TransitionCanvas tc) {
        GameInfo.setCurrentMapPos(pos);

        if(!advancing) {
            GameInfo.setCurrentRegion(GameInfo.getCurrentRegion() - 1);
        }

        tc.loadSceneWithTransition("Bridge");
    }
}

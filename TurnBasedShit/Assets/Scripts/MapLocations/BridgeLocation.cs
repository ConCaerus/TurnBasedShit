using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BridgeLocation : MapLocation {
    public bool advancing; // if the bridge is leading to the next region and not returning to a previos region

    public BridgeLocation(float yPos, bool advancingBridge, GameInfo.region reg) {
        region = reg;
        advancing = advancingBridge;
        pos = (advancing) ? new Vector2(Map.rightBound(), yPos) : new Vector2(Map.leftBound(), yPos);
    }



    public override void enterLocation(TransitionCanvas tc) {
        if(advancing) {
            GameInfo.setCurrentRegion((GameInfo.region)((int)GameInfo.getCurrentRegion() + 1));
            GameInfo.setCurrentMapPos(new Vector2(Map.leftBound(), pos.y));
        }
        else {
            GameInfo.setCurrentRegion((GameInfo.region)((int)GameInfo.getCurrentRegion() - 1));
            GameInfo.setCurrentMapPos(new Vector2(Map.rightBound(), pos.y));
        }

        tc.loadSceneWithTransition("Map");
    }

    public override bool isEqualTo(MapLocation other) {
        if(other.type != type || region != other.region)
            return false;
        return advancing == ((BridgeLocation)other).advancing;
    }
}

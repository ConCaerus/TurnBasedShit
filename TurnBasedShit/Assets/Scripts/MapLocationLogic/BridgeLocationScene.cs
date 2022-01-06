using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeLocationScene : MonoBehaviour {

    private void Start() {
        if(GameInfo.getCurrentMapPos().x < 0.0f) {
            FindObjectOfType<UnitMovement>().transform.position = new Vector3(7.0f, FindObjectOfType<UnitMovement>().transform.position.y);
        }
    }

    void goToNextRegion() {
        GameInfo.setCurrentRegion(GameInfo.getCurrentRegion() + 1);
        var pos = new Vector2(Map.leftBound(), GameInfo.getCurrentMapPos().y);
        GameInfo.setCurrentMapPos(pos);
        FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("Map");
    }

    void goToPrevRegion() {
        var pos = new Vector2(Map.rightBound(), GameInfo.getCurrentMapPos().y);
        GameInfo.setCurrentMapPos(pos);
        FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("Map");
    }

    public void leaveArea() {
        if(FindObjectOfType<UnitMovement>().transform.position.x > 0.0f)
            goToNextRegion();
        else
            goToPrevRegion();
    }
}

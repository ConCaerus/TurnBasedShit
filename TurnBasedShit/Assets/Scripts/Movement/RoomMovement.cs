using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomMovement : LocationMovement {
    [SerializeField] GameObject door;
    [SerializeField] GameObject[] buildingOjbects;

    [SerializeField] doorDestinations destination;

    public UnityEvent interactFunc, deinteractFunc;

    float distToInt = 0.5f;

    bool interacting = false;


    enum doorDestinations {
        town, map
    }


    public override void interact() {
        if(interacting)
            return;
        foreach(var i in buildingOjbects) {
            if(Mathf.Abs(i.transform.position.x - transform.position.x) < distToInt) {
                flip(false);
                canMove = false;
                interacting = true;
                interactFunc.Invoke();
            }
        }


        if(Mathf.Abs(door.transform.position.x - transform.position.x) < distToInt) {
            if(destination == doorDestinations.town)
                GameInfo.getCurrentLocationAsTown().enterLocation(FindObjectOfType<TransitionCanvas>());
            else if(destination == doorDestinations.map)
                FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("Map");
        }
    }

    public override void deinteract() {
        if(!interacting)
            return;
        flip(true);
        canMove = true;
        interacting = false;
        deinteractFunc.Invoke();
    }
}

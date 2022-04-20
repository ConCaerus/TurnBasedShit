using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomMovement : LocationMovement {
    [SerializeField] GameObject door;
    public GameObject[] buildingOjbects;
    [SerializeField] AudioClip doorSound;

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


        if(door != null && Mathf.Abs(door.transform.position.x - transform.position.x) < distToInt) {
            if(destination == doorDestinations.town) {
                FindObjectOfType<AudioManager>().playSound(doorSound);
                GameInfo.getCurrentLocationAsTown().enterLocation(FindObjectOfType<TransitionCanvas>());
            }
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

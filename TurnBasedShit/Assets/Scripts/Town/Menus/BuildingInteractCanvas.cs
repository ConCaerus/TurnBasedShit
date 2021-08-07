using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BuildingInteractCanvas : MonoBehaviour {

    public void interactWithBuilding(BuildingInstance b) {
        switch(b.b_type) {
            case Building.type.Hospital:
                FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("HospitalBuilding");
                break;

            case Building.type.Church:
                FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("HospitalBuilding");
                break;

            case Building.type.Shop:
                FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("HospitalBuilding");
                break;
        }
    }
}

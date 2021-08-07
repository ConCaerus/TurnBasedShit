using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMovement : LocationMovement {
    [SerializeField] GameObject buildingObject, door;

    float distToInt = 0.5f;


    public override void interact() {
        if(Mathf.Abs(buildingObject.transform.position.x - transform.position.x) < distToInt) {
            if(FindObjectOfType<BuildingCanvas>() != null)
                FindObjectOfType<BuildingCanvas>().showCanvas();
            else if(FindObjectOfType<ShopCanvas>() != null)
                FindObjectOfType<ShopCanvas>().showCanvas();
        }


        if(Mathf.Abs(door.transform.position.x - transform.position.x) < distToInt)
            FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("Town");
    }

    public override void deinteract() {
        if(FindObjectOfType<BuildingCanvas>() != null)
            FindObjectOfType<BuildingCanvas>().hideCanvas();
        else if(FindObjectOfType<ShopCanvas>() != null)
            FindObjectOfType<ShopCanvas>().hideCanvas();
    }
}

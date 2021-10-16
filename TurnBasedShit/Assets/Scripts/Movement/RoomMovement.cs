using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMovement : LocationMovement {
    [SerializeField] GameObject door;
    [SerializeField] GameObject[] buildingOjbects;

    [SerializeField] doorDestinations destination;

    float distToInt = 0.5f;


    enum doorDestinations {
        town, map
    }


    public override void interact() {
        foreach(var i in buildingOjbects) {
            if(Mathf.Abs(i.transform.position.x - transform.position.x) < distToInt) {
                flip(false);
                canMove = false;
                if(FindObjectOfType<BuildingCanvas>() != null)
                    FindObjectOfType<BuildingCanvas>().showCanvas();
                else if(FindObjectOfType<ShopCanvas>() != null)
                    FindObjectOfType<ShopCanvas>().showCanvas();
                else if(FindObjectOfType<CasinoCanvas>() != null && !FindObjectOfType<CasinoCanvas>().transform.GetChild(0).gameObject.activeInHierarchy)
                    FindObjectOfType<CasinoCanvas>().showCanvas(i.gameObject);
                else if(FindObjectOfType<UpgradeCanvas>() != null && !FindObjectOfType<UpgradeCanvas>().isShowing && !FindObjectOfType<UpgradeCanvas>().hasBeenUsed)
                    FindObjectOfType<UpgradeCanvas>().show();
                else if(FindObjectOfType<BlacksmithCanvas>() != null && !FindObjectOfType<BlacksmithCanvas>().isShowing)
                    FindObjectOfType<BlacksmithCanvas>().show();
                else
                    canMove = true;
            }
        }


        if(Mathf.Abs(door.transform.position.x - transform.position.x) < distToInt) {
            if(destination == doorDestinations.town)
                FindObjectOfType<TransitionCanvas>().loadSceneWithFunction(GameInfo.getCurrentLocationAsTown().enterLocation);
            else if(destination == doorDestinations.map)
                FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("Map");
        }
    }

    public override void deinteract() {
        flip(true);
        canMove = true;
        if(FindObjectOfType<BuildingCanvas>() != null)
            FindObjectOfType<BuildingCanvas>().hideCanvas();
        else if(FindObjectOfType<ShopCanvas>() != null)
            FindObjectOfType<ShopCanvas>().hideCanvas();
        else if(FindObjectOfType<CasinoCanvas>() != null)
            FindObjectOfType<CasinoCanvas>().hideCanvas();
        else if(FindObjectOfType<UpgradeCanvas>() != null && FindObjectOfType<UpgradeCanvas>().isShowing)
            FindObjectOfType<UpgradeCanvas>().hide();
        else if(FindObjectOfType<BlacksmithCanvas>() != null && FindObjectOfType<BlacksmithCanvas>().isShowing)
            FindObjectOfType<BlacksmithCanvas>().hide();
    }
}

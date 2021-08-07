using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMovement : LocationMovement {

    public override void interact() {
        //  opens the upgrade canvas
        if(!FindObjectOfType<ShrineObject>().isInMenu() && Mathf.Abs(transform.position.x - FindObjectOfType<ShrineObject>().transform.position.x) < FindObjectOfType<ShrineObject>().distToInteract)
            FindObjectOfType<ShrineObject>().showCanvas();
    }


    public override void deinteract() {
        if(FindObjectOfType<ShrineObject>().isInMenu())
            FindObjectOfType<ShrineObject>().hideCanvas();
    }

}

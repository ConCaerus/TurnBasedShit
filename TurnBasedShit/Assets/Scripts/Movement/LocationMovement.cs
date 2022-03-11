using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class LocationMovement : InteractiveMovement {
    public float rightMost, leftMost;

    public override bool canMoveAlongY() {
        return false;
    }
    public override bool showWeapon() {
        return false;
    }

    public override bool shouldInteract() {
        return Input.GetKeyDown(KeyCode.W);
    }
    public override bool shouldDeinteract() {
        return Input.GetKeyDown(KeyCode.S);
    }
}

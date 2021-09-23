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
        return Input.GetKey(KeyCode.W);
    }
    public override bool shouldDeinteract() {
        return Input.GetKey(KeyCode.S);
    }

    public override bool outOfBounds() {
        return transform.position.x > rightMost || transform.position.x < leftMost;
    }
}

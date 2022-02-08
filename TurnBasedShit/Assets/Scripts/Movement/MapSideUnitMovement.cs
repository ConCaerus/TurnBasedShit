using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSideUnitMovement : UnitMovement {
    public UnitStats referenceStats;


    private void Update() {
        if(GetComponentInChildren<UnitSpriteHandler>().initialized) {
            if(isMoving)
                GetComponentInChildren<UnitSpriteHandler>().setWalkingAnim(true);
            else
                GetComponentInChildren<UnitSpriteHandler>().setWalkingAnim(false);
        }
    }

    public void moveToPoint(Vector2 point) {
        var sp = moveSpeed + Vector2.Distance(transform.position, point) / 5.0f;
        Vector2 target = Vector2.zero;

        
        //  Y AXIS
        //  up
        if(point.y - 0.25f > transform.position.y) {
            if(movingDown)
                flip(false);
            movingDown = false;
            target += Vector2.up;
        }

        //  down
        else if(point.y + 0.25f < transform.position.y) {
            if(!movingDown)
                flip(true);
            movingDown = true;
            target += Vector2.down;
        }

        //  X AXIS
        //  right
        if(point.x - 0.25f > transform.position.x) {
            if(!movingRight)
                flip(true);

            target += Vector2.right;
        }

        //  left
        else if(point.x + 0.25f < transform.position.x) {
            if(movingRight)
                flip(true);

            target += Vector2.right;
        }

        transform.Translate(target * sp * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);

        if(anim == null)
            anim = StartCoroutine(walkAnim());
    }

    public override bool showWeapon() {
        return true;
    }
}

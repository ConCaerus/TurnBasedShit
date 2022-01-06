using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CombatSpot : MonoBehaviour {
    public GameObject unit = null;
    float speed = 0.25f;

    public bool isPlayerSpot() {
        return transform.position.x < 0.0f;
    }


    public void setColor() {
        if(unit == null) {
            GetComponent<SpriteRenderer>().DOColor(Color.gray, speed);
            return;
        }
        if(isPlayerSpot()) {
            var col = unit.GetComponent<UnitClass>().stats.u_sprite.color * 2.0f;
            GetComponent<SpriteRenderer>().DOColor(new Color(col.r, col.g, col.b, 1.0f), speed);
            return;
        }
        GetComponent<SpriteRenderer>().DOColor(Color.white, speed);
    }
}

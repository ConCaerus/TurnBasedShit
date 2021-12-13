using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CombatSpot : MonoBehaviour {
    public GameObject unit = null;
    float speed = 0.25f;

    private void Start() {
        setColor();
    }

    public bool isPlayerSpot() {
        return transform.position.x < 0.0f;
    }


    public void setColor() {
        if(unit == null)
            GetComponent<SpriteRenderer>().DOColor(Color.gray, speed);
        else if(unit != null && unit.GetComponentInChildren<UnitSpriteHandler>() != null) {
            var col = unit.GetComponentInChildren<UnitSpriteHandler>().getColor() + Color.white / 2.0f;
            GetComponent<SpriteRenderer>().DOColor(new Color(col.r, col.g, col.b, 1.0f), speed);
        }
        else if(unit != null)
            GetComponent<SpriteRenderer>().DOColor(Color.white, speed);
    }
}

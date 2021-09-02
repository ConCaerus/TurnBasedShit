using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CombatSpot : MonoBehaviour {
    public GameObject unit = null;
    float speed = 0.25f;


    [SerializeField] Material shadedMat, normalMat;

    void Start() {
        GetComponent<SpriteRenderer>().material = normalMat;
    }

    public bool isPlayerSpot() {
        return transform.position.x < 0.0f;
    }


    public void updateRenderer() {
        if(unit != null) {
            GetComponent<SpriteRenderer>().DOColor(Color.white, speed);
            GetComponent<SpriteRenderer>().material = shadedMat;
            transform.GetChild(0).gameObject.SetActive(true);
            setColor();
        }
        else {
            GetComponent<SpriteRenderer>().DOColor(Color.gray, speed);
            GetComponent<SpriteRenderer>().material = normalMat;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }


    public void setColor() {
        if(unit != null && unit.GetComponentInChildren<UnitSpriteHandler>() != null) {
            GetComponent<SpriteRenderer>().DOColor(unit.GetComponentInChildren<UnitSpriteHandler>().getColor() + Color.white / 2.0f, speed);
        }
    }
}

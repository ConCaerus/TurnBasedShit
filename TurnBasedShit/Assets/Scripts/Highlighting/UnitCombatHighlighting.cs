using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombatHighlighting : UnitHighlighting {

    private void Awake() {
        highlightName = "UnitCombatHighlighting";
    }

    private void Update() {
        if(FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().attacking && FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().isPlayerUnit) {
            foreach(var i in FindObjectsOfType<UnitClass>()) {
                if(i.isMouseOverUnit && getHighlightCount() == 0)
                    highlightUnit(i.gameObject);
            }
        }

        foreach(var i in FindObjectsOfType<UnitClass>()) {
            if(!i.isMouseOverUnit)
                dehighlightUnit(i.gameObject);
        }
    }
}

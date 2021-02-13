using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombatHighlighting : UnitHighlighting {

    private void Awake() {
        highlightName = "UnitCombatHighlighting";
    }

    private void Update() {
        if(FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>() == null)
            return;
        var playingUnit = FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>();
        if(playingUnit != null && playingUnit.attacking && FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().isPlayerUnit) {
            foreach(var i in FindObjectsOfType<UnitClass>()) {
                if(i.isMouseOverUnit && !doesUnitHaveHighlight(i.gameObject))
                    highlightUnit(i.gameObject);

                else if(!i.isMouseOverUnit)
                    dehighlightUnit(i.gameObject);
            }
        }
    }
}

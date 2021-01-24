using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDefendingHighlighting : UnitHighlighting {

    private void Awake() {
        highlightName = "DefendingHighlight";
    }

    private void Update() {
        foreach(var i in FindObjectsOfType<UnitClass>()) {
            if(i.defending && !doesUnitHaveHighlight(i.gameObject))
                highlightUnit(i.gameObject);

            else if(!i.defending)
                dehighlightUnit(i.gameObject);
        }
    }
}

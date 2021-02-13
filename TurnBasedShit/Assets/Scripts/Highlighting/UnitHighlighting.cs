using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHighlighting : MonoBehaviour {
    [SerializeField] GameObject highlight;

    public string highlightName;


    public void highlightUnit(GameObject u) {
        if(string.IsNullOrEmpty(highlightName))
            return;
        foreach(var i in u.GetComponentsInChildren<HighlightInstance>())
            i.hardDestroy();
        var h = Instantiate(highlight, u.transform.position, u.transform.rotation, u.transform);
        h.GetComponent<SpriteRenderer>().sortingOrder = u.GetComponent<SpriteRenderer>().sortingOrder - 1;
        h.GetComponent<SpriteRenderer>().sprite = u.GetComponent<SpriteRenderer>().sprite;
        h.transform.localScale = u.transform.localScale + new Vector3(0.25f, 0.25f, 0.25f);
        h.name = highlightName;
        h.GetComponent<HighlightInstance>().originName = highlightName;
    }

    public void dehighlightUnit(GameObject u) {
        if(u == null || u.GetComponentsInChildren<HighlightInstance>().Length == 0)
            return;
        foreach(var i in u.GetComponentsInChildren<HighlightInstance>()) {
            i.destroy(highlightName);
        }
    }

    public bool doesUnitHaveHighlight(GameObject u) {
        foreach(var i in u.GetComponentsInChildren<HighlightInstance>()) {
            return true;
        }
        return false;
    }
}

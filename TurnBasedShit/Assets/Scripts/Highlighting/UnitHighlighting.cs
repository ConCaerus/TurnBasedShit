using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHighlighting : MonoBehaviour {
    [SerializeField] GameObject highlight;

    List<GameObject> highlightInstances = new List<GameObject>();

    public string highlightName;


    public void highlightUnit(GameObject u) {
        var h = Instantiate(highlight, u.transform.position, u.transform.rotation, u.transform);
        h.GetComponent<SpriteRenderer>().sortingOrder = u.GetComponent<SpriteRenderer>().sortingOrder - 1;
        h.GetComponent<SpriteRenderer>().sprite = u.GetComponent<SpriteRenderer>().sprite;
        h.transform.localScale = u.transform.localScale + new Vector3(0.25f, 0.25f, 0.25f);
        h.name = highlightName;
        highlightInstances.Add(h);
    }

    public void dehighlightUnit(GameObject u) {
        GameObject ob = null;
        foreach(var i in highlightInstances) {
            if(i != null && i.transform.parent.gameObject == u) {
                ob = i.gameObject;
                break;
            }

            if(i == null)
                highlightInstances.Remove(i);
        }

        highlightInstances.Remove(ob);
        Destroy(ob);
    }

    public int getHighlightCount() {
        return highlightInstances.Count;
    }

    public bool doesUnitHaveHighlight(GameObject unit) {
        for(int i = 0; i < unit.transform.childCount; i++) {
            foreach(var h in highlightInstances) {
                if(unit.transform.GetChild(i).gameObject == h.gameObject)
                    return true;
            }
        }

        return false;
    }
}

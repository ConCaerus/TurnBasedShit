using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnitCombatHighlighter : MonoBehaviour {
    [SerializeField] GameObject highlight;
    [SerializeField] Color playerColor, playerAttacking, enemyColor;

    List<GameObject> highlightedUnits = new List<GameObject>();
    List<GameObject> highlights = new List<GameObject>();
    float animSpeed = 0.35f;
    float animMax = 1.05f, animMin = 0.95f;


    private void Start() {
        StartCoroutine(animation());
    }

    private void Update() {
        if(FindObjectOfType<BattleResultsCanvas>() != null && highlights.Count > 0) {
            endAllHighlights();
        }

        for(int i = 0; i < highlights.Count; i++) {
            if(highlights[i] == null || highlightedUnits[i] == null || !highlightedUnits[i].activeInHierarchy) {
                highlightedUnits.RemoveAt(i);
                if(highlights[i] != null)
                    highlights[i].GetComponent<CombatHighlightObject>().startEndAnim(true);
                highlights.RemoveAt(i);
                return;
            }
            highlights[i].transform.position = highlightedUnits[i].transform.position;

            if(i > 0 && highlights[i].GetComponent<CombatHighlightObject>().finishedAnim)
                highlights[i].transform.localScale = highlights[0].transform.localScale;
        }
    }



    public void endAllHighlights(bool endsScript = false) {
        foreach(var i in FindObjectsOfType<CombatHighlightObject>()) {
            i.GetComponent<CombatHighlightObject>().startEndAnim(true);
        }

        if(endsScript) {
            this.enabled = false;
            return;
        }

        highlights.Clear();
        highlightedUnits.Clear();
    }


    public void updateHighlights() {
        List<GameObject> removedUnits = new List<GameObject>();
        List<GameObject> removedHighlights = new List<GameObject>();
        removedUnits.Clear();
        removedHighlights.Clear();
        //  checks if needs to remove highlight
        for(int i = 0; i < highlightedUnits.Count; i++) {
            if(highlightedUnits[i] == null) {
                removedUnits.Add(highlightedUnits[i].gameObject);
                removedHighlights.Add(highlights[i]);
                continue;
            }
            //  not active
            if(highlightedUnits[i].gameObject != FindObjectOfType<TurnOrderSorter>().playingUnit.gameObject && !highlightedUnits[i].GetComponent<UnitClass>().isMouseOverUnit) {
                removedUnits.Add(highlightedUnits[i].gameObject);
                removedHighlights.Add(highlights[i]);
                continue;
            }

            //  active but not correct highlight
            else if(highlightedUnits[i].gameObject == FindObjectOfType<TurnOrderSorter>().playingUnit.gameObject && highlightedUnits[i].GetComponent<UnitClass>().isPlayerUnit) {
                if(FindObjectOfType<BattleOptionsCanvas>().attackState && highlights[i].GetComponent<CombatHighlightObject>().getColor() != playerAttacking) {
                    createHighlightObject(highlightedUnits[i].transform.position, playerAttacking, i);
                }
                else if(!FindObjectOfType<BattleOptionsCanvas>().attackState && highlights[i].GetComponent<CombatHighlightObject>().getColor() != playerColor) {
                    createHighlightObject(highlightedUnits[i].transform.position, playerColor, i);
                }
            }
        }

        foreach(var i in removedUnits)
            highlightedUnits.Remove(i.gameObject);
        foreach(var i in removedHighlights) {
            highlights.Remove(i.gameObject);
            i.gameObject.GetComponent<CombatHighlightObject>().startEndAnim(true);
        }

        //  checks if needs to add highlight
        foreach(var i in getNotHighlightedUnits()) {
            if(i.gameObject == FindObjectOfType<TurnOrderSorter>().playingUnit.gameObject || i.GetComponent<UnitClass>().isMouseOverUnit) {
                highlightedUnits.Add(i.gameObject);

                if(i.GetComponent<UnitClass>().isPlayerUnit) {
                    if(FindObjectOfType<BattleOptionsCanvas>().attackState)
                        createHighlightObject(i.gameObject.transform.position, playerAttacking);
                    else
                        createHighlightObject(i.gameObject.transform.position, playerColor);
                }
                else
                    createHighlightObject(i.gameObject.transform.position, enemyColor);
            }
        }
    }


    bool isUnitInList(GameObject unit) {
        foreach(var i in highlightedUnits) {
            if(i.gameObject == unit.gameObject)
                return true;
        }
        return false;
    }

    int getUnitIndexInList(GameObject unit) {
        for(int i = 0; i < highlightedUnits.Count; i++) {
            if(unit == highlightedUnits[i].gameObject)
                return i;
        }
        return -1;
    }

    List<GameObject> getNotHighlightedUnits() {
        List<GameObject> not = new List<GameObject>();
        not.Clear();
        foreach(var i in FindObjectsOfType<UnitClass>()) {
            if(!isUnitInList(i.gameObject))
                not.Add(i.gameObject);
        }

        return not;
    }

    GameObject createHighlightObject(Vector2 pos, Color col, int index = -1) {
        var temp = Instantiate(highlight.gameObject, transform);

        temp.GetComponent<CombatHighlightObject>().setColor(col);
        temp.transform.localPosition = pos;

        if(index > -1) {
            var obj = highlights[index];
            highlights[index] = temp.gameObject;
            Destroy(obj.gameObject);    //  destroy this without using the destory anim
        }
        else
            highlights.Add(temp.gameObject);
        return temp;
    }


    public IEnumerator animation() {
        if(highlights.Count > 0)
            highlights[0].transform.DOScale(animMax, animSpeed);

        yield return new WaitForSeconds(animSpeed);

        if(highlights.Count > 0)
            highlights[0].transform.DOScale(animMin, animSpeed);

        yield return new WaitForSeconds(animSpeed);

        StartCoroutine(animation());
    }
}

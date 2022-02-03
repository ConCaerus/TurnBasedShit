using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnitCombatHighlighter : MonoBehaviour {
    [SerializeField] GameObject highlightPreset;
    [SerializeField] Color playerColor, playerAttacking, playerSpecial, enemyColor;

    List<highlight> highlights = new List<highlight>();
    float animSpeed = 0.35f;
    float animMax = 1.05f, animMin = .95f;

    public enum highlightType {
        playerNorm, playerAttacking, playerSpecial, enemy
    }

    public struct highlight {
        public GameObject obj, unit;
        public highlightType type;

        public highlight(GameObject o, GameObject u, highlightType t) {
            obj = o;
            unit = u;
            type = t;
        }
    }


    private void Start() {
        StartCoroutine(highlightAnimation());
        StartCoroutine(FindObjectOfType<TransitionCanvas>().runAfterLoading(updateHighlights));
    }



    public void endAllHighlights(bool endsScript = false) {
        foreach(var i in highlights) {
            i.obj.GetComponent<CombatHighlightObject>().startEndAnim(true);
        }

        if(endsScript) {
            enabled = false;
            return;
        }
    }


    public void updateHighlights() {
        if(!FindObjectOfType<TransitionCanvas>().loaded)
            return;
        //  checks if needs to remove highlight
        for(int i = 0; i < highlights.Count; i++) {
            //  unit destroyed
            if(highlights[i].unit == null) {
                dehighlightUnit(highlights[i], true);
                continue;
            }

            //  unit not active
            if(highlights[i].unit != FindObjectOfType<TurnOrderSorter>().playingUnit && !highlights[i].unit.GetComponent<UnitClass>().isMouseOverUnit) {
                dehighlightUnit(highlights[i], true);
                continue;
            }
        }


        //  checks if needs to add highlight
        foreach(var i in getNotHighlightedUnits()) {
            if(i.gameObject == FindObjectOfType<TurnOrderSorter>().playingUnit.gameObject || i.GetComponent<UnitClass>().isMouseOverUnit) {
                if(i.GetComponent<UnitClass>().combatStats.isPlayerUnit)
                    highlightUnit(i, highlightType.playerNorm);
                else
                    highlightUnit(i, highlightType.enemy);
            }
        }


        //  update playing unit's highlight color 
        if(FindObjectOfType<TurnOrderSorter>().playingUnit != null) {
            var h = getHighlightIndexForUnit(FindObjectOfType<TurnOrderSorter>().playingUnit);
            int state = FindObjectOfType<BattleOptionsCanvas>().battleState;
            if(state == 1)  //  attacking
                setHighlightType(highlights[h], highlightType.playerAttacking);
            else if(state == 3) //  special
                setHighlightType(highlights[h], highlightType.playerSpecial);
            else
                setHighlightType(highlights[h], highlightType.playerNorm);
        }
    }

    public bool isUnitInList(GameObject unit) {
        foreach(var i in highlights) {
            if(i.unit == unit)
                return true;
        }
        return false;
    }

    List<GameObject> getNotHighlightedUnits() {
        List<GameObject> not = new List<GameObject>();
        foreach(var i in FindObjectsOfType<UnitClass>()) {
            if(!isUnitInList(i.gameObject))
                not.Add(i.gameObject);
        }

        return not;
    }

    GameObject createHighlightObject(GameObject unit, Color col) {
        var temp = Instantiate(highlightPreset.gameObject, unit.transform);

        temp.GetComponent<CombatHighlightObject>().setColor(col);
        if(unit.GetComponentInChildren<UnitSpriteHandler>() != null)
            temp.transform.localPosition = unit.GetComponentInChildren<UnitSpriteHandler>().getCombatHighlightOffset();
        else
            temp.transform.localPosition = unit.GetComponent<CombatUnitUI>().highlightOffset;
        temp.transform.localScale = Vector3.one;
        return temp;
    }

    public void highlightUnit(GameObject unit, highlightType type) {
        if(!FindObjectOfType<TransitionCanvas>().loaded)
            return;
        //  creates highlight
        var obj = createHighlightObject(unit, getRelevantColor(type));

        //  speeds up the animation speed of the unit
        if(unit != null && unit.GetComponentInChildren<UnitSpriteHandler>() != null && unit.GetComponentInChildren<UnitSpriteHandler>().isAnimIdle())
            unit.GetComponentInChildren<UnitSpriteHandler>().setAnimSpeed(2.5f);
        if(unit != null && unit.GetComponent<Animator>() != null && unit.GetComponent<EnemyUnitInstance>() != null && unit.GetComponent<EnemyUnitInstance>().isIdle())
            unit.GetComponent<Animator>().speed = 2.5f;

        //  adds the highlight to the list
        highlights.Add(new highlight(obj, unit, type));
    }

    public void setHighlightType(GameObject unit, highlightType type) {
        var index = getHighlightIndexForUnit(unit);
        setHighlightType(highlights[index], type);
    }
    public void setHighlightType(highlight h, highlightType type) {
        var index = highlights.IndexOf(h);
        h.type = type;
        h.obj.GetComponent<CombatHighlightObject>().setColor(getRelevantColor(type));

        highlights[index] = h;
    }

    public void dehighlightUnit(GameObject unit, bool showEndAnim) {
        //  finds highlight with unit
        int index = getHighlightIndexForUnit(unit);
        dehighlightUnit(highlights[index].obj, showEndAnim);
    }
    public void dehighlightUnit(highlight h, bool showEndAnim) {
        var obj = h.obj;
        var unit = h.unit;
        var index = highlights.IndexOf(h);

        //  resets the unit's animation speed
        if(FindObjectOfType<TransitionCanvas>().loaded) {
            if(unit.GetComponentInChildren<UnitSpriteHandler>() != null)
                unit.GetComponentInChildren<UnitSpriteHandler>().setAnimSpeed(1.0f);
            if(unit.GetComponent<Animator>() != null)
                unit.GetComponent<Animator>().speed = 1.0f;
        }

        //  ends the life of the highlight
        if(showEndAnim) {
            obj.GetComponent<CombatHighlightObject>().startEndAnim(true);
        }
        else {
            Destroy(obj.gameObject);
        }

        //  removes highlight
        highlights.RemoveAt(index);
    }


    public int getHighlightIndexForUnit(GameObject unit) {
        for(int i = 0; i < highlights.Count; i++) {
            if(highlights[i].unit == unit)
                return i;
        }
        return -1;
    }


    Color getRelevantColor(highlightType type) {
        switch(type) {
            case highlightType.playerNorm:
                return playerColor;
            case highlightType.playerAttacking:
                return playerAttacking;
            case highlightType.playerSpecial:
                return playerSpecial;
            case highlightType.enemy:
                return enemyColor;
        }
        return Color.white;
    }

    IEnumerator highlightAnimation() {
        foreach(var i in highlights) {
            i.obj.transform.DOScale(animMax, animSpeed);
        }

        yield return new WaitForSeconds(animSpeed);

        foreach(var i in highlights) {
            i.obj.transform.DOScale(animMin, animSpeed);
        }

        yield return new WaitForSeconds(animSpeed);

        StartCoroutine(highlightAnimation());
    }
}

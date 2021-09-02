using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattleOptionsCanvas : MonoBehaviour {
    public bool attackState = false;
    bool showing = false;
    float showTime = 0.15f;


    private void Start() {
        transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, -100.0f);
    }


    public void runCombatOptions() {
        if(FindObjectOfType<TurnOrderSorter>().playingUnit != null) {
            var playingUnit = FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>();
    
            //  player's turn
            if(playingUnit.isPlayerUnit) {
                //  skip turn if stunned
                if(playingUnit.stunned)
                    StartCoroutine(skipUnitTurn());
            }

            //  enemy's turn
            else if(!playingUnit.isPlayerUnit) {
                //  skip turn if stunned
                if(playingUnit.stunned)
                    StartCoroutine(skipUnitTurn());

                //  else decide what the enemy is going to do
                else {
                    StartCoroutine(playingUnit.GetComponent<EnemyUnitInstance>().combatTurn());
                }
            }
        }
    }

    private void Update() {
        if(FindObjectOfType<TurnOrderSorter>().playingUnit != null && FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().isPlayerUnit && !showing)
            showUI();
        else if((FindObjectOfType<TurnOrderSorter>().playingUnit == null || !FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().isPlayerUnit) && showing)
            hideUI();
    }


    void showUI() {
        transform.GetChild(0).gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0.0f, 0.0f), showTime);
        showing = true;
    }
    void hideUI() {
        transform.GetChild(0).gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0.0f, -100.0f), showTime);
        showing = false;
    }


    IEnumerator skipUnitTurn() {
        yield return new WaitForSeconds(1.0f);

        FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
    }


    //  Buttons 

    public void attack() {
        attackState = !attackState;
        FindObjectOfType<UnitCombatHighlighter>().updateHighlights();
    }

    public void defend() {
        FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().defending = true;
        FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
    }

    public void charge() {
        FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
    }

    public void special() {
        FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
    }
}

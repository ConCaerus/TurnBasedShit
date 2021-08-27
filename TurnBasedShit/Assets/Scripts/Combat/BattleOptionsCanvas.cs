using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleOptionsCanvas : MonoBehaviour {
    [SerializeField] GameObject optionsObject;

    public bool attackState = false;


    public void runCombatOptions() {
        if(FindObjectOfType<TurnOrderSorter>().playingUnit != null) {
            var playingUnit = FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>();
    
            //  player's turn
            if(playingUnit.isPlayerUnit) {
                //  skip turn if stunned
                if(playingUnit.stunned)
                    StartCoroutine(skipUnitTurn());

                //  else show options
                else {
                    optionsObject.SetActive(true);
                }
            }

            //  enemy's turn
            else if(!playingUnit.isPlayerUnit) {
                //  hides the options for the enemy
                optionsObject.SetActive(false);

                //  skip turn if stunned
                if(playingUnit.stunned)
                    StartCoroutine(skipUnitTurn());

                //  else decide what the enemy is going to do
                else {
                    StartCoroutine(playingUnit.GetComponent<EnemyUnitInstance>().combatTurn());
                }
            }
        }
        else
            optionsObject.SetActive(false);
    }

    private void Update() {
        if(FindObjectOfType<TurnOrderSorter>().playingUnit != null && FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().isPlayerUnit) {
            Vector2 target = FindObjectOfType<TurnOrderSorter>().playingUnit.transform.position;
            Vector2 offset = new Vector2(0.0f, FindObjectOfType<TurnOrderSorter>().playingUnit.transform.localScale.y);

            optionsObject.transform.position = target + offset;
        }
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
        FindObjectOfType<UnitCombatHighlighter>().updateHighlights();
    }
}

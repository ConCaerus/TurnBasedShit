using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleOptionsCanvas : MonoBehaviour {
    [SerializeField] GameObject optionsObject;


    public void runCombatLogic() {
        if(FindObjectOfType<TurnOrderSorter>().playingUnit != null) {
            var playingUnit = FindObjectOfType<TurnOrderSorter>().playingUnit;
            if(playingUnit.GetComponent<UnitClass>().isPlayerUnit && !playingUnit.GetComponent<UnitClass>().stunned) {
                optionsObject.SetActive(true);
                moveToUnit();
            }
            else if(playingUnit.GetComponent<UnitClass>().isPlayerUnit && playingUnit.GetComponent<UnitClass>().stunned) {
                StartCoroutine(skipUnitTurn());
            }

            else if(!playingUnit.GetComponent<UnitClass>().isPlayerUnit && !playingUnit.GetComponent<UnitClass>().stunned) {
                FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().attacking = true;
                optionsObject.SetActive(false);
            }
            else if(!playingUnit.GetComponent<UnitClass>().isPlayerUnit && playingUnit.GetComponent<UnitClass>().stunned) {
                StartCoroutine(skipUnitTurn());
            }
            else
                optionsObject.SetActive(false);
        }
        else
            optionsObject.SetActive(false);
    }


    void moveToUnit() {
        Vector2 target = FindObjectOfType<TurnOrderSorter>().playingUnit.transform.position;
        Vector2 offset = new Vector2(0.0f, FindObjectOfType<TurnOrderSorter>().playingUnit.transform.localScale.y);

        optionsObject.transform.position = target + offset;
    }


    IEnumerator skipUnitTurn() {
        yield return new WaitForSeconds(1.0f);

        FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
    }


    //  Buttons 

    public void attack() {
        FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().attacking = true;
    }

    public void defend() {
        FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().defending = true;
        FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().attacking = false;
    }

    public void item() {
    }
}

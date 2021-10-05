using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BattleOptionsCanvas : MonoBehaviour {
    public int battleState = 0;
    bool showing = false;
    float showTime = 0.15f;

    [SerializeField] Material selectedMat;


    private void Start() {
        transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, -100.0f);
    }


    public void runCombatOptions() {
        battleState = 0;
        if(FindObjectOfType<TurnOrderSorter>().playingUnit != null) {
            var playingUnit = FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>();
    
            //  player's turn
            if(playingUnit.isPlayerUnit) {
                //  skip turn if stunned
                if(playingUnit.isStunned())
                    StartCoroutine(skipUnitTurn(playingUnit));

                //  battle logic for summon
                else if(playingUnit.GetComponent<SummonedUnitInstance>() != null)
                    StartCoroutine(playingUnit.GetComponent<SummonedUnitInstance>().combatTurn());
            }

            //  enemy's turn
            else if(!playingUnit.isPlayerUnit) {
                //  skip turn if stunned
                if(playingUnit.isStunned())
                    StartCoroutine(skipUnitTurn(playingUnit));

                //  else decide what the enemy is going to do
                else {
                    StartCoroutine(playingUnit.GetComponent<EnemyUnitInstance>().combatTurn());
                }
            }
        }
    }

    private void Update() {
        var playingUnit = FindObjectOfType<TurnOrderSorter>().playingUnit;
        if(playingUnit != null && playingUnit.GetComponent<UnitClass>().isPlayerUnit && playingUnit.GetComponent<SummonedUnitInstance>() == null && !playingUnit.GetComponent<UnitClass>().isStunned() && !showing)
            showUI();
        else if((playingUnit == null || !playingUnit.GetComponent<UnitClass>().isPlayerUnit || playingUnit.GetComponent<UnitClass>().isStunned() || playingUnit.GetComponent<SummonedUnitInstance>() != null) && showing)
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


    IEnumerator skipUnitTurn(UnitClass unit) {
        yield return new WaitForSeconds(1.0f);

        unit.setStunned(false);
        FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
    }


    //  Buttons 

    public void attack() {
        if(battleState != 1)
            battleState = 1;
        else
            battleState = 0;
        FindObjectOfType<UnitCombatHighlighter>().updateHighlights();
    }

    public void defend() {
        battleState = 0;
        FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().setDefending(true);
        FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
    }

    public void charge() {
        battleState = 2;
        FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
    }

    public void special() {
        if(FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().stats.equippedWeapon == null || FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().stats.equippedWeapon.isEmpty())
            return;
        if(battleState != 3)
            battleState = 3;
        else
            battleState = 0;
        FindObjectOfType<UnitCombatHighlighter>().updateHighlights();
    }


    public void hoverOverButton(GameObject thing) {
        thing.GetComponent<Image>().material = selectedMat;
    }
    public void dehoverOverButton(GameObject thing) {
        thing.GetComponent<Image>().material = null;
    }
}

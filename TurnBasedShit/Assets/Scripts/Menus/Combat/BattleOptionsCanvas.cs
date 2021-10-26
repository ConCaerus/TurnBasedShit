using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class BattleOptionsCanvas : MonoBehaviour {
    [SerializeField] GameObject chooseTargetText;

    public int battleState = 0;
    bool showing = false, showingChooseTargetText = false;
    float showTime = 0.15f;
    Vector2 chooseTextOffset = new Vector2(0.0f, -10.0f);


    private void Start() {
        transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, -100.0f);
        hideChooseTargetText();
    }


    public void runCombatOptions() {
        battleState = 0;
        if(FindObjectOfType<TurnOrderSorter>().playingUnit != null) {
            var playingUnit = FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>();

            //  player's turn
            if(playingUnit.isPlayerUnit) {
                transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = playingUnit.stats.u_name;
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
        if(playingUnit != null && playingUnit.GetComponent<UnitClass>().isPlayerUnit && playingUnit.GetComponent<SummonedUnitInstance>() == null && !playingUnit.GetComponent<UnitClass>().isStunned() && !showing) {
            showUI();
        }
        else if((playingUnit == null || !playingUnit.GetComponent<UnitClass>().isPlayerUnit || playingUnit.GetComponent<UnitClass>().isStunned() || playingUnit.GetComponent<SummonedUnitInstance>() != null) && showing)
            hideUI();

        if(showing && !showingChooseTargetText && battleState == 1)
            showChooseTargetText();
        else if(showing && showingChooseTargetText && battleState != 1)
            hideChooseTargetText();
    }


    void showUI() {
        transform.GetChild(0).gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0.0f, 0.0f), showTime);
        transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().stats.u_name;
        showing = true;
    }
    void showChooseTargetText() {
        showingChooseTargetText = true;
        chooseTargetText.GetComponent<RectTransform>().DOComplete();
        chooseTargetText.GetComponent<RectTransform>().anchoredPosition = new Vector2(chooseTextOffset.x, 100.0f);
        chooseTargetText.GetComponent<RectTransform>().localScale = new Vector3(0.0f, 0.0f);
        chooseTargetText.GetComponent<TextMeshProUGUI>().color = Color.clear;

        chooseTargetText.GetComponent<RectTransform>().DOAnchorPos(chooseTextOffset, showTime);
        chooseTargetText.GetComponent<TextMeshProUGUI>().DOColor(Color.white, showTime);
        chooseTargetText.GetComponent<RectTransform>().DOScale(1.0f, showTime);
    }
    void hideUI() {
        hideChooseTargetText();
        transform.GetChild(0).gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0.0f, -100.0f), showTime);
        showing = false;
    }
    void hideChooseTargetText() {
        showingChooseTargetText = false;
        chooseTargetText.GetComponent<RectTransform>().DOComplete();

        chooseTargetText.GetComponent<RectTransform>().DOAnchorPos(new Vector2(chooseTextOffset.x, 100.0f), showTime * 1.5f);
        chooseTargetText.GetComponent<TextMeshProUGUI>().DOColor(Color.clear, showTime * 1.5f);
        chooseTargetText.GetComponent<RectTransform>().DOScale(0.0f, showTime * 1.5f);
    }


    IEnumerator skipUnitTurn(UnitClass unit) {
        yield return new WaitForSeconds(1.0f);

        unit.charging = false;
        unit.setStunned(false);
        FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
    }



    public void showAttackSpinWheel() {
        FindObjectOfType<SpinWheelShower>().showWheel(transform.GetChild(0).GetChild(1).GetChild(0).gameObject, transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Image>().color);
    }


    //  mousedOver buttons
    void universalMousedOverActions(GameObject thing) {
        thing.GetComponent<InfoBearer>().show();
        thing.transform.parent.GetComponentInChildren<SpinWheelObject>().startShowing();
        thing.GetComponent<Animator>().StopPlayback();
        thing.GetComponent<Animator>().SetFloat("moddedSpeed", 1.0f);

        thing.GetComponent<RectTransform>().DOKill();
        thing.GetComponent<RectTransform>().DORotate(new Vector3(0.0f, 0.0f, Random.Range(-25.0f, 25.0f)), 0.15f);
    }
    void universalMouseExitedActions(GameObject thing) {
        thing.GetComponent<InfoBearer>().hide();
        thing.transform.parent.GetComponentInChildren<SpinWheelObject>().startHiding();
        thing.GetComponent<Animator>().StopPlayback();
        thing.GetComponent<Animator>().SetFloat("moddedSpeed", -1.0f);

        thing.GetComponent<RectTransform>().DOKill();
        thing.GetComponent<RectTransform>().DORotate(new Vector3(0.0f, 0.0f, 0.0f), 0.25f);
    }
    void playAnimBasedOnButtonFunction(GameObject thing) {
        switch(thing.GetComponent<Button>().onClick.GetPersistentMethodName(0)) {
            case "attack":
                thing.GetComponent<Animator>().Play("AttackButtonActivateAnim");
                break;
            case "defend":
                thing.GetComponent<Animator>().Play("DefendButtonActivateAnim");
                break;
            case "charge":
                thing.GetComponent<Animator>().Play("ChargeButtonActivateAnim");
                break;
            case "special":
                thing.GetComponent<Animator>().Play("SpecialButtonActivateAnim");
                break;
        }
    }

    public void mouseEnteredButton(GameObject thing) {
        universalMousedOverActions(thing);
        playAnimBasedOnButtonFunction(thing);
    }


    public void mouseExitedButton(GameObject thing) {
        universalMouseExitedActions(thing);
        playAnimBasedOnButtonFunction(thing);
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
        if(FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().charging)
            return;
        battleState = 2;
        FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().charging = true;
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
}

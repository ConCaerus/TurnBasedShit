﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;

public class BattleOptionsCanvas : MonoBehaviour {
    [SerializeField] GameObject chooseTargetText;
    [SerializeField] GameObject attackButton, defendButton, chargeButton, specialButton;
    [SerializeField] GameObject speedButton, pauseButton;

    public int battleState = 0;
    bool showing = false, showingChooseTargetText = false;
    float showTime = 0.15f;
    Vector2 chooseTextOffset = new Vector2(0.0f, -10.0f);


    private void Start() {
        Time.timeScale = 1.0f;
        transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, -55.0f);
        hideChooseTargetText();
        FindObjectOfType<MenuCanvas>().addNewRunOnOpen(stopAllButtonAnims);
        FindObjectOfType<MenuCanvas>().addNewRunOnClose(updateButtonInteractability);
    }


    public void runCombatOptions() {
        battleState = 0;
        var playingUnit = FindObjectOfType<TurnOrderSorter>().playingUnit;
        if(FindObjectOfType<TurnOrderSorter>().playingUnit != null) {
            //  player's turn
            if(playingUnit.GetComponent<UnitClass>().combatStats.isPlayerUnit) {
                transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = playingUnit.GetComponent<UnitClass>().stats.u_name;
                //  skip turn if stunned
                if(playingUnit.GetComponent<UnitClass>().isStunned())
                    StartCoroutine(skipUnitTurn(playingUnit.GetComponent<UnitClass>()));

                //  battle logic for summon
                else if(playingUnit.GetComponent<SummonedUnitInstance>() != null)
                    StartCoroutine(playingUnit.GetComponent<SummonedUnitInstance>().combatTurn());
            }

            //  enemy's turn
            else if(!playingUnit.GetComponent<UnitClass>().combatStats.isPlayerUnit) {
                //  skip turn if stunned
                if(playingUnit.GetComponent<UnitClass>().isStunned())
                    StartCoroutine(skipUnitTurn(playingUnit.GetComponent<UnitClass>()));

                //  else decide what the enemy is going to do
                else {
                    StartCoroutine(playingUnit.GetComponent<EnemyUnitInstance>().combatTurn());
                }
            }
        }
        if(playingUnit != null && playingUnit.GetComponent<UnitClass>().combatStats.isPlayerUnit && playingUnit.GetComponent<SummonedUnitInstance>() == null && !playingUnit.GetComponent<UnitClass>().isStunned() && !showing) {
            showUI();
        }
        else if((playingUnit == null || !playingUnit.GetComponent<UnitClass>().combatStats.isPlayerUnit || playingUnit.GetComponent<UnitClass>().isStunned() || playingUnit.GetComponent<SummonedUnitInstance>() != null) && showing)
            hideUI();
    }


    public void updateButtonInteractability() {
        var unit = FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>();

        if(unit.charging) {
            chargeButton.GetComponent<Button>().interactable = false;
            chargeButton.GetComponentInChildren<Light2D>().enabled = false;
            chargeButton.GetComponent<InfoBearer>().setInfo("Already charging");
        }
        else {
            chargeButton.GetComponent<Button>().interactable = true;
            chargeButton.GetComponentInChildren<Light2D>().enabled = true;
            chargeButton.GetComponent<InfoBearer>().setInfo("Charge");
        }

        if(unit.stats.weapon != null && !unit.stats.weapon.isEmpty() && unit.stats.weapon.sUsage != (Weapon.specialUsage)(-1)) {
            specialButton.GetComponent<Button>().interactable = true;
            specialButton.GetComponentInChildren<Light2D>().enabled = true;

            if(unit.stats.weapon.sUsage == Weapon.specialUsage.healing)
                specialButton.GetComponent<InfoBearer>().setInfo("Heal");
            else if(unit.stats.weapon.sUsage == Weapon.specialUsage.summoning)
                specialButton.GetComponent<InfoBearer>().setInfo("Summon");
            else if(unit.stats.weapon.sUsage == Weapon.specialUsage.convertTarget)
                specialButton.GetComponent<InfoBearer>().setInfo("Convert");
        }
        else {
            specialButton.GetComponent<Button>().interactable = false;
            specialButton.GetComponentInChildren<Light2D>().enabled = false;
            specialButton.GetComponent<InfoBearer>().setInfo("Special");
        }
    }

    void showUI() {
        var unit = FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>();
        transform.GetChild(0).gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0.0f, 0.0f), showTime);
        transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = unit.stats.u_name;

        updateButtonInteractability();

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
        transform.GetChild(0).gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0.0f, -55.0f), showTime);
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
        unit.setUnstunned();
        FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
    }



    public void showAttackSpinWheel() {
        FindObjectOfType<SpinWheelShower>().showWheel(transform.GetChild(0).GetChild(1).GetChild(0).gameObject, transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Image>().color);
    }


    //  mousedOver buttons
    void universalMousedOverActions(GameObject thing) {
        thing.transform.parent.GetComponentInChildren<SpinWheelObject>().startShowing();
        thing.GetComponent<Animator>().StopPlayback();
        thing.GetComponent<Animator>().SetFloat("moddedSpeed", 1.0f);

        var pu = FindObjectOfType<TurnOrderSorter>().playingUnit;
        var du = FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().combatStats.attackingTarget;

        if(thing == attackButton && pu.GetComponentInChildren<CombatCards>().state != CombatCards.cardState.attacking) {
            pu.GetComponentInChildren<CombatCards>().showCards(CombatCards.cardState.attacking,
                new List<StatModifier.useTimeType>() { StatModifier.useTimeType.beforeEveryTurn, StatModifier.useTimeType.beforeAttacking, StatModifier.useTimeType.beforeTurn,
                StatModifier.useTimeType.afterKill, StatModifier.useTimeType.afterTurn, StatModifier.useTimeType.afterEveryTurn});
            if(du != null) {
                du.GetComponentInChildren<CombatCards>().showCards(CombatCards.cardState.defending,
                    new List<StatModifier.useTimeType>() { StatModifier.useTimeType.beforeDefending, StatModifier.useTimeType.beforeDying });
            }
        }
        else if(thing == defendButton && pu.GetComponentInChildren<CombatCards>().state != CombatCards.cardState.defending) {
            pu.GetComponentInChildren<CombatCards>().showCards(CombatCards.cardState.defending,
                new List<StatModifier.useTimeType>() { StatModifier.useTimeType.beforeEveryTurn, StatModifier.useTimeType.beforeDefending, StatModifier.useTimeType.beforeTurn,
                StatModifier.useTimeType.afterEveryTurn, StatModifier.useTimeType.afterTurn});
        }
        else if(thing == chargeButton && pu.GetComponentInChildren<CombatCards>().state != CombatCards.cardState.charging) {
            pu.GetComponentInChildren<CombatCards>().showCards(CombatCards.cardState.charging,
                new List<StatModifier.useTimeType>() { StatModifier.useTimeType.beforeEveryTurn, StatModifier.useTimeType.beforeAttacking, StatModifier.useTimeType.beforeTurn,
                StatModifier.useTimeType.afterKill, StatModifier.useTimeType.afterTurn, StatModifier.useTimeType.afterEveryTurn});
            if(du != null) {
                du.GetComponentInChildren<CombatCards>().showCards(CombatCards.cardState.defending,
                    new List<StatModifier.useTimeType>() { StatModifier.useTimeType.beforeDefending, StatModifier.useTimeType.beforeDying });
            }
        }
        else if(thing == specialButton && pu.GetComponentInChildren<CombatCards>().state != CombatCards.cardState.special) {
            pu.GetComponentInChildren<CombatCards>().showCards(CombatCards.cardState.special,
                new List<StatModifier.useTimeType>() { StatModifier.useTimeType.beforeEveryTurn, StatModifier.useTimeType.beforeTurn,
                StatModifier.useTimeType.afterEveryTurn, StatModifier.useTimeType.afterTurn});
        }

        thing.GetComponent<RectTransform>().DOKill();
        thing.GetComponent<RectTransform>().DORotate(new Vector3(0.0f, 0.0f, Random.Range(-25.0f, 25.0f)), 0.15f);
    }
    void universalMouseExitedActions(GameObject thing) {
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
        if(FindObjectOfType<MenuCanvas>().isOpen())
            return;
        if(!thing.GetComponent<Button>().interactable)
            return;
        universalMousedOverActions(thing);
        playAnimBasedOnButtonFunction(thing);
    }


    public void mouseExitedButton(GameObject thing) {
        if(FindObjectOfType<MenuCanvas>().isOpen())
            return;
        universalMouseExitedActions(thing);
        playAnimBasedOnButtonFunction(thing);
    }
    public void stopAllButtonAnims() {
        mouseExitedButton(attackButton);
        mouseExitedButton(defendButton);
        mouseExitedButton(chargeButton);
        mouseExitedButton(specialButton);
    }

    //  Buttons 

    public void attack() {
        if(FindObjectOfType<MenuCanvas>().isOpen())
            return;
        if(battleState != 1)
            battleState = 1;
        else
            battleState = 0;
        FindObjectOfType<UnitCombatHighlighter>().updateHighlights();

        if(!showingChooseTargetText && battleState == 1)
            showChooseTargetText();
        else if(showingChooseTargetText && battleState != 1)
            hideChooseTargetText();
    }

    public void defend() {
        if(FindObjectOfType<MenuCanvas>().isOpen())
            return;
        battleState = 0;
        FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().setDefending(true);
        FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
    }

    public void charge() {
        if(FindObjectOfType<MenuCanvas>().isOpen())
            return;
        if(FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().charging)
            return;
        battleState = 2;
        FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().charging = true;
        FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
    }

    public void special() {
        if(FindObjectOfType<MenuCanvas>().isOpen())
            return;
        if(FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().stats.weapon == null || FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>().stats.weapon.isEmpty())
            return;
        if(battleState != 3)
            battleState = 3;
        else
            battleState = 0;
        FindObjectOfType<UnitCombatHighlighter>().updateHighlights();
    }

    public void nextSpeedState() {
        switch(speedButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text) {
            case ">":
                Time.timeScale = 2.0f;
                speedButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ">>";
                pauseButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "||";
                break;

            case ">>":
                Time.timeScale = 4.0f;
                speedButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ">>>";
                pauseButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "||";
                break;

            case ">>>":
                Time.timeScale = 1.0f;
                speedButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ">";
                pauseButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "||";
                break;

        }
    }
    public void pause() {
        if(Time.timeScale != 0) {
            Time.timeScale = 0;
            pauseButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ">";
        }
        else {
            //  cycle through all of the speed states utill time is set to what it was before the pause
            nextSpeedState();
            nextSpeedState();
            nextSpeedState();
            pauseButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "||";
        }
    }
}

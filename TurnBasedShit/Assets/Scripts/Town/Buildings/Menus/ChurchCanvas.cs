using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ChurchCanvas : BuildingCanvas {
    [SerializeField] GameObject removeTraits, addTraits;
    [SerializeField] TextMeshProUGUI removeText, addText;
    [SerializeField] CoinCount coinCount;
    [SerializeField] Button confirmButton;

    [SerializeField] Color badColor, goodColor, selectedColor;

    int traitInd = -1;
    bool adding = false;

    ChurchBuilding church;

    private void Awake() {
        if(GameInfo.getCurrentLocationAsTown() != null && GameInfo.getCurrentLocationAsTown().town.hasBuilding(Building.type.Church))
            church = GameInfo.getCurrentLocationAsTown().town.holder.getObject<ChurchBuilding>(0);
        else
            church = Randomizer.randomizeBuilding(new ChurchBuilding(), FindObjectOfType<PresetLibrary>());
    }

    private void Start() {
        hideCanvas();
        coinCount.updateCount(false);
        updateCustomInfo();
    }


    public override void updateCustomInfo() {
        confirmButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = adding ? "Add" : "Remove";

        //  trait texts
        foreach(var i in removeTraits.GetComponentsInChildren<Button>()) {
            i.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            i.GetComponent<Image>().color = Color.white;

            i.interactable = false;
        }
        if(getSelectedUnit() != null) {
            for(int i = 0; i < getSelectedUnit().u_traits.Count; i++) {
                removeTraits.transform.GetChild(i).GetComponent<Button>().interactable = true;
                removeTraits.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = getSelectedUnit().u_traits[i].t_name;
                if(getSelectedUnit().u_traits[i].t_isGood)
                    removeTraits.transform.GetChild(i).transform.GetComponentInChildren<TextMeshProUGUI>().color = goodColor;
                else
                    removeTraits.transform.GetChild(i).transform.GetComponentInChildren<TextMeshProUGUI>().color = badColor;

                if(i == traitInd && !adding)
                    removeTraits.transform.GetChild(i).GetComponent<Image>().color = selectedColor;
            }
        }
        //  trait texts
        foreach(var i in addTraits.GetComponentsInChildren<Button>()) {
            i.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            i.GetComponent<Image>().color = Color.white;

            i.interactable = false;
        }
        for(int i = 0; i < church.availableTraits.Count; i++) {
            addTraits.transform.GetChild(i).GetComponent<Button>().interactable = true;
            addTraits.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = church.availableTraits[i].t_name;
            addTraits.transform.GetChild(i).transform.GetComponentInChildren<TextMeshProUGUI>().color = goodColor;

            if(i == traitInd && adding)
                addTraits.transform.GetChild(i).GetComponent<Image>().color = selectedColor;
        }

        removeText.text = "Remove Cost: " + church.priceToRemove.ToString();
        addText.text = "Add Cost: " + church.priceToAdd.ToString();
    }

    //  removes a trait
    public void confirm() {
        if(getSelectedUnit() == null || getSelectedUnit().isEmpty() || traitInd == -1)
            return;
        if(!adding) {
            if(getSelectedUnit().u_traits.Count <= traitInd)
                return;
            if(Inventory.getCoinCount() >= church.priceToRemove) {
                Inventory.addCoins(-church.priceToRemove, coinCount, true);
                var stats = getSelectedUnit();
                stats.u_traits.RemoveAt(traitInd);
                Party.overrideUnitOfSameInstance(stats);
            }
        }
        else {
            if(church.availableTraits.Count <= traitInd || getSelectedUnit().u_traits.Count >= getSelectedUnit().maxTraitCount)
                return;
            if(Inventory.getCoinCount() >= church.priceToAdd) {
                Inventory.addCoins(-church.priceToAdd, coinCount, true);
                var stats = getSelectedUnit();
                stats.u_traits.Add(church.availableTraits[traitInd]);
                Party.overrideUnitOfSameInstance(stats);
                church.availableTraits.RemoveAt(traitInd);
            }
        }

        updateInfo();
    }

    public void selectRemoveTrait(int i) {
        traitInd = i;
        adding = false;
        updateInfo();
    }

    public void selectAddTrait(int i) {
        traitInd = i;
        adding = true;
        updateInfo();
    }
}

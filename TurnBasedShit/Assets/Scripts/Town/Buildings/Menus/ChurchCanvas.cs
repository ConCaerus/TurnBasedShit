using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ChurchCanvas : BuildingCanvas {
    [SerializeField] GameObject traits;
    [SerializeField] TextMeshProUGUI removeText, addText;

    [SerializeField] Color badColor, goodColor, selectedColor;

    int traitInd = -1;

    ChurchBuilding church;

    private void Awake() {
        if(GameInfo.getCurrentLocationAsTown().town.hasBuilding(Building.type.Church))
            church = GameInfo.getCurrentLocationAsTown().town.holder.getObject<ChurchBuilding>(0);
        else
            church = Randomizer.randomizeBuilding(new ChurchBuilding());
    }


    public override void updateCustomInfo() {
        //  trait texts
        foreach(var i in traits.GetComponentsInChildren<Button>()) {
            i.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            i.GetComponent<Image>().color = Color.white;

            i.interactable = false;
        }
        if(getSelectedUnit() != null) {
            for(int i = 0; i < getSelectedUnit().u_traits.Count; i++) {
                traits.transform.GetChild(i).GetComponent<Button>().interactable = true;
                traits.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = getSelectedUnit().u_traits[i].t_name;
                if(getSelectedUnit().u_traits[i].t_isGood)
                    traits.transform.GetChild(i).transform.GetComponentInChildren<TextMeshProUGUI>().color = goodColor;
                else
                    traits.transform.GetChild(i).transform.GetComponentInChildren<TextMeshProUGUI>().color = badColor;

                if(i == traitInd)
                    traits.transform.GetChild(i).GetComponent<Image>().color = selectedColor;
            }
        }

        removeText.text = "Remove Cost: " + church.priceToRemove.ToString();
        addText.text = "Add Cost: " + church.priceToAdd.ToString();
    }

    //  removes a trait
    public void removeTrait(int index) {
        if(getSelectedUnit() == null || getSelectedUnit().isEmpty() || traitInd == -1 || getSelectedUnit().u_traits.Count <= traitInd)
            return;
        if(Inventory.getCoinCount() >= church.priceToRemove) {
            Inventory.addCoins(-church.priceToRemove);
            var stats = getSelectedUnit();
            stats.u_traits.RemoveAt(traitInd);
            Party.overrideUnit(stats);

            updateInfo();
        }
    }

    public void selectTrait(int i) {
        traitInd = i;
        updateInfo();
    }
}

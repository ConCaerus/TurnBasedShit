using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HospitalCanvas : BuildingCanvas {
    [SerializeField] TextMeshProUGUI freeText, costText, coinCount;
    float healAmt = 35.0f;

    HospitalBuilding hospital;

    private void Awake() {
        if(GameInfo.getCurrentLocationAsTown().town.hasBuilding(Building.type.Hospital))
            hospital = GameInfo.getCurrentLocationAsTown().town.holder.getObject<HospitalBuilding>(0);
        else
            hospital = Randomizer.randomizeBuilding(new HospitalBuilding());
    }


    public override void updateCustomInfo() {
        if(hospital.freeHeals == 0) {
            costText.text = "Heal Cost: " + hospital.pricePerHeal.ToString();
            freeText.text = "";
        }
        else {
            costText.text = "Heal Cost: Free";
            freeText.text = hospital.freeHeals.ToString();
        }
        coinCount.text = Inventory.getCoinCount().ToString();
    }

    public void heal() {
        if(getSelectedUnit() != null && !getSelectedUnit().isEmpty()) {
            var stats = getSelectedUnit();
            stats.addHealth(healAmt);
            Party.overrideUnit(stats);

            if(hospital.freeHeals > 0) {
                hospital.freeHeals--;
                GameInfo.getCurrentLocationAsTown().town.holder.overrideObject<HospitalBuilding>(0, hospital);
            }
            else if(Inventory.getCoinCount() >= hospital.pricePerHeal) {
                Inventory.addCoins(-hospital.pricePerHeal);
            }
            updateInfo();
        }
    }
}

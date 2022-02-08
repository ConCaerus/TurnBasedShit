using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HospitalCanvas : BuildingCanvas {
    [SerializeField] TextMeshProUGUI freeText, costText;
    [SerializeField] CoinCount coinCount;
    float healAmt = 35.0f;

    HospitalBuilding hospital;

    private void Start() {
        if(GameInfo.getCurrentLocationAsTown().town.hasBuilding(Building.type.Hospital))
            hospital = GameInfo.getCurrentLocationAsTown().town.holder.getObject<HospitalBuilding>(0);
        else
            hospital = Randomizer.randomizeBuilding(new HospitalBuilding());
        hideCanvas();
        coinCount.updateCount(false);
    }


    public override void updateCustomInfo() {
        if(hospital.freeHeals == 0) {
            costText.text = "Heal Cost: " + hospital.pricePerHeal.ToString();
            freeText.text = "Free Heals: 0";
        }
        else {
            costText.text = "Heal Cost: Free";
            freeText.text = "Free Heals: " + hospital.freeHeals.ToString();
        }
    }

    public void heal() {
        if(getSelectedUnit() != null && !getSelectedUnit().isEmpty()) {
            var stats = getSelectedUnit();
            stats.addHealth(healAmt);
            Party.overrideUnitOfSameInstance(stats);

            if(hospital.freeHeals > 0) {
                hospital.freeHeals--;
                GameInfo.getCurrentLocationAsTown().town.holder.overrideObject<HospitalBuilding>(0, hospital);
            }
            else if(Inventory.getCoinCount() >= hospital.pricePerHeal) {
                Inventory.addCoins(-hospital.pricePerHeal, coinCount, true);
            }
            updateInfo();
        }
    }
}

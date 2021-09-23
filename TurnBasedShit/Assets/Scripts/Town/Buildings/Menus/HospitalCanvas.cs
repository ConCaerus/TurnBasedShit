using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HospitalCanvas : BuildingCanvas {
    [SerializeField] TextMeshProUGUI freeText, costText, coinCount;
    float healAmt = 35.0f;

    Coroutine sliderAnim = null;

    bool justHealed = false;

    HospitalBuilding hospital;

    private void Awake() {
        hospital = GameInfo.getCurrentLocationAsTown().town.getHospital();
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

        if(sliderAnim != null)
            StopCoroutine(sliderAnim);
        sliderAnim = StartCoroutine(sliderUpdateAnim());
    }

    public void heal() {
        if(getSelectedUnit() != null && !getSelectedUnit().isEmpty()) {
            var stats = getSelectedUnit();
            stats.addHealth(healAmt);
            Party.overrideUnit(stats);

            if(hospital.freeHeals > 0) {
                hospital.freeHeals--;
                GameInfo.getCurrentLocationAsTown().town.addBuilding(hospital);
            }
            else if(Inventory.getCoinCount() >= hospital.pricePerHeal) {
                Inventory.addCoins(-hospital.pricePerHeal);
            }
            justHealed = true;
            updateInfo();
        }
    }

    IEnumerator sliderUpdateAnim() {
        float speed = 6.0f, distToStop = 0.05f;
        if(justHealed)
            speed *= 2.0f;
        yield return new WaitForEndOfFrame();
        bool cont = false;

        for(int i = 0; i < Party.getMemberCount(); i++) {
            slots.transform.GetChild(i).GetChild(1).GetComponent<Slider>().maxValue = Mathf.Lerp(slots.transform.GetChild(i).GetChild(1).GetComponent<Slider>().maxValue, Party.getMemberStats(i).getModifiedMaxHealth(), speed * Time.deltaTime);
            slots.transform.GetChild(i).GetChild(1).GetComponent<Slider>().value = Mathf.Lerp(slots.transform.GetChild(i).GetChild(1).GetComponent<Slider>().value, Party.getMemberStats(i).u_health, speed * Time.deltaTime);

            if(Mathf.Abs(slots.transform.GetChild(i).GetChild(1).GetComponent<Slider>().maxValue - Party.getMemberStats(i).getModifiedMaxHealth()) > distToStop)
                cont = true;
            else if(Mathf.Abs(slots.transform.GetChild(i).GetChild(1).GetComponent<Slider>().value - Party.getMemberStats(i).u_health) > distToStop)
                cont = true;
        }

        if(mainSlot.transform.GetChild(0).gameObject.activeInHierarchy) {
            mainSlot.transform.GetChild(1).GetComponent<Slider>().maxValue = Mathf.Lerp(mainSlot.transform.GetChild(1).GetComponent<Slider>().maxValue, getSelectedUnit().getModifiedMaxHealth(), speed * Time.deltaTime);
            mainSlot.transform.GetChild(1).GetComponent<Slider>().value = Mathf.Lerp(mainSlot.transform.GetChild(1).GetComponent<Slider>().value, getSelectedUnit().u_health, speed * Time.deltaTime);

            if(!justHealed) {
                mainSlot.transform.GetChild(2).GetComponent<Slider>().maxValue = Mathf.Lerp(mainSlot.transform.GetChild(2).GetComponent<Slider>().maxValue, getSelectedUnit().getModifiedMaxHealth(), speed * Time.deltaTime);
                mainSlot.transform.GetChild(2).GetComponent<Slider>().value = Mathf.Lerp(mainSlot.transform.GetChild(2).GetComponent<Slider>().value, getSelectedUnit().u_health + healAmt, speed * Time.deltaTime);
            }

            if(Mathf.Abs(mainSlot.transform.GetChild(1).GetComponent<Slider>().maxValue - getSelectedUnit().getModifiedMaxHealth()) > distToStop)
                cont = true;
            else if(Mathf.Abs(mainSlot.transform.GetChild(1).GetComponent<Slider>().value - getSelectedUnit().u_health) > distToStop)
                cont = true;

            if(!justHealed) {
                if(Mathf.Abs(mainSlot.transform.GetChild(2).GetComponent<Slider>().maxValue - getSelectedUnit().getModifiedMaxHealth()) > distToStop)
                    cont = true;
                else if(Mathf.Abs(mainSlot.transform.GetChild(2).GetComponent<Slider>().value - getSelectedUnit().u_health + healAmt) > distToStop)
                    cont = true;
            }
        }
        else {
            mainSlot.transform.GetChild(1).GetComponent<Slider>().value = Mathf.Lerp(mainSlot.transform.GetChild(1).GetComponent<Slider>().value, 0.0f, speed * Time.deltaTime);
            mainSlot.transform.GetChild(2).GetComponent<Slider>().value = Mathf.Lerp(mainSlot.transform.GetChild(1).GetComponent<Slider>().value, 0.0f, speed * Time.deltaTime);

            if(Mathf.Abs(mainSlot.transform.GetChild(1).GetComponent<Slider>().value - 0.0f) > distToStop)
                cont = true;
            else if(Mathf.Abs(mainSlot.transform.GetChild(2).GetComponent<Slider>().value - 0.0f) > distToStop)
                cont = true;
        }


        if(cont)
            sliderAnim = StartCoroutine(sliderUpdateAnim());
        else {
            if(justHealed) {
                justHealed = false;
                sliderAnim = StartCoroutine(sliderUpdateAnim());
            }
            else
                sliderAnim = null;
        }
    }
}

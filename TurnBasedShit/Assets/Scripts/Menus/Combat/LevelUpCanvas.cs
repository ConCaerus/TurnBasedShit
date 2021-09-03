using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpCanvas : MonoBehaviour {
    [SerializeField] GameObject canvas;
    [SerializeField] TextMeshProUGUI levelUpTxt, powerTxt, defenceTxt, speedTxt, healthTxt, critTxt;

    Vector2 offset = new Vector2(0.0f, 0.5f);

    private void Awake() {
        canvas.SetActive(false);
    }


    public void levelUpUnit(GameObject unit) {
        var stats = unit.GetComponent<UnitClass>().stats;

        canvas.SetActive(true);
        canvas.transform.position = unit.transform.position;

        levelUpTxt.text = stats.u_level.ToString() + " -> ";
        powerTxt.text = stats.u_power.ToString("0.0") + " -> ";
        defenceTxt.text = stats.u_defence.ToString("0.0") + " -> ";
        speedTxt.text = stats.u_speed.ToString("0.0") + " -> ";
        healthTxt.text = stats.getBaseMaxHealth().ToString("0.0") + " -> ";
        critTxt.text = stats.u_critChance.ToString("0.00") + " -> ";

        unit.GetComponent<UnitClass>().stats.levelUp();

        levelUpTxt.text += stats.u_level.ToString();
        powerTxt.text += stats.u_power.ToString("0.0");
        defenceTxt.text += stats.u_defence.ToString("0.0");
        speedTxt.text += stats.u_speed.ToString("0.0");
        healthTxt.text += stats.getBaseMaxHealth().ToString("0.0");
        critTxt.text += stats.u_critChance.ToString("0.00");

        StartCoroutine(canvasHider());
    }

    IEnumerator canvasHider() {
        yield return new WaitForSeconds(3.0f);

        canvas.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageTextCanvas : MonoBehaviour {
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] Color weaponColor, bleedColor, defendedColor, healingColor, critColor, chargedColor;

    public float time = 1.75f;
    Vector2 offset = new Vector2(-1.5f, 0.5f);
    public float moveY = 1.5f;


    public enum damageType {
        weapon, bleed, defended, healed, crit, charged
    }


    public void showDamageTextForUnit(GameObject unit, float dmg, damageType type) {
        var obj = createTextObj(unit);
        bool special = false;

        if(type == damageType.weapon)
            obj.color = weaponColor;
        else if(type == damageType.bleed) {
            obj.text += "Bleed\n";
            obj.color = bleedColor;
        }
        else if(type == damageType.defended) {
            obj.text += "Blocked\n";
            obj.color = defendedColor;
        }
        else if(type == damageType.healed) {
            obj.text += "Healed\n";
            obj.color = healingColor;
        }
        else if(type == damageType.charged) {
            obj.text += "Charged\n";
            obj.color = chargedColor;
        }
        else if(type == damageType.crit) {
            special = true;
            obj.text += "Crit\n";
            obj.color = critColor;
        }

        obj.text += dmg.ToString("0.#");

        StartCoroutine(animateText(obj.gameObject, unit.gameObject, special));
    }

    public void showTatterTextForUnit(GameObject unit) {
        var obj = createTextObj(unit);
        obj.text = "Tattered";
        obj.color = Color.white;
        FindObjectOfType<AudioManager>().playTatterSound();
        StartCoroutine(animateText(obj.gameObject, unit.gameObject, false));
    }
    public void showLevelUpTextForUnit(GameObject unit) {
        var obj = createTextObj(unit);
        obj.text = "Level Up!";
        obj.color = Color.white;
        StartCoroutine(animateText(obj.gameObject, unit.gameObject, false));
    }

    public void showBluntLevelUpTextForUnit(GameObject unit) {
        var obj = createTextObj(unit);
        obj.text = "Blunt Level Up!";
        obj.color = Color.white;
        StartCoroutine(animateText(obj.gameObject, unit.gameObject, false));
    }
    public void showEdgedLevelUpTextForUnit(GameObject unit) {
        var obj = createTextObj(unit);
        obj.text = "Edged Level Up!";
        obj.color = Color.white;
        StartCoroutine(animateText(obj.gameObject, unit.gameObject, false));
    }
    public void showSummonLevelUpTextForUnit(GameObject unit) {
        var obj = createTextObj(unit);
        obj.text = "Summon Level Up!";
        obj.color = Color.white;
        StartCoroutine(animateText(obj.gameObject, unit.gameObject, false));
    }
    public void showMissTextForUnit(GameObject unit) {
        var obj = createTextObj(unit);
        obj.text = "Miss";
        obj.color = Color.white;
        StartCoroutine(animateText(obj.gameObject, unit.gameObject, false));
    }


    TextMeshProUGUI createTextObj(GameObject unit) {
        var obj = Instantiate(text, transform);
        var target = (Vector2)unit.transform.position + offset;
        obj.transform.position = new Vector3(target.x, target.y, text.transform.position.z);
        obj.text = "";
        return obj;
    }

    IEnumerator animateText(GameObject obj, GameObject unit, bool supersize) {
        float xVal = unit.transform.position.x + offset.x;
        Destroy(obj.gameObject, time);
        obj.transform.DOMoveY(obj.transform.position.y + moveY, time);
        obj.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        if(!supersize)
            obj.transform.DOPunchScale(new Vector3(0.9f, 0.9f, 1.0f), time, 0, 0);
        else
            obj.transform.DOPunchScale(new Vector3(1.0f, 1.0f, 1.0f), time, 0, 0);

        float randRot = 0.0f;
        while(Mathf.Abs(randRot) < 10.0f)
            randRot = Random.Range(-45.0f, 45.0f);
        obj.transform.DORotate(new Vector3(0.0f, 0.0f, randRot), time);

        while(obj.gameObject != null) {
            obj.transform.position = new Vector3(xVal, obj.transform.position.y);
            yield return new WaitForEndOfFrame();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageTextCanvas : MonoBehaviour {
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] Color weaponColor, bleedColor, defendedColor, healingColor, critColor;

    Vector2 offset = new Vector2(-1.5f, 0.5f);
    float moveY = 1.0f;


    public enum damageType {
        weapon, bleed, defended, healed, crit
    }


    public void showTextForUnit(GameObject unit, float dmg, damageType type) {
        var obj = Instantiate(text, transform);
        var target = (Vector2)unit.transform.position + offset;
        obj.transform.position = new Vector3(target.x, target.y, text.transform.position.z);
        obj.text = dmg.ToString("0.#");

        if(type == damageType.weapon)
            obj.color = weaponColor;
        else if(type == damageType.bleed)
            obj.color = bleedColor;
        else if(type == damageType.defended)
            obj.color = defendedColor;
        else if(type == damageType.healed)
            obj.color = healingColor;
        else if(type == damageType.crit)
            obj.color = critColor;

        StartCoroutine(animateText(obj.gameObject, unit.gameObject));
    }

    IEnumerator animateText(GameObject obj, GameObject unit) {
        float xVal = unit.transform.position.x + offset.x;
        float speed = 0.75f;
        Destroy(obj.gameObject, speed);
        obj.transform.DOMoveY(obj.transform.position.y + moveY, speed);
        obj.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        obj.transform.DOPunchScale(new Vector3(1.5f, 1.5f, 1.0f), speed, 0, 0);

        float randRot = 0.0f;
        while(Mathf.Abs(randRot) < 10.0f)
            randRot = Random.Range(-45.0f, 45.0f);
        obj.transform.DORotate(new Vector3(0.0f, 0.0f, randRot), speed);

        while(obj.gameObject != null) {
            obj.transform.position = new Vector3(xVal, obj.transform.position.y);
            yield return new WaitForEndOfFrame();
        }
    }
}

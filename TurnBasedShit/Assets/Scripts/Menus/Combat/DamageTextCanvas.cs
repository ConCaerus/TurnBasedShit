using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextCanvas : MonoBehaviour {
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] Color weaponColor, bleedColor, defendedColor, healingColor, critColor;

    Vector2 offset = new Vector2(0.0f, 0.5f);


    List<damageText> texts = new List<damageText>();


    public enum damageType {
        weapon, poison, defended, healed, crit
    }


    struct damageText {
        public GameObject damagedUnit;
        public TextMeshProUGUI text;
    }


    private void LateUpdate() {
        removeTextsWithUnit(null);
        positionTexts();
    }


    public void showTextForUnit(GameObject unit, float dmg, damageType type) {
        int unitDamageCounterCount = 1;

        foreach(var i in texts) {
            if(i.damagedUnit == unit)
                unitDamageCounterCount++;
        }

        var ob = Instantiate(text, transform);
        var target = (Vector2)unit.transform.position + offset * unitDamageCounterCount;
        ob.transform.position = new Vector3(target.x, target.y, text.transform.position.z);
        ob.text = dmg.ToString("0.#");

        if(type == damageType.weapon)
            ob.color = weaponColor;
        else if(type == damageType.poison)
            ob.color = bleedColor;
        else if(type == damageType.defended)
            ob.color = defendedColor;
        else if(type == damageType.healed)
            ob.color = healingColor;
        else if(type == damageType.crit)
            ob.color = critColor;


        var temp = new damageText();
        temp.damagedUnit = unit;
        temp.text = ob;

        texts.Add(temp);

        StartCoroutine(hideWaiter(temp));
    }

    public void removeTextsWithUnit(GameObject unit) {
        List<int> removeIndexes = new List<int>();
        for(int i = 0; i < texts.Count; i++) {
            if(texts[i].damagedUnit == unit)
                removeIndexes.Add(i);
        }
        foreach(var i in removeIndexes) {
            texts.RemoveAt(i);
        }
    }

    void positionTexts() {
        int[] counts = new int[texts.Count];

        for(int i = 0; i < texts.Count; i++) {
            texts[i].text.transform.position = (Vector2)texts[i].damagedUnit.transform.position + offset * counts[i];

            for(int u = 0; u < texts.Count; u++) {
                if(i != u && texts[i].damagedUnit == texts[u].damagedUnit)
                    counts[u]++;
            }
        }
    }


    IEnumerator hideWaiter(damageText ob) {
        yield return new WaitForSeconds(1.0f);

        texts.Remove(ob);
        if(ob.text != null)
            Destroy(ob.text.gameObject);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextCanvas : MonoBehaviour {
    [SerializeField] TextMeshProUGUI text;

    Vector2 offset = new Vector2(0.0f, 0.5f);


    List<damageText> texts = new List<damageText>();


    public enum damageType {
        weapon, poison, defended
    }


    struct damageText {
        public GameObject damagedUnit;
        public TextMeshProUGUI text;
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
        ob.text = dmg.ToString("0.##");

        if(type == damageType.weapon)
            ob.color = Color.red;
        else if(type == damageType.poison)
            ob.color = Color.magenta;
        else if(type == damageType.defended)
            ob.color = Color.cyan;


            var temp = new damageText();
        temp.damagedUnit = unit;
        temp.text = ob;

        texts.Add(temp);


        StartCoroutine(hideWaiter(temp));
    }


    IEnumerator hideWaiter(damageText ob) {
        yield return new WaitForSeconds(1.0f);

        texts.Remove(ob);
        Destroy(ob.text.gameObject);
    }
}

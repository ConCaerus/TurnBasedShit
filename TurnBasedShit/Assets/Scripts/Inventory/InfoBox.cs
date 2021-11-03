using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour {
    [SerializeField] GameObject infoBox;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dataText;
    List<GameObject> dataTexts = new List<GameObject>();

    [SerializeField] float dataOffset = 1.0f;

    private void Awake() {
        hide();
    }

    private void Update() {
        if(infoBox.activeInHierarchy)
            followMouse();
    }

    void followMouse() {
        Vector3[] corners = new Vector3[4];
        infoBox.GetComponent<RectTransform>().GetWorldCorners(corners);
        var xOffset = (corners[2].x - corners[1].x) / 2.0f;
        var yOffset = (corners[1].y - corners[0].y) / 2.0f;
        infoBox.transform.position = new Vector3(GameInfo.getMousePos().x + xOffset, GameInfo.getMousePos().y - yOffset, infoBox.transform.position.z);
    }

    public void hide() {
        deleteAddDataTexts();
        infoBox.SetActive(false);
    }
    public void setAndShowInfoBox(object t) {
        if(t == null) {
            return;
        }
        infoBox.SetActive(true);
        deleteAddDataTexts();
        //  cannot use a switch for some reason
        if(t.GetType() == typeof(UnitStats)) {
            var s = (UnitStats)t;
            if(s.isEmpty()) {
                infoBox.SetActive(false);
                return;
            }
            nameText.text = FindObjectOfType<TextCreator>().createColoredText(s.u_name);
            createNewDataText(FindObjectOfType<TextCreator>().createColoredText("NUM: " + Party.getUnitIndex(s).ToString(), 1));
            createNewDataText(FindObjectOfType<TextCreator>().createColoredText("POW: " + s.u_power.ToString("0.0"), 1));
            createNewDataText(FindObjectOfType<TextCreator>().createColoredText("SPD: " + s.u_speed.ToString("0.0"), 1));
        }

        else if(t.GetType() == typeof(Weapon)) {
            var w = (Weapon)t;
            if(w.isEmpty()) {
                infoBox.SetActive(false);
                return;
            }
            nameText.text = FindObjectOfType<TextCreator>().createColoredText(w.name);
            createNewDataText(FindObjectOfType<TextCreator>().createColoredText("POW: " + w.power.ToString("0.0"), 1));
            createNewDataText(FindObjectOfType<TextCreator>().createColoredText("SPED: " + w.speedMod.ToString("0.0"), 1));

            for(int i = 0; i < Weapon.attributeCount; i++) {
                foreach(var a in w.attributes) {
                    if((int)a == i) {
                        createNewDataText(FindObjectOfType<TextCreator>().createColoredText(a.ToString().ToUpper() + " " + w.howManyOfAttribute(a).ToString(), 1));
                        break;
                    }
                }
            }
        }

        else if(t.GetType() == typeof(Armor)) {
            var a = (Armor)t;
            if(a.isEmpty()) {
                infoBox.SetActive(false);
                return;
            }
            nameText.text = FindObjectOfType<TextCreator>().createColoredText(a.name);
            createNewDataText(FindObjectOfType<TextCreator>().createColoredText("DEF: " + a.defence.ToString("0.0"), 1));
            createNewDataText(FindObjectOfType<TextCreator>().createColoredText("SPED: " + a.speedMod.ToString("0.0"), 1));

            for(int i = 0; i < Armor.attributeCount; i++) {
                foreach(var s in a.attributes) {
                    if((int)s == i) {
                        createNewDataText(FindObjectOfType<TextCreator>().createColoredText(s.ToString().ToUpper() + " " + a.howManyOfAttribute(s).ToString(), 1));
                        break;
                    }
                }
            }
        }

        else if(t.GetType() == typeof(Usable)) {
            var c = (Usable)t;
            if(c.isEmpty()) {
                infoBox.SetActive(false);
                return;
            }
            nameText.text = FindObjectOfType<TextCreator>().createColoredText(c.name);
            createNewDataText(FindObjectOfType<TextCreator>().createColoredText("EFCT: " + c.effect.ToString().ToUpper(), 1));
            if(c.effectAmount > 0)
                createNewDataText(FindObjectOfType<TextCreator>().createColoredText("AMT: " + c.effectAmount.ToString("0.0"), 1));
        }
    }


    void createNewDataText(string t) {
        var newPos = (Vector2)dataText.transform.position - (dataTexts.Count * new Vector2(0.0f, dataOffset));

        var text = Instantiate(dataText.gameObject);
        text.transform.position = newPos;
        text.GetComponent<TextMeshProUGUI>().enabled = true;
        text.GetComponent<TextMeshProUGUI>().text = t;
        text.GetComponent<RectTransform>().SetParent(dataText.GetComponent<RectTransform>().parent.transform);
        text.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        dataTexts.Add(text);
    }

    void deleteAddDataTexts() {
        foreach(var i in dataTexts) {
            Destroy(i.gameObject);
        }
        dataTexts.Clear();
    }
}

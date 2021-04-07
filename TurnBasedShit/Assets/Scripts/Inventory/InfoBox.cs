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
        if(t.GetType() == typeof(UnitClassStats)) {
            var s = (UnitClassStats)t;
            if(s.isEmpty()) {
                infoBox.SetActive(false);
                return;
            }
            nameText.text = FindObjectOfType<TextCreator>().createColoredText(s.u_name);
            createNewDataText(FindObjectOfType<TextCreator>().createColoredText("NUM: " + s.u_order.ToString(), 1));
            createNewDataText(FindObjectOfType<TextCreator>().createColoredText("POW: " + s.u_power.ToString(), 1));
            createNewDataText(FindObjectOfType<TextCreator>().createColoredText("SPD: " + s.u_speed.ToString(), 1));
        }

        else if(t.GetType() == typeof(Weapon)) {
            var w = (Weapon)t;
            if(w.isEmpty()) {
                infoBox.SetActive(false);
                return;
            }
            nameText.text = FindObjectOfType<TextCreator>().createColoredText(w.w_name);
            createNewDataText(FindObjectOfType<TextCreator>().createColoredText("POW: " + w.w_power.ToString(), 1));
            createNewDataText(FindObjectOfType<TextCreator>().createColoredText("SPED: " + w.w_speedMod.ToString(), 1));

            for(int i = 0; i < w.w_attributes.Count; i++) {
                createNewDataText(FindObjectOfType<TextCreator>().createColoredText(w.w_attributes[i].ToString().ToUpper() + " " + w.howManyOfAttribute(w.w_attributes[i]).ToString(), 1));
            }
        }

        else if(t.GetType() == typeof(Armor)) {
            var a = (Armor)t;
            if(a.isEmpty()) {
                infoBox.SetActive(false);
                return;
            }
            nameText.text = FindObjectOfType<TextCreator>().createColoredText(a.a_name);
            createNewDataText(FindObjectOfType<TextCreator>().createColoredText("DEF: " + a.a_defence.ToString(), 1));
            createNewDataText(FindObjectOfType<TextCreator>().createColoredText("SPED: " + a.a_speedMod.ToString(), 1));

            for(int i = 0; i < a.a_attributes.Count; i++) {
                createNewDataText(FindObjectOfType<TextCreator>().createColoredText(a.a_attributes[i].ToString().ToUpper() + " " + a.howManyOfAttribute(a.a_attributes[i]).ToString(), 1));
            }
        }

        else if(t.GetType() == typeof(Consumable)) {
            var c = (Consumable)t;
            if(c.isEmpty()) {
                infoBox.SetActive(false);
                return;
            }
            nameText.text = FindObjectOfType<TextCreator>().createColoredText(c.c_name);
            createNewDataText(FindObjectOfType<TextCreator>().createColoredText("EFCT: " + c.c_effect.ToString().ToUpper(), 1));
            if(c.c_effectAmount > 0)
                createNewDataText(FindObjectOfType<TextCreator>().createColoredText("AMT: " + c.c_effectAmount.ToString(), 1));
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

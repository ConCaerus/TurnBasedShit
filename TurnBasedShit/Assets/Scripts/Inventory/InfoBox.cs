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
        dataText.enabled = false;
        setAndShowInfoBox(Inventory.getRandomWeaponPreset());
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
        infoBox.transform.position = new Vector3(GameState.getMousePos().x + xOffset, GameState.getMousePos().y - yOffset, infoBox.transform.position.z);
    }

    public void hide() {
        deleteAddDataTexts();
        infoBox.SetActive(false);
    }
    public void setAndShowInfoBox(object t) {
        infoBox.SetActive(true);
        deleteAddDataTexts();
        //  cannot use a switch for some reason
        if(t.GetType() == typeof(Weapon)) {
            var w = (Weapon)t;
            nameText.text = w.w_name;
            createNewDataText("POW: " + w.w_power.ToString());
            createNewDataText("SPED: " + w.w_speedMod.ToString());

            for(int i = 0; i < w.w_attributes.Count; i++) {
                createNewDataText(w.w_attributes[i].ToString().ToUpper() + " " + w.howManyOfAttribute(w.w_attributes[i]).ToString());
            }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderSelectionCanvas : MonoBehaviour {
    [SerializeField] Image unitImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject canvas;
    UnitStats shownUnit;

    private void Start() {
        FindObjectOfType<MapTrail>().enabled = false;
        updateInfo();
    }

    void updateInfo() {
        if(shownUnit == null)
            shownUnit = Party.getMemberStats(0);

        unitImage.GetComponent<Image>().sprite = shownUnit.u_sprite.getSprite();
        unitImage.GetComponent<Image>().color = shownUnit.u_color;
        nameText.text = shownUnit.u_name;
    }


    //  buttons
    public void cycleUnit(bool right) {
        int index = shownUnit.u_order;

        //  right
        if(right) {
            index++;
        }
        //  left
        else {
            index--;
        }

        if(index >= Party.getPartySize())
            index = 0;
        else if(index < 0)
            index = Party.getPartySize() - 1;

        shownUnit = Party.getMemberStats(index);
        updateInfo();
    }

    public void select() {
        Party.setLeader(shownUnit);
        canvas.SetActive(false);
        FindObjectOfType<MapTrail>().enabled = true;
    }
}

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
        if(Party.getLeaderID() == -1) {
            updateInfo();
        }
    }

    void updateInfo() {
        if(shownUnit == null)
            shownUnit = Party.getMemberStats(0);

        unitImage.GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getPlayerUnitSprite().sprite;
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

        if(index >= Party.getMemberCount())
            index = 0;
        else if(index < 0)
            index = Party.getMemberCount() - 1;

        shownUnit = Party.getMemberStats(index);
        updateInfo();
    }

    public void select() {
        Party.setLeader(shownUnit);
        canvas.SetActive(false);
    }
}

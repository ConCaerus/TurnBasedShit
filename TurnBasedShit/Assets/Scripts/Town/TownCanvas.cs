using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TownCanvas : MonoBehaviour {
    [SerializeField] GameObject buildingDetails;
    [SerializeField] TextMeshProUGUI buildingName, buildingIsOpen;
    [SerializeField] Vector2 detailsMouseOffset;


    private void Update() {
        if(getShownBuilding() != null && !FindObjectOfType<QuestSelectionCanvas>().showing()) {
            buildingDetails.SetActive(true);
            setBuildingDetails();
            buildingDetailsToFollowMouse();
        }
        else
            buildingDetails.SetActive(false);
    }

    public void setBuildingDetails() {
        var building = getShownBuilding();
        buildingName.text = building.b_type.ToString();
        buildingIsOpen.text = "Open";
        if(!building.canBeInteractedWith)
            buildingIsOpen.text = "Closed";
    }
    void buildingDetailsToFollowMouse() {
        buildingDetails.transform.position = GameInfo.getMousePos() + detailsMouseOffset;
    }

    public Building getShownBuilding() {
        foreach(var i in FindObjectsOfType<BuildingInstance>()) {
            if(i.isMouseOver)
                return i.building;
        }
        return null;
    }



    public void leaveTown() {
        SceneManager.LoadScene("Map");
    }
}

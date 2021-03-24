using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingInstance : MonoBehaviour {
    public Building building;
    public bool isMouseOver;

    public BuildingInstance(Building b) {
        building = b;
    }
    public BuildingInstance(Building.type type) {
        building = FindObjectOfType<PresetLibrary>().getBuilding(type);
    }

    private void Start() {
        if(building.b_type == Building.type.House) {
            int rand = Random.Range(0, 101);
            if(rand > 75)
                building.canBeInteractedWith = true;
        }

        if(!building.canBeInteractedWith)
            GetComponent<SpriteRenderer>().color = Color.gray;
    }

    private void OnMouseOver() {
        if(building.canBeInteractedWith && Input.GetMouseButtonDown(0)) {
            FindObjectOfType<StoryCanvas>().playStory(building.getRandStory());
        }
    }

    private void OnMouseEnter() {
        isMouseOver = true;
        FindObjectOfType<BuildingHighlighting>().highlightUnit(gameObject);
    }
    private void OnMouseExit() {
        isMouseOver = false;
        FindObjectOfType<BuildingHighlighting>().dehighlightUnit(gameObject);
    }
}

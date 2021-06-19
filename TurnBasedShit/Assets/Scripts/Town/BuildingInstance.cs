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
        building.setup(FindObjectOfType<PresetLibrary>());

        if(!building.canBeInteractedWith)
            GetComponent<SpriteRenderer>().color = Color.gray;
    }

    private void OnMouseOver() {
        if(building.canBeInteractedWith && Input.GetMouseButtonDown(0)) {
            if(building.b_quests != null && building.b_quests.Count > 0)
                FindObjectOfType<QuestSelectionCanvas>().showMenu(building.b_quests);
            else if(building.b_storyBeginnings.Count > 0)
                FindObjectOfType<StoryCanvas>().playStory(building.getRandStory());
        }
    }

    private void OnMouseEnter() {
        isMouseOver = true;
    }
    private void OnMouseExit() {
        isMouseOver = false;
    }
}

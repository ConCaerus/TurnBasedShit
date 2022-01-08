using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TownMenuCanvas : MonoBehaviour {
    [SerializeField] SlotMenu slot;
    [SerializeField] GameObject townSlotPreset;
    [SerializeField] TextMeshProUGUI countText;

    [SerializeField] Color visitedColor, unvisitedColor;
    [SerializeField] Color presetBuildingColor, absentBuildingColor;


    //  true for going by visited, false for time 
    bool state = true;

    private void Update() {
        slot.run();
    }


    public void updateSlots() {
        slot.createANumberOfSlots(MapLocationHolder.getHolder().getObjectCount<TownLocation>(), unvisitedColor);
        int count = 0;
        //  by time
        if(!state) {
            for(int i = 0; i < MapLocationHolder.getHolder().getObjectCount<TownLocation>(); i++) {
                var loc = MapLocationHolder.getHolder().getObject<TownLocation>(i);
                if(loc.town.visited) {
                    var obj = slot.replaceSlot(i, townSlotPreset, slot.gameObject.transform.GetChild(0).transform, visitedColor);
                    obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = loc.town.t_name;
                    obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = loc.town.townMemberCount.ToString();
                    obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = loc.town.getMembersWithActiveQuests().Count.ToString();
                    obj.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = loc.town.getMembersWithInactiveQuests().Count.ToString();

                    if(loc.town.hasBuilding(Building.type.Church))
                        obj.transform.GetChild(4).GetComponent<Image>().color = presetBuildingColor;
                    else
                        obj.transform.GetChild(4).GetComponent<Image>().color = absentBuildingColor;

                    if(loc.town.hasBuilding(Building.type.Hospital))
                        obj.transform.GetChild(5).GetComponent<Image>().color = presetBuildingColor;
                    else
                        obj.transform.GetChild(5).GetComponent<Image>().color = absentBuildingColor;

                    if(loc.town.hasBuilding(Building.type.Shop))
                        obj.transform.GetChild(6).GetComponent<Image>().color = presetBuildingColor;
                    else
                        obj.transform.GetChild(6).GetComponent<Image>().color = absentBuildingColor;
                    count++;
                }
            }
        }

        //  by visited
        else {
            for(int i = 0; i < MapLocationHolder.getHolder().getObjectCount<TownLocation>(); i++) {
                var loc = MapLocationHolder.getHolder().getObject<TownLocation>(i);
                if(loc.town.visited) {
                    var obj = slot.replaceSlot(count, townSlotPreset, slot.gameObject.transform.GetChild(0).transform, visitedColor);
                    obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = loc.town.t_name;
                    obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = loc.town.townMemberCount.ToString();
                    obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = loc.town.getMembersWithActiveQuests().Count.ToString();
                    obj.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = loc.town.getMembersWithInactiveQuests().Count.ToString();

                    if(loc.town.hasBuilding(Building.type.Church))
                        obj.transform.GetChild(4).GetComponent<Image>().color = presetBuildingColor;
                    else
                        obj.transform.GetChild(4).GetComponent<Image>().color = absentBuildingColor;

                    if(loc.town.hasBuilding(Building.type.Hospital))
                        obj.transform.GetChild(5).GetComponent<Image>().color = presetBuildingColor;
                    else
                        obj.transform.GetChild(5).GetComponent<Image>().color = absentBuildingColor;

                    if(loc.town.hasBuilding(Building.type.Shop))
                        obj.transform.GetChild(6).GetComponent<Image>().color = presetBuildingColor;
                    else
                        obj.transform.GetChild(6).GetComponent<Image>().color = absentBuildingColor;

                    count++;
                }
            }
        }

        countText.text = count.ToString() + " / " + MapLocationHolder.getHolder().getObjectCount<TownLocation>();
    }


    public void toggleState() {
        state = !state;
        updateSlots();
    }
}

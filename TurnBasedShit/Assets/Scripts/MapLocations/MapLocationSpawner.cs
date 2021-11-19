using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapLocationSpawner : MonoBehaviour {
    public GameObject bossLocationPreset, rescueLocationPreset, townLocationPreset, upgradeLocationPreset, pickupLocationPreset, fishingLocationPreset, eyeLocationPreset;

    public List<List<GameObject>> currentIcons = new List<List<GameObject>>();

    float distToInteract = 1.0f;



    private void Start() {
        createIcons();
        GameInfo.clearCombatDetails();
    }

    public void createIcons() {
        foreach(var i in GetComponentsInChildren<SpriteRenderer>())
            Destroy(i.gameObject);


        currentIcons.Add(new List<GameObject>());
        currentIcons.Add(new List<GameObject>());
        currentIcons.Add(new List<GameObject>());
        currentIcons.Add(new List<GameObject>());
        currentIcons.Add(new List<GameObject>());
        currentIcons.Add(new List<GameObject>());

        //  Boss Locations
        for(int i = 0; i < ActiveQuests.getBossFightQuestCount(); i++) {
            //  positioning and scaling
            var obj = Instantiate(bossLocationPreset.gameObject);
            obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            obj.transform.position = ActiveQuests.getBossFightQuest(i).location.pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons[(int)Map.getDiffForX(obj.transform.position.x)].Add(obj);
        }


        //  Town locations
        for(int i = 0; i < MapLocationHolder.getTownCount(); i++) {
            //  positioning and scaling
            var obj = Instantiate(townLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getTownLocation(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons[(int)Map.getDiffForX(MapLocationHolder.getTownLocation(i).pos.x)].Add(obj);
        }

        //  Rescue Locations
        for(int i = 0; i < ActiveQuests.getRescueQuestCount(); i++) {
            //  positioning and scaling
            var obj = Instantiate(rescueLocationPreset.gameObject);
            obj.transform.position = ActiveQuests.getRescueQuest(i).location.pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons[(int)Map.getDiffForX(ActiveQuests.getRescueQuest(i).location.pos.x)].Add(obj);
        }

        //  Upgrade Locations
        for(int i = 0; i < MapLocationHolder.getUpgradeCount(); i++) {
            //  positioning and scaling
            var obj = Instantiate(upgradeLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getUpgradeLocation(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons[(int)Map.getDiffForX(MapLocationHolder.getUpgradeLocation(i).pos.x)].Add(obj);
        }

        //  Pickup Locations
        for(int i = 0; i < ActiveQuests.getPickupQuestCount(); i++) {
            //  positioning and scaling
            var obj = Instantiate(pickupLocationPreset.gameObject);
            obj.transform.position = ActiveQuests.getPickupQuest(i).location.pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons[(int)Map.getDiffForX(ActiveQuests.getPickupQuest(i).location.pos.x)].Add(obj);
        }

        //  FISHING
        for(int i = 0; i < MapLocationHolder.getFishingCount(); i++) {
            //  positioning and scaling
            var obj = Instantiate(fishingLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getFishingLocation(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons[(int)Map.getDiffForX(MapLocationHolder.getFishingLocation(i).pos.x)].Add(obj);
        }

        //  eyes
        for(int i = 0; i < MapLocationHolder.getEyeCount(); i++) {
            //  positioning and scaling
            var obj = Instantiate(eyeLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getEyeLocation(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons[(int)Map.getDiffForX(MapLocationHolder.getEyeLocation(i).pos.x)].Add(obj);
        }
    }

    public void removeIconAtPos(Vector2 pos) {
        foreach(var i in currentIcons) {
            foreach(var j in i) {
                if((Vector2)j.transform.position == pos) {
                    var temp = j.gameObject;
                    i.Remove(j);
                    Destroy(temp.gameObject);
                    return;
                }
            }
        }
    }

    GameObject checkIconsProximity() {
        GameObject selected = null;
        float d = distToInteract + 1.0f;
        foreach(var i in currentIcons[(int)Map.getDiffForX(FindObjectOfType<MapMovement>().transform.position.x)]) {
            var dist = Vector2.Distance(i.transform.position, FindObjectOfType<MapMovement>().transform.position);

            if(dist < distToInteract && selected == null) {
                selected = i.gameObject;
                d = dist;
            }

            else if(dist < distToInteract && selected != null && dist < d) {
                selected = i.gameObject;
                d = dist;
            }
        }

        return selected;
    }
}
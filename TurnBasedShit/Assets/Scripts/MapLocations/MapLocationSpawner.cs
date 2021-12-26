using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapLocationSpawner : MonoBehaviour {
    public GameObject bridgePreset;
    public GameObject bossLocationPreset, rescueLocationPreset, townLocationPreset, upgradeLocationPreset, pickupLocationPreset, fishingLocationPreset, eyeLocationPreset;

    public List<GameObject> currentIcons = new List<GameObject>();

    float distToInteract = 1.0f;



    private void Start() {
        createIcons();
        GameInfo.clearCombatDetails();
    }

    public void createIcons() {
        foreach(var i in GetComponentsInChildren<SpriteRenderer>())
            Destroy(i.gameObject);

        //  Boss Locations
        for(int i = 0; i < ActiveQuests.getBossFightQuestCount(); i++) {
            if(ActiveQuests.getBossFightQuest(i).location.region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(bossLocationPreset.gameObject);
            obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            obj.transform.position = ActiveQuests.getBossFightQuest(i).location.pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons.Add(obj);
        }

        //  Town locations
        for(int i = 0; i < MapLocationHolder.getTownCount(); i++) {
            if(MapLocationHolder.getTownLocation(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(townLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getTownLocation(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons.Add(obj);
        }

        //  Rescue Locations
        for(int i = 0; i < ActiveQuests.getRescueQuestCount(); i++) {
            if(ActiveQuests.getRescueQuest(i).location.region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(rescueLocationPreset.gameObject);
            obj.transform.position = ActiveQuests.getRescueQuest(i).location.pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons.Add(obj);
        }

        //  Upgrade Locations
        for(int i = 0; i < MapLocationHolder.getUpgradeCount(); i++) {
            if(MapLocationHolder.getUpgradeLocation(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(upgradeLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getUpgradeLocation(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons.Add(obj);
        }

        //  Pickup Locations
        for(int i = 0; i < ActiveQuests.getPickupQuestCount(); i++) {
            if(ActiveQuests.getPickupQuest(i).location.region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(pickupLocationPreset.gameObject);
            obj.transform.position = ActiveQuests.getPickupQuest(i).location.pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons.Add(obj);
        }

        //  FISHING
        for(int i = 0; i < MapLocationHolder.getFishingCount(); i++) {
            if(MapLocationHolder.getFishingLocation(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(fishingLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getFishingLocation(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons.Add(obj);
        }

        //  eyes
        for(int i = 0; i < MapLocationHolder.getEyeCount(); i++) {
            if(MapLocationHolder.getEyeLocation(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(eyeLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getEyeLocation(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons.Add(obj);
        }

        //  bridges
        for(int i = 0; i < MapLocationHolder.getBridgeCount(); i++) {
            if(MapLocationHolder.getBridgeLocation(i).region != GameInfo.getCurrentRegion())
                continue;

            //  positioning and scaling
            var obj = Instantiate(bridgePreset.gameObject);
            obj.transform.position = MapLocationHolder.getBridgeLocation(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            if(!MapLocationHolder.getBridgeLocation(i).advancing)
                obj.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

            currentIcons.Add(obj);
        }
    }

    public void removeIconAtPos(Vector2 pos) {
        foreach(var i in currentIcons) {
            if((Vector2)i.transform.position == pos) {
                var temp = i.gameObject;
                currentIcons.Remove(i);
                Destroy(temp.gameObject);
                return;
            }
        }
    }

    GameObject checkIconsProximity() {
        GameObject selected = null;
        float d = distToInteract + 1.0f;
        foreach(var i in currentIcons) {
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
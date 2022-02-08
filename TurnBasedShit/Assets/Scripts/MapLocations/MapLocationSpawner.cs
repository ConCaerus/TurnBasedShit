using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapLocationSpawner : MonoBehaviour {
    public GameObject bridgePreset;
    public GameObject bossLocationPreset, rescueLocationPreset, townLocationPreset, upgradeLocationPreset, pickupLocationPreset, fishingLocationPreset, eyeLocationPreset, lootLocationPreset;

    List<GameObject> currentIcons = new List<GameObject>();



    private void Start() {
        createIcons();
        GameInfo.clearCombatDetails();
    }

    public void createIcons() {
        foreach(var i in GetComponentsInChildren<SpriteRenderer>())
            Destroy(i.gameObject);

        //  Boss Locations
        for(int i = 0; i < MapLocationHolder.getHolder().getObjectCount<BossLocation>(); i++) {
            if(MapLocationHolder.getHolder().getObject<BossLocation>(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(bossLocationPreset.gameObject);
            obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            obj.transform.position = MapLocationHolder.getHolder().getObject<BossLocation>(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons.Add(obj);
        }

        //  Town locations
        for(int i = 0; i < MapLocationHolder.getHolder().getObjectCount<TownLocation>(); i++) {
            if(MapLocationHolder.getHolder().getObject<TownLocation>(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(townLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getHolder().getObject<TownLocation>(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons.Add(obj);
        }

        //  Rescue Locations
        for(int i = 0; i < MapLocationHolder.getHolder().getObjectCount<RescueLocation>(); i++) {
            if(MapLocationHolder.getHolder().getObject<RescueLocation>(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(rescueLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getHolder().getObject<RescueLocation>(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons.Add(obj);
        }

        //  Upgrade Locations
        for(int i = 0; i < MapLocationHolder.getHolder().getObjectCount<UpgradeLocation>(); i++) {
            if(MapLocationHolder.getHolder().getObject<UpgradeLocation>(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(upgradeLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getHolder().getObject<UpgradeLocation>(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons.Add(obj);
        }

        //  Pickup Locations
        for(int i = 0; i < MapLocationHolder.getHolder().getObjectCount<PickupLocation>(); i++) {
            if(MapLocationHolder.getHolder().getObject<PickupLocation>(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(pickupLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getHolder().getObject<PickupLocation>(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons.Add(obj);
        }

        //  FISHING
        for(int i = 0; i < MapLocationHolder.getHolder().getObjectCount<FishingLocation>(); i++) {
            if(MapLocationHolder.getHolder().getObject<FishingLocation>(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(fishingLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getHolder().getObject<FishingLocation>(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons.Add(obj);
        }

        //  eyes
        for(int i = 0; i < MapLocationHolder.getHolder().getObjectCount<EyeLocation>(); i++) {
            if(MapLocationHolder.getHolder().getObject<EyeLocation>(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(eyeLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getHolder().getObject<EyeLocation>(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons.Add(obj);
        }

        //  loot
        for(int i = 0; i < MapLocationHolder.getHolder().getObjectCount<LootLocation>(); i++) {
            if(MapLocationHolder.getHolder().getObject<LootLocation>(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(lootLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getHolder().getObject<LootLocation>(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons.Add(obj);
        }

        //  bridges
        for(int i = 0; i < MapLocationHolder.getHolder().getObjectCount<BridgeLocation>(); i++) {
            if(MapLocationHolder.getHolder().getObject<BridgeLocation>(i).region != GameInfo.getCurrentRegion())
                continue;

            //  positioning and scaling
            var obj = Instantiate(bridgePreset.gameObject);
            obj.transform.position = MapLocationHolder.getHolder().getObject<BridgeLocation>(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            if(!MapLocationHolder.getHolder().getObject<BridgeLocation>(i).advancing)
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

    public List<GameObject> getCurrentObjects() {
        var temp = new List<GameObject>();
        foreach(var i in currentIcons) {
            if(FindObjectOfType<MapFogTexture>().isPositionCleared(i.transform.position))
                temp.Add(i.gameObject);
        }

        return temp;
    }
}
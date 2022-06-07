using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MapLocationSpawner : MonoBehaviour {
    public GameObject bridgePreset;
    public GameObject bossLocationPreset, rescueLocationPreset, townLocationPreset, upgradeLocationPreset, pickupLocationPreset, fishingLocationPreset, eyeLocationPreset, lootLocationPreset;
    public GameObject merchantPreset;

    List<GameObject> currentIcons = new List<GameObject>();



    private void Start() {
        createIcons();
        GameInfo.clearCombatDetails();
    }

    public void createIcons() {
        foreach(var i in GetComponentsInChildren<SpriteRenderer>())
            Destroy(i.gameObject);

        //  Boss Locations
        for(int i = 0; i < MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObjectCount<BossLocation>(); i++) {
            if(MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<BossLocation>(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(bossLocationPreset.gameObject);
            obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            obj.transform.position = MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<BossLocation>(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;
            obj.GetComponent<MapIcon>().reference = MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<BossLocation>(i);
            obj.GetComponent<MapIcon>().indexInHolder = i;

            currentIcons.Add(obj);
        }

        //  Town locations
        for(int i = 0; i < MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObjectCount<TownLocation>(); i++) {
            if(MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<TownLocation>(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(townLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<TownLocation>(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;
            obj.GetComponent<MapIcon>().reference = MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<TownLocation>(i);
            obj.GetComponent<MapIcon>().indexInHolder = i;

            currentIcons.Add(obj);
        }

        //  Rescue Locations
        for(int i = 0; i < MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObjectCount<RescueLocation>(); i++) {
            if(MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<RescueLocation>(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(rescueLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<RescueLocation>(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;
            obj.GetComponent<MapIcon>().reference = MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<RescueLocation>(i);
            obj.GetComponent<MapIcon>().indexInHolder = i;

            currentIcons.Add(obj);
        }

        //  Upgrade Locations
        for(int i = 0; i < MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObjectCount<UpgradeLocation>(); i++) {
            if(MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<UpgradeLocation>(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(upgradeLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<UpgradeLocation>(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;
            obj.GetComponent<MapIcon>().reference = MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<UpgradeLocation>(i);
            obj.GetComponent<MapIcon>().indexInHolder = i;

            currentIcons.Add(obj);
        }

        //  Pickup Locations
        for(int i = 0; i < MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObjectCount<PickupLocation>(); i++) {
            if(MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<PickupLocation>(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(pickupLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<PickupLocation>(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;
            obj.GetComponent<MapIcon>().reference = MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<PickupLocation>(i);
            obj.GetComponent<MapIcon>().indexInHolder = i;

            currentIcons.Add(obj);
        }

        /*
        //  FISHING
        for(int i = 0; i < MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObjectCount<FishingLocation>(); i++) {
            if(MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<FishingLocation>(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(fishingLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<FishingLocation>(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;
            obj.GetComponent<MapIcon>().reference = MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<FishingLocation>(i);
            obj.GetComponent<MapIcon>().indexInHolder = i;

            currentIcons.Add(obj);
        }*/

        //  eyes
        for(int i = 0; i < MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObjectCount<EyeLocation>(); i++) {
            if(MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<EyeLocation>(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(eyeLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<EyeLocation>(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;
            obj.GetComponent<MapIcon>().reference = MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<EyeLocation>(i);
            obj.GetComponent<MapIcon>().indexInHolder = i;

            currentIcons.Add(obj);
        }

        //  loot
        for(int i = 0; i < MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObjectCount<LootLocation>(); i++) {
            if(MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<LootLocation>(i).region != GameInfo.getCurrentRegion())
                continue;
            //  positioning and scaling
            var obj = Instantiate(lootLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<LootLocation>(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;
            obj.GetComponent<MapIcon>().reference = MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<LootLocation>(i);
            obj.GetComponent<MapIcon>().indexInHolder = i;

            currentIcons.Add(obj);
        }

        //  bridges
        for(int i = 0; i < MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObjectCount<BridgeLocation>(); i++) {
            if(MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<BridgeLocation>(i).region != GameInfo.getCurrentRegion())
                continue;

            //  positioning and scaling
            var obj = Instantiate(bridgePreset.gameObject);
            obj.transform.position = MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<BridgeLocation>(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;
            obj.GetComponent<MapIcon>().reference = MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<BridgeLocation>(i);
            obj.GetComponent<MapIcon>().indexInHolder = i;

            if(!MapLocationHolder.getHolder(GameInfo.getCurrentRegion()).getObject<BridgeLocation>(i).advancing)
                obj.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

            currentIcons.Add(obj);
        }

        /*
        //  merchant
        for(int i = 0; i < MapMerchantManager.getContainer().getMercsForRegion(GameInfo.getCurrentRegion()).Count; i++) {
            var obj = Instantiate(merchantPreset.gameObject);
            obj.GetComponent<MapMerchant>().setReference(MapMerchantManager.getMerchantInRegion(i, GameInfo.getCurrentRegion()));
            obj.transform.position = Map.getRandPos();
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            currentIcons.Add(obj);
        }*/
    }

    public void removeIonAtPos(Vector2 pos) {
        foreach(var i in currentIcons) {
            if((Vector2)i.transform.position == pos) {
                var temp = i.gameObject;
                currentIcons.Remove(i);
                Destroy(temp.gameObject);
                return;
            }
        }
    }

    public void removeIcon(GameObject icon) {
        int index = currentIcons.IndexOf(icon);
        if(index == -1)
            return;
        var obj = currentIcons[index];
        currentIcons.RemoveAt(index);

        var prev = obj.transform.localScale.x;
        obj.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        obj.transform.DOPunchScale(new Vector3(prev + .25f, prev + .25f), .25f);
        Destroy(obj.gameObject, .25f);    //  laggs the game a little, might as well keep
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
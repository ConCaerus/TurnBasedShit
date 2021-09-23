using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapLocationSpawner : MonoBehaviour {
    public GameObject bossLocationPreset, rescueLocationPreset, townLocationPreset, upgradeLocationPreset, pickupLocationPreset;

    public List<GameObject> currentIcons = new List<GameObject>();



    private void Start() {
        createIcons();
        GameInfo.resetCombatDetails();
    }

    public void createIcons() {
        foreach(var i in GetComponentsInChildren<SpriteRenderer>())
            Destroy(i.gameObject);

        //  Boss Locations
        for(int i = 0; i < MapLocationHolder.getBossCount(); i++) {
            //  positioning and scaling
            var obj = Instantiate(bossLocationPreset.gameObject);
            obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            obj.transform.position = MapLocationHolder.getBossLocation(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            //  info shit
            obj.GetComponent<InfoBearer>().titleText = MapLocationHolder.getBossLocation(i).bossUnit.u_name;

            currentIcons.Add(obj);
        }

        
        //  Town locations
        for(int i = 0; i < MapLocationHolder.getTownCount(); i++) {
            //  positioning and scaling
            var obj = Instantiate(townLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getTownLocation(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            //  info shit
            obj.GetComponent<InfoBearer>().titleText = MapLocationHolder.getTownLocation(i).town.t_name;
            obj.GetComponent<InfoBearer>().firstText = "Buidling Count: " + MapLocationHolder.getTownLocation(i).town.getBuildingCount().ToString();
            obj.GetComponent<InfoBearer>().secondText = "Population: " + MapLocationHolder.getTownLocation(i).town.townMemberCount.ToString();

            currentIcons.Add(obj);
        }

        //  Rescue Locations
        for(int i = 0; i < MapLocationHolder.getRescueCount(); i++) {
            //  positioning and scaling
            var obj = Instantiate(rescueLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getRescueLocation(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            //  info shit
            obj.GetComponent<InfoBearer>().titleText = MapLocationHolder.getRescueLocation(i).unit.u_name;

            currentIcons.Add(obj);
        }

        //  Upgrade Locations
        for(int i = 0; i < MapLocationHolder.getUpgradeCount(); i++) {
            //  positioning and scaling
            var obj = Instantiate(upgradeLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getUpgradeLocation(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            //  info shit
            obj.GetComponent<InfoBearer>().titleText = MapLocationHolder.getUpgradeLocation(i).state.ToString();

            currentIcons.Add(obj);
        }

        //  Pickup Locations
        for(int i = 0; i < MapLocationHolder.getPickupCount(); i++) {
            //  positioning and scaling
            var obj = Instantiate(pickupLocationPreset.gameObject);
            obj.transform.position = MapLocationHolder.getPickupLocation(i).pos;
            obj.transform.SetParent(transform.GetChild(0));
            obj.transform.localScale = Vector3.one / 2.0f;

            //  info shit
            obj.GetComponent<InfoBearer>().titleText = MapLocationHolder.getPickupLocation(i).pType.ToString();

            currentIcons.Add(obj);
        }
    }


    public List<GameObject> getShownLocationObjects() {
        List<GameObject> temp = new List<GameObject>();
        foreach(var i in GetComponentsInChildren<SpriteRenderer>())
            temp.Add(i.gameObject);
        return temp;
    }
}
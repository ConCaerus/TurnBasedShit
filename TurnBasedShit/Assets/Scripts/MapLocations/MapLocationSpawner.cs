﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocationSpawner : MonoBehaviour {
    [SerializeField] int numberOfTowns = 8;
    [SerializeField] int numberOfEquipmentPickups = 15;

    [SerializeField] GameObject iconPreset;



    private void Start() {
        //MapLocationHolder.clearSaveData();
        //Inventory.clearInventory();
        if(MapLocationHolder.getLocationCount() <= 3) {
            if(MapLocationHolder.getPickupCount() == 0)
                createNewRandomEquipmentPickup();
            if(MapLocationHolder.getUpgradeCount() == 0)
                createNewRandomEquipmentUpgrade();
            if(MapLocationHolder.getTownCount() == 0)
                createNewRandomTown();
        }

        createIcons();
        GameInfo.resetCombatDetails();
    }



    public void createIcons() {
        foreach(var i in GetComponentsInChildren<SpriteRenderer>())
            Destroy(i.gameObject);

        for(int i = 0; i < MapLocationHolder.getLocationCount(); i++) {
            MapLocation loc = MapLocationHolder.getMapLocation(i);
            //  equipmentUpgrade shit
            var obj = Instantiate(iconPreset.gameObject);
            obj.transform.position = loc.pos;
            obj.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getMapLocationSprite(loc);
            obj.transform.SetParent(transform);
        }
    }


    public void createNewRandomTown() {
        var pos = Map.getRandPos();
        GameInfo.diffLvl diff = FindObjectOfType<RegionDivider>().getRelevantDifficultyLevel(pos.x);

        TownLocation temp = new TownLocation(pos, diff, FindObjectOfType<PresetLibrary>());


        //  creates an icon for the TownLocation
        var obj = Instantiate(iconPreset.gameObject);
        obj.transform.position = temp.pos;
        obj.transform.SetParent(transform);
        obj.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getMapLocationSprite(temp);


        //  saves the TownLocation
        MapLocationHolder.saveNewLocation(temp);
    }

    public void createNewRandomEquipmentPickup() {
        //  creates a new TownLocation
        var randX = Random.Range(-18.0f, 18.0f);
        var randY = Random.Range(-18.0f, 18.0f);

        GameInfo.diffLvl diff = FindObjectOfType<RegionDivider>().getRelevantDifficultyLevel(randX);

        PickupLocation temp = null;

        var rand = Random.Range(0, 3);
        rand = 0;
        if(rand == 0) {
            Weapon we = FindObjectOfType<PresetLibrary>().getRandomWeapon();
            temp = new PickupLocation(new Vector2(randX, randY), Randomizer.randomizeWeapon(we, diff), FindObjectOfType<PresetLibrary>(), diff);
        }
        else if(rand == 1) {
            Armor ar = FindObjectOfType<PresetLibrary>().getRandomArmor();
            temp = new PickupLocation(new Vector2(randX, randY), Randomizer.randomizeArmor(ar, diff), FindObjectOfType<PresetLibrary>(), diff);
        }
        else if(rand == 2) {
            Consumable con = FindObjectOfType<PresetLibrary>().getRandomConsumable((GameInfo.rarityLvl)diff);
            int conCount = con.c_maxStackCount;
            temp = new PickupLocation(new Vector2(randX, randY), con, Random.Range(1, conCount + 1), FindObjectOfType<PresetLibrary>(), diff);
        }

        //  creates an icon for the TownLocation
        var obj = Instantiate(iconPreset.gameObject);
        obj.transform.position = temp.pos;
        obj.transform.SetParent(transform);
        obj.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getPickupLocationSprite(temp);


        //  saves the TownLocation
        MapLocationHolder.saveNewLocation(temp);
    }

    public void createNewRandomEquipmentUpgrade() {
        //  creates a new TownLocation
        var randX = Random.Range(-18.0f, 18.0f);
        var randY = Random.Range(-18.0f, 18.0f);

        UpgradeLocation temp = null;

        var rand = Random.Range(0, 2);
        if(rand == 0) {
            temp = new UpgradeLocation(new Vector2(randX, randY), 0);
        }
        else if(rand == 1) {
            temp = new UpgradeLocation(new Vector2(randX, randY), 1);
        }

        //  creates an icon for the TownLocation
        var obj = Instantiate(iconPreset.gameObject);
        obj.transform.position = temp.pos;
        obj.transform.SetParent(transform);
        obj.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getUpgradeLocationSprite(temp);


        //  saves the TownLocation
        MapLocationHolder.saveNewLocation(temp);
    }

    public void createNewRandomNest() {
        //  creates a new TownLocation
        var randX = Random.Range(-18.0f, 18.0f);
        var randY = Random.Range(-18.0f, 18.0f);

        GameInfo.diffLvl diff = FindObjectOfType<RegionDivider>().getRelevantDifficultyLevel(randX);

        NestLocation temp = new NestLocation(new Vector2(randX, randY), Random.Range(3, 6), FindObjectOfType<PresetLibrary>(), diff);

        //  creates an icon for the TownLocation
        var obj = Instantiate(iconPreset.gameObject);
        obj.transform.position = temp.pos;
        obj.transform.SetParent(transform);
        obj.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getNestLocationSprite();


        //  saves the TownLocation
        MapLocationHolder.saveNewLocation(temp);
    }

    public void createNewBoss() {
        //  creates a new TownLocation
        var randX = Random.Range(-18.0f, 18.0f);
        var randY = Random.Range(-18.0f, 18.0f);

        GameInfo.diffLvl diff = FindObjectOfType<RegionDivider>().getRelevantDifficultyLevel(randX);

        BossLocation temp = new BossLocation(new Vector2(randX, randY), FindObjectOfType<PresetLibrary>().getRandomBoss(), diff, FindObjectOfType<PresetLibrary>());

        //  creates an icon for the TownLocation
        var obj = Instantiate(iconPreset.gameObject);
        obj.transform.position = temp.pos;
        obj.transform.SetParent(transform);
        obj.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getBossFightLocationSprite(temp);


        //  saves the TownLocation
        MapLocationHolder.saveNewLocation(temp);
    }


    public List<GameObject> getShownLocationObjects() {
        List<GameObject> temp = new List<GameObject>();
        foreach(var i in GetComponentsInChildren<SpriteRenderer>())
            temp.Add(i.gameObject);
        return temp;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocationSpawner : MonoBehaviour {
    [SerializeField] int numberOfTowns = 8;
    [SerializeField] int numberOfEquipmentPickups = 15;

    [SerializeField] GameObject iconPreset;

    [SerializeField] Sprite weaponUpgradeIcon, armorUpgradeIcon;



    private void Start() {
        //createLocations();
        //MapLocationHolder.clearSaveData();
        //Inventory.clearInventory();
        if(MapLocationHolder.getLocationCount() <= 1) {
            if(MapLocationHolder.getPickupCount() == 0)
                createNewRandomEquipmentPickup();
            if(MapLocationHolder.getUpgradeCount() == 0)
                createNewRandomEquipmentUpgrade();
        }

        createIcons();
        GameInfo.resetCombatDetails();
    }



    public void createIcons() {
        foreach(var i in GetComponentsInChildren<SpriteRenderer>())
            Destroy(i.gameObject);

        for(int i = 0; i < MapLocationHolder.getLocationCount(); i++) {
            var loc = MapLocationHolder.getMapLocation(i);
            //  equipmentUpgrade shit
            if(loc.sprite.getSprite(true) != null) {
                var obj = Instantiate(iconPreset.gameObject);
                obj.transform.position = loc.pos;
                obj.GetComponent<SpriteRenderer>().sprite = loc.sprite.getSprite();
                obj.transform.SetParent(transform);
            }
        }
    }



    public void createNewRandomEquipmentPickup() {
        //  creates a new TownLocation
        var randX = Random.Range(-18.0f, 18.0f);
        var randY = Random.Range(-18.0f, 18.0f);

        //  gets a relevant combatlocation
        CombatLocation cl = FindObjectOfType<PresetLibrary>().createCombatLocation(FindObjectOfType<RegionDivider>().getRelevantDifficultyLevel(randX));
        cl = Randomizer.randomizeCombatLocation(cl);

        PickupLocation temp = null;

        var rand = Random.Range(0, 3);
        if(rand == 0) {
            Weapon we = FindObjectOfType<PresetLibrary>().getRandomWeapon();
            temp = new PickupLocation(new Vector2(randX, randY), Randomizer.randomizeWeapon(we, cl.difficulty), cl);
        }
        else if(rand == 1) {
            Armor ar = FindObjectOfType<PresetLibrary>().getRandomArmor();
            temp = new PickupLocation(new Vector2(randX, randY), Randomizer.randomizeArmor(ar, cl.difficulty), cl);
        }
        else if(rand == 2) {
            Consumable con = FindObjectOfType<PresetLibrary>().getRandomConsumable((GameInfo.rarityLvl)cl.difficulty);
            int conCount = con.c_maxStackCount;
            temp = new PickupLocation(new Vector2(randX, randY), con, Random.Range(1, conCount + 1), cl);
        }

        //  creates an icon for the TownLocation
        var obj = Instantiate(iconPreset.gameObject);
        obj.transform.position = temp.pos;
        obj.transform.SetParent(transform);
        obj.GetComponent<SpriteRenderer>().sprite = temp.sprite.getSprite();


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
            temp = new UpgradeLocation(new Vector2(randX, randY), weaponUpgradeIcon, 0);
        }
        else if(rand == 1) {
            temp = new UpgradeLocation(new Vector2(randX, randY), armorUpgradeIcon, 1);
        }

        //  creates an icon for the TownLocation
        var obj = Instantiate(iconPreset.gameObject);
        obj.transform.position = temp.pos;
        obj.transform.SetParent(transform);
        obj.GetComponent<SpriteRenderer>().sprite = temp.sprite.getSprite();


        //  saves the TownLocation
        MapLocationHolder.saveNewLocation(temp);
    }

    public void createLocations() {
        //  towns
        for(int i = 0; i < numberOfTowns; i++) {
            //  creates a new TownLocation
            var randX = Random.Range(-8.0f, 8.0f);
            var randY = Random.Range(-8.0f, 8.0f);

            var temp = new TownLocation(new Vector2(randX, randY), iconPreset.GetComponent<SpriteRenderer>().sprite, Randomizer.createRandomTown());

            //  creates an icon for the TownLocation
            var obj = Instantiate(iconPreset.gameObject);
            obj.transform.position = temp.pos;
            obj.transform.SetParent(transform);


            //  saves the TownLocation
            MapLocationHolder.saveNewLocation(temp);
        }

        //  equipmentPickup
        for(int i = 0; i < numberOfEquipmentPickups; i++) {
            createNewRandomEquipmentPickup();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapLocationSpawner : MonoBehaviour {
    [SerializeField] const int numberOfTowns = 8;
    [SerializeField] const int numberOfEquipmentPickups = 15;

    [SerializeField] GameObject townIconPreset;



    private void Start() {
        createIcons();
        GameState.resetCombatDetails();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space))
            createWeaponPickupLocation();
    }



    public void createIcons() {
        if(MapLocationHolder.getLocationCount() == 0)
            createLocations();

        for(int i = 0; i < MapLocationHolder.getLocationCount(); i++) {
            var loc = MapLocationHolder.getMapLocation(i);
            //  town shit
            if(loc.type == MapLocation.locationType.town) {
                var obj = Instantiate(townIconPreset.gameObject);
                obj.transform.position = loc.pos;
                obj.transform.SetParent(transform);
            }

            //  equipmentPickup shit
            else if(loc.type == MapLocation.locationType.equipmentPickup) {
                var obj = Instantiate(townIconPreset.gameObject);
                obj.transform.position = loc.pos;
                obj.GetComponent<SpriteRenderer>().sprite = loc.sprite.getSprite();
                obj.transform.SetParent(transform);
            }
        }
    }

    
    public void createLocations() {
        //  towns
        for(int i = 0; i < numberOfTowns; i++) {
            //  creates a new TownLocation
            var randX = Random.Range(-8.0f, 8.0f);
            var randY = Random.Range(-8.0f, 8.0f);

            var temp = new TownLocation(new Vector2(randX, randY), townIconPreset.GetComponent<SpriteRenderer>().sprite);

            //  creates an icon for the TownLocation
            var obj = Instantiate(townIconPreset.gameObject);
            obj.transform.position = temp.pos;
            obj.transform.SetParent(transform);


            //  saves the TownLocation
            MapLocationHolder.saveNewLocation(temp);
        }
        
        //  equipmentPickup
        for(int i = 0; i < numberOfEquipmentPickups; i++) {
            //  creates a new TownLocation
            var randX = Random.Range(-18.0f, 18.0f);
            var randY = Random.Range(-18.0f, 18.0f);

            PickupLocation temp = null;

            var rand = Random.Range(0, 3);
            if(rand == 0)
                temp = new PickupLocation(new Vector2(randX, randY), Randomizer.randomizeWeapon(FindObjectOfType<PresetLibrary>().getRandomWeapon()));
            else if(rand == 1)
                temp = new PickupLocation(new Vector2(randX, randY), Randomizer.randomizeArmor(FindObjectOfType<PresetLibrary>().getRandomArmor()));
            else if(rand == 2)
                temp = new PickupLocation(new Vector2(randX, randY), Randomizer.randomizeConsumable(FindObjectOfType<PresetLibrary>().getRandomConsumable()));

            //  creates an icon for the TownLocation
            var obj = Instantiate(townIconPreset.gameObject);
            obj.transform.position = temp.pos;
            obj.transform.SetParent(transform);
            obj.GetComponent<SpriteRenderer>().sprite = temp.sprite.getSprite();


            //  saves the TownLocation
            MapLocationHolder.saveNewLocation(temp);
        }
    }


    //  weaponPickup shit
    public void createWeaponPickupLocation() {
        var randX = Random.Range(-15.0f, 15.0f);
        var randY = Random.Range(-15.0f, 15.0f);

        var temp = new PickupLocation(new Vector2(randX, randY), Randomizer.randomizeWeapon(FindObjectOfType<PresetLibrary>().getRandomWeapon()));

        //  creats an icon for the location
        var obj = Instantiate(townIconPreset.gameObject);
        obj.transform.position = temp.pos;
        obj.GetComponent<SpriteRenderer>().sprite = temp.sprite.getSprite();
        obj.transform.SetParent(transform);

        //  saves the location
        MapLocationHolder.saveNewLocation(temp);
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(MapLocationSpawner))]
public class MapLocationSpawnerEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();


        if(GUILayout.Button("Reset Saved Locations"))
            MapLocationHolder.clearSaveData();
    }
}
#endif

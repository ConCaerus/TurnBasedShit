using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocationSpawner : MonoBehaviour {
    [SerializeField] const int numberOfTowns = 8;

    [SerializeField] GameObject townIconPreset;

    public MapLocationHolder holder = new MapLocationHolder();



    private void Start() {
        loadLocations();
        createTownIcons();
    }


    public void createTowns() {
        for(int i = 0; i < numberOfTowns; i++) {
            var randX = Random.Range(-8.0f, 8.0f);
            var randY = Random.Range(-8.0f, 8.0f);

            MapLocation temp = new MapLocation(MapLocation.locationType.town, new Vector2(randX, randY));

            var obj = Instantiate(townIconPreset.gameObject);
            obj.transform.position = temp.pos;
            obj.transform.SetParent(transform);


            holder.locations.Add(temp);
        }

        saveLocations();
    }

    public void createTownIcons() {
        foreach(var i in holder.locations) {
            if(i.type == MapLocation.locationType.town) {
                var obj = Instantiate(townIconPreset.gameObject);
                obj.transform.position = i.pos;
                obj.transform.SetParent(transform);
            }
        }
    }


    public void saveLocations() {
        var data = JsonUtility.ToJson(holder);
        PlayerPrefs.SetString("Map Locations", data);
    }
    public void loadLocations() {
        var data = PlayerPrefs.GetString("Map Locations");

        if(string.IsNullOrEmpty(data)) {
            Debug.Log("here");
            createTowns();
            saveLocations();
        }
        else
            holder = JsonUtility.FromJson<MapLocationHolder>(data);
    }
}

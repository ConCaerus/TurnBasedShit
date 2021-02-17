using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TownInstance : MonoBehaviour {
    public Town town;
    public int buildingCount = 5;
    public List<Vector2> buildingSpawnPoses = new List<Vector2>();

    private void Start() {
        town = new Town(buildingCount);
    }


    
    public void instantiateBuildings() {
        List<Vector2> unusedSpawnPoses = new List<Vector2>();
        foreach(var i in buildingSpawnPoses)
            unusedSpawnPoses.Add(i);
        if(town.t_buildings == null)
            town.t_buildings = new List<Building>();

        foreach(var i in town.t_buildings) {
            var temp = new GameObject();
            temp.name = i.b_type.ToString();


            var sr = temp.AddComponent<SpriteRenderer>();
            sr.sprite = i.b_sprite.getSprite();

            var bi = temp.AddComponent<BuildingInstance>();
            bi.building = i;

            var col = temp.AddComponent<BoxCollider2D>();

            int rand = Random.Range(0, unusedSpawnPoses.Count);
            temp.transform.position = unusedSpawnPoses[rand];
            unusedSpawnPoses.RemoveAt(rand);
            temp.transform.SetParent(transform);
        }
    }
}

[System.Serializable]
public class Town {
    public int t_buildingCount;

    public List<Building> t_buildings = new List<Building>();

    public Town(int buildingCount) {
        t_buildingCount = buildingCount;
        t_buildings.Clear();
        for(int i = 0; i < t_buildingCount; i++) {
            t_buildings.Add(new Building());
        }
    }


    public void addEmptyBuildings(int count = 0) {
        if(count == 0)
            count = t_buildingCount;
        t_buildings = new List<Building>();
        t_buildings.Clear();
        for(int i = 0; i < count; i++) {
            t_buildings.Add(BuildingLibrary.getBuildingOfType(Building.type.Empty));
        }
        Debug.Log(t_buildings.Count);
    }
    public void addRandomBuildings(int count = 0) {
        if(t_buildings == null)
            t_buildings = new List<Building>();
        if(count == 0)
            count = t_buildingCount;
        t_buildings.Clear();
        for(int i = 0; i < count; i++) {
            int rand = Random.Range(0, Building.buildingTypeCount);
            var building = BuildingLibrary.getBuildingFromIndex(rand);

            //  already has one of this type and shouldn't
            while(building.isOnlyOne && hasBuilding(building.b_type)) {
                rand = Random.Range(0, Building.buildingTypeCount);
                building = BuildingLibrary.getBuildingFromIndex(rand);
            }


            t_buildings.Add(building);
        }
    }

    public bool hasBuilding(Building.type t) {
        foreach(var i in t_buildings)
            if(i.b_type == t)
                return true;
        return false;
    }
    public Building getBuildingOfType(Building.type t) {
        foreach(var i in t_buildings)
            if(i.b_type == t)
                return i;
        return null;
    }
}

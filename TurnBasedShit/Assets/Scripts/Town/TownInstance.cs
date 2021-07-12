using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(TownInstance))]
public class TownEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        TownInstance ti = (TownInstance)target;

        if(GUILayout.Button("Add Rand Buildings")) {
            ti.addRandomBuildings();
        }
        if(GUILayout.Button("Add Empty Buildings")) {
            ti.addEmptyBuildings();
        }
        if(GUILayout.Button("Instantiate Buildings")) {
            foreach(var i in FindObjectsOfType<BuildingInstance>())
                DestroyImmediate(i.gameObject);
        }
    }
}
#endif


public class TownInstance : MonoBehaviour {
    public Town town;
    public int buildingCount = 5;
    public List<Vector2> buildingSpawnPoses = new List<Vector2>();

    private void Awake() {
        if(GameInfo.getCurrentMapLocation() == null || GameInfo.getCurrentMapLocation().type != MapLocation.locationType.town) {
            town = TownLibrary.addNewTownAndSetIndex(new Town(buildingCount, GameInfo.getDiffRegion(), FindObjectOfType<PresetLibrary>()));
            ShopInventory.populateShop(town.t_index, GameInfo.getDiffRegion(), FindObjectOfType<PresetLibrary>());
        }
        else
            town = TownLibrary.getTown(((TownLocation)GameInfo.getCurrentMapLocation()).town.t_index);
    }

    private void Start() {
        instantiateBuildings();
    }


    public void addRandomBuildings() {
        town.t_buildings.Clear();
        for(int i = 0; i < town.t_buildingCount; i++) {
            int rand = Random.Range(0, Building.buildingTypeCount);
            var building = FindObjectOfType<PresetLibrary>().getBuilding((Building.type)rand);

            //  already has one of this type and shouldn't
            while(building.isOnlyOne && town.hasBuilding(building.b_type)) {
                rand = Random.Range(0, Building.buildingTypeCount);
                building = FindObjectOfType<PresetLibrary>().getBuilding((Building.type)rand);
            }


            town.t_buildings.Add(building);
        }
    }
    public void addEmptyBuildings() {
        town.t_buildings.Clear();
        for(int i = 0; i < town.t_buildingCount; i++) {
            town.t_buildings.Add(FindObjectOfType<PresetLibrary>().getBuilding(Building.type.Empty));
        }
    }
    
    void instantiateBuildings() {
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
            temp.transform.localScale = new Vector3(0.75f, 0.75f, 0.0f);
        }
    }
}

[System.Serializable]
public class Town {
    public string t_name = "No Name";

    public int t_index = -1;
    public int t_buildingCount;

    public bool deliveryLocation = false;

    //  shop information
    public float shopSellReduction = 0.05f;
    public float shopPriceMod = 0.0f;

    public List<Building> t_buildings = new List<Building>();

    public Town(int buildingCount, GameInfo.diffLvl diff, PresetLibrary lib) {
        t_buildingCount = buildingCount;
        t_buildings.Clear();
        for(int i = 0; i < t_buildingCount; i++) {
            t_buildings.Add(lib.getRandomBuilding());
        }

        //  shop shit
        ShopInventory.populateShop(t_index, diff, lib);
        shopSellReduction = Random.Range(0.0f, 0.15f);
        shopPriceMod = Random.Range(-0.2f, 0.2f);
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

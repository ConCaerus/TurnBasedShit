using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour {
    public Town reference;

    public Vector2 startingPoint;
    public float buildingBuffer;
    public float endingX;

    float distToInteract = 1.0f;

    public List<GameObject> buildingObjects = new List<GameObject>();

    private void Awake() {
        if(GameInfo.getCurrentLocationAsTown() != null && false)
            reference = GameInfo.getCurrentLocationAsTown().town;
        else {
            var temp = Map.getRandomTownLocationInRegion((int)GameInfo.getCurrentDiff());
            temp.town.visited = true;
            MapLocationHolder.overrideTownLocation(temp);
            GameInfo.setCurrentLocationAsTown(temp);
            reference = temp.town;
        }

        spawnBuildings();

        //  check if been here before
        if(reference.interactedBuildingIndex != -1) {
            FindObjectOfType<TownMovement>().transform.position = new Vector3(getXPosForBuildingAtIndex(reference.interactedBuildingIndex), FindObjectOfType<TownMovement>().transform.position.y, 0.0f);
            StartCoroutine(FindObjectOfType<TownMovement>().exitBuilding());
            FindObjectOfType<TownCameraMovement>().hardMove();
            FindObjectOfType<TownCameraMovement>().zoomOut();
        }
    }



    void spawnBuildings() {
        var list = reference.getBuildings();

        for(int i = 0; i < list.Count; i++) {
            Vector2 pos = startingPoint + new Vector2(buildingBuffer * list[i].orderInTown, 0.0f);
            var obj = Instantiate(FindObjectOfType<PresetLibrary>().getBuilding(list[i].b_type).gameObject, transform);
            obj.transform.localPosition = pos;

            if(obj.GetComponent<HospitalInstance>() != null)
                obj.GetComponent<HospitalInstance>().reference.setEqualTo(reference.getHospital());
            else if(obj.GetComponent<ChurchInstance>() != null)
                obj.GetComponent<ChurchInstance>().reference.setEqualTo(reference.getChurch());
            else if(obj.GetComponent<ShopInstance>() != null)
                obj.GetComponent<ShopInstance>().reference.setEqualTo(reference.getShop());

            buildingObjects.Add(obj.gameObject);
        }

        endingX = startingPoint.x + buildingBuffer * (list[list.Count - 1].orderInTown + 1);
    }


    public int getBuildingOrderIndex(Building.type type) {
        var list = reference.getBuildings();

        for(int i = 0; i < list.Count; i++) {
            if(list[i].b_type == type)
                return list[i].orderInTown;
        }
        return -1;
    }

    public GameObject getInstanceWithType(Building.type type) {
        foreach(var i in buildingObjects) {
            if(i.GetComponent<BuildingInstance>().b_type == type)
                return i.gameObject;
        }
        return null;
    }

    public float getXPosForBuildingAtIndex(int index) {
        var list = reference.getBuildings();
        foreach(var i in list) {
            if(i.orderInTown == index)
                return getInstanceWithType(i.b_type).transform.position.x;
        }

        return -2.0f;
    }


    public GameObject getBuildingWithinInteractRange(float x) {
        foreach(var i in buildingObjects) {
            if(Mathf.Abs(x - i.transform.position.x) < distToInteract)
                return i.gameObject;
        }
        return null;
    }

    public float getXThatIsntInfrontOfADoor() {
        float x = Random.Range(startingPoint.x, endingX);
        if(reference.getBuildings().Count > 2) {
            while(getBuildingWithinInteractRange(x) != null) {
                x = Random.Range(startingPoint.x, endingX);
            }
        }

        return x;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour {
    [SerializeField] GameObject townEndPresset;
    GameObject townEnd;

    public Town reference;

    public Vector2 startingPoint;
    public float buildingBuffer;
    public float endingX;

    float distToInteract = 1.0f;
    public bool createNewTown = false;

    public List<GameObject> buildingObjects = new List<GameObject>();

    private void Awake() {
        GameInfo.currentGameState = GameInfo.state.town;
        if(GameInfo.getCurrentLocationAsTown() != null && !createNewTown)
            reference = GameInfo.getCurrentLocationAsTown().town;
        else {
            var temp = Map.getRandomTownLocationInRegion(GameInfo.getCurrentRegion());
            MapLocationHolder.overrideLocationOfSameType(temp);
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



    private void Update() {
        GameObject closest = null;
        foreach(var i in buildingObjects) {
            if(Mathf.Abs(i.transform.position.x - FindObjectOfType<LocationMovement>().transform.position.x) < distToInteract) {
                closest = i.gameObject;
            }
        }

        if(closest != null)
            FindObjectOfType<InteractionCanvas>().show(closest.transform.position);
        else
            FindObjectOfType<InteractionCanvas>().hide();
    }



    void spawnBuildings() {
        for(int i = 0; i < reference.holder.getObjectCount<Building>(); i++) {
            Vector2 pos = startingPoint + new Vector2(buildingBuffer * i, 0.0f);
            var obj = Instantiate(FindObjectOfType<PresetLibrary>().getBuilding(reference.getBuidingTypeWithOrder(i)).gameObject, transform);
            obj.transform.localPosition = pos;

            if(obj.GetComponent<HospitalInstance>() != null)
                obj.GetComponent<HospitalInstance>().reference.setEqualTo(reference.holder.getObject<HospitalBuilding>(0));
            else if(obj.GetComponent<ChurchInstance>() != null)
                obj.GetComponent<ChurchInstance>().reference.setEqualTo(reference.holder.getObject<ChurchBuilding>(0));
            else if(obj.GetComponent<ShopInstance>() != null)
                obj.GetComponent<ShopInstance>().reference.setEqualTo(reference.holder.getObject<ShopBuilding>(0));
            else if(obj.GetComponent<CasinoInstance>() != null)
                obj.GetComponent<CasinoInstance>().reference.setEqualTo(reference.holder.getObject<CasinoBuilding>(0));
            else if(obj.GetComponent<BlacksmithInstance>() != null)
                obj.GetComponent<BlacksmithInstance>().reference.setEqualTo(reference.holder.getObject<BlacksmithBuilding>(0));

            buildingObjects.Add(obj.gameObject);
        }

        endingX = startingPoint.x + (buildingBuffer * 2.0f) * (reference.holder.getObjectCount<Building>() + 1);
        townEnd = Instantiate(townEndPresset.gameObject, transform);
        townEnd.transform.position = new Vector3(endingX, startingPoint.y);
    }

    public GameObject getInstanceWithType(Building.type type) {
        foreach(var i in buildingObjects) {
            if(i.GetComponent<BuildingInstance>().b_type == type)
                return i.gameObject;
        }
        return null;
    }

    public float getXPosForBuildingAtIndex(int index) {
        return getInstanceWithType(reference.getBuidingTypeWithOrder(index)).transform.position.x;
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
        if(reference.holder.getObjectCount<Building>() > 2) {
            while(getBuildingWithinInteractRange(x) != null) {
                x = Random.Range(startingPoint.x, endingX);
            }
        }

        return x;
    }
}

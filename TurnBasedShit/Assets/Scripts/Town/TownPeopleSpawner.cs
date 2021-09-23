using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownPeopleSpawner : MonoBehaviour {
    Town reference;

    [SerializeField] float memberYPos;

    public List<GameObject> memberObjects = new List<GameObject>();

    private void Start() {
        reference = FindObjectOfType<BuildingSpawner>().reference;
        spawnMembers();
    }



    void spawnMembers() {
        for(int i = 0; i < reference.townMemberCount; i++) {
            float x = FindObjectOfType<BuildingSpawner>().getXThatIsntInfrontOfADoor();
            
            var pos = new Vector2(x, memberYPos);
            var obj = Instantiate(FindObjectOfType<PresetLibrary>().getTownMemberObj(false).gameObject);
            obj.GetComponentInChildren<TownMemberInstance>().reference.setEqualsTo(reference.getMember(i), true);
            obj.GetComponentInChildren<UnitSpriteHandler>().setLayerOffset(-(i + 1) * 10);

            obj.transform.position = pos;

            memberObjects.Add(obj.gameObject);
        }
    }


    public GameObject getMemberWithinInteractRange(float x, int distToInteract) {
        foreach(var i in memberObjects) {
            if(Mathf.Abs(x - i.transform.position.x) < distToInteract)
                return i.gameObject;
        }
        return null;
    }
}

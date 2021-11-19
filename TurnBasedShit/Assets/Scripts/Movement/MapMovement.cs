using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : InteractiveMovement {
    [SerializeField] GameObject sideUnitPreset;
    List<int> sideUnitIndexes = new List<int>();
    List<GameObject> sideUnits = new List<GameObject>();

    GameInfo.region currentDiff;

    float distToInt = 1f;


    private void Start() {
        transform.position = GameInfo.getCurrentMapPos();
        currentDiff = GameInfo.getCurrentRegion();
        GameInfo.currentGameState = GameInfo.state.map;
        hideInteractText();
        createSideUnitObjects();
    }


    private void LateUpdate() {
        if(Map.getDiffForX(transform.position.x) != currentDiff && FindObjectOfType<RegionNotificationCanvas>().canAnimate()) {
            currentDiff = Map.getDiffForX(transform.position.x);
            GameInfo.setCurrentMapPos(transform.position);
            FindObjectOfType<MapFogTexture>().saveTexture();
            FindObjectOfType<RegionNotificationCanvas>().startShowing();
        }

        moveSideUnits();
    }


    void createSideUnitObjects() {
        var offset = new Vector3(0.75f, 0.0f, 0.0f);
        var last = transform.position;
        for(int i = 0; i < Party.getMemberCount(); i++) {
            if(Party.getMemberStats(i).isTheSameInstanceAs(Party.getLeaderStats()))
                continue;


            var temp = Instantiate(sideUnitPreset.gameObject, transform.parent);
            temp.GetComponentInChildren<UnitSpriteHandler>().setEverything(Party.getMemberStats(i));
            temp.GetComponent<MapSideUnitMovement>().moveSpeed = moveSpeed / 1.25f;
            temp.transform.localScale = new Vector3(0.25f, 0.25f, 1.0f);
            temp.transform.position = last + offset;
            last = temp.transform.position;
            sideUnits.Add(temp.gameObject);
            sideUnitIndexes.Add(i);
        }
    }


    void moveSideUnits() {
        float maxDist = 1.5f;
        GameObject target = unit;
        for(int i = 0; i < sideUnits.Count; i++) {
            if(Vector2.Distance(sideUnits[i].transform.position, target.transform.position) > maxDist) {
                sideUnits[i].GetComponent<MapSideUnitMovement>().isMoving = true;
                sideUnits[i].GetComponent<MapSideUnitMovement>().moveToPoint(target.transform.position);
            }
            else {
                sideUnits[i].GetComponent<MapSideUnitMovement>().isMoving = false;
                if(!isMoving) {
                    if(!sideUnits[i].GetComponentInChildren<UnitSpriteHandler>().isFaceShown())
                        sideUnits[i].GetComponent<MapSideUnitMovement>().flip(true);
                }
            }
            target = sideUnits[i];
        }
    }

    public void setSideUnitVisuals() {
        for(int i = 0; i < sideUnits.Count; i++) {
            sideUnits[i].GetComponentInChildren<UnitSpriteHandler>().setEverything(Party.getMemberStats(sideUnitIndexes[i]));
        }
    }


    public override bool outOfBounds() {
        bool x = transform.position.x > Map.rightBound || transform.position.x < Map.leftBound;
        bool y = transform.position.y > Map.topBound || transform.position.y < Map.botBound;

        return x || y;
    }

    public override bool canMoveAlongY() {
        return true;
    }
    public override bool showWeapon() {
        return true;
    }

    public override bool shouldInteract() {
        return Input.GetKey(KeyCode.Space);
    }
    public override bool shouldDeinteract() {
        return false;
    }

    public override void interact() {
        if(MapLocationHolder.locationCloseToPos(transform.position, distToInt)) {
            var loc = MapLocationHolder.getClostestLocation(transform.position);
            GameInfo.setCurrentMapPos(MapLocationHolder.getClostestLocation(transform.position).pos);
            FindObjectOfType<MapFogTexture>().saveTexture();

            if(loc.type == MapLocation.locationType.eye)
                ((EyeLocation)loc).activate(FindObjectOfType<MapFogTexture>(), FindObjectOfType<MapLocationSpawner>());
            else
                MapLocationHolder.getClostestLocation(transform.position).enterLocation(FindObjectOfType<TransitionCanvas>());
        }
    }

    public override void deinteract() {
    }
}

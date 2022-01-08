using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : InteractiveMovement {
    [SerializeField] GameObject sideUnitPreset;
    List<int> sideUnitIndexes = new List<int>();
    List<GameObject> sideUnits = new List<GameObject>();

    Vector2 lastAteAtPos;
    float distToEat = 10.0f;

    GameInfo.region currentDiff;

    public GameObject closestIcon = null;


    private void Start() {
        transform.position = GameInfo.getCurrentMapPos();
        currentDiff = GameInfo.getCurrentRegion();
        GameInfo.currentGameState = GameInfo.state.map;
        createSideUnitObjects();
        lastAteAtPos = transform.position;
    }




    private void LateUpdate() {
        moveSideUnits();
        eatFood();
    }


    void createSideUnitObjects() {
        var offset = new Vector3(0.75f, 0.0f, 0.0f);
        var last = transform.position;
        for(int i = 0; i < Party.getMemberCount(); i++) {
            if(Party.getMemberStats(i).isTheSameInstanceAs(Party.getLeaderStats()))
                continue;


            var temp = Instantiate(sideUnitPreset.gameObject, transform.parent);
            temp.GetComponentInChildren<UnitSpriteHandler>().setReference(Party.getMemberStats(i), true);
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

    public void eatFood() {
        if(Vector2.Distance(transform.position, lastAteAtPos) > distToEat) {
            lastAteAtPos = transform.position;
            for(int p = 0; p < Party.getMemberCount(); p++) {
                var food = Inventory.eatFood();
                if(food == null || food.isEmpty()) {
                    for(int i = 0; i < Party.getMemberCount(); i++) {
                        var stats = Party.getMemberStats(i);
                        stats.addHealth(-2f);
                        Party.overrideUnit(stats);
                    }
                    return;
                }
                else
                    FindObjectOfType<MapCollectCanvas>().showCanvas(false, food);
            }
        }
    }

    public void setSideUnitVisuals() {
        for(int i = 0; i < sideUnits.Count; i++) {
            sideUnits[i].GetComponentInChildren<UnitSpriteHandler>().setReference(Party.getMemberStats(sideUnitIndexes[i]), true);
        }
    }


    public override bool outOfBounds() {
        bool x = transform.position.x > Map.rightBound() || transform.position.x < Map.leftBound();
        bool y = transform.position.y > Map.topBound() || transform.position.y < Map.botBound();

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
        if(closestIcon == null)
            return;
        var loc = MapLocationHolder.getLocationAtPos(closestIcon.transform.position);
        if(loc == null)
            return;
        GameInfo.setCurrentMapPos(closestIcon.transform.position);
        FindObjectOfType<MapFogTexture>().saveTexture();

        if(loc.type == MapLocation.locationType.eye)
            ((EyeLocation)loc).activate(FindObjectOfType<MapFogTexture>(), FindObjectOfType<MapLocationSpawner>());
        else
            loc.enterLocation(FindObjectOfType<TransitionCanvas>());
    }

    public override void deinteract() {
    }
}

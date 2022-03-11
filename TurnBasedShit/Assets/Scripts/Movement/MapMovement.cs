using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : InteractiveMovement {
    [SerializeField] GameObject sideUnitPreset;
    List<int> sideUnitIndexes = new List<int>();
    List<GameObject> sideUnits = new List<GameObject>();

    Vector2 lastAteAtPos;
    float distToEat = 10.0f;

    public GameObject closestIcon = null;


    private void Start() {
        transform.position = GameInfo.getCurrentMapPos();
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
        for(int i = 0; i < Party.getHolder().getObjectCount<UnitStats>(); i++) {
            if(Party.getHolder().getObject<UnitStats>(i).isTheSameInstanceAs(Party.getLeaderStats()))
                continue;


            var temp = Instantiate(sideUnitPreset.gameObject, transform.parent);
            temp.GetComponent<MapSideUnitMovement>().moveSpeed = moveSpeed / 1.25f;
            temp.transform.localScale = new Vector3(0.25f, 0.25f, 1.0f);
            temp.transform.position = last + offset;
            last = temp.transform.position;
            sideUnits.Add(temp.gameObject);
            sideUnitIndexes.Add(i);
        }

        setSideUnitVisuals();
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
                sideUnits[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                if(!isMoving) {
                    if(sideUnits[i].GetComponentInChildren<UnitSpriteHandler>().initialized) {
                        if(!sideUnits[i].GetComponentInChildren<UnitSpriteHandler>().isFaceShown())
                            sideUnits[i].GetComponent<MapSideUnitMovement>().flip(true);
                    }
                }
            }
            target = sideUnits[i];
        }
    }

    public void eatFood() {
        if(Vector2.Distance(transform.position, lastAteAtPos) > distToEat) {
            lastAteAtPos = transform.position;
            for(int p = 0; p < Party.getHolder().getObjectCount<UnitStats>(); p++) {
                var food = Inventory.eatFood();
                if(food == null || food.isEmpty()) {
                    for(int i = 0; i < Party.getHolder().getObjectCount<UnitStats>(); i++) {
                        var stats = Party.getHolder().getObject<UnitStats>(i);
                        stats.addHealth(-2f);
                        Party.overrideUnitOfSameInstance(stats);
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
            sideUnits[i].GetComponentInChildren<UnitSpriteHandler>().setReference(Party.getHolder().getObject<UnitStats>(sideUnitIndexes[i]), true);
        }
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
        if(closestIcon.GetComponent<MapMerchant>() != null) {
            closestIcon.GetComponent<MapMerchant>().showCanvas();
        }

        else if(closestIcon.GetComponent<MapIcon>() != null) {
            var loc = closestIcon.GetComponent<MapIcon>().reference;
            if(loc == null)
                return;

            if(loc.type == MapLocation.locationType.eye) {
                FindObjectOfType<MapFogTexture>().clearFogAroundPos(loc.pos, 10f, true);
                MapLocationHolder.removeLocation<EyeLocation>(closestIcon.GetComponent<MapIcon>().indexInHolder);   //  this
                FindObjectOfType<MapLocationSpawner>().removeIcon(closestIcon); //  not this
            }
            else if(loc.type == MapLocation.locationType.loot) {
                ((LootLocation)loc).activate(FindObjectOfType<MapLootCanvas>(), FindObjectOfType<PresetLibrary>(), FindObjectOfType<FullInventoryCanvas>());
                FindObjectOfType<MapLocationSpawner>().removeIcon(closestIcon);
            }
            else {
                GameInfo.setCurrentMapPos(closestIcon.transform.position);
                FindObjectOfType<MapFogTexture>().saveTexture();
                loc.enterLocation(FindObjectOfType<TransitionCanvas>());
            }
        }
    }

    public override void deinteract() {
    }
}

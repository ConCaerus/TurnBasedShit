using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TownMovement : LocationMovement {


    private void Start() {
        var town = GameInfo.getCurrentLocationAsTown();
        hideInteractText();

        rightMost = FindObjectOfType<BuildingSpawner>().endingX;

        if(town.town.interactedBuildingIndex != -1) {
            canMove = false;
            FindObjectOfType<TownCameraMovement>().hardMove();
            transform.position = new Vector3(FindObjectOfType<BuildingSpawner>().getXPosForBuildingAtIndex(town.town.interactedBuildingIndex), transform.position.y, 0.0f);
            setVisuals();
            StartCoroutine(exitBuilding());
        }
    }

    public override void interact() {
        //  int with person
        var p = FindObjectOfType<TownPeopleSpawner>().getMemberWithinInteractRange(transform.position.x, interactDist);
        if(p != null) {
            FindObjectOfType<TownCameraMovement>().zoomIn(2.0f);
            p.GetComponentInChildren<TownMemberInstance>().interacting = true;
            return;
        }


        //  int with building
        var b = FindObjectOfType<BuildingSpawner>().getBuildingWithinInteractRange(transform.position.x);
        if(b != null) {
            FindObjectOfType<TownCameraMovement>().zoomIn(2.0f);
            StartCoroutine(enterBuilding(b.GetComponent<BuildingInstance>()));
            return;
        }

        //  int with end of town
        if(Mathf.Abs(FindObjectOfType<BuildingSpawner>().endingX - transform.position.x) < interactDist) {
            FindObjectOfType<TownCameraMovement>().zoomIn(2.0f);
            StartCoroutine(exitTown());
        }
    }

    public override void deinteract() {
        foreach(var i in FindObjectsOfType<TownMemberInstance>()) {
            if(i.interacting) {
                FindObjectOfType<TownCameraMovement>().zoomOut();
                i.interacting = false;
            }
        }
    }


    IEnumerator enterBuilding(BuildingInstance bi) {
        float time = 0.25f;
        unit.transform.DOComplete();
        canMove = false;

        flip(false);

        yield return new WaitForSeconds(time);
        unit.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), time * 1.5f);

        yield return new WaitForSeconds(time * 1.5f);

        var town = GameInfo.getCurrentLocationAsTown();
        town.town.interactedBuildingIndex = town.town.getOrderForBuilding(bi.b_type);
        MapLocationHolder.overrideTownLocation(town);
        GameInfo.setCurrentLocationAsTown(town);

        switch(bi.b_type) {
            case Building.type.Hospital:
                FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("HospitalBuilding");
                break;

            case Building.type.Church:
                FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("ChurchBuilding");
                break;

            case Building.type.Shop:
                FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("ShopBuilding");
                break;

            case Building.type.Casino:
                FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("CasinoBuilding");
                break;

            case Building.type.Blacksmith:
                FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("BlacksmithBuilding");
                break;
        }
    }

    IEnumerator exitTown() {
        float time = 0.25f;
        unit.transform.DOComplete();
        canMove = false;
        var t = GameInfo.getCurrentLocationAsTown();
        t.town.visited = true;
        t.town.interactedBuildingIndex = -1;
        MapLocationHolder.overrideTownLocation(t);

        flip(false);

        yield return new WaitForSeconds(time);
        unit.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), time * 1.5f);

        yield return new WaitForSeconds(time * 1.5f);
        FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("Map");
    }

    public IEnumerator exitBuilding() {
        GetComponentInChildren<UnitSpriteHandler>().hideFace();
        float time = 0.25f;
        unit.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        unit.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), time * 1.5f);


        yield return new WaitForSeconds(time);

        while(!GetComponentInChildren<UnitSpriteHandler>().isFaceShown()) {
            flip(true);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(time / 2.0f);

        canMove = true;
    }
}

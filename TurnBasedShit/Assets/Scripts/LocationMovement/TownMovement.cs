using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TownMovement : LocationMovement {


    private void Start() {
        var town = GameInfo.getCurrentLocationAsTown();

        if(town.town.interactedBuildingIndex != -1) {
            FindObjectOfType<TownCameraMovement>().hardMove();
            transform.position = new Vector3(FindObjectOfType<BuildingSpawner>().getXPosForBuildingAtIndex(town.town.interactedBuildingIndex), transform.position.y, 0.0f);
            StartCoroutine(exitBuilding());
        }
    }


    private void LateUpdate() {
        if(FindObjectOfType<MenuCanvas>().isOpen()) {
            setVisuals();
        }
    }

    public override void interact() {
        //  int with person
        var p = FindObjectOfType<TownPeopleSpawner>().getMemberWithinInteractRange(transform.position.x);
        if(p != null) {
            FindObjectOfType<TownCameraMovement>().zoomIn(2.0f);
            p.GetComponentInChildren<TownMemberInstance>().interacting = true;
            return;
        }


        //  int with building
        var b = FindObjectOfType<BuildingSpawner>().getBuildingWithinInteractRange(transform.position.x);
        if(b != null) {
            FindObjectOfType<TownCameraMovement>().zoomIn();
            StartCoroutine(enterBuilding(b.GetComponent<BuildingInstance>()));
            return;
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

        flip();

        yield return new WaitForSeconds(time);
        unit.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), time * 1.5f);

        yield return new WaitForSeconds(time * 1.5f);

        var town = GameInfo.getCurrentLocationAsTown();
        town.town.interactedBuildingIndex = FindObjectOfType<BuildingSpawner>().getBuildingOrderIndex(bi.b_type);
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
        }
    }

    public IEnumerator exitBuilding() {
        float time = 0.25f;
        unit.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        unit.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), time * 1.5f);


        var town = GameInfo.getCurrentLocationAsTown();
        town.town.interactedBuildingIndex = -1;
        GameInfo.setCurrentLocationAsTown(town);

        yield return new WaitForSeconds(time);
        flip();

        yield return new WaitForSeconds(time / 2.0f);

        canMove = true;
    }
}

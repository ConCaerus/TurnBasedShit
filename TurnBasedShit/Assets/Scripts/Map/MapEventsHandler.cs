using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapEventsHandler : MonoBehaviour {

    private void Awake() {
        DOTween.Init();
    }

    public void triggerEncounter(bool special) {
        //  creates a combat location
        CombatLocation cl;
        if(!special)
            cl = FindObjectOfType<PresetLibrary>().createCombatLocation(GameInfo.getCurrentRegion());
        else
            cl = FindObjectOfType<PresetLibrary>().createSpecialCombatLocation(GameInfo.getCurrentRegion());
        GameInfo.setCombatDetails(cl);

        //  flair
        FindObjectOfType<MapMovement>().canMove = false;
        foreach(var i in FindObjectsOfType<MapEnemyMovement>())
            i.enabled = false;
        FindObjectOfType<EnounterCanvas>().showEnemyEncounterAlert();
    }

    public void triggerAnchorEvents(Vector2 partyPos) {
        //  boss
        for(int i = 0; i < MapLocationHolder.getHolder().getObjectCount<BossLocation>(); i++) {
            var loc = MapLocationHolder.getHolder().getObject<BossLocation>(i);

            //  Party is over a map location
            if(partyPos == loc.pos) {
                FindObjectOfType<EnounterCanvas>().showEnemyEncounterAlert(loc);
                return;
            }
        }

        for(int i = 0; i < MapLocationHolder.getHolder().getObjectCount<PickupLocation>(); i++) {
            var loc = MapLocationHolder.getHolder().getObject<PickupLocation>(i);

            if(partyPos == loc.pos) {
                FindObjectOfType<EnounterCanvas>().showEnemyEncounterAlert(loc);
                return;
            }
        }
    }
}

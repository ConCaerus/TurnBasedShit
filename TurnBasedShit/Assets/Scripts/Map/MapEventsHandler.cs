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

        //  enemy events

        for(int i = 0; i < MapLocationHolder.getTownCount(); i++) {
            var loc = MapLocationHolder.getTownLocation(i);

            if(partyPos == loc.pos) {
                loc.enterLocation(FindObjectOfType<TransitionCanvas>());
                return;
            }
        }
        //  boss
        for(int i = 0; i < MapLocationHolder.getBossCount(); i++) {
            var loc = MapLocationHolder.getBossLocation(i);

            //  Party is over a map location
            if(partyPos == loc.pos) {
                FindObjectOfType<EnounterCanvas>().showEnemyEncounterAlert(loc);
                return;
            }
        }

        for(int i = 0; i < MapLocationHolder.getPickupCount(); i++) {
            var loc = MapLocationHolder.getPickupLocation(i);

            if(partyPos == loc.pos) {
                FindObjectOfType<EnounterCanvas>().showEnemyEncounterAlert(loc);
                return;
            }
        }

        for(int i = 0; i < MapLocationHolder.getUpgradeCount(); i++) {
            var loc = MapLocationHolder.getUpgradeLocation(i);

            if(partyPos == loc.pos) {
                FindObjectOfType<EnounterCanvas>().showEnemyEncounterAlert(loc);
                return;
            }
        }

        for(int i = 0; i < MapLocationHolder.getNestCount(); i++) {
            var loc = MapLocationHolder.getNestLocation(i);

            if(partyPos == loc.pos) {
                FindObjectOfType<EnounterCanvas>().showEnemyEncounterAlert(loc);
                return;
            }
        }
    }
}

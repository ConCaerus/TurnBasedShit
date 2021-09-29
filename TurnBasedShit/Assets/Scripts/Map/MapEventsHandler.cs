﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapEventsHandler : MonoBehaviour {

    private void Awake() {
        DOTween.Init();
    }

    public void triggerEncounter() {
        //  creates a combat location
        var cl = FindObjectOfType<PresetLibrary>().createCombatLocation(Map.getDiffForX(FindObjectOfType<MapMovement>().transform.position.x));
        cl = Randomizer.randomizeCombatLocation(cl);
        GameInfo.setCombatDetails(cl);

        //  flair
        FindObjectOfType<MapMovement>().canMove = false;
        FindObjectOfType<EnounterCanvas>().showEnemyEncounterAlert();
    }

    public void triggerAnchorEvents(Vector2 partyPos) {

        //  enemy events

        for(int i = 0; i < MapLocationHolder.getTownCount(); i++) {
            var loc = MapLocationHolder.getTownLocation(i);

            if(partyPos == loc.pos) {
                FindObjectOfType<TransitionCanvas>().loadSceneWithFunction(loc.enterLocation);
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

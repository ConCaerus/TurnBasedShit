using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapEventsHandler : MonoBehaviour {

    private void Awake() {
        DOTween.Init();
    }

    public void chanceEncounter(GameInfo.diffLvl regionDiff) {
        return;
        switch(regionDiff) {
            case GameInfo.diffLvl.Cake:
            case GameInfo.diffLvl.Easy:
            case GameInfo.diffLvl.Normal:
            case GameInfo.diffLvl.Inter:
                if(Random.Range(0, 11) == 0)    //  10%
                    triggerEncounter(regionDiff);
                break;

            case GameInfo.diffLvl.Hard:
            case GameInfo.diffLvl.Heroic:
                if(Random.Range(0, 101) <= 15)    //  15%
                    triggerEncounter(regionDiff);
                break;

            case GameInfo.diffLvl.Legendary:
                if(Random.Range(0, 101) <= 26)    //  25%
                    triggerEncounter(regionDiff);
                break;
        }
    }

    public void triggerEncounter(GameInfo.diffLvl regionDiff) {
        //  creates a combat location
        var cl = FindObjectOfType<PresetLibrary>().createCombatLocation(regionDiff);
        cl = Randomizer.randomizeCombatLocation(cl);
        GameInfo.setCombatDetails(cl);

        //  flair
        FindObjectOfType<RoadRenderer>().shrinkPartyObj();
        FindObjectOfType<RoadRenderer>().canMove = false;
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


        //  if the code gets here, the party is not over a map location and the code runs combat chance
        chanceEncounter(FindObjectOfType<RegionDivider>().getRelevantDifficultyLevel(partyPos.x));
    }
}

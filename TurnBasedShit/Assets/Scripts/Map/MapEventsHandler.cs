using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapEventsHandler : MonoBehaviour {

    private void Awake() {
        DOTween.Init();
    }

    public void chanceEncounter(GameInfo.diffLvl regionDiff) {
        //  for debugging perposes
        triggerEncounter(regionDiff);
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
        var cl = FindObjectOfType<PresetLibrary>().createCombatLocation(regionDiff);
        cl = Randomizer.randomizeCombatLocation(cl);
        GameInfo.setCombatDetails(cl);

        FindObjectOfType<EnounterCanvas>().showEnemyEncounterAlert();
    }

    public void triggerAnchorEvents(Vector2 partyPos) {
        for(int i = 0; i < MapLocationHolder.getLocationCount(); i++) {
            var loc = MapLocationHolder.getMapLocation(i);

            //  Party is over a map location
            if(partyPos == loc.pos) {
                FindObjectOfType<EnounterCanvas>().showEnemyEncounterAlert(loc);
                Debug.Log("here");
                return;
            }
        }

        //  if the code gets here, the party is not over a map location and the code runs combat chance
        chanceEncounter(FindObjectOfType<RegionDivider>().getRelevantDifficultyLevel(partyPos.x));
    }
}

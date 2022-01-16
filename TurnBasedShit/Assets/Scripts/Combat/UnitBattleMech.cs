using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class UnitBattleMech : MonoBehaviour {
    [SerializeField] GameObject battleResultsCanvas;

    [SerializeField] ParticleSystem bloodParticles;

    int waveIndex = 0;

    bool battleEnded = false;

    private void Awake() {
        GameInfo.currentGameState = GameInfo.state.combat;
    }

    private void Start() {
        foreach(var i in FindObjectOfType<CombatCameraController>().getEnivironmentHolder().GetComponentsInChildren<Animator>()) {
            i.ForceStateNormalizedTime(i.gameObject.transform.position.x / 20.0f);
        }
        setUp();
    }

    void setUp() {
        if(GameInfo.getCombatDetails() == null || GameInfo.getCombatDetails().waves == null || GameInfo.getCombatDetails().waves.Count == 0 || GameInfo.getCombatDetails().waves[0].enemyIndexes.Count == 0) {
            var loc = FindObjectOfType<PresetLibrary>().createCombatLocation(GameInfo.getCurrentRegion());
            for(int i = 0; i < 10; i++) {
                var col = FindObjectOfType<PresetLibrary>().getRandomCollectable();

                switch(col.type) {
                    case Collectable.collectableType.weapon:
                        loc.spoils.addObject<Weapon>((Weapon)col);
                        break;
                    case Collectable.collectableType.armor:
                        loc.spoils.addObject<Armor>((Armor)col);
                        break;
                    case Collectable.collectableType.item:
                        loc.spoils.addObject<Item>((Item)col);
                        break;
                    case Collectable.collectableType.usable:
                        loc.spoils.addObject<Usable>((Usable)col);
                        break;
                    case Collectable.collectableType.unusable:
                        loc.spoils.addObject<Unusable>((Unusable)col);
                        break;
                }
            }
            GameInfo.setCombatDetails(loc);

            Debug.Log("created");
        }

        //  mechs
        FindObjectOfType<PartyObject>().instantiatePartyMembers();
        FindObjectOfType<EnemyUnitSpawner>().spawnEnemies(0);
        battleResultsCanvas.SetActive(false);
        resetBattleRound();
        FindObjectOfType<RoundCounterCanvas>().updateInfo();
        StartCoroutine(checkIfBattleEnded());
    }

    public void resetBattleRound() {
        FindObjectOfType<TurnOrderSorter>().resetList();
        FindObjectOfType<RoundCounterCanvas>().incrementAndUpdateRoundCount();
    }


    public void endBattle() {
        //  shit
        FindObjectOfType<TurnOrderSorter>().enabled = false;
        foreach(var i in FindObjectsOfType<CombatUnitUI>()) {
            i.removeUI();
        }

        //  enemies killed all of the players units
        if(FindObjectsOfType<PlayerUnitInstance>().Length == 0) {
            FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("MainMenu");
        }

        //  player killed all enemies and the battle is over
        else if(FindObjectsOfType<EnemyUnitInstance>().Length == 0) {
            showBattleResults();
            GameInfo.getCombatDetails().receiveSpoils();
            battleResultsCanvas.GetComponent<BattleResultsCanvas>().showCombatLocationEquipment();
        }

        updateMapAndQuests();
    }

    void showBattleResults() {
        battleResultsCanvas.SetActive(true);
    }


    void updateMapAndQuests() {
        //  pickup 
        for(int i = 0; i < MapLocationHolder.getHolder().getObjectCount<PickupLocation>(); i++) {
            if(MapLocationHolder.getHolder().getObject<PickupLocation>(i).pos == GameInfo.getCurrentMapPos()) {
                MapLocationHolder.removeLocation(MapLocationHolder.getHolder().getObject<PickupLocation>(i));
                break;
            }
        }
        for(int i = 0; i < ActiveQuests.getHolder().getObjectCount<PickupQuest>(); i++) {
            var j = ActiveQuests.getHolder().getObject<PickupQuest>(i);
            if(j.location.pos == GameInfo.getCurrentMapPos()) {
                MapLocationHolder.removeLocation(j.location);
                ActiveQuests.completeQuest(j, FindObjectOfType<QuestCompleteCanvas>());
                break;
            }
        }

        //  rescue
        for(int i = 0; i < MapLocationHolder.getHolder().getObjectCount<RescueLocation>(); i++) {
            if(MapLocationHolder.getHolder().getObject<RescueLocation>(i).pos == GameInfo.getCurrentMapPos()) {
                MapLocationHolder.removeLocation(MapLocationHolder.getHolder().getObject<RescueLocation>(i));
                break;
            }
        }
        for(int i = 0; i < ActiveQuests.getHolder().getObjectCount<RescueQuest>(); i++) {
            var j = ActiveQuests.getHolder().getObject<RescueQuest>(i);
            if(j.location.pos == GameInfo.getCurrentMapPos()) {
                ActiveQuests.completeQuest(j, FindObjectOfType<QuestCompleteCanvas>());
                break;
            }
        }
    }

    IEnumerator checkIfBattleEnded() {
        yield return new WaitForEndOfFrame();

        bool runAgain = true;

        if(FindObjectOfType<TransitionCanvas>().loaded && !battleEnded && (FindObjectsOfType<PlayerUnitInstance>().Length == 0 || FindObjectsOfType<EnemyUnitInstance>().Length == 0)) {
            //  Player won
            if(FindObjectsOfType<EnemyUnitInstance>().Length == 0 && FindObjectsOfType<PlayerUnitInstance>().Length > 0) {
                //  check that there are no more waves
                if(GameInfo.getCombatDetails() != null && waveIndex >= GameInfo.getCombatDetails().waves.Count - 1) {
                    yield return new WaitForSeconds(0.15f);
                    battleEnded = true;
                    endBattle();
                    runAgain = false;
                }

                //  spawn next wave
                else if(GameInfo.getCombatDetails() != null && waveIndex < GameInfo.getCombatDetails().waves.Count - 1) {
                    waveIndex++;
                    FindObjectOfType<RoundCounterCanvas>().incrementAndUpdateWaveCount();
                    FindObjectOfType<EnemyUnitSpawner>().spawnEnemies(waveIndex);
                    foreach(var i in FindObjectsOfType<EnemyUnitInstance>()) {
                        FindObjectOfType<TurnOrderSorter>().addUnitToList(i.gameObject);
                    }
                }
            }


            //  Player lost
            else if(FindObjectsOfType<PlayerUnitInstance>().Length == 0) {
                //  fuckin' do shit
                battleEnded = true;
                endBattle();
                runAgain = false;
            }
        }
        if(runAgain)
            StartCoroutine(checkIfBattleEnded());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnitBattleMech : MonoBehaviour {
    [SerializeField] GameObject battleResultsCanvas;

    [SerializeField] ParticleSystem bloodParticles;

    int waveIndex = 0;

    bool battleEnded = false;

    private void Awake() {
        StartCoroutine(FindObjectOfType<TransitionCanvas>().runAfterLoading(setUp));
    }

    void setUp() {
        if(GameInfo.getCombatDetails() == null || GameInfo.getCombatDetails().waves == null || GameInfo.getCombatDetails().waves.Count == 0 || GameInfo.getCombatDetails().waves[0].enemies.Count == 0 || GameInfo.getCombatDetails().waves[0].enemies[0] == null || true) {
            GameInfo.setCombatDetails(FindObjectOfType<PresetLibrary>().createCombatLocation(0));
            Debug.Log("created");
        }

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

        //  flair
        FindObjectOfType<CombatCameraController>().moveToMiddle();

        //  enemies killed all of the players units
        if(FindObjectsOfType<PlayerUnitInstance>().Length == 0) {
            //  do stuff.
        }

        //  player killed all enemies and the battle is over
        else if(FindObjectsOfType<EnemyUnitInstance>().Length == 0) {
            GameInfo.getCombatDetails().addSpoils();
            showBattleResults();
            battleResultsCanvas.GetComponent<BattleResultsCanvas>().showCombatLocationEquipment();
        }

        GameInfo.setCombatDetails(null);
    }

    void showBattleResults() {
        battleResultsCanvas.SetActive(true);
    }


    IEnumerator checkIfBattleEnded() {
        yield return new WaitForEndOfFrame();

        bool runAgain = true;

        if(FindObjectOfType<TransitionCanvas>().loaded && !battleEnded && (FindObjectsOfType<PlayerUnitInstance>().Length == 0 || FindObjectsOfType<EnemyUnitInstance>().Length == 0)) {
            //  Player won
            if(FindObjectsOfType<EnemyUnitInstance>().Length == 0 && FindObjectsOfType<PlayerUnitInstance>().Length > 0) {
                //  check that there are no more waves
                if(GameInfo.getCombatDetails() != null && waveIndex >= GameInfo.getCombatDetails().waves.Count - 1) {
                    battleEnded = true;
                    endBattle();
                    runAgain = false;
                }

                //  spawn next wave
                else if(GameInfo.getCombatDetails() != null && waveIndex < GameInfo.getCombatDetails().waves.Count - 1) {
                    waveIndex++;
                    FindObjectOfType<RoundCounterCanvas>().incrementAndUpdateWaveCount();
                    FindObjectOfType<TurnOrderSorter>().resetList();
                    FindObjectOfType<EnemyUnitSpawner>().spawnEnemies(waveIndex);
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

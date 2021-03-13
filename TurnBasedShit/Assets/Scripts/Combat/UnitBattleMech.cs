using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBattleMech : MonoBehaviour {
    [SerializeField] GameObject battleResultsCanvas;

    bool battleEnded = false;

    private void Awake() {
        FindObjectOfType<PartyObject>().instantiatePartyMembers();
        FindObjectOfType<EnemyUnitSpawner>().spawnEnemies();
        battleResultsCanvas.SetActive(false);
    }

    private void Start() {
        //FindObjectOfType<CombatEnvironmentSpawner>().spawnEnivronmentObjects();
        resetBattleRound();
    }

    private void Update() {
        if(!battleEnded && (FindObjectsOfType<PlayerUnitInstance>().Length == 0 || FindObjectsOfType<EnemyUnitInstance>().Length == 0)) {
            battleEnded = true;
            endBattle();
        }
    }

    public void resetBattleRound() {
        FindObjectOfType<TurnOrderSorter>().resetList();
        FindObjectOfType<RoundCounterCanvas>().roundCount++;
    }


    public void endBattle() {
        //  enemies killed all of the players units
        if(FindObjectsOfType<PlayerUnitInstance>().Length == 0) {
            //  do stuff.
        }

        //  player killed all enemies and the battle is over
        else if(FindObjectsOfType<EnemyUnitInstance>().Length == 0) {
            GameState.getCombatDetails().addSpoilsToInventory();
            showBattleResults();
            battleResultsCanvas.GetComponent<BattleResultsCanvas>().showCombatLocationEquipment();
        }
    }

    void showBattleResults() {
        battleResultsCanvas.SetActive(true);
    }
}

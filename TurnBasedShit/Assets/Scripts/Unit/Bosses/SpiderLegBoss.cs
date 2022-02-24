using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLegBoss : BossUnitInstance {

    private void Start() {
        FindObjectOfType<UnitBattleMech>().addRunOnRoundReset(chanceNewLegSpawned);
    }


    //  after each round, 50% chance that a new leg spawns
    public void chanceNewLegSpawned(int roundIndex) {
        if(GameVariables.chanceOutOfHundred(50) && GameInfo.getCombatDetails().waves[0].enemyIndexes.Count > 0)
            FindObjectOfType<EnemyUnitSpawner>().spawnEnemies(FindObjectOfType<EnemyUnitSpawner>().lastWaveSpawned + 1);
    }
}

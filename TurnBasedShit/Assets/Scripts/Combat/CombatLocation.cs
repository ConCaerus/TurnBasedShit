using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatLocation {
    public int instanceID = -1;
    [SerializeField] public List<Wave> waves = new List<Wave>(1);


    public int coins = 0;
    public ObjectHolder spoils = new ObjectHolder();

    public GameInfo.region reg;
    public bool isBoss = false;


    public CombatLocation(GameInfo.region diff, PresetLibrary lib, int numberOfWaves = 2, int minNumberOfEnemies = 2, int maxNumberOfEnemies = 4) {
        createWaves(diff, lib, numberOfWaves, minNumberOfEnemies, maxNumberOfEnemies);
    }

    public void addBossesToWave(int waveIndex, List<GameInfo.combatUnitType> bosses, PresetLibrary lib) {
        while(waveIndex >= waves.Count)
            waves.Add(new Wave());

        var temp = new Wave();
        for(int i = 0; i < bosses.Count; i++) {
            var index = lib.getBossIndex(bosses[i]);
            temp.bossIndexes.Add(index);
        }

        waves[waveIndex] = temp;
    }
    public void createWaves(GameInfo.region diff, PresetLibrary lib, int numberOfWaves, int minNumberOfEnemies = 2, int maxNumberOfEnemies = 4) {
        waves.Clear();
        addWaves(diff, lib, numberOfWaves, minNumberOfEnemies, maxNumberOfEnemies);
    }
    public void addWaves(GameInfo.region diff, PresetLibrary lib, int numberOfWaves, int minNumberOfEnemies = 2, int maxNumberOfEnemies = 4) {
        for(int i = 0; i < numberOfWaves; i++) {
            Wave temp = new Wave();

            int enemyCount = Random.Range(minNumberOfEnemies, maxNumberOfEnemies + 1);
            for(int e = 0; e < enemyCount; e++) {
                var enemy = lib.getRandomEnemyIndex(diff);
                if(lib.getEnemy(enemy).GetComponent<UnitClass>().stats.u_type == GameInfo.combatUnitType.deadUnit && Graveyard.getHolder().getObjectCount<UnitStats>() == 0)
                    continue;
                temp.enemyIndexes.Add(enemy);
                if(lib.getEnemy(enemy).GetComponent<UnitClass>().stats.u_type == GameInfo.combatUnitType.deadUnit) {
                    temp.graveyardIndexes.Add(Random.Range(0, Graveyard.getHolder().getObjectCount<UnitStats>()));
                }
            }


            waves.Add(temp);
        }
    }
    public void removeAllWavesButOne() {
        Wave wave = waves[0];
        waves.Clear();
        waves.Add(wave);
    }
    public void clearWaves() {
        waves.Clear();
    }

    public bool isEmpty() {
        return waves == null || waves.Count == 0 || (waves[0].enemyIndexes.Count == 0 && waves[0].bossIndexes.Count == 0 && waves[0].graveyardIndexes.Count == 0);
    }

    public void receiveSpoils(PresetLibrary lib, FullInventoryCanvas fic) {
        Inventory.addCoins(coins);
        Inventory.addCollectables(spoils.getCollectables(), lib, fic);

        foreach(var i in spoils.getObjects<UnitStats>())
            Party.addUnit(i);
    }
}

[System.Serializable]
public class Wave {
    public List<int> enemyIndexes = new List<int>();
    public List<int> graveyardIndexes = new List<int>();
    public List<int> bossIndexes = new List<int>();
}
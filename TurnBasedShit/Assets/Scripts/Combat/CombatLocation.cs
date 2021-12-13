using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatLocation {
    [SerializeField] public List<Wave> waves = new List<Wave>(1);

    public GameInfo.region difficulty;

    public int coinReward = 0;


    public List<Collectable> collectables = new List<Collectable>();
    public List<UnitStats> rescuedUnits = new List<UnitStats>();

    public CombatLocation(GameInfo.region diff, PresetLibrary lib, int numberOfWaves = 2, int minNumberOfEnemies = 2, int maxNumberOfEnemies = 4) {
        createWaves(diff, lib, numberOfWaves, minNumberOfEnemies, maxNumberOfEnemies);
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
                temp.enemyIndexes.Add(lib.getRandomEnemyIndex(diff));
            }
            waves.Add(temp);
        }
    }
    public void removeAllWavesButOne() {
        Wave wave = waves[0];
        waves.Clear();
        waves.Add(wave);
    }

    public void addSpoils() {
        Inventory.addCoins(coinReward);

        foreach(var i in collectables)
            Inventory.addCollectable(i);
        foreach(var i in rescuedUnits)
            Party.addUnit(i);
    }
}

[System.Serializable]
public class Wave {
    public List<int> enemyIndexes = new List<int>();
    public List<int> bossIndexes = new List<int>();
}
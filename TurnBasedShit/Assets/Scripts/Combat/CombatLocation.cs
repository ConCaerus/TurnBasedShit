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



    public void receiveSpoils(PresetLibrary lib) {
        Inventory.addCoins(coins);

        foreach(var i in spoils.getObjects<Weapon>())
            Inventory.addCollectable(i, lib);
        foreach(var i in spoils.getObjects<Armor>())
            Inventory.addCollectable(i, lib);
        foreach(var i in spoils.getObjects<Item>())
            Inventory.addCollectable(i, lib);
        foreach(var i in spoils.getObjects<Usable>())
            Inventory.addCollectable(i, lib);
        foreach(var i in spoils.getObjects<Unusable>())
            Inventory.addCollectable(i, lib);
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
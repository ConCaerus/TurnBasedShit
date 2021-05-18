using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatLocation {
    [SerializeField] public List<Wave> waves = new List<Wave>(1);

    public GameInfo.diffLvl difficulty;

    public int coinReward = 0;

    public List<Weapon> weapons = new List<Weapon>();
    public List<Armor> armor = new List<Armor>();
    public List<Consumable> consumables = new List<Consumable>();
    public List<Item> items = new List<Item>();
    public List<UnitStats> rescuedUnits = new List<UnitStats>();

    public CombatLocation(GameInfo.diffLvl diff, PresetLibrary lib, int numberOfWaves = 2, int minNumberOfEnemies = 2, int maxNumberOfEnemies = 4) {
        createWaves(diff, lib, numberOfWaves, minNumberOfEnemies, maxNumberOfEnemies);
    }

    public void createWaves(GameInfo.diffLvl diff, PresetLibrary lib, int numberOfWaves, int minNumberOfEnemies = 2, int maxNumberOfEnemies = 4) {
        waves.Clear();
        addWaves(diff, lib, numberOfWaves, minNumberOfEnemies, maxNumberOfEnemies);
    }
    public void addWaves(GameInfo.diffLvl diff, PresetLibrary lib, int numberOfWaves, int minNumberOfEnemies = 2, int maxNumberOfEnemies = 4) {
        for(int i = 0; i < numberOfWaves; i++) {
            Wave temp = new Wave();

            int enemyCount = Random.Range(minNumberOfEnemies, maxNumberOfEnemies + 1);
            for(int e = 0; e < enemyCount; e++) {
                var enemy = lib.getRandomEnemy(diff);
                enemy = Randomizer.randomizeUnitStats(enemy);
                enemy.u_name = NameLibrary.getRandomEnemyName();
                temp.enemies.Add(enemy);
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

        foreach(var i in weapons)
            Inventory.addWeapon(i);
        foreach(var i in armor)
            Inventory.addArmor(i);
        foreach(var i in consumables)
            Inventory.addConsumable(i);
        foreach(var i in items)
            Inventory.addItem(i);
        foreach(var i in rescuedUnits)
            Party.addNewUnit(i);
    }
}

[System.Serializable]
public class Wave {
    public List<UnitStats> enemies = new List<UnitStats>();
}
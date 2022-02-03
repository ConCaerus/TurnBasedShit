using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyUnitSpawner : MonoBehaviour {
    [SerializeField] float showingSpeed = 0.15f;

    public void spawnEnemies(int waveIndex) {
        var info = GameInfo.getCombatDetails();

        List<GameObject> unusedSpawnPoses = new List<GameObject>();
        foreach(var i in FindObjectsOfType<CombatSpot>()) {
            if(!i.isPlayerSpot())
                unusedSpawnPoses.Add(i.gameObject);
        }

        int graveIndex = 0;
        foreach(var i in info.waves[waveIndex].enemyIndexes) { 
            if(i == -1) {
                continue;
            }
            var obj = Instantiate(FindObjectOfType<PresetLibrary>().getEnemy(i).gameObject);
            obj.name = "Enemy: " + obj.GetComponent<UnitClass>().stats.u_name;

            if(obj.GetComponent<UnitClass>().stats.u_type == GameInfo.combatUnitType.deadUnit) {
                var prevStats = Graveyard.getHolder().getObject<UnitStats>(graveIndex);
                prevStats.u_type = GameInfo.combatUnitType.deadUnit;
                obj.GetComponent<UnitClass>().stats = prevStats;
                obj.GetComponentInChildren<UnitSpriteHandler>().setReference(prevStats, true);
                graveIndex++;
            }

            //  sassigns a spot
            var rand = Random.Range(0, unusedSpawnPoses.Count);
            unusedSpawnPoses[rand].GetComponent<CombatSpot>().unit = obj.gameObject;
            unusedSpawnPoses[rand].GetComponent<CombatSpot>().setColor();

            obj.GetComponent<UnitClass>().setup();

            unusedSpawnPoses.RemoveAt(rand);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyUnitSpawner : MonoBehaviour {
    [SerializeField] GameObject enemyPreset;

    [SerializeField] float showingSpeed = 0.15f;

    public void spawnEnemies(int waveIndex) {
        var info = GameInfo.getCombatDetails();

        List<GameObject> unusedSpawnPoses = new List<GameObject>();
        foreach(var i in FindObjectsOfType<CombatSpot>()) {
            if(!i.isPlayerSpot())
                unusedSpawnPoses.Add(i.gameObject);
        }

        foreach(var i in info.waves[waveIndex].enemyIndexes) { 
            if(i == -1) {
                continue;
            }
            var obj = Instantiate(FindObjectOfType<PresetLibrary>().getEnemy(i).gameObject);
            obj.name = "Enemy: " + obj.GetComponent<UnitClass>().stats.u_name;

            //  sassigns a spot
            var rand = Random.Range(0, unusedSpawnPoses.Count);
            unusedSpawnPoses[rand].GetComponent<CombatSpot>().unit = obj.gameObject;

            obj.GetComponent<UnitClass>().setup();

            unusedSpawnPoses.RemoveAt(rand);
        }
    }
}

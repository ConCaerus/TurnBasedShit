using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitSpawner : MonoBehaviour {
    [SerializeField] List<GameObject> enemySpawnPoses;
    [SerializeField] GameObject enemyPreset;

    public void spawnEnemies() {
        var info = GameState.getCombatDetails();

        if(info.enemies.Count == 0)
            info = Randomizer.getRandomCombatLocation();

        List<GameObject> unusedSpawnPoses = new List<GameObject>();
        foreach(var i in enemySpawnPoses)
            unusedSpawnPoses.Add(i.gameObject);

        foreach(var i in info.enemies) {
            var obj = Instantiate(enemyPreset.gameObject);
            obj.name = "Enemy: " + i.u_name;

            //  sets a random position
            var rand = Random.Range(0, unusedSpawnPoses.Count);
            obj.transform.position = unusedSpawnPoses[rand].transform.position;
            unusedSpawnPoses.RemoveAt(rand);

            obj.GetComponent<UnitClass>().stats = i;
        }
    }
}

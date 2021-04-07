using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitSpawner : MonoBehaviour {
    [SerializeField] List<GameObject> enemySpawnPoses;
    [SerializeField] GameObject enemyPreset;

    public void spawnEnemies() {
        var info = FindObjectOfType<PresetLibrary>().createCombatLocation(GameInfo.getDiffRegion());

        if(info == null || info.enemies.Count == 0)
            info = FindObjectOfType<PresetLibrary>().createCombatLocation(0);

        List<GameObject> unusedSpawnPoses = new List<GameObject>();
        foreach(var i in enemySpawnPoses)
            unusedSpawnPoses.Add(i.gameObject);

        foreach(var i in info.enemies) {
            var obj = Instantiate(enemyPreset.gameObject);
            Randomizer.randomizeUnitStats(i);
            obj.name = "Enemy: " + i.u_name;
            obj.GetComponent<SpriteRenderer>().sprite = i.u_sprite.getSprite();

            //  sets a random position
            var rand = Random.Range(0, unusedSpawnPoses.Count);
            obj.transform.position = unusedSpawnPoses[rand].transform.position;
            unusedSpawnPoses.RemoveAt(rand);

            obj.GetComponent<UnitClass>().stats = i;
        }
    }
}

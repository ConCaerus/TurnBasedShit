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

        foreach(var i in info.waves[waveIndex].enemies) { 
            var obj = Instantiate(i.gameObject);
            obj.name = "Enemy: " + i.GetComponent<UnitClass>().stats.u_name;

            obj.GetComponent<UnitClass>().setup();

            //  sets a random position
            var rand = Random.Range(0, unusedSpawnPoses.Count);
            obj.transform.position = unusedSpawnPoses[rand].transform.position + new Vector3(0.0f, obj.GetComponent<UnitClass>().spotOffset, 0.0f);
            unusedSpawnPoses[rand].GetComponent<CombatSpot>().unit = obj.gameObject;
            animateEnemy(obj);
            unusedSpawnPoses.RemoveAt(rand);
        }
    }

    public void animateEnemy(GameObject thing) {
        var target = thing.transform.position;

        thing.transform.position = new Vector3(thing.transform.position.x + 5.0f, thing.transform.position.y, 0.0f);
        thing.transform.DOMove(target, showingSpeed);
    }
}

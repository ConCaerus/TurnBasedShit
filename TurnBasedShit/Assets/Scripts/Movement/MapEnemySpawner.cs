using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEnemySpawner : MonoBehaviour {
    [SerializeField] GameObject enemyPreset;
    [SerializeField] int enemyCount = 5;

    private void Start() {
        for(int i = 0; i < enemyCount; i++)
            createEnemyAroundPlayer();
    }


    public void createEnemyAroundPlayer() {
        float minDistFromPlayer = 5.0f, maxDistFromPlayer = 10.0f;
        var pos = Map.getRandomPosInRegion((int)GameInfo.getCurrentDiff());
        while(Vector2.Distance(pos, FindObjectOfType<MapMovement>().transform.position) < minDistFromPlayer || Vector2.Distance(pos, FindObjectOfType<MapMovement>().transform.position) > maxDistFromPlayer)
            pos = Map.getRandomPosInRegion((int)GameInfo.getCurrentDiff());

        var obj = Instantiate(enemyPreset.gameObject, transform);
        obj.transform.localPosition = pos;
    }
}

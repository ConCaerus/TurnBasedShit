using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEnemySpawner : MonoBehaviour {
    [SerializeField] GameObject enemyPreset;

    private void Start() {
        for(int i = 0; i < 20; i++)
            createEnemyAroundPlayer();
    }


    public void createEnemyAroundPlayer() {
        float minDistFromPlayer = 5.0f;
        var pos = Map.getRandomPosInRegion((int)GameInfo.getCurrentDiff());
        while(Vector2.Distance(pos, FindObjectOfType<MapMovement>().transform.position) < minDistFromPlayer)
            pos = Map.getRandomPosInRegion((int)GameInfo.getCurrentDiff());

        var obj = Instantiate(enemyPreset.gameObject, transform);
        obj.transform.localPosition = pos;
    }
}

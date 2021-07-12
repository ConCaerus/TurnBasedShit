using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEnvironmentSpawner : MonoBehaviour {
    [SerializeField] GameObject mapTree;

    float treeBuffer = 0.75f, treeRandOffset = 0.25f;
    float distToClear = 0.5f;

    float clearSpeed = 1.0f;
    List<GameObject> trees = new List<GameObject>();

    private void Awake() {
        spawnTrees();
    }


    void spawnTrees() {
        for(float x = Map.leftBound; x < Map.rightBound; x+=treeBuffer) {
            for(float y = Map.botBound; y < Map.topBound; y+=treeBuffer) {
                var temp = Instantiate(mapTree, transform);

                var randOffset = new Vector3(Random.Range(-treeRandOffset, treeRandOffset), Random.Range(-treeRandOffset, treeRandOffset), 0.0f) * Mathf.PerlinNoise(x, y);

                temp.transform.localPosition = new Vector3(x, y) + randOffset;
                temp.GetComponent<SpriteRenderer>().sortingOrder = (int)(treeBuffer - (y / treeBuffer) - 30);

                trees.Add(temp.gameObject);
            }
        }
    }


    public IEnumerator clearTreesFromArea(Vector2 point) {
        foreach(var i in trees) {
            while(Vector2.Distance(point, i.transform.position) < distToClear) {
                yield return new WaitForEndOfFrame();

                i.transform.position = Vector2.MoveTowards(i.transform.position, point, -Time.deltaTime * clearSpeed);
            }
        }
    }

    public void instantlyClearTreesFromArea(Vector2 point) {
        foreach(var i in trees) {
            while(Vector2.Distance(point, i.transform.position) < distToClear)
                i.transform.position = Vector2.MoveTowards(i.transform.position, point, -Time.deltaTime * clearSpeed);
        }
    }
}

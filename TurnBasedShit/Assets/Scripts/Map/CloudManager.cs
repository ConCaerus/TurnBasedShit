using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour {
    [SerializeField] GameObject cloudPreset;
    [SerializeField] float density = 10.0f;

    List<GameObject> clouds = new List<GameObject>();


    private void Start() {
        spawnClouds();
    }

    private void Update() {
        moveClouds();
    }


    void spawnClouds() {
        for(int i = 0; i < cloudCount(); i++) {
            var temp = Instantiate(cloudPreset, transform);

            temp.transform.position = Map.getRandPos();
            temp.transform.localScale = new Vector3(cloudPreset.transform.localScale.x / transform.localScale.x, cloudPreset.transform.localScale.y / transform.localScale.y, 0.0f);
            clouds.Add(temp.gameObject);
        }
    }


    void moveClouds() {
        foreach(var i in clouds) {
        }
    }


    float cloudCount() {
        float xCount = ((Mathf.Abs(Map.leftBound) + Map.rightBound) / 2.0f) * density;
        float yCount = ((Mathf.Abs(Map.botBound) + Map.topBound) / 2.0f) * density;

        return (xCount + yCount) / 2.0f;
    }
}

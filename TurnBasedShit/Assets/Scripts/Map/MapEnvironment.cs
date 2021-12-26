using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEnvironment : MonoBehaviour {
    [SerializeField] GameObject background;
    [SerializeField] GameObject grasslandEnv, forestEnv, swampEnv, mountainEnv, hellEnv;
    [SerializeField] Color[] regionColors = new Color[5];


    private void Start() {
        background.transform.localScale = new Vector3(Map.width * 2, Map.height * 2);
        background.GetComponent<SpriteRenderer>().color = regionColors[(int)GameInfo.getCurrentRegion()];

        grasslandEnv.SetActive(false);
        forestEnv.SetActive(false);
        swampEnv.SetActive(false);
        mountainEnv.SetActive(false);
        hellEnv.SetActive(false);

        switch(GameInfo.getCurrentRegion()) {
            case GameInfo.region.grassland: grasslandEnv.SetActive(true); break;
            case GameInfo.region.forest:    forestEnv.SetActive(true); break;
            case GameInfo.region.swamp:     swampEnv.SetActive(true); break;
            case GameInfo.region.mountains: mountainEnv.SetActive(true); break;
            case GameInfo.region.hell:      hellEnv.SetActive(true); break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RegionDivider : MonoBehaviour {
    [SerializeField] GameObject regionDividerPreset;
    List<GameObject> dividers = new List<GameObject>();


    private void Start() {
        spawnDividers();
    }


    void spawnDividers() {
        for(int i = 1; i < (int)GameInfo.region.hell; i++) {
            var obj = Instantiate(regionDividerPreset.gameObject, transform.GetChild(0));
            obj.transform.position = new Vector3(Map.getRegionXStartPoint(i), 0.0f, 0.0f);

            dividers.Add(obj.gameObject);
        }
    }
}

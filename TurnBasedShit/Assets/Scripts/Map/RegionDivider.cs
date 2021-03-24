using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RegionDivider : MonoBehaviour {
    [SerializeField] float startingPoint = -400.0f, regionBuffer = 800.0f;
    [SerializeField] int numOfRegions = 6;

    [SerializeField] GameObject regionDividerPreset;
    List<GameObject> dividers = new List<GameObject>();

    private void Awake() {
        createDividers();
    }

    private void Update() {
        moveDividersAlongY();
    }


    void createDividers() {
        dividers.Clear();

        for(int i = 0; i < numOfRegions; i++) {
            var obj = Instantiate(regionDividerPreset.gameObject);
            obj.transform.SetParent(transform);
            obj.transform.position = new Vector3(startingPoint + (i * regionBuffer), 0.0f, transform.position.z);
            obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            obj.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();

            dividers.Add(obj.gameObject);
        }
    }

    void moveDividersAlongY() {
        foreach(var i in dividers) {
            float newY = Camera.main.transform.position.y -(6 - Camera.main.orthographicSize);
            i.transform.position = new Vector3(i.transform.position.x, newY, i.transform.position.z);
            i.GetComponentInChildren<TextMeshProUGUI>().fontSize = Mathf.Clamp(23 * -(4 - Camera.main.orthographicSize), 0.25f, 75.0f);
        }
    }


    public int getRelevantRegionLevel(float x) {
        var relevant = dividers[0];
        foreach(var i in dividers) {
            if(x > i.transform.position.x)
                relevant = i;
            else
                return int.Parse(relevant.GetComponentInChildren<TextMeshProUGUI>().text);
        }

        return int.Parse(relevant.GetComponentInChildren<TextMeshProUGUI>().text);
    }
    public CombatLocation.diffLevel getRelevantDifficultyLevel(float x) {
        return (CombatLocation.diffLevel)getRelevantRegionLevel(x);
    }
}

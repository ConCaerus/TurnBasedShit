using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SummonSpotSpawner : MonoBehaviour {
    [SerializeField] GameObject spotPreset;
    [SerializeField] Vector2 centerOffset;

    List<GameObject> spawnedSpots = new List<GameObject>();

    [SerializeField] float spotOffset = 1.75f;

    int lastKnownUnitCount = 0;

    private void Start() {
        StartCoroutine(FindObjectOfType<TransitionCanvas>().runAfterLoading(delegate { updateSpots(); }));
    }

    private void Update() {
        if(lastKnownUnitCount != FindObjectsOfType<UnitClass>().Length) {
            updateSpots();
            lastKnownUnitCount = FindObjectsOfType<UnitClass>().Length;
        }
    }

    public void updateSpots() {
        var temp = new List<GameObject>();
        foreach(var i in spawnedSpots) {
            if(i.transform.parent.GetComponent<CombatSpot>().unit == null) {
                hideSpot(i.gameObject);
                continue;
            }
            var unit = i.transform.parent.GetComponent<CombatSpot>().unit.GetComponent<UnitClass>();
            if(unit == null || unit.stats.weapon == null || unit.stats.weapon.isEmpty() || unit.stats.weapon.sUsage != Weapon.specialUsage.summoning || i.transform.parent.childCount != unit.stats.getSummonedLevel())
                hideSpot(i.gameObject);
            else
                temp.Add(i.gameObject);
        }

        spawnedSpots.Clear();
        foreach(var i in temp)
            spawnedSpots.Add(i.gameObject);

        List<CombatSpot> spots = new List<CombatSpot>();
        foreach(var i in FindObjectsOfType<CombatSpot>()) {
            if(!i.isPlayerSpot() || i.unit == null)
                continue;
            var stats = i.unit.GetComponent<UnitClass>().stats;
            if(stats.weapon != null && !stats.weapon.isEmpty() && stats.weapon.sUsage == Weapon.specialUsage.summoning)
                spots.Add(i);
        }

        foreach(var i in spots) {
            var stats = i.unit.GetComponent<UnitClass>().stats;

            if(i.transform.childCount == stats.getSummonedLevel())
                continue;

            float rot = 360.0f / stats.getSummonedLevel();
            for(int s = 0; s < stats.getSummonedLevel(); s++) {
                var holder = new GameObject("Holder");
                holder.transform.SetParent(i.gameObject.transform);
                holder.transform.localPosition = centerOffset;
                holder.transform.localScale = new Vector3(1.0f, 1.0f);

                var obj = showSpot(Instantiate(spotPreset.gameObject, holder.transform));

                holder.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, rot * s);
                obj.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -rot * s);
                spawnedSpots.Add(holder.gameObject);
            }
        }
    }

    public GameObject getCombatSpotAtIndexForUnit(GameObject unit, int index) {
        foreach(var i in FindObjectsOfType<CombatSpot>()) {
            if(i.unit == unit) {
                return i.transform.GetChild(index).gameObject;
            }
        }
        return null;
    }

    GameObject showSpot(GameObject obj) {
        float speed = 0.25f;
        obj.GetComponent<SpriteRenderer>().sortingOrder -= 1;
        obj.GetComponent<CombatSpot>().setColor();
        obj.transform.localScale = new Vector3(0.0f, 0.0f);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.DOScale(0.75f, speed);
        obj.transform.DOLocalMoveX(spotOffset, speed);

        return obj;
    }

    void hideSpot(GameObject obj) {
        float speed = 0.25f;
        obj.transform.GetChild(0).transform.DOLocalMove(new Vector3(0.0f, 0.0f), speed);
        obj.transform.GetChild(0).transform.DOScale(0.0f, speed);
        Destroy(obj.gameObject, speed);

        if(obj.GetComponentInChildren<CombatSpot>().unit != null)
            obj.GetComponentInChildren<CombatSpot>().unit.GetComponent<UnitClass>().die(DeathInfo.killCause.murdered);
    }
}

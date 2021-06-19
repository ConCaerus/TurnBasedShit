using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitCombatInformationCanvas : MonoBehaviour {
    [SerializeField] Slider healthSliderPreset;
    [SerializeField] TextMeshProUGUI levelPreset;
    [SerializeField] TextMeshProUGUI bleedCountPreset;

    public Vector2 healthOffset;
    public Vector2 levelOffset;
    public Vector2 bleedOffset;

    List<info> activeInfos = new List<info>();

    struct info {
        public GameObject unit;

        public string name;
        public Slider healthSlider;
        public TextMeshProUGUI level;
        public TextMeshProUGUI bleedCount;

        public void destory() {
            Destroy(healthSlider.gameObject);
            Destroy(level.gameObject);
            Destroy(bleedCount.gameObject);
            Destroy(GameObject.Find(name));
        }

        public bool update() {
            if(unit == null)
                return false;
            healthSlider.maxValue = unit.GetComponent<UnitClass>().stats.getModifiedMaxHealth();
            healthSlider.value = unit.GetComponent<UnitClass>().stats.u_health;
            healthSlider.transform.position = (Vector2)unit.transform.position + FindObjectOfType<UnitCombatInformationCanvas>().healthOffset;

            level.text = unit.GetComponent<UnitClass>().stats.u_level.ToString();
            level.transform.position = (Vector2)unit.transform.position + FindObjectOfType<UnitCombatInformationCanvas>().levelOffset;

            if(unit.GetComponent<UnitClass>().stats.u_bleedCount <= 0)
                bleedCount.enabled = false;
            else {
                bleedCount.enabled = true;
                bleedCount.text = unit.GetComponent<UnitClass>().stats.u_bleedCount.ToString();
                bleedCount.transform.position = (Vector2)unit.transform.position + FindObjectOfType<UnitCombatInformationCanvas>().bleedOffset;
            }

            return true;
        }
    }

    private void Start() {
        StartCoroutine(FindObjectOfType<TransitionCanvas>().runAfterLoading(createInfos));
    }

    private void Update() {
        updateInfos();
    }


    public void removeInfoForUnit(GameObject unit) {
        foreach(var i in activeInfos) {
            if(i.unit == unit) {
                activeInfos.Remove(i);
                i.destory();
                return;
            }
        }
    }

    void clearInfos() {
        foreach(var i in activeInfos) {
            i.destory();
        }
        activeInfos.Clear();
    }

    void createInfos() {
        clearInfos();
        foreach(var i in FindObjectsOfType<UnitClass>()) {
            createNewInfoForUnit(i.gameObject);
        }
    }

    void updateInfos() {
        List<info> useless = new List<info>();
        useless.Clear();
        foreach(var i in activeInfos) {
            if(!i.update()) {
                useless.Add(i);
            }
        }

        foreach(var i in useless) {
            activeInfos.Remove(i);
            i.destory();
        }
    }


    info createNewInfoForUnit(GameObject unit) {
        var temp = new info();
        temp.unit = unit;

        var holder = new GameObject("Info #" + activeInfos.Count.ToString());
        temp.name = "Info #" + activeInfos.Count.ToString();
        holder.transform.SetParent(transform);
        holder.transform.localScale = Vector3.one;

        temp.healthSlider = Instantiate(healthSliderPreset, holder.transform);
        temp.healthSlider.maxValue = unit.GetComponent<UnitClass>().stats.getModifiedMaxHealth();
        temp.healthSlider.value = unit.GetComponent<UnitClass>().stats.u_health;
        temp.healthSlider.transform.position = (Vector2)unit.transform.position + healthOffset;

        temp.level = Instantiate(levelPreset, holder.transform);
        temp.level.text = unit.GetComponent<UnitClass>().stats.u_level.ToString();
        temp.level.transform.position = (Vector2)unit.transform.position + levelOffset;

        temp.bleedCount = Instantiate(bleedCountPreset, holder.transform);
        temp.bleedCount.text = unit.GetComponent<UnitClass>().stats.u_bleedCount.ToString();
        temp.bleedCount.transform.position = (Vector2)unit.transform.position + bleedOffset;

        activeInfos.Add(temp);

        return temp;
    }
}

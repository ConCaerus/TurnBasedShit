using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarCanvas : MonoBehaviour {
    [SerializeField] Slider barPrefab;

    struct healthBar {
        public GameObject unit;
        public Slider bar;


        public void update() {
            bar.value = Mathf.Lerp(bar.value, unit.GetComponent<UnitClass>().stats.u_health, 50.0f * Time.deltaTime);
        }

        public void setMaxBarValue() {
            bar.maxValue = unit.GetComponent<UnitClass>().stats.u_maxHealth;
        }

        public void destroyHealthBar() {
            var temp = bar;
            bar = null;
            Destroy(temp.gameObject);

        }
    }


    List<healthBar> healthBars = new List<healthBar>();


    private void Start() {
        createHealthBars();
    }

    private void Update() {
        foreach(var i in healthBars)
            i.update();
    }


    void createHealthBars() {
        foreach(var i in FindObjectsOfType<UnitClass>()) {
            var bar = Instantiate(barPrefab, transform);
            var yOffset = new Vector3(0.0f, i.gameObject.transform.localScale.y / 1.0f, 0.0f);
            var target = i.gameObject.transform.position + yOffset;
            bar.transform.position = new Vector3(target.x, target.y, bar.transform.position.z);
            bar.maxValue = i.stats.u_maxHealth;
            bar.value = i.stats.u_health;
            var temp = new healthBar();
            temp.unit = i.gameObject;
            temp.bar = bar;
            healthBars.Add(temp);
        }
    }

    public void destroyHealthBarForUnit(GameObject u) {
        foreach(healthBar i in healthBars.ToArray()) {
            if(i.unit == u) {
                healthBars.Remove(i);
                i.destroyHealthBar();
            }
        }
    }
}

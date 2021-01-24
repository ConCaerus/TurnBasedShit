using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour {
    List<UnitClassStats> unitsInParty = new List<UnitClassStats>();

    [SerializeField] GameObject goodUnitPreset;


    public void addUnitToParty(GameObject unit) {
        unitsInParty.Add(unit.GetComponent<UnitClass>().stats);
    }

    public void resetUnitsInParty() {
        unitsInParty.Clear();
        int index = 0;
        foreach(var i in FindObjectsOfType<PlayerUnitInstance>()) {
            unitsInParty.Add(i.stats);
            i.setNewOrderNumber(index);
            index++;
        }
    }

    public void removeUnitFromParty(GameObject unit) {
        unitsInParty.Remove(unit.GetComponent<UnitClass>().stats);
    }


    public void setUnitOrder() {
        for(int i = 0; i < unitsInParty.Count; i++) {
            unitsInParty[i].u_order = i;
        }
    }


    public void instantiateUnitsInParty() {
        if(unitsInParty.Count > 0) {
            for(int i = 0; i < unitsInParty.Count; i++) {
                var temp = Instantiate(goodUnitPreset);
                temp.GetComponent<UnitClass>().stats = unitsInParty[i];

                //  set random pos
                var randX = Random.Range(-8.0f, -3.0f);
                var randY = Random.Range(-3.0f, 3.0f);
                temp.transform.position = new Vector2(randX, randY);
            }
        }
    }
    

    public void saveParty() {
        resetUnitsInParty();
        for(int i = 0; i < unitsInParty.Count; i++) {
            var data = JsonUtility.ToJson(unitsInParty[i]);
            PlayerPrefs.SetString("Party" + i.ToString(), data);
        }

        PlayerPrefs.SetInt("PartySize", unitsInParty.Count);
    }

    public void loadParty() {
        if(PlayerPrefs.GetInt("PartySize") > 0) {
            for(int i = 0; i < PlayerPrefs.GetInt("PartySize"); i++) {
                var data = PlayerPrefs.GetString("Party" + i.ToString());
                unitsInParty.Add(JsonUtility.FromJson<UnitClassStats>(data));
            }
        }
    }
}

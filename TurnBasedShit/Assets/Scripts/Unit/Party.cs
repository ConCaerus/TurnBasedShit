using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour {
    List<UnitClassStats> unitsInParty = new List<UnitClassStats>();

    [SerializeField] GameObject goodUnitPreset;


    public void addUnitToParty(GameObject unit) {
        unitsInParty.Add(unit.GetComponent<UnitClass>().stats);
        saveUnit(unit.GetComponent<UnitClass>().stats);
    }

    public void resetUnitsInParty() {
        unitsInParty.Clear();
        int index = 0;
        foreach(var i in FindObjectsOfType<PlayerUnitInstance>()) {
            unitsInParty.Add(i.stats);
            i.stats.u_order = index;
            i.resetSpriteRenderer();
            index++;
        }
    }

    public void removeUnitFromParty(GameObject unit) {
        unitsInParty.Remove(unit.GetComponent<UnitClass>().stats);
        clearUnitSaveData(unit.GetComponent<UnitClass>().stats);
    }


    public void setUnitOrder() {
        for(int i = 0; i < unitsInParty.Count; i++) {
            unitsInParty[i].u_order = i;
        }
    }


    public void instantiateUnitsInParty() {
        if(unitsInParty.Count > 0) {
            for(int i = 0; i < unitsInParty.Count; i++) {
                GameObject temp = null;
                if(FindObjectsOfType<PlayerUnitInstance>().Length < i) {
                    temp = Instantiate(goodUnitPreset);
                }
                else
                    temp = FindObjectsOfType<PlayerUnitInstance>()[i].gameObject;
                temp.GetComponent<UnitClass>().stats = unitsInParty[i];
                temp.GetComponent<UnitClass>().setup();

                //  set random pos
                var randX = Random.Range(-8.0f, -3.0f);
                var randY = Random.Range(-2.0f, 2.0f);
                temp.transform.position = new Vector2(randX, randY);
            }
        }
    }


    public void saveParty() {
        for(int i = 0; i < unitsInParty.Count; i++) {
            var data = JsonUtility.ToJson(unitsInParty[i]);
            PlayerPrefs.SetString("Party" + i.ToString(), data);
        }

        PlayerPrefs.SetInt("PartySize", unitsInParty.Count);
        PlayerPrefs.Save();
    }

    public void loadParty() {
        if(PlayerPrefs.GetInt("PartySize") > 0) {
            for(int i = 0; i < PlayerPrefs.GetInt("PartySize"); i++) {
                var data = PlayerPrefs.GetString("Party" + i.ToString());
                unitsInParty.Add(JsonUtility.FromJson<UnitClassStats>(data));
                PlayerPrefs.Save();
            }
        }
    }


    public void clearUnitSaveData(UnitClassStats stats) {
        for(int i = 0; i < unitsInParty.Count; i++) {
            if(unitsInParty[i] == stats) {
                PlayerPrefs.DeleteKey("Party" + i.ToString());
                PlayerPrefs.Save();
                break;
            }
        }
    }

    public void resaveUnitSaveData(UnitClassStats stats) {
        for(int i = 0; i < unitsInParty.Count; i++) {
            if(unitsInParty[i] == stats) {
                var data = JsonUtility.ToJson(stats);
                PlayerPrefs.SetString("Party" + i.ToString(), data);
                PlayerPrefs.Save();
            }
        }
    }

    public void saveUnit(int index, UnitClassStats stats) {
        var data = JsonUtility.ToJson(stats);
        PlayerPrefs.SetString("Party" + index.ToString(), data);
        PlayerPrefs.Save();
    }
    public void saveUnit(UnitClassStats stats) {
        var data = JsonUtility.ToJson(stats);
        PlayerPrefs.SetString("Party" + stats.u_order.ToString(), data);
        PlayerPrefs.Save();
    }


    public int getPartyCount() {
        return unitsInParty.Count;
    }

    public GameObject getPartyMember(int index) {
        foreach(var i in FindObjectsOfType<PlayerUnitInstance>()) {
            if(i.stats.u_order == index)
                return i.gameObject;
        }
        return null;
    }
}

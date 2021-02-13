using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyObject : MonoBehaviour {
    [SerializeField] GameObject unitPrefab;
    [SerializeField] List<Vector2> spawnPoses;

    [SerializeField] GameObject unitToAdd;


    public void instantiatePartyMembers() {
        //  Deletes all existing objects
        foreach(var i in FindObjectsOfType<PlayerUnitInstance>()) {
            i.die();
        }

        List<Vector2> unusedSpawnPoses = spawnPoses;
        for(int i = 0; i < Party.getPartySize(); i++) {
            var obj = Instantiate(unitPrefab);

            //  sets up stats
            obj.GetComponent<UnitClass>().stats = Party.getMemberStats(i);
            obj.GetComponent<UnitClass>().setup();

            //  sets sprite
            obj.GetComponent<SpriteRenderer>().sprite = obj.GetComponent<UnitClass>().stats.u_sprite.getSprite();
            obj.GetComponent<SpriteRenderer>().color = obj.GetComponent<UnitClass>().stats.u_color;

            //  sets pos
            int randIndex = Random.Range(0, unusedSpawnPoses.Count);
            obj.transform.position = unusedSpawnPoses[randIndex];
            unusedSpawnPoses.RemoveAt(randIndex);

            //  does other shit
            FindObjectOfType<HealthBarCanvas>().createHealthBar(obj.GetComponent<UnitClass>());
        }

        FindObjectOfType<TurnOrderSorter>().resetList();
    }

    public void addUnitToAdd() {
        if(unitToAdd != null) {
            unitToAdd.GetComponent<UnitClass>().setEquipment();
            Party.addNewUnit(unitToAdd.GetComponent<UnitClass>().stats);
        }
        unitToAdd = null;
    }

    public void saveParty() {
        foreach(var i in FindObjectsOfType<PlayerUnitInstance>()) {
            Party.resaveUnit(i.stats);
        }
    }
}


//  Actual Party Script
public static class Party {
    const string partySizeTag = "PartySize";
    static string memberTag(int index) { return "Party" + index.ToString(); }


    public static void clearParty() {
        for(int i = 0; i < PlayerPrefs.GetInt(partySizeTag); i++) {
            PlayerPrefs.DeleteKey(memberTag(i));
        }
        PlayerPrefs.DeleteKey(partySizeTag);

        PlayerPrefs.Save();
    }
    public static void clearPartyEquipment() {
        for(int i = 0; i < PlayerPrefs.GetInt(partySizeTag); i++) {
            var data = PlayerPrefs.GetString(memberTag(i));
            var stats = JsonUtility.FromJson<UnitClassStats>(data);

            stats.equippedWeapon.resetWeaponStats();
            stats.equippedArmor.resetArmorStats();
            resaveUnit(stats);
        }
    }

    //  you left off needing to add that this function sets a new order for the unit.
    public static void addNewUnit(UnitClassStats stats) {
        stats.u_order = PlayerPrefs.GetInt(partySizeTag);

        int index = PlayerPrefs.GetInt(partySizeTag);
        var data = JsonUtility.ToJson(stats);

        PlayerPrefs.SetString(memberTag(index), data);
        PlayerPrefs.SetInt(partySizeTag, index + 1);

        PlayerPrefs.Save();
    }
    public static void removeUnit(UnitClassStats stats) {
        for(int i = 0; i < PlayerPrefs.GetInt(partySizeTag); i++) {
            var data = PlayerPrefs.GetString(memberTag(i));
            var temp = JsonUtility.FromJson<UnitClassStats>(data);

            if(temp == stats) {
                PlayerPrefs.DeleteKey(memberTag(i));
                PlayerPrefs.SetInt(partySizeTag, PlayerPrefs.GetInt(partySizeTag) - 1);
                PlayerPrefs.Save();
                return;
            }
        }
    }

    public static void resaveUnit(UnitClassStats stats) {
        var data = JsonUtility.ToJson(stats);
        PlayerPrefs.SetString(memberTag(stats.u_order), data);
        PlayerPrefs.Save();
    }


    public static int getPartySize() {
        return PlayerPrefs.GetInt(partySizeTag);
    }
    public static UnitClassStats getMemberStats(int index) {
        var data = PlayerPrefs.GetString(memberTag(index));
        return JsonUtility.FromJson<UnitClassStats>(data);
    }
}

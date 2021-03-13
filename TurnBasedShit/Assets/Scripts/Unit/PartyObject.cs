using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyObject : MonoBehaviour {
    [SerializeField] GameObject unitPrefab;
    [SerializeField] List<GameObject> spawnPoses;

    [SerializeField] GameObject unitToAdd;


    public void instantiatePartyMembers() {
        //  Deletes all existing objects
        foreach(var i in FindObjectsOfType<PlayerUnitInstance>()) {
            i.die();
        }

        List<GameObject> unusedSpawnPoses = spawnPoses;
        for(int i = 0; i < Party.getPartySize(); i++) {
            var obj = Instantiate(unitPrefab);

            //  sets up stats
            obj.GetComponent<UnitClass>().stats = Party.getMemberStats(i);
            if(string.IsNullOrEmpty(obj.GetComponent<UnitClass>().stats.u_name)) {
                obj.GetComponent<UnitClass>().stats.u_name = NameLibrary.getRandomUsableName();
            }
            obj.name = Party.getMemberStats(i).u_name;

            //  sets sprite
            obj.GetComponent<SpriteRenderer>().sprite = Party.getMemberStats(i).u_sprite.getSprite();
            obj.GetComponent<SpriteRenderer>().color = Party.getMemberStats(i).u_color;

            //  sets pos
            int randIndex = Random.Range(0, unusedSpawnPoses.Count);
            obj.transform.position = unusedSpawnPoses[randIndex].transform.position;
            unusedSpawnPoses.RemoveAt(randIndex);

            //  does other shit
            FindObjectOfType<HealthBarCanvas>().createHealthBar(obj.GetComponent<UnitClass>());

            Party.resaveUnit(obj.GetComponent<UnitClass>().stats);
        }

        FindObjectOfType<TurnOrderSorter>().resetList();
    }

    public void addUnitToAdd() {
        if(unitToAdd != null) {
            unitToAdd.GetComponent<UnitClass>().setEquipment();
            unitToAdd.GetComponent<UnitClass>().resetSpriteAndColor();
            Party.addNewUnit(unitToAdd.GetComponent<UnitClass>().stats);
            saveParty();
        }
        unitToAdd = null;
    }

    public void resaveInstantiatedUnit(UnitClassStats stats) {
        Party.resaveUnit(stats);

        foreach(var i in FindObjectsOfType<PlayerUnitInstance>()) {
            if(i.stats.u_order == stats.u_order) {
                i.stats = stats;
                i.name = stats.u_name;
                break;
            }
        }
    }

    public void saveParty() {
        foreach(var i in FindObjectsOfType<PlayerUnitInstance>()) {
            Party.resaveUnit(i.stats);
        }
    }

    public GameObject getInstantiatedMember(UnitClassStats stats) {
        foreach(var i in FindObjectsOfType<PlayerUnitInstance>()) {
            if(stats.u_order == i.stats.u_order)
                return i.gameObject;
        }
        return null;
    }
}


//  Actual Party Script
public static class Party {
    const string partySizeTag = "PartySize";
    static string memberTag(int index) { return "Party" + index.ToString(); }


    public static void clearParty() {
        for(int i = 0; i < SaveData.getInt(partySizeTag); i++) {
            SaveData.deleteKey(memberTag(i));
        }
        SaveData.deleteKey(partySizeTag);

        SaveData.save();
    }
    public static void clearPartyEquipment() {
        for(int i = 0; i < SaveData.getInt(partySizeTag); i++) {
            var data = SaveData.getString(memberTag(i));
            var stats = JsonUtility.FromJson<UnitClassStats>(data);

            stats.equippedWeapon.resetWeaponStats();
            stats.equippedArmor.resetArmorStats();
            resaveUnit(stats);
        }
    }

    public static void addNewUnit(UnitClassStats stats) {
        stats.u_order = SaveData.getInt(partySizeTag);

        int index = SaveData.getInt(partySizeTag);
        var data = JsonUtility.ToJson(stats);

        SaveData.setString(memberTag(index), data);
        SaveData.setInt(partySizeTag, index + 1);

        SaveData.save();
    }
    public static void removeUnit(UnitClassStats stats) {
        int index = 0;
        bool shrinkCount = false;

        for(int i = 0; i < SaveData.getInt(partySizeTag); i++) {
            var data = SaveData.getString(memberTag(i));
            var temp = JsonUtility.FromJson<UnitClassStats>(data);

            //  remove this unit
            if(temp.equals(stats)) {
                shrinkCount = true;
            }

            //  else set new order for the unit
            else {
                temp.u_order = index;
                data = JsonUtility.ToJson(temp);
                SaveData.setString(memberTag(index), data);
                index++;
            }
        }
        if(shrinkCount) {
            SaveData.deleteKey(memberTag(getPartySize() - 1));
            SaveData.setInt(partySizeTag, SaveData.getInt(partySizeTag) - 1);
        }
        SaveData.save();
    }
    public static void removeUnitAtIndex(int order) {
        removeUnit(getMemberStats(order));
    }

    public static void resaveUnit(UnitClassStats stats) {
        var data = JsonUtility.ToJson(stats);
        SaveData.setString(memberTag(stats.u_order), data);
        SaveData.save();
    }


    public static int getPartySize() {
        return SaveData.getInt(partySizeTag);
    }
    public static UnitClassStats getMemberStats(int index) {
        var data = SaveData.getString(memberTag(index));
        return JsonUtility.FromJson<UnitClassStats>(data);
    }
}

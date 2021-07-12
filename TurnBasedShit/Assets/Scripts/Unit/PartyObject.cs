using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyObject : MonoBehaviour {
    public void instantiatePartyMembers() {
        //  Deletes all existing objects
        for(int i = 0; i < FindObjectsOfType<PlayerUnitInstance>().Length; i++)
            Destroy(FindObjectsOfType<PlayerUnitInstance>()[i].gameObject);

        List<GameObject> unusedSpawnPoses = new List<GameObject>();
        foreach(var i in FindObjectsOfType<CombatSpot>()) {
            if(i.isPlayerSpot())
                unusedSpawnPoses.Add(i.gameObject);
        }

        for(int i = 0; i < Party.getMemberCount(); i++) {
            var obj = Instantiate(FindObjectOfType<PresetLibrary>().getPlayerUnit());
            if(Party.getMemberStats(i) == null || Party.getMemberStats(i).isEmpty()) {
                obj.gameObject.GetComponent<UnitClass>().setEquipment();
                Party.addUnitAtIndex(i, obj.gameObject.GetComponent<UnitClass>().stats);
            }

            //  sets up stats
            obj.GetComponent<UnitClass>().stats = Party.getMemberStats(i);
            obj.GetComponent<UnitClass>().setup();

            //  adds a trait if traits are empty
            if(Party.getMemberStats(i).u_traits.Count == 0)
                obj.GetComponent<UnitClass>().stats.u_traits.Add(FindObjectOfType<PresetLibrary>().getRandomUnitTrait());


            //  sets sprite
            obj.GetComponent<SpriteRenderer>().color = Party.getMemberStats(i).u_color;

            //  sets pos
            int randIndex = Random.Range(0, unusedSpawnPoses.Count);
            obj.transform.position = unusedSpawnPoses[randIndex].transform.position;
            unusedSpawnPoses.RemoveAt(randIndex);

            Party.overrideUnit(obj.GetComponent<UnitClass>().stats);
        }
    }

    public void resaveInstantiatedUnit(UnitStats stats) {
        Party.overrideUnit(stats);

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
            Party.overrideUnit(i.stats);
        }
    }

    public GameObject getInstantiatedMember(UnitStats stats) {
        foreach(var i in FindObjectsOfType<PlayerUnitInstance>()) {
            if(stats.u_order == i.stats.u_order)
                return i.gameObject;
        }
        return null;
    }
}


//  Actual Party Script
public static class Party {
    public const string partySizeTag = "PartySize";
    public const string partyLeaderTag = "PartyLeaderID";
    public static string memberTag(int index) { return "Party" + index.ToString(); }



    public static void createDefaultParty() {
        addNewUnit(new UnitStats());
        addNewUnit(new UnitStats());

        SaveData.setInt(partySizeTag, 2);
    }

    public static void clearParty() {
        for(int i = 0; i < SaveData.getInt(partySizeTag); i++) {
            SaveData.deleteKey(memberTag(i));
        }
        SaveData.deleteKey(partySizeTag);
    }
    public static void clearPartyEquipment() {
        for(int i = 0; i < SaveData.getInt(partySizeTag); i++) {
            var data = SaveData.getString(memberTag(i));
            var stats = JsonUtility.FromJson<UnitStats>(data);

            stats.equippedWeapon.resetWeaponStats();
            stats.equippedArmor.resetArmorStats();
            overrideUnit(stats);
        }
    }

    public static void addNewUnit(UnitStats stats) {
        stats.u_order = SaveData.getInt(partySizeTag);

        int index = SaveData.getInt(partySizeTag);
        var data = JsonUtility.ToJson(stats);

        SaveData.setString(memberTag(index), data);
        SaveData.setInt(partySizeTag, index + 1);
    }
    public static void addUnitAtIndex(int index, UnitStats stats) {
        stats.u_order = index;

        var data = JsonUtility.ToJson(stats);

        SaveData.setString(memberTag(index), data);
    }
    public static void setLeader(UnitStats stats) {
        SaveData.setInt(partyLeaderTag, stats.u_order);
    }
    public static void removeUnit(UnitStats stats) {
        var tData = JsonUtility.ToJson(stats);
        bool past = false;
        for(int i = 0; i < SaveData.getInt(partySizeTag); i++) {
            var data = SaveData.getString(memberTag(i));

            if(data == tData && !past) {
                SaveData.deleteKey(memberTag(i));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(memberTag(i));
                overrideUnit(i - 1, JsonUtility.FromJson<UnitStats>(data));
            }
        }
        SaveData.setInt(partySizeTag, SaveData.getInt(partySizeTag) - 1);
    }
    public static void removeUnit(int order) {
        removeUnit(getMemberStats(order));
    }
    public static void removeLeader() {
        SaveData.deleteKey(partyLeaderTag);
    }

    public static void overrideUnit(UnitStats stats) {
        var data = JsonUtility.ToJson(stats);
        SaveData.setString(memberTag(stats.u_order), data);
    }
    public static void overrideUnit(int i, UnitStats stats) {
        var data = JsonUtility.ToJson(stats);
        SaveData.setString(memberTag(i), data);
    }


    public static int getMemberCount() {
        return SaveData.getInt(partySizeTag);
    }
    public static int getLeaderID() {
        return SaveData.getInt(partyLeaderTag);
    }
    public static int getUnitIndex(UnitStats stats) {
        for(int i = 0; i < getMemberCount(); i++) {
            if(getMemberStats(i) == stats)
                return i;
        }
        return -1;
    }
    public static UnitStats getMemberStats(int index) {
        var data = SaveData.getString(memberTag(index));
        return JsonUtility.FromJson<UnitStats>(data);
    }
    public static UnitStats getLeaderStats() {
        int leaderID = SaveData.getInt(partyLeaderTag);
        return getMemberStats(leaderID);
    }
}

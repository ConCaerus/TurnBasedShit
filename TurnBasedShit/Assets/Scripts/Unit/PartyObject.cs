using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyObject : MonoBehaviour {
    float unitSpotXOffset = -0.05f;
    float unitSpotYOffset = -0.5f;

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
            var obj = Instantiate(FindObjectOfType<PresetLibrary>().getPlayerUnitObject());
            obj.GetComponent<UnitClass>().stats = FindObjectOfType<PresetLibrary>().createRandomPlayerUnitStats(false);

            if(Party.getMemberStats(i) == null || Party.getMemberStats(i).isEmpty()) {
                obj.gameObject.GetComponent<UnitClass>().setEquipment();
                Party.addUnitAtIndex(i, obj.gameObject.GetComponent<UnitClass>().stats);
            }

            //  sets up stats
            obj.GetComponent<UnitClass>().stats = Party.getMemberStats(i);
            obj.GetComponent<UnitClass>().setup();

            //  adds a trait if traits are empty
            if(Party.getMemberStats(i).u_traits.Count == 0)
                obj.GetComponent<UnitClass>().stats.u_traits.Add(FindObjectOfType<PresetLibrary>().getRandomUnusedUnitTrait(obj.GetComponent<UnitClass>().stats));


            //  sets sprite
            obj.GetComponentInChildren<UnitSpriteHandler>().setEverything(obj.GetComponent<UnitClass>().stats.u_sprite, obj.GetComponent<UnitClass>().stats.equippedWeapon, obj.GetComponent<UnitClass>().stats.equippedArmor);

            //  sets pos
            int randIndex = Random.Range(0, unusedSpawnPoses.Count);
            obj.transform.position = unusedSpawnPoses[randIndex].transform.position + new Vector3(unitSpotXOffset, obj.GetComponentInChildren<UnitSpriteHandler>().getHeight() + unitSpotYOffset, 0.0f);
            unusedSpawnPoses[randIndex].GetComponent<CombatSpot>().unit = obj.gameObject;
            unusedSpawnPoses.RemoveAt(randIndex);

            Party.overrideUnit(obj.GetComponent<UnitClass>().stats);
        }
    }

    public void resaveInstantiatedUnit(UnitStats stats) {
        Party.overrideUnit(stats);

        foreach(var i in FindObjectsOfType<PlayerUnitInstance>()) {
            if(i.stats.isEqualTo(stats)) {
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
            if(stats.isEqualTo(i.stats))
                return i.gameObject;
        }
        return null;
    }


    public void repositionUnit(GameObject unit) {
        if(unit == null)
            return;
        float closestDist = 100.0f;
        GameObject spot = null;
        foreach(var i in FindObjectsOfType<CombatSpot>()) {
            if(Vector2.Distance(unit.transform.position, i.transform.position) < closestDist) {
                closestDist = Vector2.Distance(unit.transform.position, i.transform.position);
                spot = i.gameObject;
            }
        }


        unit.transform.position = spot.transform.position + new Vector3(unitSpotXOffset, unit.GetComponentInChildren<UnitSpriteHandler>().getHeight() + unitSpotYOffset, 0.0f);
    }
}


//  Actual Party Script
public static class Party {
    public const string partySizeTag = "PartySize";
    public const string partyLeaderTag = "PartyLeader";
    public static string memberTag(int index) { return "Party" + index.ToString(); }



    public static void createDefaultParty(PresetLibrary lib) {
        addUnit(new UnitStats());
        addUnit(new UnitStats());

        SaveData.setInt(partySizeTag, 2);
    }

    public static void clearParty(bool resetInstanceQueue) {
        for(int i = 0; i < SaveData.getInt(partySizeTag); i++) {
            SaveData.deleteKey(memberTag(i));
        }
        SaveData.deleteKey(partySizeTag);

        if(resetInstanceQueue)
            GameInfo.clearUnitInstanceIDQueue();
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

    public static void addUnit(UnitStats stats) {
        int index = SaveData.getInt(partySizeTag);
        var data = JsonUtility.ToJson(stats);

        SaveData.setString(memberTag(index), data);
        SaveData.setInt(partySizeTag, index + 1);
    }
    public static void addUnitAtIndex(int index, UnitStats stats) {
        var data = JsonUtility.ToJson(stats);
        SaveData.setString(memberTag(index), data);
    }
    public static void setLeader(UnitStats stats) {
        int index = getUnitIndex(stats);
        if(index == -1)
            return;
        SaveData.setInt(partyLeaderTag, index);
    }
    public static void removeUnit(UnitStats stats) {
        if(stats == null || stats.isEmpty())
            return;
        List<UnitStats> temp = new List<UnitStats>();
        for(int i = 0; i < getMemberCount(); i++) {
            UnitStats mem = getMemberStats(i);
            if(mem != null && !mem.isEmpty() && !mem.isEqualTo(stats))
                temp.Add(mem);
        }

        clearParty(false);
        foreach(var i in temp)
            addUnit(i);
    }
    public static void removeUnit(int order) {
        removeUnit(getMemberStats(order));
    }

    public static void overrideUnit(UnitStats stats) {
        int index = getUnitIndex(stats);
        if(index == -1)
            return;
        var data = JsonUtility.ToJson(stats);
        SaveData.setString(memberTag(index), data);
    }
    public static void overrideUnit(int i, UnitStats stats) {
        var data = JsonUtility.ToJson(stats);
        SaveData.setString(memberTag(i), data);
    }


    public static int getMemberCount() {
        return SaveData.getInt(partySizeTag);
    }
    public static int getUnitIndex(UnitStats stats) {
        for(int i = 0; i < getMemberCount(); i++) {
            if(getMemberStats(i).isEqualTo(stats))
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
        var temp = getMemberStats(leaderID);

        //  if no leader, set leader to first member and return
        if(temp == null || temp.isEmpty()) {
            setLeader(getMemberStats(0));
            return getMemberStats(0);
        }
        return getMemberStats(leaderID);
    }
}

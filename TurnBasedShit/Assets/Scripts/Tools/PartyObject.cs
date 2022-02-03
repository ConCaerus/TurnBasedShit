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

        for(int i = 0; i < Party.getHolder().getObjectCount<UnitStats>(); i++) {
            var obj = Instantiate(FindObjectOfType<PresetLibrary>().getPlayerUnitObject());
            obj.GetComponent<UnitClass>().stats = FindObjectOfType<PresetLibrary>().createRandomPlayerUnitStats(false);

            //  assigns spot
            int randIndex = Random.Range(0, unusedSpawnPoses.Count);
            unusedSpawnPoses[randIndex].GetComponent<CombatSpot>().unit = obj.gameObject;
            unusedSpawnPoses[randIndex].GetComponent<CombatSpot>().setColor();
            unusedSpawnPoses.RemoveAt(randIndex);

            //  sets up stats
            obj.GetComponent<UnitClass>().stats = Party.getHolder().getObject<UnitStats>(i);
            obj.GetComponent<UnitClass>().combatStats.isPlayerUnit = true;
            obj.GetComponent<UnitClass>().setup();

            //  adds a trait if traits are empty
            if(Party.getHolder().getObject<UnitStats>(i).u_traits.Count == 0)
                obj.GetComponent<UnitClass>().stats.u_traits.Add(FindObjectOfType<PresetLibrary>().getRandomUnusedUnitTrait(obj.GetComponent<UnitClass>().stats));


            //  sets sprite
            obj.GetComponentInChildren<UnitSpriteHandler>().setReference(Party.getHolder().getObject<UnitStats>(i), true);

            Party.overrideUnitOfSameInstance(obj.GetComponent<UnitClass>().stats);
        }
    }

    public void resaveInstantiatedUnit(UnitStats stats) {
        Party.overrideUnitOfSameInstance(stats);

        foreach(var i in FindObjectsOfType<PlayerUnitInstance>()) {
            if(i.stats.isTheSameInstanceAs(stats)) {
                i.stats = stats;
                i.name = stats.u_name;
                break;
            }
        }
    }

    public void saveParty() {
        foreach(var i in FindObjectsOfType<PlayerUnitInstance>()) {
            Party.overrideUnitOfSameInstance(i.stats);
        }
    }

    public GameObject getInstantiatedMember(UnitStats stats) {
        foreach(var i in FindObjectsOfType<PlayerUnitInstance>()) {
            if(stats.isTheSameInstanceAs(i.stats))
                return i.gameObject;
        }
        return null;
    }
    public List<GameObject> getInstantiatedMembers() {
        var temp = new List<GameObject>();
        foreach(var i in FindObjectsOfType<PlayerUnitInstance>()) {
            temp.Add(i.gameObject);
        }
        return temp;
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
    const string holderTag = "PartyHolderTag";

    public static ObjectHolder getHolder() {
        var data = SaveData.getString(holderTag);
        return JsonUtility.FromJson<ObjectHolder>(data);
    }
    static void saveHolder(ObjectHolder holder) {
        var data = JsonUtility.ToJson(holder);
        SaveData.setString(holderTag, data);
    }


    public static void clear(bool clearInstanceQueue) {
        saveHolder(new ObjectHolder());

        if(clearInstanceQueue)
            GameInfo.clearUnitInstanceIDQueue();
    }


    public static void addUnit(UnitStats col) {
        if(col == null || col.isEmpty())
            return;

        if(getHolder() == null)
            saveHolder(new ObjectHolder());

        var holder = getHolder();
        holder.addObject<UnitStats>(col);
        saveHolder(holder);
    }
    public static void overrideUnitOfSameInstance(UnitStats col) {
        if(col == null)
            return;
        var holder = getHolder();
        int index = holder.getUnitStatsIndex(col);
        if(index == -1)
            return;
        holder.overrideObject<UnitStats>(index, col);
        saveHolder(holder);
    }
    public static void removeUnit(UnitStats col) {
        if(col == null)
            return;
        var holder = getHolder();
        holder.removeObject<UnitStats>(holder.getUnitStatsIndex(col));
        saveHolder(holder);
    }
    public static void removeUnit(int index) {
        if(index == -1)
            return;

        var holder = getHolder();
        holder.removeObject<UnitStats>(index);
        saveHolder(holder);
    }


    public static UnitStats getLeaderStats() {
        var holder = getHolder();
        foreach(var i in holder.getObjects<UnitStats>()) {
            if(i.u_isLeader)
                return i;
        }

        //  creates a new leader if non were found
        var temp = holder.getObject<UnitStats>(0);
        temp.u_isLeader = true;
        overrideUnitOfSameInstance(temp);

        return holder.getObject<UnitStats>(0);
    }
    public static void setLeader(UnitStats stats) {
        var holder = getHolder();
        int index = holder.getUnitStatsIndex(stats);
        for(int i = 0; i < holder.getObjectCount<UnitStats>(); i++) {
            var unit = holder.getObject<UnitStats>(i);

            if(i == index) {
                unit.u_isLeader = true;
                overrideUnitOfSameInstance(unit);
            }
            else {
                unit.u_isLeader = false;
                overrideUnitOfSameInstance(unit);
            }
        }
    }
    /*
    public const string partySizeTag = "PartySize";
    public const string partyLeaderTag = "PartyLeader";
    public static string memberTag(int index) { return "Party" + index.ToString(); }


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

            stats.weapon = null;
            stats.armor = null;
            stats.item = null;
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
            if(mem != null && !mem.isEmpty() && !mem.isTheSameInstanceAs(stats))
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
            if(getMemberStats(i).isTheSameInstanceAs(stats))
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
    }*/
}

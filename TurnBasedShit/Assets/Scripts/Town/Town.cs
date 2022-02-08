using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Town {
    public string t_name = "No Name";

    public GameInfo.region region;

    public int t_instanceID = -1;

    public int interactedBuildingIndex = -1;
    public int townMemberCount = 0;
    public int townNPCCount = 0;

    public bool visited = false;

    //  Building Save Data
    public ObjectHolder holder;
    /*
    public void clearBuildings() {
        for(int i = 0; i < Building.buildingTypeCount; i++) {
            SaveData.deleteKey(buildingTag((Building.type)i));
        }
    }
    public void addBuilding(HospitalBuilding hos) {
        if(getHospital() == null)
            hos.orderInTown = getBuildingCount();
        var data = JsonUtility.ToJson(hos);
        SaveData.setString(buildingTag(Building.type.Hospital), data);
    }
    public void addBuilding(ChurchBuilding chur) {
        if(getChurch() == null)
            chur.orderInTown = getBuildingCount();
        var data = JsonUtility.ToJson(chur);
        SaveData.setString(buildingTag(Building.type.Church), data);
    }
    public void addBuilding(ShopBuilding shop) {
        if(getShop() == null)
            shop.orderInTown = getBuildingCount();
        var data = JsonUtility.ToJson(shop);
        SaveData.setString(buildingTag(Building.type.Shop), data);
    }
    public void addBuilding(CasinoBuilding cas) {
        if(getCasino() == null)
            cas.orderInTown = getBuildingCount();
        var data = JsonUtility.ToJson(cas);
        SaveData.setString(buildingTag(Building.type.Casino), data);
    }
    public void addBuilding(BlacksmithBuilding blac) {
        if(getBlacksmith() == null)
            blac.orderInTown = getBuildingCount();
        var data = JsonUtility.ToJson(blac);
        SaveData.setString(buildingTag(Building.type.Blacksmith), data);
    }

    public void removeBuilding(Building.type t) {
        SaveData.deleteKey(buildingTag(t));
    }
    public HospitalBuilding getHospital() {
        var data = SaveData.getString(buildingTag(Building.type.Hospital));
        if(string.IsNullOrEmpty(data))
            return null;
        return JsonUtility.FromJson<HospitalBuilding>(data);
    }
    public ChurchBuilding getChurch() {
        var data = SaveData.getString(buildingTag(Building.type.Church));
        if(string.IsNullOrEmpty(data))
            return null;
        return JsonUtility.FromJson<ChurchBuilding>(data);
    }
    public ShopBuilding getShop() {
        var data = SaveData.getString(buildingTag(Building.type.Shop));
        if(string.IsNullOrEmpty(data))
            return null;
        return JsonUtility.FromJson<ShopBuilding>(data);
    }
    public CasinoBuilding getCasino() {
        var data = SaveData.getString(buildingTag(Building.type.Casino));
        if(string.IsNullOrEmpty(data))
            return null;
        return JsonUtility.FromJson<CasinoBuilding>(data);
    }
    public BlacksmithBuilding getBlacksmith() {
        var data = SaveData.getString(buildingTag(Building.type.Blacksmith));
        if(string.IsNullOrEmpty(data))
            return null;
        return JsonUtility.FromJson<BlacksmithBuilding>(data);
    }
    public int getBuildingCount() {
        int count = 0;
        if(getHospital() != null)
            count++;
        if(getChurch() != null)
            count++;
        if(getShop() != null)
            count++;
        if(getCasino() != null)
            count++;
        if(getBlacksmith() != null)
            count++;
        return count;
    }*/
    public void randomizeBuildings() {
        List<int> useables = new List<int>();
        for(int i = 0; i < holder.getObjectCount<Building>(); i++)
            useables.Add(i);

        if(holder.getObject<HospitalBuilding>(0) != null) {
            var temp = holder.getObject<HospitalBuilding>(0);
            int rand = Random.Range(0, useables.Count);
            temp.orderInTown = useables[rand];
            useables.RemoveAt(rand);
            holder.overrideObject<Building>(0, temp);
        }
        if(holder.getObject<ChurchBuilding>(0) != null) {
            var temp = holder.getObject<ChurchBuilding>(0);
            int rand = Random.Range(0, useables.Count);
            temp.orderInTown = useables[rand];
            useables.RemoveAt(rand);
            holder.overrideObject<Building>(0, temp);
        }
        if(holder.getObject<ShopBuilding>(0) != null) {
            var temp = holder.getObject<ShopBuilding>(0);
            int rand = Random.Range(0, useables.Count);
            temp.orderInTown = useables[rand];
            useables.RemoveAt(rand);
            holder.overrideObject<Building>(0, temp);
        }
        if(holder.getObject<CasinoBuilding>(0) != null) {
            var temp = holder.getObject<CasinoBuilding>(0);
            int rand = Random.Range(0, useables.Count);
            temp.orderInTown = useables[rand];
            useables.RemoveAt(rand);
            holder.overrideObject<Building>(0, temp);
        }
        if(holder.getObject<BlacksmithBuilding>(0) != null) {
            var temp = holder.getObject<BlacksmithBuilding>(0);
            int rand = Random.Range(0, useables.Count);
            temp.orderInTown = useables[rand];
            useables.RemoveAt(rand);
            holder.overrideObject<Building>(0, temp);
        }
    }
    public Building.type getBuidingTypeWithOrder(int or) {
        if(holder.getObject<HospitalBuilding>(0) != null && holder.getObject<HospitalBuilding>(0).orderInTown == or)
            return Building.type.Hospital;
        if(holder.getObject<ChurchBuilding>(0) != null && holder.getObject<ChurchBuilding>(0).orderInTown == or)
            return Building.type.Church;
        if(holder.getObject<ShopBuilding>(0) != null && holder.getObject<ShopBuilding>(0).orderInTown == or)
            return Building.type.Shop;
        if(holder.getObject<CasinoBuilding>(0) != null && holder.getObject<CasinoBuilding>(0).orderInTown == or)
            return Building.type.Casino;
        if(holder.getObject<BlacksmithBuilding>(0) != null && holder.getObject<BlacksmithBuilding>(0).orderInTown == or)
            return Building.type.Blacksmith;
        return (Building.type)(-1);
    }
    public int getOrderForBuilding(Building.type t) {
        switch(t) {
            case Building.type.Hospital:
                if(holder.getObject<HospitalBuilding>(0) != null)
                    return holder.getObject<HospitalBuilding>(0).orderInTown;
                return -1;

            case Building.type.Church:
                if(holder.getObject<ChurchBuilding>(0) != null)
                    return holder.getObject<ChurchBuilding>(0).orderInTown;
                return -1;

            case Building.type.Shop:
                if(holder.getObject<ShopBuilding>(0) != null)
                    return holder.getObject<ShopBuilding>(0).orderInTown;
                return -1;

            case Building.type.Casino:
                if(holder.getObject<CasinoBuilding>(0) != null)
                    return holder.getObject<CasinoBuilding>(0).orderInTown;
                return -1;

            case Building.type.Blacksmith:
                if(holder.getObject<BlacksmithBuilding>(0) != null)
                    return holder.getObject<BlacksmithBuilding>(0).orderInTown;
                return -1;
        }
        return -1;
    }

    public void saveBuildingOrder(Building.type t, int order) {
        switch(t) {
            case Building.type.Hospital:
                var hos = holder.getObject<HospitalBuilding>(0);
                hos.orderInTown = order;
                holder.overrideObject<Building>(0, hos);
                break;

            case Building.type.Shop:
                var shop = holder.getObject<ShopBuilding>(0);
                shop.orderInTown = order;
                holder.overrideObject<Building>(0, shop);
                break;

            case Building.type.Church:
                var chur = holder.getObject<ChurchBuilding>(0);
                chur.orderInTown = order;
                holder.overrideObject<Building>(0, chur);
                break;

            case Building.type.Casino:
                var cas = holder.getObject<CasinoBuilding>(0);
                cas.orderInTown = order;
                holder.overrideObject<Building>(0, cas);
                break;

            case Building.type.Blacksmith:
                var blac = holder.getObject<BlacksmithBuilding>(0);
                blac.orderInTown = order;
                holder.overrideObject<Building>(0, blac);
                break;
        }
    }

    //  Member Save Data
    string memberTag(int index) {
        return "Town Member Tag" + t_instanceID.ToString() + " " + index.ToString();
    }
    string npcTag(int index) {
        return "Town NPC Tag" + t_instanceID.ToString() + " " + index.ToString();
    }

    public void clearMembers() {
        for(int i = 0; i < townMemberCount; i++) {
            SaveData.deleteKey(memberTag(i));
        }
    }
    public void addMembers(PresetLibrary lib) {
        clearMembers();
        int index = 0;
        for(int i = 0; i < townMemberCount; i++, index++) {
            var mem = lib.createRandomTownMember(this);
            var data = JsonUtility.ToJson(mem);
            SaveData.setString(memberTag(index), data);
        }
        for(int i = 0; i < townNPCCount; i++, index++) {
            var npc = lib.getRandomTownNPC(this);
            var data = JsonUtility.ToJson(npc);
            SaveData.setString(memberTag(index), data);
        }
    }
    public void addNewMember(TownMember mem) {
        var data = JsonUtility.ToJson(mem);
        SaveData.setString(memberTag(getTotalResidentCount()), data);

        if(mem.isNPC)
            townNPCCount++;
        else
            townMemberCount++;
    }
    public TownMember getMember(int index) {
        var data = SaveData.getString(memberTag(index));
        return JsonUtility.FromJson<TownMember>(data);
    }
    public List<TownMember> getMembersWithQuests() {
        var temp = new List<TownMember>();
        for(int i = 0; i < townMemberCount + townNPCCount; i++) {
            var mem = getMember(i);
            if(mem.hasQuest)
                temp.Add(mem);
        }
        return temp;
    }
    public List<TownMember> getMembersWithInactiveQuests() {
        var temp = new List<TownMember>();

        foreach(var i in getMembersWithQuests()) {
            if(!i.isQuestActive())
                temp.Add(i);
        }

        return temp;
    }
    public List<TownMember> getMembersWithActiveQuests() {
        var temp = new List<TownMember>();

        foreach(var i in getMembersWithQuests()) {
            if(i.isQuestActive())
                temp.Add(i);
        }

        return temp;
    }
    public List<TownMember> getNPCs() {
        var temp = new List<TownMember>();

        for(int i = 0; i < townMemberCount + townNPCCount; i++) {
            var mem = getMember(i);
            if(mem.isNPC)
                temp.Add(mem);
        }
        return temp;
    }

    public int getTotalResidentCount() {
        return townMemberCount + townNPCCount;
    }

    public bool hasBuilding(Building.type t) {
        switch(t) {
            case Building.type.Church:
                return holder.getObject<ChurchBuilding>(0) != null;

            case Building.type.Hospital:
                return holder.getObject<HospitalBuilding>(0) != null;

            case Building.type.Shop:
                return holder.getObject<ShopBuilding>(0) != null;

            case Building.type.Casino:
                return holder.getObject<CasinoBuilding>(0) != null;

            case Building.type.Blacksmith:
                return holder.getObject<BlacksmithBuilding>(0) != null;
        }

        return false;
    }


    public Town(GameInfo.region diff, PresetLibrary lib, bool giveID) {
        if(giveID)
            t_instanceID = GameInfo.getNextTownInstanceID();

        t_name = NameLibrary.getRandomUsableTownName();
        region = diff;

        holder = new ObjectHolder();
        int index = 0;
        for(int i = 0; i < Building.buildingTypeCount; i++) {
            if(GameVariables.shouldTownHaveBuilding((Building.type)i)) {
                var b = lib.getBuilding((Building.type)i);

                if(b.GetComponent<HospitalInstance>() != null) {
                    b.GetComponent<HospitalInstance>().reference.orderInTown = index;
                    b.GetComponent<HospitalInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<HospitalInstance>().reference));
                    holder.addObject<Building>(b.GetComponent<HospitalInstance>().reference);
                }
                else if(b.GetComponent<ChurchInstance>() != null) {
                    b.GetComponent<ChurchInstance>().reference.orderInTown = index;
                    b.GetComponent<ChurchInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<ChurchInstance>().reference, lib));
                    holder.addObject<Building>(b.GetComponent<ChurchInstance>().reference);
                }
                else if(b.GetComponent<ShopInstance>() != null) {
                    b.GetComponent<ShopInstance>().reference.orderInTown = index;
                    b.GetComponent<ShopInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<ShopInstance>().reference));
                    holder.addObject<Building>(b.GetComponent<ShopInstance>().reference);
                }
                else if(b.GetComponent<CasinoInstance>() != null) {
                    b.GetComponent<CasinoInstance>().reference.orderInTown = index;
                    b.GetComponent<CasinoInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<CasinoInstance>().reference));
                    holder.addObject<Building>(b.GetComponent<CasinoInstance>().reference);
                }
                else if(b.GetComponent<BlacksmithInstance>() != null) {
                    b.GetComponent<BlacksmithInstance>().reference.orderInTown = index;
                    b.GetComponent<BlacksmithInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<BlacksmithInstance>().reference));
                    holder.addObject<Building>(b.GetComponent<BlacksmithInstance>().reference);
                }

                index++;
            }
        }

        //  town came out with no buildings also randomizes the order of buildings
        if(holder.getObjectCount<Building>() == 0) {
            var b = lib.getBuilding((Building.type)Random.Range(0, Building.buildingTypeCount));

            if(b.GetComponent<HospitalInstance>() != null) {
                b.GetComponent<HospitalInstance>().reference.orderInTown = index;
                b.GetComponent<HospitalInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<HospitalInstance>().reference));
                holder.addObject<Building>(b.GetComponent<HospitalInstance>().reference);
            }
            else if(b.GetComponent<ChurchInstance>() != null) {
                b.GetComponent<ChurchInstance>().reference.orderInTown = index;
                b.GetComponent<ChurchInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<ChurchInstance>().reference, lib));
                holder.addObject<Building>(b.GetComponent<ChurchInstance>().reference);
            }
            else if(b.GetComponent<ShopInstance>() != null) {
                b.GetComponent<ShopInstance>().reference.orderInTown = index;
                b.GetComponent<ShopInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<ShopInstance>().reference));
                holder.addObject<Building>(b.GetComponent<ShopInstance>().reference);
            }
            else if(b.GetComponent<CasinoInstance>() != null) {
                b.GetComponent<CasinoInstance>().reference.orderInTown = index;
                b.GetComponent<CasinoInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<CasinoInstance>().reference));
                holder.addObject<Building>(b.GetComponent<CasinoInstance>().reference);
            }
            else if(b.GetComponent<BlacksmithInstance>() != null) {
                b.GetComponent<BlacksmithInstance>().reference.orderInTown = index;
                b.GetComponent<BlacksmithInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<BlacksmithInstance>().reference));
                holder.addObject<Building>(b.GetComponent<BlacksmithInstance>().reference);
            }
            index++;
        }

        randomizeBuildings();

        //  shop shit
        ShopInventory.populateShop(t_instanceID, diff, lib);

        //  member shit
        townMemberCount = GameVariables.createTownMemberCount(index);
        townNPCCount = GameVariables.createTownNPCCount(townMemberCount);
        townNPCCount = 1;
        addMembers(lib);
    }


    public bool isEqualTo(Town other) {
        return t_instanceID == other.t_instanceID;
    }
}
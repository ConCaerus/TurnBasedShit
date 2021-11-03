using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Town {
    public string t_name = "No Name";

    public int t_instanceID = -1;

    public int interactedBuildingIndex = -1;
    public int townMemberCount = 0;

    public bool visited = false;

    //  Building Save Data
    string buildingTag(Building.type t) {
        return "Buildings In Town " + t_instanceID.ToString() + " " + t.ToString();
    }

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
    }
    public void randomizeBuildings() {
        List<int> useables = new List<int>();
        for(int i = 0; i < getBuildingCount(); i++)
            useables.Add(i);

        if(getHospital() != null) {
            var temp = getHospital();
            int rand = Random.Range(0, useables.Count);
            temp.orderInTown = useables[rand];
            useables.RemoveAt(rand);
            addBuilding(temp);
        }
        if(getChurch() != null) {
            var temp = getChurch();
            int rand = Random.Range(0, useables.Count);
            temp.orderInTown = useables[rand];
            useables.RemoveAt(rand);
            addBuilding(temp);
        }
        if(getShop() != null) {
            var temp = getShop();
            int rand = Random.Range(0, useables.Count);
            temp.orderInTown = useables[rand];
            useables.RemoveAt(rand);
            addBuilding(temp);
        }
        if(getCasino() != null) {
            var temp = getCasino();
            int rand = Random.Range(0, useables.Count);
            temp.orderInTown = useables[rand];
            useables.RemoveAt(rand);
            addBuilding(temp);
        }
        if(getBlacksmith() != null) {
            var temp = getBlacksmith();
            int rand = Random.Range(0, useables.Count);
            temp.orderInTown = useables[rand];
            useables.RemoveAt(rand);
            addBuilding(temp);
        }
    }
    public Building.type getBuidingTypeWithOrder(int or) {
        if(getHospital() != null && getHospital().orderInTown == or)
            return Building.type.Hospital;
        if(getChurch() != null && getChurch().orderInTown == or)
            return Building.type.Church;
        if(getShop() != null && getShop().orderInTown == or)
            return Building.type.Shop;
        if(getCasino() != null && getCasino().orderInTown == or)
            return Building.type.Casino;
        if(getBlacksmith() != null && getBlacksmith().orderInTown == or)
            return Building.type.Blacksmith;
        return (Building.type)(-1);
    }
    public int getOrderForBuilding(Building.type t) {
        switch(t) {
            case Building.type.Hospital:
                if(getHospital() != null)
                    return getHospital().orderInTown;
                return -1;

            case Building.type.Church:
                if(getChurch() != null)
                    return getChurch().orderInTown;
                return -1;

            case Building.type.Shop:
                if(getShop() != null)
                    return getShop().orderInTown;
                return -1;

            case Building.type.Casino:
                if(getCasino() != null)
                    return getCasino().orderInTown;
                return -1;

            case Building.type.Blacksmith:
                if(getBlacksmith() != null)
                    return getBlacksmith().orderInTown;
                return -1;
        }
        return -1;
    }

    public void saveBuildingOrder(Building.type t, int order) {
        switch(t) {
            case Building.type.Hospital:
                var hos = getHospital();
                hos.orderInTown = order;
                addBuilding(hos);
                break;

            case Building.type.Shop:
                var shop = getShop();
                shop.orderInTown = order;
                addBuilding(shop);
                break;

            case Building.type.Church:
                var chur = getChurch();
                chur.orderInTown = order;
                addBuilding(chur);
                break;

            case Building.type.Casino:
                var cas = getCasino();
                cas.orderInTown = order;
                addBuilding(cas);
                break;

            case Building.type.Blacksmith:
                var blac = getBlacksmith();
                blac.orderInTown = order;
                addBuilding(blac);
                break;
        }
    }

    //  Member Save Data
    string memberTag(int index) {
        return "Town Member Tag" + t_instanceID.ToString() + " " + index.ToString();
    }

    public void clearMembers() {
        for(int i = 0; i < townMemberCount; i++) {
            SaveData.deleteKey(memberTag(i));
        }
    }
    public void addMembers(PresetLibrary lib) {
        clearMembers();
        for(int i = 0; i < townMemberCount; i++) {
            var mem = lib.createRandomTownMember();
            var data = JsonUtility.ToJson(mem);
            SaveData.setString(memberTag(i), data);
        }
    }
    public TownMember getMember(int index) {
        var data = SaveData.getString(memberTag(index));
        return JsonUtility.FromJson<TownMember>(data);
    }
    public List<TownMember> getMembersWithQuests() {
        var temp = new List<TownMember>();
        for(int i = 0; i < townMemberCount; i++) {
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

    public bool hasBuilding(Building.type t) {
        switch(t) {
            case Building.type.Church:
                return getChurch() != null;

            case Building.type.Hospital:
                return getHospital() != null;

            case Building.type.Shop:
                return getShop() != null;

            case Building.type.Casino:
                return getCasino() != null;

            case Building.type.Blacksmith:
                return getBlacksmith() != null;
        }

        return false;
    }


    public Town(GameInfo.region diff, PresetLibrary lib, bool giveID) {
        if(giveID)
            t_instanceID = GameInfo.getNextTownInstanceID();

        t_name = NameLibrary.getRandomUsableTownName();

        clearBuildings();
        int index = 0;
        for(int i = 0; i < Building.buildingTypeCount; i++) {
            if(GameVariables.shouldTownHaveBuilding((Building.type)i)) {
                var b = lib.getBuilding((Building.type)i);

                if(b.GetComponent<HospitalInstance>() != null) {
                    b.GetComponent<HospitalInstance>().reference.orderInTown = index;
                    b.GetComponent<HospitalInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<HospitalInstance>().reference));
                    addBuilding(b.GetComponent<HospitalInstance>().reference);
                }
                else if(b.GetComponent<ChurchInstance>() != null) {
                    b.GetComponent<ChurchInstance>().reference.orderInTown = index;
                    b.GetComponent<ChurchInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<ChurchInstance>().reference));
                    addBuilding(b.GetComponent<ChurchInstance>().reference);
                }
                else if(b.GetComponent<ShopInstance>() != null) {
                    b.GetComponent<ShopInstance>().reference.orderInTown = index;
                    b.GetComponent<ShopInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<ShopInstance>().reference));
                    addBuilding(b.GetComponent<ShopInstance>().reference);
                }
                else if(b.GetComponent<CasinoInstance>() != null) {
                    b.GetComponent<CasinoInstance>().reference.orderInTown = index;
                    b.GetComponent<CasinoInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<CasinoInstance>().reference));
                    addBuilding(b.GetComponent<CasinoInstance>().reference);
                }
                else if(b.GetComponent<BlacksmithInstance>() != null) {
                    b.GetComponent<BlacksmithInstance>().reference.orderInTown = index;
                    b.GetComponent<BlacksmithInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<BlacksmithInstance>().reference));
                    addBuilding(b.GetComponent<BlacksmithInstance>().reference);
                }

                index++;
            }
        }

        //  town came out with no buildings also randomizes the order of buildings
        if(getBuildingCount() == 0) {
            var b = lib.getBuilding((Building.type)Random.Range(0, Building.buildingTypeCount));

            if(b.GetComponent<HospitalInstance>() != null) {
                b.GetComponent<HospitalInstance>().reference.orderInTown = index;
                b.GetComponent<HospitalInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<HospitalInstance>().reference));
                addBuilding(b.GetComponent<HospitalInstance>().reference);
            }
            else if(b.GetComponent<ChurchInstance>() != null) {
                b.GetComponent<ChurchInstance>().reference.orderInTown = index;
                b.GetComponent<ChurchInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<ChurchInstance>().reference));
                addBuilding(b.GetComponent<ChurchInstance>().reference);
            }
            else if(b.GetComponent<ShopInstance>() != null) {
                b.GetComponent<ShopInstance>().reference.orderInTown = index;
                b.GetComponent<ShopInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<ShopInstance>().reference));
                addBuilding(b.GetComponent<ShopInstance>().reference);
            }
            else if(b.GetComponent<CasinoInstance>() != null) {
                b.GetComponent<CasinoInstance>().reference.orderInTown = index;
                b.GetComponent<CasinoInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<CasinoInstance>().reference));
                addBuilding(b.GetComponent<CasinoInstance>().reference);
            }
            else if(b.GetComponent<BlacksmithInstance>() != null) {
                b.GetComponent<BlacksmithInstance>().reference.orderInTown = index;
                b.GetComponent<BlacksmithInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<BlacksmithInstance>().reference));
                addBuilding(b.GetComponent<BlacksmithInstance>().reference);
            }
            index++;
        }

        randomizeBuildings();

        //  shop shit
        ShopInventory.populateShop(t_instanceID, diff, lib);

        //  member shit
        townMemberCount = GameVariables.createTownMemberCount(index);
        addMembers(lib);
    }


    public bool isEqualTo(Town other) {
        return t_instanceID == other.t_instanceID;
    }
}
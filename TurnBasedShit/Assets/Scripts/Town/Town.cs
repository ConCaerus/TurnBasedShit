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
    public void addHospital(HospitalBuilding hos) {
        var data = JsonUtility.ToJson(hos);
        SaveData.setString(buildingTag(Building.type.Hospital), data);
    }
    public void addChurch(ChurchBuilding chur) {
        var data = JsonUtility.ToJson(chur);
        SaveData.setString(buildingTag(Building.type.Church), data);
    }
    public void addShop(ShopBuilding shop) {
        var data = JsonUtility.ToJson(shop);
        SaveData.setString(buildingTag(Building.type.Shop), data);
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
    public List<Building> getBuildings() {
        var temp = new List<Building>();
        if(getHospital() != null)
            temp.Add(getHospital());
        if(getChurch() != null)
            temp.Add(getChurch());
        if(getShop() != null)
            temp.Add(getShop());

        //  randomizes the list
        System.Random rng = new System.Random();
        int n = temp.Count;
        while(n > 1) {
            n--;
            int k = rng.Next(n + 1);
            Building value = temp[k];
            temp[k] = temp[n];
            temp[n] = value;
        }

        return temp;
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
        }

        return false;
    }


    public Town(GameInfo.diffLvl diff, PresetLibrary lib, bool giveID) {
        if(giveID)
            t_instanceID = GameInfo.getNextTownInstanceID();

        t_name = NameLibrary.getRandomUsableTownName();

        clearBuildings();
        int index = 0;
        for(int i = 0; i < Building.buildingTypeCount; i++) {
            if(GameVariables.shouldTownHaveBuilding((Building.type)i)) { //  50% 
                var b = lib.getBuilding((Building.type)i);

                if(b.GetComponent<HospitalInstance>() != null) {
                    b.GetComponent<HospitalInstance>().reference.orderInTown = index;
                    b.GetComponent<HospitalInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<HospitalInstance>().reference));
                    addHospital(b.GetComponent<HospitalInstance>().reference);
                }
                else if(b.GetComponent<ChurchInstance>() != null) {
                    b.GetComponent<ChurchInstance>().reference.orderInTown = index;
                    b.GetComponent<ChurchInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<ChurchInstance>().reference));
                    addChurch(b.GetComponent<ChurchInstance>().reference);
                }
                else if(b.GetComponent<ShopInstance>() != null) {
                    b.GetComponent<ShopInstance>().reference.orderInTown = index;
                    b.GetComponent<ShopInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<ShopInstance>().reference));
                    addShop(b.GetComponent<ShopInstance>().reference);
                }

                index++;
            }
        }

        //  town came out with no buildings
        if(getBuildings().Count == 0) {
            var b = lib.getBuilding((Building.type)Random.Range(0, Building.buildingTypeCount));

            if(b.GetComponent<HospitalInstance>() != null) {
                b.GetComponent<HospitalInstance>().reference.orderInTown = index;
                b.GetComponent<HospitalInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<HospitalInstance>().reference));
                addHospital(b.GetComponent<HospitalInstance>().reference);
            }
            else if(b.GetComponent<ChurchInstance>() != null) {
                b.GetComponent<ChurchInstance>().reference.orderInTown = index;
                b.GetComponent<ChurchInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<ChurchInstance>().reference));
                addChurch(b.GetComponent<ChurchInstance>().reference);
            }
            else if(b.GetComponent<ShopInstance>() != null) {
                b.GetComponent<ShopInstance>().reference.orderInTown = index;
                b.GetComponent<ShopInstance>().reference.setEqualTo(Randomizer.randomizeBuilding(b.GetComponent<ShopInstance>().reference));
                addShop(b.GetComponent<ShopInstance>().reference);
            }
            index++;
        }
            
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
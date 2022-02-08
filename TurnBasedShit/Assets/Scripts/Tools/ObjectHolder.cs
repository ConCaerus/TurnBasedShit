using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectHolder {
    public int instanceID = -1;

    //  tages
    string tag<T>(int index) { return "Object Holder: " + instanceID.ToString() + " " + typeof(T) + " index: " + index.ToString(); }
    string countTag<T>() { return "Object Holder: " + instanceID.ToString() + " " + typeof(T) + " count"; }

    //  to clear all, just set to a new instance of ObjectHolder
    //  clear
    public void clearObjects<T>() {
        for(int i = 0; i < getObjectCount<T>(); i++)
            SaveData.deleteKey(tag<T>(i));
        SaveData.deleteKey(countTag<T>());
    }

    //  add
    public void addObject<T>(object thing) {
        if(thing == null) {
            return;
        }
        if(instanceID == -1)
            instanceID = GameInfo.getNextObjectHolderInstanceID();

        if(typeof(T) == typeof(Collectable)) {
            addCollectable((Collectable)thing);
            return;
        }
        if(typeof(T) == typeof(Quest)) {
            addQuest((Quest)thing);
            return;
        }
        if(typeof(T) == typeof(Building)) {
            addBuilding((Building)thing);
            return;
        }
        if(typeof(T) == typeof(MapLocation)) {
            addMapLocation((MapLocation)thing);
            return;
        }

        int index = getObjectCount<T>();
        var data = JsonUtility.ToJson((T)thing);
        SaveData.setString(tag<T>(index), data);
        SaveData.setInt(countTag<T>(), index + 1);
    }
    void addCollectable(Collectable thing) {
        if(thing == null || thing.isEmpty())
            return;
        switch(thing.type) {
            case Collectable.collectableType.Weapon:
                addObject<Weapon>((Weapon)thing);
                return;

            case Collectable.collectableType.Armor:
                addObject<Armor>((Armor)thing);
                return;

            case Collectable.collectableType.Item:
                addObject<Item>((Item)thing);
                return;

            case Collectable.collectableType.Usable:
                addObject<Usable>((Usable)thing);
                return;

            case Collectable.collectableType.Unusable:
                addObject<Unusable>((Unusable)thing);
                return;
        }
    }
    void addQuest(Quest thing) {
        if(thing == null)
            return;
        switch(thing.getQuestType()) {
            case Quest.questType.bossFight:
                addObject<BossFightQuest>((BossFightQuest)thing);
                return;

            case Quest.questType.pickup:
                addObject<PickupQuest>((PickupQuest)thing);
                return;

            case Quest.questType.delivery:
                addObject<DeliveryQuest>((DeliveryQuest)thing);
                return;

            case Quest.questType.kill:
                addObject<KillQuest>((KillQuest)thing);
                return;

            case Quest.questType.rescue:
                addObject<RescueQuest>((RescueQuest)thing);
                return;

            case Quest.questType.fishing:
                addObject<FishingQuest>((FishingQuest)thing);
                return;
        }
    }
    void addBuilding(Building thing) {
        if(thing == null)
            return;
        switch(thing.b_type) {
            case Building.type.Hospital:
                addObject<HospitalBuilding>((HospitalBuilding)thing);
                return;

            case Building.type.Shop:
                addObject<ShopBuilding>((ShopBuilding)thing);
                return;

            case Building.type.Church:
                addObject<ChurchBuilding>((ChurchBuilding)thing);
                return;

            case Building.type.Casino:
                addObject<CasinoBuilding>((CasinoBuilding)thing);
                return;

            case Building.type.Blacksmith:
                addObject<BlacksmithBuilding>((BlacksmithBuilding)thing);
                return;
        }
    }
    void addMapLocation(MapLocation thing) {
        if(thing == null)
            return;
        switch(thing.type) {
            case MapLocation.locationType.town:
                addObject<TownLocation>((TownLocation)thing);
                return;
            case MapLocation.locationType.pickup:
                addObject<PickupLocation>((PickupLocation)thing);
                return;
            case MapLocation.locationType.upgrade:
                addObject<UpgradeLocation>((UpgradeLocation)thing);
                return;
            case MapLocation.locationType.rescue:
                addObject<RescueLocation>((RescueLocation)thing);
                return;
            case MapLocation.locationType.boss:
                addObject<BossLocation>((BossLocation)thing);
                return;
            case MapLocation.locationType.fishing:
                addObject<FishingLocation>((FishingLocation)thing);
                return;
            case MapLocation.locationType.eye:
                addObject<EyeLocation>((EyeLocation)thing);
                return;
            case MapLocation.locationType.bridge:
                addObject<BridgeLocation>((BridgeLocation)thing);
                return;
            case MapLocation.locationType.loot:
                addObject<LootLocation>((LootLocation)thing);
                return;
        }
    }


    //  override
    public void overrideObject<T>(int index, object thing) {
        if(typeof(T) == typeof(Collectable)) {
            overrideCollectable(index, (Collectable)thing);
            return;
        }
        if(typeof(T) == typeof(Quest)) {
            overrideQuest(index, (Quest)thing);
            return;
        }
        if(typeof(T) == typeof(Building)) {
            overrideBuilding(index, (Building)thing);
            return;
        }
        if(typeof(T) == typeof(MapLocation)) {
            overrideMapLocation(index, (MapLocation)thing);
            return;
        }
        var data = JsonUtility.ToJson((T)thing);
        SaveData.setString(tag<T>(index), data);
    }
    public void overrideCollectableOfSameInstance(Collectable thing) {
        switch(thing.type) {
            case Collectable.collectableType.Weapon:
                var ws = getObjects<Weapon>();
                for(int i = 0; i < ws.Count; i++) {
                    if(ws[i].isTheSameInstanceAs(thing)) {
                        var data = JsonUtility.ToJson((Weapon)thing);
                        SaveData.setString(tag<Weapon>(i), data);
                        return;
                    }
                }
                return;

            case Collectable.collectableType.Armor:
                var ass = getObjects<Armor>();
                for(int i = 0; i < ass.Count; i++) {
                    if(ass[i].isTheSameInstanceAs(thing)) {
                        var data = JsonUtility.ToJson((Armor)thing);
                        SaveData.setString(tag<Armor>(i), data);
                        return;
                    }
                }
                return;

            case Collectable.collectableType.Item:
                var iss = getObjects<Item>();
                for(int i = 0; i < iss.Count; i++) {
                    if(iss[i].isTheSameInstanceAs(thing)) {
                        var data = JsonUtility.ToJson((Item)thing);
                        SaveData.setString(tag<Item>(i), data);
                        return;
                    }
                }
                return;

            case Collectable.collectableType.Usable:
                var us = getObjects<Usable>();
                for(int i = 0; i < us.Count; i++) {
                    if(us[i].isTheSameInstanceAs(thing)) {
                        var data = JsonUtility.ToJson((Usable)thing);
                        SaveData.setString(tag<Usable>(i), data);
                        return;
                    }
                }
                return;

            case Collectable.collectableType.Unusable:
                var uns = getObjects<Unusable>();
                for(int i = 0; i < uns.Count; i++) {
                    if(uns[i].isTheSameInstanceAs(thing)) {
                        var data = JsonUtility.ToJson((Unusable)thing);
                        SaveData.setString(tag<Unusable>(i), data);
                        return;
                    }
                }
                return;
        }
    }
    public void overrideQuestOfSameInstance(Quest thing) {
        switch(thing.getQuestType()) {
            case Quest.questType.bossFight:
                var a = getObjects<BossFightQuest>();
                for(int i = 0; i < a.Count; i++) {
                    if(a[i].isTheSameInstanceAs(thing)) {
                        var data = JsonUtility.ToJson((BossFightQuest)thing);
                        SaveData.setString(tag<BossFightQuest>(i), data);
                        return;
                    }
                }
                return;
            case Quest.questType.pickup:
                var b = getObjects<PickupQuest>();
                for(int i = 0; i < b.Count; i++) {
                    if(b[i].isTheSameInstanceAs(thing)) {
                        var data = JsonUtility.ToJson((PickupQuest)thing);
                        SaveData.setString(tag<PickupQuest>(i), data);
                        return;
                    }
                }
                return;
            case Quest.questType.delivery:
                var c = getObjects<DeliveryQuest>();
                for(int i = 0; i < c.Count; i++) {
                    if(c[i].isTheSameInstanceAs(thing)) {
                        var data = JsonUtility.ToJson((DeliveryQuest)thing);
                        SaveData.setString(tag<DeliveryQuest>(i), data);
                        return;
                    }
                }
                return;
            case Quest.questType.kill:
                var d = getObjects<KillQuest>();
                for(int i = 0; i < d.Count; i++) {
                    if(d[i].isTheSameInstanceAs(thing)) {
                        var data = JsonUtility.ToJson((KillQuest)thing);
                        SaveData.setString(tag<KillQuest>(i), data);
                        return;
                    }
                }
                return;
            case Quest.questType.rescue:
                var e = getObjects<RescueQuest>();
                for(int i = 0; i < e.Count; i++) {
                    if(e[i].isTheSameInstanceAs(thing)) {
                        var data = JsonUtility.ToJson((RescueQuest)thing);
                        SaveData.setString(tag<RescueQuest>(i), data);
                        return;
                    }
                }
                return;
            case Quest.questType.fishing:
                var f = getObjects<FishingQuest>();
                for(int i = 0; i < f.Count; i++) {
                    if(f[i].isTheSameInstanceAs(thing)) {
                        var data = JsonUtility.ToJson((FishingQuest)thing);
                        SaveData.setString(tag<FishingQuest>(i), data);
                        return;
                    }
                }
                return;
        }
    }
    public void overrideMapLocationOfSameType(MapLocation thing) {
        if(thing == null)
            return;
        int index = getMapLocationIndex(thing);
        switch(thing.type) {
            case MapLocation.locationType.town:
                overrideObject<TownLocation>(index, (TownLocation)thing);
                return;
            case MapLocation.locationType.pickup:
                overrideObject<PickupLocation>(index, (PickupLocation)thing);
                return;
            case MapLocation.locationType.upgrade:
                overrideObject<UpgradeLocation>(index, (UpgradeLocation)thing);
                return;
            case MapLocation.locationType.rescue:
                overrideObject<RescueLocation>(index, (RescueLocation)thing);
                return;
            case MapLocation.locationType.boss:
                overrideObject<BossLocation>(index, (BossLocation)thing);
                return;
            case MapLocation.locationType.fishing:
                overrideObject<FishingLocation>(index, (FishingLocation)thing);
                return;
            case MapLocation.locationType.eye:
                overrideObject<EyeLocation>(index, (EyeLocation)thing);
                return;
            case MapLocation.locationType.bridge:
                overrideObject<BridgeLocation>(index, (BridgeLocation)thing);
                return;
            case MapLocation.locationType.loot:
                overrideObject<LootLocation>(index, (LootLocation)thing);
                return;
        }
    }
    void overrideCollectable(int index, Collectable thing) {
        if(thing == null || thing.isEmpty())
            return;
        switch(thing.type) {
            case Collectable.collectableType.Weapon:
                overrideObject<Weapon>(index, (Weapon)thing);
                return;

            case Collectable.collectableType.Armor:
                overrideObject<Armor>(index, (Armor)thing);
                return;

            case Collectable.collectableType.Item:
                overrideObject<Item>(index, (Item)thing);
                return;

            case Collectable.collectableType.Usable:
                overrideObject<Usable>(index, (Usable)thing);
                return;

            case Collectable.collectableType.Unusable:
                overrideObject<Unusable>(index, (Unusable)thing);
                return;
        }
    }
    void overrideQuest(int index, Quest thing) {
        if(thing == null)
            return;
        switch(thing.getQuestType()) {
            case Quest.questType.bossFight:
                overrideObject<BossFightQuest>(index, (BossFightQuest)thing);
                return;

            case Quest.questType.pickup:
                overrideObject<PickupQuest>(index, (PickupQuest)thing);
                return;

            case Quest.questType.delivery:
                overrideObject<DeliveryQuest>(index, (DeliveryQuest)thing);
                return;

            case Quest.questType.kill:
                overrideObject<KillQuest>(index, (KillQuest)thing);
                return;

            case Quest.questType.rescue:
                overrideObject<RescueQuest>(index, (RescueQuest)thing);
                return;

            case Quest.questType.fishing:
                overrideObject<FishingQuest>(index, (FishingQuest)thing);
                return;
        }
    }
    void overrideBuilding(int index, Building thing) {
        if(thing == null)
            return;
        switch(thing.b_type) {
            case Building.type.Hospital:
                overrideObject<HospitalBuilding>(index, (HospitalBuilding)thing);
                return;
            case Building.type.Shop:
                overrideObject<ShopBuilding>(index, (ShopBuilding)thing);
                return;
            case Building.type.Church:
                overrideObject<ChurchBuilding>(index, (ChurchBuilding)thing);
                return;
            case Building.type.Casino:
                overrideObject<CasinoBuilding>(index, (CasinoBuilding)thing);
                return;
            case Building.type.Blacksmith:
                overrideObject<BlacksmithBuilding>(index, (BlacksmithBuilding)thing);
                return;
        }
    }
    void overrideMapLocation(int index, MapLocation thing) {
        if(thing == null)
            return;
        switch(thing.type) {
            case MapLocation.locationType.town:
                overrideObject<TownLocation>(index, (TownLocation)thing);
                return;
            case MapLocation.locationType.pickup:
                overrideObject<PickupLocation>(index, (PickupLocation)thing);
                return;
            case MapLocation.locationType.upgrade:
                overrideObject<UpgradeLocation>(index, (UpgradeLocation)thing);
                return;
            case MapLocation.locationType.rescue:
                overrideObject<RescueLocation>(index, (RescueLocation)thing);
                return;
            case MapLocation.locationType.boss:
                overrideObject<BossLocation>(index, (BossLocation)thing);
                return;
            case MapLocation.locationType.fishing:
                overrideObject<FishingLocation>(index, (FishingLocation)thing);
                return;
            case MapLocation.locationType.eye:
                overrideObject<EyeLocation>(index, (EyeLocation)thing);
                return;
            case MapLocation.locationType.bridge:
                overrideObject<BridgeLocation>(index, (BridgeLocation)thing);
                return;
            case MapLocation.locationType.loot:
                overrideObject<LootLocation>(index, (LootLocation)thing);
                return;
        }
    }

    //  remove
    public void removeObject<T>(int index) {
        if(invalidTyping<T>()) {
            Debug.LogError("Use specific function instead");
            return;
        }
        var thing = getObject<T>(index);

        if(thing == null)
            return;

        for(int i = index; i < getObjectCount<T>(); i++) {
            overrideObject<T>(i, getObject<T>(i + 1));
        }

        SaveData.setInt(countTag<T>(), getObjectCount<T>() - 1);
    }
    public void removeCollectable(Collectable thing) {
        if(thing == null || thing.isEmpty())
            return;

        int index = getCollectableIndex(thing);
        switch((thing).type) {
            case Collectable.collectableType.Weapon:
                removeObject<Weapon>(index);
                break;

            case Collectable.collectableType.Armor:
                removeObject<Armor>(index);
                break;

            case Collectable.collectableType.Item:
                removeObject<Item>(index);
                break;

            case Collectable.collectableType.Usable:
                removeObject<Usable>(index);
                break;

            case Collectable.collectableType.Unusable:
                removeObject<Unusable>(index);
                break;
        }
    }
    public void removeQuest(Quest thing) {
        if(thing == null)
            return;

        int index = getQuestIndex(thing);
        if(index < 0)
            return;
        switch(thing.getQuestType()) {
            case Quest.questType.bossFight:
                removeObject<BossFightQuest>(index);
                break;

            case Quest.questType.pickup:
                removeObject<PickupQuest>(index);
                break;

            case Quest.questType.delivery:
                removeObject<DeliveryQuest>(index);
                break;

            case Quest.questType.kill:
                removeObject<KillQuest>(index);
                break;

            case Quest.questType.rescue:
                removeObject<RescueQuest>(index);
                break;

            case Quest.questType.fishing:
                removeObject<FishingQuest>(index);
                break;
        }
    }
    public void removeBuilding(Building thing) {
        if(thing == null)
            return;

        int index = getBuildingIndex(thing);
        if(index < 0)
            return;
        switch(thing.b_type) {
            case Building.type.Hospital:
                removeObject<HospitalBuilding>(index);
                return;
            case Building.type.Shop:
                removeObject<ShopBuilding>(index);
                return;
            case Building.type.Church:
                removeObject<ChurchBuilding>(index);
                return;
            case Building.type.Casino:
                removeObject<CasinoBuilding>(index);
                return;
            case Building.type.Blacksmith:
                removeObject<BlacksmithBuilding>(index);
                return;
        }
    }
    public void removeMapLocation(MapLocation thing) {
        if(thing == null)
            return;
        int index = getMapLocationIndex(thing);
        switch(thing.type) {
            case MapLocation.locationType.town:
                removeObject<TownLocation>(index);
                return;
            case MapLocation.locationType.pickup:
                removeObject<PickupLocation>(index);
                return;
            case MapLocation.locationType.upgrade:
                removeObject<UpgradeLocation>(index);
                return;
            case MapLocation.locationType.rescue:
                removeObject<RescueLocation>(index);
                return;
            case MapLocation.locationType.boss:
                removeObject<BossLocation>(index);
                return;
            case MapLocation.locationType.fishing:
                removeObject<FishingLocation>(index);
                return;
            case MapLocation.locationType.eye:
                removeObject<EyeLocation>(index);
                return;
            case MapLocation.locationType.bridge:
                removeObject<BridgeLocation>(index);
                return;
            case MapLocation.locationType.loot:
                removeObject<LootLocation>(index);
                return;
        }
    }

    //  get multiple
    public List<T> getObjects<T>() {
        if(invalidTyping<T>()) {
            Debug.LogError("Use specific function instead");
            return null;
        }

        var temp = new List<T>();
        for(int i = 0; i < getObjectCount<T>(); i++) {
            temp.Add(getObject<T>(i));
        }
        return temp;
    }
    public List<Collectable> getCollectables() {
        var cols = new List<Collectable>();
        foreach(var i in getObjects<Weapon>())
            cols.Add(i);
        foreach(var i in getObjects<Armor>())
            cols.Add(i);
        foreach(var i in getObjects<Item>())
            cols.Add(i);
        foreach(var i in getObjects<Usable>())
            cols.Add(i);
        foreach(var i in getObjects<Unusable>())
            cols.Add(i);
        return cols;
    }
    public List<Quest> getQuests() {
        var cols = new List<Quest>();
        foreach(var i in getObjects<BossFightQuest>())
            cols.Add(i);
        foreach(var i in getObjects<PickupQuest>())
            cols.Add(i);
        foreach(var i in getObjects<DeliveryQuest>())
            cols.Add(i);
        foreach(var i in getObjects<KillQuest>())
            cols.Add(i);
        foreach(var i in getObjects<RescueQuest>())
            cols.Add(i);
        foreach(var i in getObjects<FishingQuest>())
            cols.Add(i);
        return cols;
    }
    public List<Building> getBuildings() {
        var buils = new List<Building>();
        foreach(var i in getObjects<HospitalBuilding>())
            buils.Add(i);
        foreach(var i in getObjects<ShopBuilding>())
            buils.Add(i);
        foreach(var i in getObjects<ChurchBuilding>())
            buils.Add(i);
        foreach(var i in getObjects<CasinoBuilding>())
            buils.Add(i);
        foreach(var i in getObjects<BlacksmithBuilding>())
            buils.Add(i);
        return buils;
    }
    public List<MapLocation> getMapLocations() {
        var mls = new List<MapLocation>();
        foreach(var i in getObjects<TownLocation>())
            mls.Add(i);
        foreach(var i in getObjects<PickupLocation>())
            mls.Add(i);
        foreach(var i in getObjects<UpgradeLocation>())
            mls.Add(i);
        foreach(var i in getObjects<RescueLocation>())
            mls.Add(i);
        foreach(var i in getObjects<BossLocation>())
            mls.Add(i);
        foreach(var i in getObjects<FishingLocation>())
            mls.Add(i);
        foreach(var i in getObjects<EyeLocation>())
            mls.Add(i);
        foreach(var i in getObjects<BridgeLocation>())
            mls.Add(i);
        foreach(var i in getObjects<LootLocation>())
            mls.Add(i);
        return mls;
    }

    //  get singular
    public T getObject<T>(int index) {
        if(invalidTyping<T>()) {
            Debug.LogError("Cannot get object of type Collectable");
        }

        var data = SaveData.getString(tag<T>(index));
        return JsonUtility.FromJson<T>(data);
    }

    //  get index
    public int getCollectableIndex(Collectable thing) {
        if(thing == null || thing.isEmpty())
            return -1;

        switch(thing.type) {
            case Collectable.collectableType.Weapon:
                for(int i = 0; i < getObjectCount<Weapon>(); i++) {
                    if(getObject<Weapon>(i).isTheSameInstanceAs(thing))
                        return i;
                }
                break;

            case Collectable.collectableType.Armor:
                for(int i = 0; i < getObjectCount<Armor>(); i++) {
                    if(getObject<Armor>(i).isTheSameInstanceAs(thing))
                        return i;
                }
                break;

            case Collectable.collectableType.Item:
                for(int i = 0; i < getObjectCount<Item>(); i++) {
                    if(getObject<Item>(i).isTheSameInstanceAs(thing))
                        return i;
                }
                break;

            case Collectable.collectableType.Usable:
                for(int i = 0; i < getObjectCount<Usable>(); i++) {
                    if(getObject<Usable>(i).isTheSameInstanceAs(thing))
                        return i;
                }
                break;

            case Collectable.collectableType.Unusable:
                for(int i = 0; i < getObjectCount<Unusable>(); i++) {
                    if(getObject<Unusable>(i).isTheSameInstanceAs(thing))
                        return i;
                }
                break;
        }

        return -1;
    }
    public int getQuestIndex(Quest thing) {
        if(thing == null)
            return -1;

        switch(thing.getQuestType()) {
            case Quest.questType.bossFight:
                for(int i = 0; i < getObjectCount<BossFightQuest>(); i++) {
                    if(getObject<BossFightQuest>(i).isTheSameInstanceAs(thing))
                        return i;
                }
                break;

            case Quest.questType.pickup:
                for(int i = 0; i < getObjectCount<PickupQuest>(); i++) {
                    if(getObject<PickupQuest>(i).isTheSameInstanceAs(thing))
                        return i;
                }
                break;

            case Quest.questType.delivery:
                for(int i = 0; i < getObjectCount<DeliveryQuest>(); i++) {
                    if(getObject<DeliveryQuest>(i).isTheSameInstanceAs(thing))
                        return i;
                }
                break;

            case Quest.questType.kill:
                for(int i = 0; i < getObjectCount<KillQuest>(); i++) {
                    if(getObject<KillQuest>(i).isTheSameInstanceAs(thing))
                        return i;
                }
                break;

            case Quest.questType.rescue:
                for(int i = 0; i < getObjectCount<RescueQuest>(); i++) {
                    if(getObject<RescueQuest>(i).isTheSameInstanceAs(thing))
                        return i;
                }
                break;

            case Quest.questType.fishing:
                for(int i = 0; i < getObjectCount<FishingQuest>(); i++) {
                    if(getObject<FishingQuest>(i).isTheSameInstanceAs(thing))
                        return i;
                }
                break;
        }

        return -1;
    }
    public int getUnitStatsIndex(UnitStats thing) {
        for(int i = 0; i < getObjectCount<UnitStats>(); i++) {
            if(thing.isTheSameInstanceAs(getObject<UnitStats>(i)))
                return i;
        }
        return -1;
    }
    public int getBuildingIndex(Building thing) {
        if(thing == null)
            return -1;

        switch(thing.b_type) {
            case Building.type.Hospital:
                for(int i = 0; i < getObjectCount<HospitalBuilding>(); i++) {
                    if(getObject<HospitalBuilding>(i).isEqualTo((HospitalBuilding)thing))
                        return i;
                }
                break;
            case Building.type.Shop:
                for(int i = 0; i < getObjectCount<ShopBuilding>(); i++) {
                    if(getObject<ShopBuilding>(i).isEqualTo((ShopBuilding)thing))
                        return i;
                }
                break;
            case Building.type.Church:
                for(int i = 0; i < getObjectCount<ChurchBuilding>(); i++) {
                    if(getObject<ChurchBuilding>(i).isEqualTo((ChurchBuilding)thing))
                        return i;
                }
                break;
            case Building.type.Casino:
                for(int i = 0; i < getObjectCount<CasinoBuilding>(); i++) {
                    if(getObject<CasinoBuilding>(i).isEqualTo((CasinoBuilding)thing))
                        return i;
                }
                break;
            case Building.type.Blacksmith:
                for(int i = 0; i < getObjectCount<BlacksmithBuilding>(); i++) {
                    if(getObject<BlacksmithBuilding>(i).isEqualTo((BlacksmithBuilding)thing))
                        return i;
                }
                break;
        }

        return -1;
    }
    public int getMapLocationIndex(MapLocation thing) {
        if(thing == null)
            return -1;

        switch(thing.type) {
            case MapLocation.locationType.town:
                for(int i = 0; i < getObjectCount<TownLocation>(); i++) {
                    if(getObject<TownLocation>(i).isEqualTo((TownLocation)thing))
                        return i;
                }
                break;
            case MapLocation.locationType.pickup:
                for(int i = 0; i < getObjectCount<PickupLocation>(); i++) {
                    if(getObject<PickupLocation>(i).isEqualTo((PickupLocation)thing))
                        return i;
                }
                break;
            case MapLocation.locationType.upgrade:
                for(int i = 0; i < getObjectCount<UpgradeLocation>(); i++) {
                    if(getObject<UpgradeLocation>(i).isEqualTo((UpgradeLocation)thing))
                        return i;
                }
                break;
            case MapLocation.locationType.rescue:
                for(int i = 0; i < getObjectCount<RescueLocation>(); i++) {
                    if(getObject<RescueLocation>(i).isEqualTo((RescueLocation)thing))
                        return i;
                }
                break;
            case MapLocation.locationType.boss:
                for(int i = 0; i < getObjectCount<BossLocation>(); i++) {
                    if(getObject<BossLocation>(i).isEqualTo((BossLocation)thing))
                        return i;
                }
                break;
            case MapLocation.locationType.fishing:
                for(int i = 0; i < getObjectCount<FishingLocation>(); i++) {
                    if(getObject<FishingLocation>(i).isEqualTo((FishingLocation)thing))
                        return i;
                }
                break;
            case MapLocation.locationType.eye:
                for(int i = 0; i < getObjectCount<EyeLocation>(); i++) {
                    if(getObject<EyeLocation>(i).isEqualTo((EyeLocation)thing))
                        return i;
                }
                break;
            case MapLocation.locationType.bridge:
                for(int i = 0; i < getObjectCount<BridgeLocation>(); i++) {
                    if(getObject<BridgeLocation>(i).isEqualTo((BridgeLocation)thing))
                        return i;
                }
                break;
            case MapLocation.locationType.loot:
                for(int i = 0; i < getObjectCount<LootLocation>(); i++) {
                    if(getObject<LootLocation>(i).isEqualTo((LootLocation)thing))
                        return i;
                }
                break;
        }

        return -1;
    }

    //  get count
    public int getObjectCount<T>() {
        if(typeof(T) == typeof(Collectable))
            return getCollectableCount();
        if(typeof(T) == typeof(Quest))
            return getQuestCount();
        if(typeof(T) == typeof(Building))
            return getBuildingCount();
        if(typeof(T) == typeof(MapLocation))
            return getMapLocationCount();
        return SaveData.getInt(countTag<T>());
    }
    int getCollectableCount() {
        int count = 0;
        count += getObjectCount<Weapon>();
        count += getObjectCount<Armor>();
        count += getObjectCount<Item>();
        count += getObjectCount<Usable>();
        count += getObjectCount<Unusable>();
        return count;
    }
    int getQuestCount() {
        int count = 0;
        count += getObjectCount<BossFightQuest>();
        count += getObjectCount<DeliveryQuest>();
        count += getObjectCount<PickupQuest>();
        count += getObjectCount<KillQuest>();
        count += getObjectCount<RescueQuest>();
        count += getObjectCount<FishingQuest>();
        return count;
    }
    int getBuildingCount() {
        int count = 0;
        count += getObjectCount<HospitalBuilding>();
        count += getObjectCount<ShopBuilding>();
        count += getObjectCount<ChurchBuilding>();
        count += getObjectCount<CasinoBuilding>();
        count += getObjectCount<BlacksmithBuilding>();
        return count;
    }
    int getMapLocationCount() {
        int count = 0;
        count += getObjectCount<TownLocation>();
        count += getObjectCount<PickupLocation>();
        count += getObjectCount<UpgradeLocation>();
        count += getObjectCount<RescueLocation>();
        count += getObjectCount<BossLocation>();
        count += getObjectCount<FishingLocation>();
        count += getObjectCount<EyeLocation>();
        count += getObjectCount<BridgeLocation>();
        count += getObjectCount<LootLocation>();
        return count;
    }


    bool invalidTyping<T>() {
        return typeof(T) == typeof(Collectable) || typeof(T) == typeof(Quest) || typeof(T) == typeof(Building) || typeof(T) == typeof(MapLocation);
    }
}

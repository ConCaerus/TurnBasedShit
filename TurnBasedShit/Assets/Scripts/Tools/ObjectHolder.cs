using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectHolder {
    public int instanceID = -1;

    public enum changes {
        add, over, remove
    }

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

        int index = getObjectCount<T>();
        var data = JsonUtility.ToJson((T)thing);
        SaveData.setString(tag<T>(index), data);
        SaveData.setInt(countTag<T>(), index + 1);
    }
    void addCollectable(Collectable thing) {
        switch(thing.type) {
            case Collectable.collectableType.weapon:
                addObject<Weapon>((Weapon)thing);
                return;

            case Collectable.collectableType.armor:
                addObject<Armor>((Armor)thing);
                return;

            case Collectable.collectableType.item:
                addObject<Item>((Item)thing);
                return;

            case Collectable.collectableType.usable:
                addObject<Usable>((Usable)thing);
                return;

            case Collectable.collectableType.unusable:
                addObject<Unusable>((Unusable)thing);
                return;
        }
    }
    void addQuest(Quest thing) {
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

    //  override
    public void overrideObject<T>(int index, object thing) {
        var data = JsonUtility.ToJson((T)thing);
        SaveData.setString(tag<T>(index), data);
    }
    public void overrideCollectableOfSameInstance(Collectable thing) {
        switch(thing.type) {
            case Collectable.collectableType.weapon:
                var ws = getObjects<Weapon>();
                for(int i = 0; i < ws.Count; i++) {
                    if(ws[i].isTheSameInstanceAs(thing)) {
                        var data = JsonUtility.ToJson((Weapon)thing);
                        SaveData.setString(tag<Weapon>(i), data);
                        return;
                    }
                }
                return;

            case Collectable.collectableType.armor:
                var ass = getObjects<Armor>();
                for(int i = 0; i < ass.Count; i++) {
                    if(ass[i].isTheSameInstanceAs(thing)) {
                        var data = JsonUtility.ToJson((Armor)thing);
                        SaveData.setString(tag<Armor>(i), data);
                        return;
                    }
                }
                return;

            case Collectable.collectableType.item:
                var iss = getObjects<Item>();
                for(int i = 0; i < iss.Count; i++) {
                    if(iss[i].isTheSameInstanceAs(thing)) {
                        var data = JsonUtility.ToJson((Item)thing);
                        SaveData.setString(tag<Item>(i), data);
                        return;
                    }
                }
                return;

            case Collectable.collectableType.usable:
                var us = getObjects<Usable>();
                for(int i = 0; i < us.Count; i++) {
                    if(us[i].isTheSameInstanceAs(thing)) {
                        var data = JsonUtility.ToJson((Usable)thing);
                        SaveData.setString(tag<Usable>(i), data);
                        return;
                    }
                }
                return;

            case Collectable.collectableType.unusable:
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

    //  remove
    public void removeObject<T>(int index) {
        if(typeof(T) == typeof(Collectable) || typeof(T) == typeof(Quest)) {
            Debug.LogError("Use removeCollectalbe() instead");
            return;
        }

        List<T> temp = getObjects<T>();
        temp.RemoveAt(index);

        clearObjects<T>();
        foreach(var i in temp)
            addObject<T>(i);
    }
    public void removeCollectable(Collectable thing) {
        if(thing == null || thing.isEmpty())
            return;

        int startingIndex = 0;
        switch((thing).type) {
            case Collectable.collectableType.weapon:
                startingIndex = getCollectableIndex(thing);
                for(int i = startingIndex; i < getObjectCount<Weapon>(); i++) {
                    overrideObject<Weapon>(i, getObject<Weapon>(i + 1));
                }

                SaveData.setInt(countTag<Weapon>(), getObjectCount<Weapon>() - 1);
                break;

            case Collectable.collectableType.armor:
                startingIndex = getCollectableIndex(thing);
                for(int i = startingIndex; i < getObjectCount<Armor>(); i++) {
                    overrideObject<Armor>(i, getObject<Armor>(i + 1));
                }

                SaveData.setInt(countTag<Armor>(), getObjectCount<Armor>() - 1);
                break;

            case Collectable.collectableType.item:
                startingIndex = getCollectableIndex(thing);
                for(int i = startingIndex; i < getObjectCount<Item>(); i++) {
                    overrideObject<Item>(i, getObject<Item>(i + 1));
                }

                SaveData.setInt(countTag<Item>(), getObjectCount<Item>() - 1);
                break;

            case Collectable.collectableType.usable:
                startingIndex = getCollectableIndex(thing);
                for(int i = startingIndex; i < getObjectCount<Usable>(); i++) {
                    overrideObject<Usable>(i, getObject<Usable>(i + 1));
                }

                SaveData.setInt(countTag<Usable>(), getObjectCount<Usable>() - 1);
                break;

            case Collectable.collectableType.unusable:
                startingIndex = getCollectableIndex(thing);
                for(int i = startingIndex; i < getObjectCount<Unusable>(); i++) {
                    overrideObject<Unusable>(i, getObject<Unusable>(i + 1));
                }

                SaveData.setInt(countTag<Unusable>(), getObjectCount<Unusable>() - 1);
                break;
        }
    }
    public void removeQuest(Quest thing) {
        if(thing == null)
            return;

        int startingIndex;
        switch(thing.getQuestType()) {
            case Quest.questType.bossFight:
                startingIndex = getQuestIndex(thing);
                for(int i = startingIndex; i < getObjectCount<BossFightQuest>(); i++) {
                    overrideObject<BossFightQuest>(i, getObject<BossFightQuest>(i + 1));
                }

                SaveData.setInt(countTag<BossFightQuest>(), getObjectCount<BossFightQuest>() - 1);
                break;

            case Quest.questType.pickup:
                startingIndex = getQuestIndex(thing);
                for(int i = startingIndex; i < getObjectCount<PickupQuest>(); i++) {
                    overrideObject<PickupQuest>(i, getObject<PickupQuest>(i + 1));
                }

                SaveData.setInt(countTag<PickupQuest>(), getObjectCount<PickupQuest>() - 1);
                break;

            case Quest.questType.delivery:
                startingIndex = getQuestIndex(thing);
                for(int i = startingIndex; i < getObjectCount<DeliveryQuest>(); i++) {
                    overrideObject<DeliveryQuest>(i, getObject<DeliveryQuest>(i + 1));
                }

                SaveData.setInt(countTag<DeliveryQuest>(), getObjectCount<DeliveryQuest>() - 1);
                break;

            case Quest.questType.kill:
                startingIndex = getQuestIndex(thing);
                for(int i = startingIndex; i < getObjectCount<KillQuest>(); i++) {
                    overrideObject<KillQuest>(i, getObject<KillQuest>(i + 1));
                }

                SaveData.setInt(countTag<KillQuest>(), getObjectCount<KillQuest>() - 1);
                break;

            case Quest.questType.rescue:
                startingIndex = getQuestIndex(thing);
                for(int i = startingIndex; i < getObjectCount<RescueQuest>(); i++) {
                    overrideObject<RescueQuest>(i, getObject<RescueQuest>(i + 1));
                }

                SaveData.setInt(countTag<RescueQuest>(), getObjectCount<RescueQuest>() - 1);
                break;

            case Quest.questType.fishing:
                startingIndex = getQuestIndex(thing);
                for(int i = startingIndex; i < getObjectCount<FishingQuest>(); i++) {
                    overrideObject<FishingQuest>(i, getObject<FishingQuest>(i + 1));
                }

                SaveData.setInt(countTag<FishingQuest>(), getObjectCount<FishingQuest>() - 1);
                break;
        }
    }

    //  get multiple
    public List<T> getObjects<T>() {
        if(typeof(T) == typeof(Collectable)) {
            Debug.LogError("Use getCollectables() instead");
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

    //  get singular
    public T getObject<T>(int index) {
        if(typeof(T) == typeof(Collectable) || typeof(T) == typeof(Quest)) {
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
            case Collectable.collectableType.weapon:
                for(int i = 0; i < getObjectCount<Weapon>(); i++) {
                    if(getObject<Weapon>(i).isTheSameInstanceAs(thing))
                        return i;
                }
                break;

            case Collectable.collectableType.armor:
                for(int i = 0; i < getObjectCount<Armor>(); i++) {
                    if(getObject<Armor>(i).isTheSameInstanceAs(thing))
                        return i;
                }
                break;

            case Collectable.collectableType.item:
                for(int i = 0; i < getObjectCount<Item>(); i++) {
                    if(getObject<Item>(i).isTheSameInstanceAs(thing))
                        return i;
                }
                break;

            case Collectable.collectableType.usable:
                for(int i = 0; i < getObjectCount<Usable>(); i++) {
                    if(getObject<Usable>(i).isTheSameInstanceAs(thing))
                        return i;
                }
                break;

            case Collectable.collectableType.unusable:
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

    //  get count
    public int getObjectCount<T>() {
        if(typeof(T) == typeof(Collectable))
            return getCollectableCount();
        if(typeof(T) == typeof(Quest))
            return getQuestCount();
        return SaveData.getInt(countTag<T>());
    }
    public int getCollectableCount() {
        int count = 0;
        count += getObjectCount<Weapon>();
        count += getObjectCount<Armor>();
        count += getObjectCount<Item>();
        count += getObjectCount<Usable>();
        count += getObjectCount<Unusable>();
        return count;
    }
    public int getQuestCount() {
        int count = 0;
        count += getObjectCount<BossFightQuest>();
        count += getObjectCount<DeliveryQuest>();
        count += getObjectCount<PickupQuest>();
        count += getObjectCount<KillQuest>();
        count += getObjectCount<RescueQuest>();
        count += getObjectCount<FishingQuest>();
        return count;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//  Actual Inventory Script
public static class Collection {
    const string holderTag = "CollectionHolderTag";

    public static ObjectHolder getHolder() {
        var data = SaveData.getString(holderTag);
        return JsonUtility.FromJson<ObjectHolder>(data);
    }
    static void saveHolder(ObjectHolder holder) {
        var data = JsonUtility.ToJson(holder);
        SaveData.setString(holderTag, data);
    }


    public static void clear() {
        var holder = new ObjectHolder();
        holder.addObject<CollectionInfo>(new CollectionInfo());
        saveHolder(holder);
        
    }


    //  NOTE: already tried just adding plain ints to the object holder
    //  didn't work
    public static void addCollectable(Collectable col, PresetLibrary lib) {
        if(col == null)
            return;

        if(getHolder() == null)
            clear();

        var list = lib.getAllCollectables();
        var index = alreadyHasCollectable(col, list);
        if(index == -1)  //  already has collectable
            return;

        var holder = getHolder();
        var temp = holder.getObject<CollectionInfo>(0);
        temp.indexes.Add(index);
        temp.numberCollected++;
        holder.overrideObject<CollectionInfo>(0, temp);
        saveHolder(holder);
    }

    public static bool hasCollectable(Collectable col) {
        var holder = getHolder();
        switch(col.type) {
            case Collectable.collectableType.Weapon:
                foreach(var i in holder.getObjects<Weapon>()) {
                    if(i.isTheSameTypeAs(col))
                        return true;
                }
                break;

            case Collectable.collectableType.Armor:
                foreach(var i in holder.getObjects<Armor>()) {
                    if(i.isTheSameTypeAs(col))
                        return true;
                }
                break;

            case Collectable.collectableType.Item:
                foreach(var i in holder.getObjects<Item>()) {
                    if(i.isTheSameTypeAs(col))
                        return true;
                }
                break;

            case Collectable.collectableType.Usable:
                foreach(var i in holder.getObjects<Usable>()) {
                    if(i.isTheSameTypeAs(col))
                        return true;
                }
                break;

            case Collectable.collectableType.Unusable:
                foreach(var i in holder.getObjects<Unusable>()) {
                    if(i.isTheSameTypeAs(col))
                        return true;
                }
                break;
        }
        return false;
    }

    //  returns -1 if already has collectable; returns the index of the collectable in the list if not already has collectable
    public static int alreadyHasCollectable(Collectable col, List<Collectable> list) {
        var holder = getHolder();

        for(int i = 0; i < list.Count; i++) {
            if(list[i].isTheSameTypeAs(col)) {
                return holder.getObject<CollectionInfo>(0).indexes.Contains(i) ? -1 : i;
            }
        }
        return -1;
    }

    public static List<Usable> getUniqueUsables() {
        List<Usable> temp = new List<Usable>();
        for(int i = 0; i < getHolder().getObjectCount<Usable>(); i++) {
            var u = getHolder().getObject<Usable>(i);

            bool add = true;
            foreach(var t in temp) {
                if(t.isTheSameTypeAs(u)) {
                    add = false;
                    break;
                }
            }

            if(add)
                temp.Add(getHolder().getObject<Usable>(i));
        }
        return temp;
    }
    public static List<Unusable> getUniqueUnusables() {
        List<Unusable> temp = new List<Unusable>();
        for(int i = 0; i < getHolder().getObjectCount<Unusable>(); i++) {
            var u = getHolder().getObject<Unusable>(i);

            bool add = true;
            foreach(var t in temp) {
                if(t.isTheSameTypeAs(u)) {
                    add = false;
                    break;
                }
            }

            if(add)
                temp.Add(getHolder().getObject<Unusable>(i));
        }
        return temp;
    }

    public static Usable getFirstMatchingUsable(Usable u) {
        for(int i = 0; i < getHolder().getObjectCount<Usable>(); i++) {
            if(getHolder().getObject<Usable>(i).isTheSameTypeAs(u))
                return getHolder().getObject<Usable>(i);
        }
        return null;
    }
    public static int getNumberOfMatchingUsables(Usable con) {
        int count = 0;
        for(int i = 0; i < getHolder().getObjectCount<Usable>(); i++) {
            if(getHolder().getObject<Usable>(i).isTheSameTypeAs(con))
                count++;
        }
        return count;
    }
    public static Unusable getFirstMatchingUnusable(Unusable u) {
        for(int i = 0; i < getHolder().getObjectCount<Unusable>(); i++) {
            if(getHolder().getObject<Unusable>(i).isTheSameTypeAs(u))
                return getHolder().getObject<Unusable>(i);
        }
        return null;
    }
    public static int getNumberOfMatchingUnusables(Unusable con) {
        int count = 0;
        for(int i = 0; i < getHolder().getObjectCount<Unusable>(); i++) {
            if(getHolder().getObject<Unusable>(i).isTheSameTypeAs(con))
                count++;
        }
        return count;
    }
}


[System.Serializable]
public class CollectionInfo {
    public List<int> indexes = new List<int>();
    public int numberCollected = 0;
}
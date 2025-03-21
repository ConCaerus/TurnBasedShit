﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActiveQuests {

    const string questTag = "ActiveQuestHolderTag";

    public static ObjectHolder getHolder() {
        var data = SaveData.getString(questTag);
        return JsonUtility.FromJson<ObjectHolder>(data);
    }
    static void saveHolder(ObjectHolder holder) {
        var data = JsonUtility.ToJson(holder);
        SaveData.setString(questTag, data);
    }


    public static void clear(bool clearInstanceQueue) {
        saveHolder(new ObjectHolder());

        if(clearInstanceQueue)
            GameInfo.clearQuestInstanceIDQueue();
    }
    public static void addQuest(Quest qu) {
        if(qu == null)
            return;

        if(getHolder() == null)
            saveHolder(new ObjectHolder());

        var holder = getHolder();
        holder.addObject<Quest>(qu);
        saveHolder(holder);
    }
    public static void overrideQuest(int index, Quest qu) {
        if(qu == null || index == -1)
            return;

        var holder = getHolder();
        holder.overrideObject<Quest>(index, qu);
        saveHolder(holder);
    }
    public static void removeQuest(Quest qu) {
        if(qu == null)
            return;
        var holder = getHolder();
        holder.removeQuest(qu);
        saveHolder(holder);
    }

    public static bool hasQuest(Quest qu) {
        foreach(var i in getHolder().getQuests()) {
            if(i.isTheSameInstanceAs(qu))
                return true;
        }
        return false;
    }


    public static void completeQuest(Quest qu, QuestCompleteCanvas canvas) {
        canvas.showCanvas();
        qu.completed = true;
        overrideQuest(getHolder().getQuestIndex(qu), qu);
    }

    /*
    public static string countTag(Quest.questType type) {
        if(type == Quest.questType.kill)
            return "KillQuestCount";

        else if(type == Quest.questType.bossFight)
            return "BossFightQuestCount";

        else if(type == Quest.questType.delivery)
            return "DeliveryQuestCount";

        else if(type == Quest.questType.pickup)
            return "PickupQuestCount";

        else if(type == Quest.questType.rescue)
            return "RescueQuestCount";

        else if(type == Quest.questType.fishing)
            return "FishingQuestCount";

        return null;
    }

    public static string tag(int index, Quest.questType type) {
        if(type == Quest.questType.kill)
            return "KillQuest" + index.ToString();

        else if(type == Quest.questType.bossFight)
            return "BossFightQuest" + index.ToString();

        else if(type == Quest.questType.delivery)
            return "DeliveryQuest" + index.ToString();

        else if(type == Quest.questType.pickup)
            return "PickupQuest" + index.ToString();

        else if(type == Quest.questType.rescue)
            return "RescueQuest" + index.ToString();

        else if(type == Quest.questType.fishing)
            return "FishingQuest" + index.ToString();

        return null;
    }


    public static void clear(bool clearInstanceQueue) {
        clearKillQuests();
        clearBossFightQuests();
        clearDeliveryQuests();
        clearPickupQuests();
        clearRescueQuests();
        clearFishingQuests();

        if(clearInstanceQueue)
            GameInfo.clearQuestInstanceIDQueue();
    }
    public static void clearKillQuests() {
        for(int i = 0; i < getKillQuestCount(); i++) {
            SaveData.deleteKey(tag(i, Quest.questType.kill));
        }
        SaveData.deleteKey(countTag(Quest.questType.kill));
    }
    public static void clearBossFightQuests() {
        for(int i = 0; i < getBossFightQuestCount(); i++) {
            SaveData.deleteKey(tag(i, Quest.questType.bossFight));
        }
        SaveData.deleteKey(countTag(Quest.questType.bossFight));
    }
    public static void clearDeliveryQuests() {
        for(int i = 0; i < getDeliveryQuestCount(); i++) {
            SaveData.deleteKey(tag(i, Quest.questType.delivery));
        }
        SaveData.deleteKey(countTag(Quest.questType.delivery));
    }
    public static void clearPickupQuests() {
        for(int i = 0; i < getPickupQuestCount(); i++) {
            SaveData.deleteKey(tag(i, Quest.questType.pickup));
        }
        SaveData.deleteKey(countTag(Quest.questType.pickup));
    }
    public static void clearRescueQuests() {
        for(int i = 0; i < getRescueQuestCount(); i++) {
            SaveData.deleteKey(tag(i, Quest.questType.rescue));
        }
        SaveData.deleteKey(countTag(Quest.questType.rescue));
    }
    public static void clearFishingQuests() {
        for(int i = 0; i < getFishingQuestCount(); i++) {
            SaveData.deleteKey(tag(i, Quest.questType.fishing));
        }
        SaveData.deleteKey(countTag(Quest.questType.fishing));
    }

    public static void addQuest(Quest qu) {
        switch(qu.getQuestType()) {
            case Quest.questType.bossFight: addQuest((BossFightQuest)qu); break;
            case Quest.questType.pickup: addQuest((PickupQuest)qu); break;
            case Quest.questType.delivery: addQuest((DeliveryQuest)qu); break;
            case Quest.questType.kill: addQuest((KillQuest)qu); break;
            case Quest.questType.rescue: addQuest((RescueQuest)qu); break;
            case Quest.questType.fishing: addQuest((FishingQuest)qu); break;
        }
    }
    public static void addQuest(KillQuest qu) {
        int index = getKillQuestCount();

        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, Quest.questType.kill), data);
        SaveData.setInt(countTag(Quest.questType.kill), index + 1);
    }
    public static void addQuest(BossFightQuest qu) {
        int index = getBossFightQuestCount();

        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, Quest.questType.bossFight), data);
        SaveData.setInt(countTag(Quest.questType.bossFight), index + 1);
    }
    public static void addQuest(DeliveryQuest qu) {
        int index = getDeliveryQuestCount();

        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, Quest.questType.delivery), data);
        SaveData.setInt(countTag(Quest.questType.delivery), index + 1);
    }
    public static void addQuest(PickupQuest qu) {
        int index = getPickupQuestCount();

        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, Quest.questType.pickup), data);
        SaveData.setInt(countTag(Quest.questType.pickup), index + 1);
    }
    public static void addQuest(RescueQuest qu) {
        int index = getRescueQuestCount();

        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, Quest.questType.rescue), data);
        SaveData.setInt(countTag(Quest.questType.rescue), index + 1);
    }
    public static void addQuest(FishingQuest qu) {
        int index = getPickupQuestCount();

        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, Quest.questType.pickup), data);
        SaveData.setInt(countTag(Quest.questType.pickup), index + 1);
    }


    public static void removeQuest(KillQuest quest) {
        if(quest == null)
            return;
        List<KillQuest> temp = new List<KillQuest>();
        for(int i = 0; i < getKillQuestCount(); i++) {
            var qu = getKillQuest(i);
            if(qu != null && !qu.isTheSameInstanceAs(quest))
                temp.Add(qu);
        }

        clearKillQuests();
        foreach(var i in temp)
            addQuest(i);
    }
    public static void removeQuest(BossFightQuest quest) {
        if(quest == null)
            return;
        List<BossFightQuest> temp = new List<BossFightQuest>();
        for(int i = 0; i < getBossFightQuestCount(); i++) {
            var qu = getBossFightQuest(i);
            if(qu != null && !qu.isTheSameInstanceAs(quest))
                temp.Add(qu);
        }

        clearBossFightQuests();
        foreach(var i in temp)
            addQuest(i);
    }
    public static void removeQuest(DeliveryQuest quest) {
        if(quest == null)
            return;
        List<DeliveryQuest> temp = new List<DeliveryQuest>();
        for(int i = 0; i < getDeliveryQuestCount(); i++) {
            var qu = getDeliveryQuest(i);
            if(qu != null && !qu.isTheSameInstanceAs(quest))
                temp.Add(qu);
        }

        clearDeliveryQuests();
        foreach(var i in temp)
            addQuest(i);
    }
    public static void removeQuest(PickupQuest quest) {
        if(quest == null)
            return;
        List<PickupQuest> temp = new List<PickupQuest>();
        for(int i = 0; i < getPickupQuestCount(); i++) {
            var qu = getPickupQuest(i);
            if(qu != null && !qu.isTheSameInstanceAs(quest))
                temp.Add(qu);
        }

        clearPickupQuests();
        foreach(var i in temp)
            addQuest(i);
    }
    public static void removeQuest(RescueQuest quest) {
        if(quest == null)
            return;
        List<RescueQuest> temp = new List<RescueQuest>();
        for(int i = 0; i < getRescueQuestCount(); i++) {
            var qu = getRescueQuest(i);
            if(qu != null && !qu.isTheSameInstanceAs(quest))
                temp.Add(qu);
        }

        clearRescueQuests();
        foreach(var i in temp)
            addQuest(i);
    }

    public static void overrideQuest(int index, KillQuest qu) {
        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, Quest.questType.kill), data);
    }
    public static void overrideQuest(int index, BossFightQuest qu) {
        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, Quest.questType.bossFight), data);
    }
    public static void overrideQuest(int index, DeliveryQuest qu) {
        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, Quest.questType.delivery), data);
    }
    public static void overrideQuest(int index, PickupQuest qu) {
        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, Quest.questType.pickup), data);
    }
    public static void overrideQuest(int index, RescueQuest qu) {
        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, Quest.questType.rescue), data);
    }

    public static KillQuest getKillQuest(int index) {
        var data = SaveData.getString(tag(index, Quest.questType.kill));
        return JsonUtility.FromJson<KillQuest>(data);
    }
    public static BossFightQuest getBossFightQuest(int index) {
        var data = SaveData.getString(tag(index, Quest.questType.bossFight));
        return JsonUtility.FromJson<BossFightQuest>(data);
    }
    public static DeliveryQuest getDeliveryQuest(int index) {
        var data = SaveData.getString(tag(index, Quest.questType.delivery));
        return JsonUtility.FromJson<DeliveryQuest>(data);
    }
    public static PickupQuest getPickupQuest(int index) {
        var data = SaveData.getString(tag(index, Quest.questType.pickup));
        return JsonUtility.FromJson<PickupQuest>(data);
    }
    public static RescueQuest getRescueQuest(int index) {
        var data = SaveData.getString(tag(index, Quest.questType.rescue));
        return JsonUtility.FromJson<RescueQuest>(data);
    }

    public static int getQuestCount() {
        int count = 0;
        count += SaveData.getInt(countTag(Quest.questType.kill));
        count += SaveData.getInt(countTag(Quest.questType.bossFight));
        count += SaveData.getInt(countTag(Quest.questType.delivery));
        count += SaveData.getInt(countTag(Quest.questType.pickup));
        return count;
    }
    public static int getKillQuestCount() {
        return SaveData.getInt(countTag(Quest.questType.kill));
    }
    public static int getBossFightQuestCount() {
        return SaveData.getInt(countTag(Quest.questType.bossFight));
    }
    public static int getDeliveryQuestCount() {
        return SaveData.getInt(countTag(Quest.questType.delivery));
    }
    public static int getPickupQuestCount() {
        return SaveData.getInt(countTag(Quest.questType.pickup));
    }
    public static int getRescueQuestCount() {
        return SaveData.getInt(countTag(Quest.questType.rescue));
    }

    public static bool hasQuest(Quest qu) {
        if(qu == null)
            return false;
        switch(qu.getQuestType()) {
            case Quest.questType.bossFight: return hasQuest((BossFightQuest)qu);
            case Quest.questType.pickup: return hasQuest((PickupQuest)qu);
            case Quest.questType.delivery: return hasQuest((DeliveryQuest)qu);
            case Quest.questType.kill: return hasQuest((KillQuest)qu);
            case Quest.questType.rescue: return hasQuest((RescueQuest)qu);
            case Quest.questType.fishing: return hasQuest((FishingQuest)qu);
        }
        return false;
    }
    public static bool hasQuest(KillQuest q) {
        if(q == null)
            return false;
        for(int i = 0; i < getKillQuestCount(); i++) {
            if(getKillQuest(i).isTheSameInstanceAs(q))
                return true;
        }

        return false;
    }
    public static bool hasQuest(BossFightQuest q) {
        if(q == null)
            return false;
        for(int i = 0; i < getBossFightQuestCount(); i++) {
            if(getBossFightQuest(i).isTheSameInstanceAs(q))
                return true;
        }

        return false;
    }
    public static bool hasQuest(DeliveryQuest q) {
        if(q == null)
            return false;
        for(int i = 0; i < getDeliveryQuestCount(); i++) {
            if(getDeliveryQuest(i).isTheSameInstanceAs(q))
                return true;
        }

        return false;
    }
    public static bool hasQuest(PickupQuest q) {
        if(q == null)
            return false;
        for(int i = 0; i < getPickupQuestCount(); i++) {
            if(getPickupQuest(i).isTheSameInstanceAs(q))
                return true;
        }

        return false;
    }
    public static bool hasQuest(RescueQuest q) {
        if(q == null)
            return false;
        for(int i = 0; i < getRescueQuestCount(); i++) {
            if(getRescueQuest(i).isTheSameInstanceAs(q))
                return true;
        }

        return false;
    }
    public static bool hasQuest(FishingQuest q) {
        if(q == null)
            return false;
        for(int i = 0; i < getFishingQuestCount(); i++) {
            if(getRescueQuest(i).isTheSameInstanceAs(q))
                return true;
        }

        return false;
    }

    public static bool hasQuestWithInstanceID(int id) {
        for(int i = 0; i < getKillQuestCount(); i++) {
            if(getKillQuest(i).instanceID == id)
                return true;
        }
        for(int i = 0; i < getBossFightQuestCount(); i++) {
            if(getBossFightQuest(i).instanceID == id)
                return true;
        }
        for(int i = 0; i < getDeliveryQuestCount(); i++) {
            if(getDeliveryQuest(i).instanceID == id)
                return true;
        }
        for(int i = 0; i < getPickupQuestCount(); i++) {
            if(getPickupQuest(i).instanceID == id)
                return true;
        }
        for(int i = 0; i < getRescueQuestCount(); i++) {
            if(getPickupQuest(i).instanceID == id)
                return true;
        }
        return false;
    }
    public static KillQuest getKillQuestWithInstanceID(int id) {
        for(int i = 0; i < getKillQuestCount(); i++) {
            if(getKillQuest(i).instanceID == id)
                return getKillQuest(i);
        }
        return null;
    }
    public static BossFightQuest getBossFightQuestWithInstanceID(int id) {
        for(int i = 0; i < getBossFightQuestCount(); i++) {
            if(getBossFightQuest(i).instanceID == id)
                return getBossFightQuest(i);
        }
        return null;
    }
    public static DeliveryQuest getDeliveryQuestWithInstanceID(int id) {
        for(int i = 0; i < getDeliveryQuestCount(); i++) {
            if(getDeliveryQuest(i).instanceID == id)
                return getDeliveryQuest(i);
        }
        return null;
    }
    public static PickupQuest getPickupQuestWithInstanceID(int id) {
        for(int i = 0; i < getPickupQuestCount(); i++) {
            if(getPickupQuest(i).instanceID == id)
                return getPickupQuest(i);
        }
        return null;
    }
    public static RescueQuest getRescueQuestWithInstanceID(int id) {
        for(int i = 0; i < getRescueQuestCount(); i++) {
            if(getRescueQuest(i).instanceID == id)
                return getRescueQuest(i);
        }
        return null;
    }

    public static List<KillQuest> getAllKillQuests() {
        var temp = new List<KillQuest>();
        for(int i = 0; i < getKillQuestCount(); i++)
            temp.Add(getKillQuest(i));
        return temp;
    }
    public static List<BossFightQuest> getAllBossFightQuests() {
        var temp = new List<BossFightQuest>();
        for(int i = 0; i < getBossFightQuestCount(); i++)
            temp.Add(getBossFightQuest(i));
        return temp;
    }
    public static List<DeliveryQuest> getAllDeliveryQuests() {
        var temp = new List<DeliveryQuest>();
        for(int i = 0; i < getDeliveryQuestCount(); i++)
            temp.Add(getDeliveryQuest(i));
        return temp;
    }
    public static List<PickupQuest> getAllPickupQuests() {
        var temp = new List<PickupQuest>();
        for(int i = 0; i < getPickupQuestCount(); i++)
            temp.Add(getPickupQuest(i));
        return temp;
    }
    public static List<RescueQuest> getAllRescueQuests() {
        var temp = new List<RescueQuest>();
        for(int i = 0; i < getRescueQuestCount(); i++)
            temp.Add(getRescueQuest(i));
        return temp;
    }

    public static int getMostRecentQuestInstanceID() {
        int temp = 0;
        foreach(var i in getAllKillQuests()) {
            if(i.instanceID > temp)
                temp = i.instanceID;
        }
        foreach(var i in getAllBossFightQuests()) {
            if(i.instanceID > temp)
                temp = i.instanceID;
        }
        foreach(var i in getAllDeliveryQuests()) {
            if(i.instanceID > temp)
                temp = i.instanceID;
        }
        foreach(var i in getAllPickupQuests()) {
            if(i.instanceID > temp)
                temp = i.instanceID;
        }
        foreach(var i in getAllRescueQuests()) {
            if(i.instanceID > temp)
                temp = i.instanceID;
        }

        return temp;
    }*/
}

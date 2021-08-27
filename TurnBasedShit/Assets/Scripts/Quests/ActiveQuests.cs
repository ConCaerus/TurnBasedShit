using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActiveQuests {
    public static string countTag(GameInfo.questType type) {
        if(type == GameInfo.questType.kill)
            return "KillQuestCount";

        else if(type == GameInfo.questType.bossFight)
            return "BossFightQuestCount";

        else if(type == GameInfo.questType.delivery)
            return "DeliveryQuestCount";

        else if(type == GameInfo.questType.pickup)
            return "PickupQuestCount";

        return null;
    }

    public static string tag(int index, GameInfo.questType type) {
        if(type == GameInfo.questType.kill)
            return "KillQuest" + index.ToString();

        else if(type == GameInfo.questType.bossFight)
            return "BossFightQuest" + index.ToString();

        else if(type == GameInfo.questType.delivery)
            return "DeliveryQuest" + index.ToString();

        else if(type == GameInfo.questType.pickup)
            return "PickupQuest" + index.ToString();

        return null;
    }


    public static void clear() {
        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.kill); i++) {
            var data = SaveData.getString(tag(i, GameInfo.questType.kill));
            var thing = JsonUtility.FromJson<KillQuest>(data);
            thing.questDestory();

            SaveData.deleteKey(tag(i, GameInfo.questType.kill));
        }

        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.bossFight); i++) {
            var data = SaveData.getString(tag(i, GameInfo.questType.bossFight));
            var thing = JsonUtility.FromJson<BossFightQuest>(data);
            thing.questDestory();

            SaveData.deleteKey(tag(i, GameInfo.questType.bossFight));
        }

        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.delivery); i++) {
            var data = SaveData.getString(tag(i, GameInfo.questType.delivery));
            var thing = JsonUtility.FromJson<DeliveryQuest>(data);
            thing.questDestory();

            SaveData.deleteKey(tag(i, GameInfo.questType.delivery));
        }

        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.pickup); i++) {
            var data = SaveData.getString(tag(i, GameInfo.questType.pickup));
            var thing = JsonUtility.FromJson<PickupQuest>(data);
            thing.questDestory();

            SaveData.deleteKey(tag(i, GameInfo.questType.pickup));
        }

        SaveData.deleteKey(countTag(GameInfo.questType.kill));
        SaveData.deleteKey(countTag(GameInfo.questType.bossFight));
        SaveData.deleteKey(countTag(GameInfo.questType.delivery));
        SaveData.deleteKey(countTag(GameInfo.questType.pickup));
    }

    public static void addQuest(KillQuest qu) {
        int index = getQuestTypeCount(GameInfo.questType.kill);

        qu.questInit();

        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, GameInfo.questType.kill), data);
        SaveData.setInt(countTag(GameInfo.questType.kill), index + 1);
    }
    public static void addQuest(BossFightQuest qu) {
        int index = getQuestTypeCount(GameInfo.questType.bossFight);

        qu.questInit();

        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, GameInfo.questType.bossFight), data);
        SaveData.setInt(countTag(GameInfo.questType.bossFight), index + 1);
    }
    public static void addQuest(DeliveryQuest qu) {
        int index = getQuestTypeCount(GameInfo.questType.delivery);

        qu.questInit();

        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, GameInfo.questType.delivery), data);
        SaveData.setInt(countTag(GameInfo.questType.delivery), index + 1);
    }
    public static void addQuest(PickupQuest qu) {
        int index = getQuestTypeCount(GameInfo.questType.pickup);

        qu.questInit();

        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, GameInfo.questType.pickup), data);
        SaveData.setInt(countTag(GameInfo.questType.pickup), index + 1);
    }


    public static void removeQuest(KillQuest quest) {
        GameInfo.questType type = GameInfo.questType.kill;
        var tData = JsonUtility.ToJson(quest);
        bool past = false;
        for(int i = 0; i < getQuestTypeCount(type); i++) {
            var data = SaveData.getString(tag(i, type));

            if(data == tData && !past) {
                var thing = JsonUtility.FromJson<KillQuest>(data);
                thing.questDestory();

                SaveData.deleteKey(tag(i, type));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(tag(i, type));
                overrideQuest(i - 1, JsonUtility.FromJson<KillQuest>(data));
            }
        }
        SaveData.setInt(countTag(type), SaveData.getInt(countTag(type)) - 1);
    }
    public static void removeQuest(BossFightQuest quest) {
        GameInfo.questType type = GameInfo.questType.bossFight;
        var tData = JsonUtility.ToJson(quest);
        bool past = false;
        for(int i = 0; i < getQuestTypeCount(type); i++) {
            var data = SaveData.getString(tag(i, type));

            if(data == tData && !past) {
                var thing = JsonUtility.FromJson<BossFightQuest>(data);
                thing.questDestory();

                SaveData.deleteKey(tag(i, type));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(tag(i, type));
                overrideQuest(i - 1, JsonUtility.FromJson<BossFightQuest>(data));
            }
        }
        SaveData.setInt(countTag(type), SaveData.getInt(countTag(type)) - 1);
    }
    public static void removeQuest(DeliveryQuest quest) {
        GameInfo.questType type = GameInfo.questType.delivery;
        var tData = JsonUtility.ToJson(quest);
        bool past = false;
        for(int i = 0; i < getQuestTypeCount(type); i++) {
            var data = SaveData.getString(tag(i, type));

            if(data == tData && !past) {
                var thing = JsonUtility.FromJson<DeliveryQuest>(data);
                thing.questDestory();

                SaveData.deleteKey(tag(i, type));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(tag(i, type));
                overrideQuest(i - 1, JsonUtility.FromJson<DeliveryQuest>(data));
            }
        }
        SaveData.setInt(countTag(type), SaveData.getInt(countTag(type)) - 1);
    }
    public static void removeQuest(PickupQuest quest) {
        GameInfo.questType type = GameInfo.questType.pickup;
        var tData = JsonUtility.ToJson(quest);
        bool past = false;
        for(int i = 0; i < getQuestTypeCount(type); i++) {
            var data = SaveData.getString(tag(i, type));

            if(data == tData && !past) {
                var thing = JsonUtility.FromJson<PickupQuest>(data);
                thing.questDestory();

                SaveData.deleteKey(tag(i, type));
                past = true;
                continue;
            }
            else if(past) {
                SaveData.deleteKey(tag(i, type));
                overrideQuest(i - 1, JsonUtility.FromJson<PickupQuest>(data));
            }
        }
        SaveData.setInt(countTag(type), SaveData.getInt(countTag(type)) - 1);
    }

    public static void overrideQuest(int index, KillQuest qu) {
        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, GameInfo.questType.kill), data);
    }
    public static void overrideQuest(int index, BossFightQuest qu) {
        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, GameInfo.questType.bossFight), data);
    }
    public static void overrideQuest(int index, DeliveryQuest qu) {
        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, GameInfo.questType.delivery), data);
    }
    public static void overrideQuest(int index, PickupQuest qu) {
        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, GameInfo.questType.pickup), data);
    }

    public static KillQuest getKillQuest(int index) {
        var data = SaveData.getString(tag(index, GameInfo.questType.kill));
        return JsonUtility.FromJson<KillQuest>(data);
    }
    public static BossFightQuest getBossFightQuest(int index) {
        var data = SaveData.getString(tag(index, GameInfo.questType.bossFight));
        return JsonUtility.FromJson<BossFightQuest>(data);
    }
    public static DeliveryQuest getDeliveryQuest(int index) {
        var data = SaveData.getString(tag(index, GameInfo.questType.delivery));
        return JsonUtility.FromJson<DeliveryQuest>(data);
    }
    public static PickupQuest getPickupQuest(int index) {
        var data = SaveData.getString(tag(index, GameInfo.questType.pickup));
        return JsonUtility.FromJson<PickupQuest>(data);
    }

    public static int getQuestTypeCount(GameInfo.questType type) {
        return SaveData.getInt(countTag(type));
    }
    public static int getQuestCount() {
        int count = 0;
        count += SaveData.getInt(countTag(GameInfo.questType.kill));
        count += SaveData.getInt(countTag(GameInfo.questType.bossFight));
        count += SaveData.getInt(countTag(GameInfo.questType.delivery));
        count += SaveData.getInt(countTag(GameInfo.questType.pickup));
        return count;
    }

    public static bool hasQuest(KillQuest q) {
        if(q == null)
            return false;
        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.kill); i++) {
            if(getKillQuest(i).isEqualTo(q))
                return true;
        }

        return false;
    }
    public static bool hasQuest(BossFightQuest q) {
        if(q == null)
            return false;
        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.bossFight); i++) {
            if(getBossFightQuest(i).isEqualTo(q))
                return true;
        }

        return false;
    }
    public static bool hasQuest(DeliveryQuest q) {
        if(q == null)
            return false;
        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.delivery); i++) {
            if(getDeliveryQuest(i).isEqualTo(q))
                return true;
        }

        return false;
    }
    public static bool hasQuest(PickupQuest q) {
        if(q == null)
            return false;
        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.pickup); i++) {
            if(getPickupQuest(i).isEqualTo(q))
                return true;
        }

        return false;
    }

    public static bool hasQuestWithInstanceID(int id) {
        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.kill); i++) {
            if(getKillQuest(i).q_instanceID == id)
                return true;
        }
        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.bossFight); i++) {
            if(getBossFightQuest(i).q_instanceID == id)
                return true;
        }
        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.delivery); i++) {
            if(getDeliveryQuest(i).q_instanceID == id)
                return true;
        }
        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.pickup); i++) {
            if(getPickupQuest(i).q_instanceID == id)
                return true;
        }
        return false;
    }
    public static KillQuest getKillQuestWithInstanceID(int id) {
        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.kill); i++) {
            if(getKillQuest(i).q_instanceID == id)
                return getKillQuest(i);
        }
        return null;
    }
    public static BossFightQuest getBossFightQuestWithInstanceID(int id) {
        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.bossFight); i++) {
            if(getBossFightQuest(i).q_instanceID == id)
                return getBossFightQuest(i);
        }
        return null;
    }
    public static DeliveryQuest getDeliveryQuestWithInstanceID(int id) {
        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.delivery); i++) {
            if(getDeliveryQuest(i).q_instanceID == id)
                return getDeliveryQuest(i);
        }
        return null;
    }
    public static PickupQuest getPickupQuestWithInstanceID(int id) {
        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.pickup); i++) {
            if(getPickupQuest(i).q_instanceID == id)
                return getPickupQuest(i);
        }
        return null;
    }

    public static List<KillQuest> getAllKillQuests() {
        var temp = new List<KillQuest>();
        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.kill); i++)
            temp.Add(getKillQuest(i));
        return temp;
    }
    public static List<BossFightQuest> getAllBossFightQuests() {
        var temp = new List<BossFightQuest>();
        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.bossFight); i++)
            temp.Add(getBossFightQuest(i));
        return temp;
    }
    public static List<DeliveryQuest> getAllDeliveryQuests() {
        var temp = new List<DeliveryQuest>();
        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.delivery); i++)
            temp.Add(getDeliveryQuest(i));
        return temp;
    }
    public static List<PickupQuest> getAllPickupQuests() {
        var temp = new List<PickupQuest>();
        for(int i = 0; i < getQuestTypeCount(GameInfo.questType.pickup); i++)
            temp.Add(getPickupQuest(i));
        return temp;
    }

    public static int getMostRecentQuestInstanceID() {
        int temp = 0;
        foreach(var i in getAllKillQuests()) {
            if(i.q_instanceID > temp)
                temp = i.q_instanceID;
        }
        foreach(var i in getAllBossFightQuests()) {
            if(i.q_instanceID > temp)
                temp = i.q_instanceID;
        }
        foreach(var i in getAllDeliveryQuests()) {
            if(i.q_instanceID > temp)
                temp = i.q_instanceID;
        }
        foreach(var i in getAllPickupQuests()) {
            if(i.q_instanceID > temp)
                temp = i.q_instanceID;
        }

        return temp;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActiveQuests {
    public static string countTag(System.Type type) {
        if(type == typeof(KillQuest))
            return "KillQuestCount";

        else if(type == typeof(BossFightQuest))
            return "BossFightQuestCount";

        else if(type == typeof(DeliveryQuest))
            return "DeliveryQuestCount";

        else if(type == typeof(PickupQuest))
            return "PickupQuestCount";

        return null;
    }

    public static string tag(int index, System.Type type) {
        if(type == typeof(KillQuest))
            return "KillQuest" + index.ToString();

        else if(type == typeof(BossFightQuest))
            return "BossFightQuest" + index.ToString();

        else if(type == typeof(DeliveryQuest))
            return "DeliveryQuest" + index.ToString();

        else if(type == typeof(PickupQuest))
            return "PickupQuest" + index.ToString();

        return null;
    }


    public static void clear() {
        for(int i = 0; i < getQuestTypeCount(typeof(KillQuest)); i++) {
            var data = SaveData.getString(tag(i, typeof(KillQuest)));
            var thing = JsonUtility.FromJson<KillQuest>(data);
            thing.questDestory();

            SaveData.deleteKey(tag(i, typeof(KillQuest)));
        }

        for(int i = 0; i < getQuestTypeCount(typeof(BossFightQuest)); i++) {
            var data = SaveData.getString(tag(i, typeof(BossFightQuest)));
            var thing = JsonUtility.FromJson<BossFightQuest>(data);
            thing.questDestory();

            SaveData.deleteKey(tag(i, typeof(BossFightQuest)));
        }

        for(int i = 0; i < getQuestTypeCount(typeof(DeliveryQuest)); i++) {
            var data = SaveData.getString(tag(i, typeof(DeliveryQuest)));
            var thing = JsonUtility.FromJson<DeliveryQuest>(data);
            thing.questDestory();

            SaveData.deleteKey(tag(i, typeof(DeliveryQuest)));
        }

        for(int i = 0; i < getQuestTypeCount(typeof(PickupQuest)); i++) {
            var data = SaveData.getString(tag(i, typeof(PickupQuest)));
            var thing = JsonUtility.FromJson<PickupQuest>(data);
            thing.questDestory();

            SaveData.deleteKey(tag(i, typeof(PickupQuest)));
        }

        SaveData.deleteKey(countTag(typeof(KillQuest)));
        SaveData.deleteKey(countTag(typeof(BossFightQuest)));
        SaveData.deleteKey(countTag(typeof(DeliveryQuest)));
        SaveData.deleteKey(countTag(typeof(PickupQuest)));
    }

    public static void addQuest(KillQuest qu) {
        int index = getQuestTypeCount(typeof(KillQuest));

        qu.questInit();

        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, typeof(KillQuest)), data);
        SaveData.setInt(countTag(typeof(KillQuest)), index + 1);
    }
    public static void addQuest(BossFightQuest qu) {
        int index = getQuestTypeCount(typeof(BossFightQuest));

        qu.questInit();

        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, typeof(BossFightQuest)), data);
        SaveData.setInt(countTag(typeof(BossFightQuest)), index + 1);
    }
    public static void addQuest(DeliveryQuest qu) {
        int index = getQuestTypeCount(typeof(DeliveryQuest));

        qu.questInit();

        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, typeof(DeliveryQuest)), data);
        SaveData.setInt(countTag(typeof(DeliveryQuest)), index + 1);
    }
    public static void addQuest(PickupQuest qu) {
        int index = getQuestTypeCount(typeof(PickupQuest));

        qu.questInit();

        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, typeof(PickupQuest)), data);
        SaveData.setInt(countTag(typeof(PickupQuest)), index + 1);
    }


    public static void removeQuest(KillQuest quest) {
        System.Type type = typeof(KillQuest);
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
        System.Type type = typeof(BossFightQuest);
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
        System.Type type = typeof(DeliveryQuest);
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
        System.Type type = typeof(PickupQuest);
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
        SaveData.setString(tag(index, typeof(KillQuest)), data);
    }
    public static void overrideQuest(int index, BossFightQuest qu) {
        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, typeof(BossFightQuest)), data);
    }
    public static void overrideQuest(int index, DeliveryQuest qu) {
        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, typeof(DeliveryQuest)), data);
    }
    public static void overrideQuest(int index, PickupQuest qu) {
        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, typeof(PickupQuest)), data);
    }

    public static KillQuest getKillQuest(int index) {
        var data = SaveData.getString(tag(index, typeof(KillQuest)));
        return JsonUtility.FromJson<KillQuest>(data);
    }
    public static BossFightQuest getBossFightQuest(int index) {
        var data = SaveData.getString(tag(index, typeof(BossFightQuest)));
        return JsonUtility.FromJson<BossFightQuest>(data);
    }
    public static DeliveryQuest getDeliveryQuest(int index) {
        var data = SaveData.getString(tag(index, typeof(DeliveryQuest)));
        return JsonUtility.FromJson<DeliveryQuest>(data);
    }
    public static PickupQuest getPickupQuest(int index) {
        var data = SaveData.getString(tag(index, typeof(PickupQuest)));
        return JsonUtility.FromJson<PickupQuest>(data);
    }

    public static int getQuestTypeCount(System.Type type) {
        return SaveData.getInt(countTag(type));
    }
    public static int getQuestCount() {
        int count = 0;
        count += SaveData.getInt(countTag(typeof(KillQuest)));
        count += SaveData.getInt(countTag(typeof(BossFightQuest)));
        count += SaveData.getInt(countTag(typeof(DeliveryQuest)));
        count += SaveData.getInt(countTag(typeof(PickupQuest)));
        return count;
    }
}

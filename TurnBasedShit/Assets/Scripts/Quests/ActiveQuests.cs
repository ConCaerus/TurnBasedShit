using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActiveQuests {
    public static string countTag(Quest.questType type) {
        if(type == Quest.questType.kill)
            return "KillQuestCount";

        else if(type == Quest.questType.bossFight)
            return "BossFightQuestCount";

        else if(type == Quest.questType.delivery)
            return "DeliveryQuestCount";

        else if(type == Quest.questType.pickup)
            return "PickupQuestCount";

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

        return null;
    }


    public static void clear() {
        for(int i = 0; i < getQuestTypeCount(Quest.questType.kill); i++) {
            var data = SaveData.getString(tag(i, Quest.questType.kill));
            var thing = JsonUtility.FromJson<KillQuest>(data);
            thing.questDestory();

            SaveData.deleteKey(tag(i, Quest.questType.kill));
        }

        for(int i = 0; i < getQuestTypeCount(Quest.questType.bossFight); i++) {
            var data = SaveData.getString(tag(i, Quest.questType.bossFight));
            var thing = JsonUtility.FromJson<BossFightQuest>(data);
            thing.questDestory();

            SaveData.deleteKey(tag(i, Quest.questType.bossFight));
        }

        for(int i = 0; i < getQuestTypeCount(Quest.questType.delivery); i++) {
            var data = SaveData.getString(tag(i, Quest.questType.delivery));
            var thing = JsonUtility.FromJson<DeliveryQuest>(data);
            thing.questDestory();

            SaveData.deleteKey(tag(i, Quest.questType.delivery));
        }

        for(int i = 0; i < getQuestTypeCount(Quest.questType.pickup); i++) {
            var data = SaveData.getString(tag(i, Quest.questType.pickup));
            var thing = JsonUtility.FromJson<PickupQuest>(data);
            thing.questDestory();

            SaveData.deleteKey(tag(i, Quest.questType.pickup));
        }

        SaveData.deleteKey(countTag(Quest.questType.kill));
        SaveData.deleteKey(countTag(Quest.questType.bossFight));
        SaveData.deleteKey(countTag(Quest.questType.delivery));
        SaveData.deleteKey(countTag(Quest.questType.pickup));
    }

    public static void addQuest(KillQuest qu) {
        int index = getQuestTypeCount(Quest.questType.pickup);

        qu.questInit(true);

        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, Quest.questType.kill), data);
        SaveData.setInt(countTag(Quest.questType.kill), index + 1);
    }
    public static void addQuest(BossFightQuest qu) {
        int index = getQuestTypeCount(Quest.questType.bossFight);

        qu.questInit(true);

        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, Quest.questType.bossFight), data);
        SaveData.setInt(countTag(Quest.questType.bossFight), index + 1);
    }
    public static void addQuest(DeliveryQuest qu) {
        int index = getQuestTypeCount(Quest.questType.delivery);

        qu.questInit(true);

        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, Quest.questType.delivery), data);
        SaveData.setInt(countTag(Quest.questType.delivery), index + 1);
    }
    public static void addQuest(PickupQuest qu) {
        int index = getQuestTypeCount(Quest.questType.pickup);

        qu.questInit(true);

        var data = JsonUtility.ToJson(qu);
        SaveData.setString(tag(index, Quest.questType.pickup), data);
        SaveData.setInt(countTag(Quest.questType.pickup), index + 1);
    }


    public static void removeQuest(KillQuest quest) {
        Quest.questType type = Quest.questType.kill;
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
        Quest.questType type = Quest.questType.bossFight;
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
        Quest.questType type = Quest.questType.delivery;
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
        Quest.questType type = Quest.questType.pickup;
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

    public static int getQuestTypeCount(Quest.questType type) {
        return SaveData.getInt(countTag(type));
    }
    public static int getQuestCount() {
        int count = 0;
        count += SaveData.getInt(countTag(Quest.questType.kill));
        count += SaveData.getInt(countTag(Quest.questType.bossFight));
        count += SaveData.getInt(countTag(Quest.questType.delivery));
        count += SaveData.getInt(countTag(Quest.questType.pickup));
        return count;
    }

    public static bool hasQuest(Quest q) {
        if(q == null)
            return false;
        for(int i = 0; i < getQuestTypeCount(q.q_type); i++) {
            switch(q.q_type) {
                case Quest.questType.kill:
                    if(getKillQuest(i).isEqualTo(q))
                        return true;
                    break;

                case Quest.questType.bossFight:
                    if(getBossFightQuest(i).isEqualTo(q))
                        return true;
                    break;

                case Quest.questType.delivery:
                    if(getDeliveryQuest(i).isEqualTo(q))
                        return true;
                    break;

                case Quest.questType.pickup:
                    if(getPickupQuest(i).isEqualTo(q))
                        return true;
                    break;
            }
        }

        return false;
    }
}

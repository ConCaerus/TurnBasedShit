using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestCanvas : MonoBehaviour {
    [SerializeField] SlotMenu slot;

    [SerializeField] TextMeshProUGUI nameText, infoText, countText, coinText;

    public Color bossColor, killColor, delColor, pickColor, fishColor, resColor;


    List<questSlotInfo> infos = new List<questSlotInfo>();

    //  false for sort by time
    //  true for sort by type
    bool sortType = false;


    class questSlotInfo {
        public Quest.questType type;
        public int questInstanceID;

        public questSlotInfo(Quest.questType t, int id) {
            type = t;
            questInstanceID = id;
        }
    }


    private void Start() {
        updateMenu();
    }


    private void Update() {
        if(slot.run())
            updateInfo();
    }

    public void updateMenu() {
        updateList();
        updateInfo();
    }

    public void updateList() {
        infos.Clear();
        var q = ActiveQuests.getHolder().getQuests();
        int slotIndex = 0;

        for(int i = q.Count - 1; i >= 0; i--) {
            if(q[i].completed) {
                slot.createSlot(slotIndex, getColorForQuest(q[i]));
                infos.Add(new questSlotInfo(q[i].getQuestType(), q[i].instanceID));
                slotIndex++;
                q.RemoveAt(i);
            }
        }

        if(!sortType) {
            int index = 0;
            while(q.Count > 0) {
                foreach(var i in q) {
                    if(i.instanceID == index) {
                        slot.createSlot(slotIndex, getColorForQuest(i));
                        infos.Add(new questSlotInfo(i.getQuestType(), i.instanceID));
                        slotIndex++;
                        q.Remove(i);
                        break;
                    }
                }
                index++;
            }
        }
        else {
            foreach(var i in q) {
                slot.createSlot(slotIndex, getColorForQuest(i));
                infos.Add(new questSlotInfo(i.getQuestType(), i.instanceID));
                slotIndex++;
            }
        }
    }

    public void updateInfo() {
        countText.text = ActiveQuests.getHolder().getObjectCount<Quest>().ToString();
        coinText.text = "";
        nameText.text = "";
        infoText.text = "";


        if(slot.getSelectedSlotIndex() == -1 || slot.getSelectedSlotIndex() >= infos.Count)
            return;
        var currentInfo = infos[slot.getSelectedSlotIndex()];


        switch(currentInfo.type) {
            case Quest.questType.bossFight:
                nameText.text = "Boss: " + currentInfo.questInstanceID.ToString();
                BossFightQuest b = null;
                foreach(var i in ActiveQuests.getHolder().getObjects<BossFightQuest>()) {
                    if(i.instanceID == currentInfo.questInstanceID) {
                        b = i;
                        break;
                    }
                }
                coinText.text = b.reward.ToString() + "c";
                infoText.text = "   Kill " + b.bossUnit.u_name;
                break;

            case Quest.questType.kill:
                nameText.text = "Kill: " + currentInfo.questInstanceID.ToString();
                KillQuest k = null;
                foreach(var i in ActiveQuests.getHolder().getObjects<KillQuest>()) {
                    if(i.instanceID == currentInfo.questInstanceID) {
                        k = i;
                        break;
                    }
                }
                coinText.text = k.reward.ToString() + "c";
                infoText.text = "   Kill " + k.howManyToKill.ToString() + " " + k.enemyType.ToString() + "s";
                break;

            case Quest.questType.delivery:
                nameText.text = "Delivery: " + currentInfo.questInstanceID.ToString();
                DeliveryQuest d = null;
                foreach(var i in ActiveQuests.getHolder().getObjects<DeliveryQuest>()) {
                    if(i.instanceID == currentInfo.questInstanceID) {
                        d = i;
                        break;
                    }
                }
                coinText.text = d.reward.ToString() + "c";
                infoText.text = "   Deliver " + d.type.ToString() + " to " + d.deliveryLocation.town.t_name;
                break;

            case Quest.questType.pickup:
                nameText.text = "Pickup: " + currentInfo.questInstanceID.ToString();
                PickupQuest p = null;
                foreach(var i in ActiveQuests.getHolder().getObjects<PickupQuest>()) {
                    if(i.instanceID == currentInfo.questInstanceID) {
                        p = i;
                        break;
                    }
                }
                coinText.text = p.reward.ToString() + "c";
                infoText.text = "   Pickup a " + p.pType.ToString();
                break;

            case Quest.questType.rescue:
                nameText.text = "Rescue: " + currentInfo.questInstanceID.ToString();
                RescueQuest r = null;
                foreach(var i in ActiveQuests.getHolder().getObjects<RescueQuest>()) {
                    if(i.instanceID == currentInfo.questInstanceID) {
                        r = i;
                        break;
                    }
                }
                coinText.text = r.reward.ToString() + "c";
                infoText.text = "   Rescue " + r.location.unit.u_name;
                break;

            case Quest.questType.fishing:
                nameText.text = "Fishing: " + currentInfo.questInstanceID.ToString();
                FishingQuest f = null;
                foreach(var i in ActiveQuests.getHolder().getObjects<FishingQuest>()) {
                    if(i.instanceID == currentInfo.questInstanceID) {
                        f = i;
                        break;
                    }
                }
                coinText.text = f.reward.ToString() + "c";
                infoText.text = "   Catch a " + f.fish.name;
                break;
        }
    }


    Color getColorForQuest(Quest qu) {
        switch(qu.getQuestType()) {
            case Quest.questType.bossFight:
                return bossColor;
            case Quest.questType.pickup:
                return pickColor;
            case Quest.questType.delivery:
                return delColor;
            case Quest.questType.kill:
                return killColor;
            case Quest.questType.rescue:
                return resColor;
            case Quest.questType.fishing:
                return fishColor;
        }
        return Color.white;
    }

    public void toggleSortType() {
        sortType = !sortType;
        updateList();
    }


    public void abandonQuest() {
        if(slot.getSelectedSlotIndex() == -1 && slot.getSelectedSlotIndex() <= infos.Count - 1)
            return;
        var currentInfo = infos[slot.getSelectedSlotIndex()];

        foreach(var i in ActiveQuests.getHolder().getQuests()) {
            if(i.instanceID == currentInfo.questInstanceID) {
                ActiveQuests.removeQuest(i);
                break;
            }
        }

        infos.RemoveAt(slot.getSelectedSlotIndex());
        slot.deleteSlotAtIndex(slot.getSelectedSlotIndex());
        updateInfo();
    }

    public void claimReward() {
        if(slot.getSelectedSlotIndex() == -1 && slot.getSelectedSlotIndex() <= infos.Count - 1)
            return;
        var currentInfo = infos[slot.getSelectedSlotIndex()];

        foreach(var i in ActiveQuests.getHolder().getQuests()) {
            if(i.instanceID == currentInfo.questInstanceID) {
                if(!i.completed)
                    return;
                var qu = i;
                ActiveQuests.removeQuest(i);
                var reward = GameVariables.getCoinRewardForQuest(qu);
                Inventory.addCoins(reward);
                break;
            }
        }

        infos.RemoveAt(slot.getSelectedSlotIndex());
        slot.deleteSlotAtIndex(slot.getSelectedSlotIndex());
        updateInfo();
    }
}

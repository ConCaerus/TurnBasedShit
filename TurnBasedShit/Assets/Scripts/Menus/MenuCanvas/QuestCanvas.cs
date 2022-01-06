using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestCanvas : MonoBehaviour {
    [SerializeField] SlotMenu slot;

    [SerializeField] TextMeshProUGUI nameText, infoText, countText;

    public Color bossColor, killColor, delColor, pickColor;


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
        var b = ActiveQuests.getQuestHolder().getObjects<BossFightQuest>();
        var k = ActiveQuests.getQuestHolder().getObjects<KillQuest>();
        var d = ActiveQuests.getQuestHolder().getObjects<DeliveryQuest>();
        var p = ActiveQuests.getQuestHolder().getObjects<PickupQuest>();
        var f = ActiveQuests.getQuestHolder().getObjects<FishingQuest>();

        int slotIndex = 0;
        if(!sortType) {
            int index = 0;
            while((b.Count > 0 || k.Count > 0 || d.Count > 0 || p.Count > 0 || f.Count > 0)) {
                bool found = false;
                foreach(var i in b) {
                    if(i.instanceID == index) {
                        slot.createSlot(slotIndex, bossColor);
                        infos.Add(new questSlotInfo(Quest.questType.bossFight, i.instanceID));
                        slotIndex++;
                        found = true;
                        b.Remove(i);
                        break;
                    }
                }

                if(found) {
                    index++;
                    continue;
                }
                foreach(var i in k) {
                    if(i.instanceID == index) {
                        slot.createSlot(slotIndex, killColor);
                        infos.Add(new questSlotInfo(Quest.questType.kill, i.instanceID));
                        slotIndex++;
                        found = true;
                        k.Remove(i);
                        break;
                    }
                }

                if(found) {
                    index++;
                    continue;
                }
                foreach(var i in d) {
                    if(i.instanceID == index) {
                        slot.createSlot(slotIndex, delColor);
                        infos.Add(new questSlotInfo(Quest.questType.delivery, i.instanceID));
                        slotIndex++;
                        found = true;
                        d.Remove(i);
                        break;
                    }
                }

                if(found) {
                    index++;
                    continue;
                }
                foreach(var i in p) {
                    if(i.instanceID == index) {
                        slot.createSlot(slotIndex, pickColor);
                        infos.Add(new questSlotInfo(Quest.questType.pickup, i.instanceID));
                        slotIndex++;
                        found = true;
                        p.Remove(i);
                        break;
                    }
                }

                index++;
            }
        }
        else {
            foreach(var i in b) {
                slot.createSlot(slotIndex, bossColor);
                infos.Add(new questSlotInfo(Quest.questType.bossFight, i.instanceID));
                slotIndex++;
            }
            foreach(var i in k) {
                slot.createSlot(slotIndex, killColor);
                infos.Add(new questSlotInfo(Quest.questType.kill, i.instanceID));
                slotIndex++;
            }
            foreach(var i in d) {
                slot.createSlot(slotIndex, delColor);
                infos.Add(new questSlotInfo(Quest.questType.delivery, i.instanceID));
                slotIndex++;
            }
            foreach(var i in p) {
                slot.createSlot(slotIndex, pickColor);
                infos.Add(new questSlotInfo(Quest.questType.pickup, i.instanceID));
                slotIndex++;
            }
        }
    }

    public void updateInfo() {
        countText.text = ActiveQuests.getQuestHolder().getObjectCount<Quest>().ToString();


        if(slot.getSelectedSlotIndex() == -1 || slot.getSelectedSlotIndex() >= infos.Count)
            return;
        var currentInfo = infos[slot.getSelectedSlotIndex()];

        switch(currentInfo.type) {
            case Quest.questType.bossFight:
                nameText.text = "Boss: " + currentInfo.questInstanceID.ToString();
                BossFightQuest b = null;
                foreach(var i in ActiveQuests.getQuestHolder().getObjects<BossFightQuest>()) {
                    if(i.instanceID == currentInfo.questInstanceID) {
                        b = i;
                        break;
                    }
                }
                infoText.text = "   Kill " + b.bossUnit.u_name;
                break;

            case Quest.questType.kill:
                nameText.text = "Kill: " + currentInfo.questInstanceID.ToString();
                KillQuest k = null;
                foreach(var i in ActiveQuests.getQuestHolder().getObjects<KillQuest>()) {
                    if(i.instanceID == currentInfo.questInstanceID) {
                        k = i;
                        break;
                    }
                }
                infoText.text = "   Kill " + k.howManyToKill.ToString() + " " + k.enemyType.ToString() + "s";
                break;

            case Quest.questType.delivery:
                nameText.text = "Delivery: " + currentInfo.questInstanceID.ToString();
                DeliveryQuest d = null;
                foreach(var i in ActiveQuests.getQuestHolder().getObjects<DeliveryQuest>()) {
                    if(i.instanceID == currentInfo.questInstanceID) {
                        d = i;
                        break;
                    }
                }
                infoText.text = "   Deliver " + d.type.ToString() + " to " + d.deliveryLocation.town.t_name;
                break;

            case Quest.questType.pickup:
                nameText.text = "Pickup: " + currentInfo.questInstanceID.ToString();
                PickupQuest p = null;
                foreach(var i in ActiveQuests.getQuestHolder().getObjects<PickupQuest>()) {
                    if(i.instanceID == currentInfo.questInstanceID) {
                        p = i;
                        break;
                    }
                }
                infoText.text = "   Pickup a " + p.pType.ToString();
                break;
        }
    }

    public void toggleSortType() {
        sortType = !sortType;
        updateList();
    }


    public void abandonQuest() {
        if(slot.getSelectedSlotIndex() == -1 && slot.getSelectedSlotIndex() <= infos.Count - 1)
            return;
        var currentInfo = infos[slot.getSelectedSlotIndex()];

        switch(currentInfo.type) {
            case Quest.questType.bossFight:
                for(int i = 0; i < ActiveQuests.getQuestHolder().getObjectCount<BossFightQuest>(); i++) {
                    var b = ActiveQuests.getQuestHolder().getObject<BossFightQuest>(i);
                    if(b.instanceID == currentInfo.questInstanceID) {
                        ActiveQuests.removeQuest(b);
                        break;
                    }
                }
                break;

            case Quest.questType.kill:
                for(int i = 0; i < ActiveQuests.getQuestHolder().getObjectCount<KillQuest>(); i++) {
                    var b = ActiveQuests.getQuestHolder().getObject<KillQuest>(i);
                    if(b.instanceID == currentInfo.questInstanceID) {
                        ActiveQuests.removeQuest(b);
                        break;
                    }
                }
                break;

            case Quest.questType.delivery:
                for(int i = 0; i < ActiveQuests.getQuestHolder().getObjectCount<DeliveryQuest>(); i++) {
                    var b = ActiveQuests.getQuestHolder().getObject<DeliveryQuest>(i);
                    if(b.instanceID == currentInfo.questInstanceID) {
                        ActiveQuests.removeQuest(b);
                        break;
                    }
                }
                break;

            case Quest.questType.pickup:
                for(int i = 0; i < ActiveQuests.getQuestHolder().getObjectCount<PickupQuest>(); i++) {
                    var b = ActiveQuests.getQuestHolder().getObject<PickupQuest>(i);
                    if(b.instanceID == currentInfo.questInstanceID) {
                        ActiveQuests.removeQuest(b);
                        break;
                    }
                }
                break;
        }

        infos.RemoveAt(slot.getSelectedSlotIndex());
        slot.deleteSlotAtIndex(slot.getSelectedSlotIndex());
        updateInfo();
    }
}

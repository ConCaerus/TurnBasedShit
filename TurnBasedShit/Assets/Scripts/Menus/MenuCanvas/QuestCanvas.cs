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
        var b = ActiveQuests.getAllBossFightQuests();
        var k = ActiveQuests.getAllKillQuests();
        var d = ActiveQuests.getAllDeliveryQuests();
        var p = ActiveQuests.getAllPickupQuests();

        int slotIndex = 0;
        if(!sortType) {
            int index = 0;
            int stopPoint = ActiveQuests.getMostRecentQuestInstanceID();
            while((b.Count > 0 || k.Count > 0 || d.Count > 0 || p.Count > 0) && index <= stopPoint) {
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
        countText.text = ActiveQuests.getQuestCount().ToString();


        if(slot.getSelectedSlotIndex() == -1 || slot.getSelectedSlotIndex() >= infos.Count)
            return;
        var currentInfo = infos[slot.getSelectedSlotIndex()];

        switch(currentInfo.type) {
            case Quest.questType.bossFight:
                nameText.text = "Boss: " + currentInfo.questInstanceID.ToString();
                BossFightQuest b = ActiveQuests.getBossFightQuestWithInstanceID(currentInfo.questInstanceID);
                infoText.text = "   Kill " + b.bossUnit.u_name;
                break;

            case Quest.questType.kill:
                nameText.text = "Kill: " + currentInfo.questInstanceID.ToString();
                KillQuest k = ActiveQuests.getKillQuestWithInstanceID(currentInfo.questInstanceID);
                infoText.text = "   Kill " + k.howManyToKill.ToString() + " " + k.enemyType.ToString() + "s";
                break;

            case Quest.questType.delivery:
                nameText.text = "Delivery: " + currentInfo.questInstanceID.ToString();
                DeliveryQuest d = ActiveQuests.getDeliveryQuestWithInstanceID(currentInfo.questInstanceID);
                infoText.text = "   Deliver " + d.type.ToString() + " to " + d.deliveryLocation.town.t_name;
                break;

            case Quest.questType.pickup:
                nameText.text = "Pickup: " + currentInfo.questInstanceID.ToString();
                PickupQuest p = ActiveQuests.getPickupQuestWithInstanceID(currentInfo.questInstanceID);
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
                ActiveQuests.removeQuest(ActiveQuests.getBossFightQuestWithInstanceID(currentInfo.questInstanceID));
                break;

            case Quest.questType.kill:
                ActiveQuests.removeQuest(ActiveQuests.getKillQuestWithInstanceID(currentInfo.questInstanceID));
                break;

            case Quest.questType.delivery:
                ActiveQuests.removeQuest(ActiveQuests.getDeliveryQuestWithInstanceID(currentInfo.questInstanceID));
                break;

            case Quest.questType.pickup:
                ActiveQuests.removeQuest(ActiveQuests.getPickupQuestWithInstanceID(currentInfo.questInstanceID));
                break;
        }

        infos.RemoveAt(slot.getSelectedSlotIndex());
        slot.deleteSlotAtIndex(slot.getSelectedSlotIndex());
        updateInfo();
    }
}

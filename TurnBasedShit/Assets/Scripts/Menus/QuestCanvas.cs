using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestCanvas : MonoBehaviour {
    [SerializeField] SlotMenu slot;
    [SerializeField] GameObject slotObj;

    [SerializeField] TextMeshProUGUI nameText, infoText, countText;

    [SerializeField] Color bossColor, killColor, delColor, pickColor;


    List<questSlotInfo> infos = new List<questSlotInfo>();

    //  false for sort by time
    //  true for sort by type
    bool sortType = false;


    class questSlotInfo {
        public GameInfo.questType type;
        public int questInstanceID;

        public questSlotInfo(GameInfo.questType t, int id) {
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
                    if(i.q_instanceID == index) {
                        slot.createNewSlot(slotIndex, slotObj, slot.gameObject.transform.GetChild(0).transform, bossColor);
                        infos.Add(new questSlotInfo(GameInfo.questType.bossFight, i.q_instanceID));
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
                    if(i.q_instanceID == index) {
                        slot.createNewSlot(slotIndex, slotObj, slot.gameObject.transform.GetChild(0).transform, killColor);
                        infos.Add(new questSlotInfo(GameInfo.questType.kill, i.q_instanceID));
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
                    if(i.q_instanceID == index) {
                        slot.createNewSlot(slotIndex, slotObj, slot.gameObject.transform.GetChild(0).transform, delColor);
                        infos.Add(new questSlotInfo(GameInfo.questType.delivery, i.q_instanceID));
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
                    if(i.q_instanceID == index) {
                        slot.createNewSlot(slotIndex, slotObj, slot.gameObject.transform.GetChild(0).transform, pickColor);
                        infos.Add(new questSlotInfo(GameInfo.questType.pickup, i.q_instanceID));
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
                slot.createNewSlot(slotIndex, slotObj, slot.gameObject.transform.GetChild(0).transform, bossColor);
                infos.Add(new questSlotInfo(GameInfo.questType.bossFight, i.q_instanceID));
                slotIndex++;
            }
            foreach(var i in k) {
                slot.createNewSlot(slotIndex, slotObj, slot.gameObject.transform.GetChild(0).transform, killColor);
                infos.Add(new questSlotInfo(GameInfo.questType.kill, i.q_instanceID));
                slotIndex++;
            }
            foreach(var i in d) {
                slot.createNewSlot(slotIndex, slotObj, slot.gameObject.transform.GetChild(0).transform, delColor);
                infos.Add(new questSlotInfo(GameInfo.questType.delivery, i.q_instanceID));
                slotIndex++;
            }
            foreach(var i in p) {
                slot.createNewSlot(slotIndex, slotObj, slot.gameObject.transform.GetChild(0).transform, pickColor);
                infos.Add(new questSlotInfo(GameInfo.questType.pickup, i.q_instanceID));
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
            case GameInfo.questType.bossFight:
                nameText.text = "Boss: " + currentInfo.questInstanceID.ToString();
                BossFightQuest b = ActiveQuests.getBossFightQuestWithInstanceID(currentInfo.questInstanceID);
                infoText.text = "   Kill " + b.bossUnit.u_name;
                break;

            case GameInfo.questType.kill:
                nameText.text = "Kill: " + currentInfo.questInstanceID.ToString();
                KillQuest k = ActiveQuests.getKillQuestWithInstanceID(currentInfo.questInstanceID);
                infoText.text = "   Kill " + k.howManyToKill.ToString() + " " + k.enemyType.ToString() + "s";
                break;

            case GameInfo.questType.delivery:
                nameText.text = "Delivery: " + currentInfo.questInstanceID.ToString();
                DeliveryQuest d = ActiveQuests.getDeliveryQuestWithInstanceID(currentInfo.questInstanceID);
                infoText.text = "   Deliver " + d.type.ToString() + " to " + d.deliveryLocation.t_name;
                break;

            case GameInfo.questType.pickup:
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
            case GameInfo.questType.bossFight:
                ActiveQuests.removeQuest(ActiveQuests.getBossFightQuestWithInstanceID(currentInfo.questInstanceID));
                break;

            case GameInfo.questType.kill:
                ActiveQuests.removeQuest(ActiveQuests.getKillQuestWithInstanceID(currentInfo.questInstanceID));
                break;

            case GameInfo.questType.delivery:
                ActiveQuests.removeQuest(ActiveQuests.getDeliveryQuestWithInstanceID(currentInfo.questInstanceID));
                break;

            case GameInfo.questType.pickup:
                ActiveQuests.removeQuest(ActiveQuests.getPickupQuestWithInstanceID(currentInfo.questInstanceID));
                break;
        }

        infos.RemoveAt(slot.getSelectedSlotIndex());
        slot.deleteSlotAtIndex(slot.getSelectedSlotIndex());
        updateInfo();
    }
}

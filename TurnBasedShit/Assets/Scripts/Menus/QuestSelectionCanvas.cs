using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestSelectionCanvas : MonoBehaviour {
    [SerializeField] GameObject preset, holder, obj;
    [SerializeField] TextMeshProUGUI typeText;

    SlotMenu menu = new SlotMenu();

    List<Quest> quests = new List<Quest>();


    private void Start() {
        ActiveQuests.clear();
        hideMenu();
    }

    private void Update() {
        if(obj.activeInHierarchy) {
            if(menu.run())
                updateSlotInfo();
        }
    }



    public void showMenu(List<Quest> q) {
        obj.SetActive(true);
        quests.Clear();
        menu = new SlotMenu();
        quests = q;

        for(int i = 0; i < quests.Count; i++)
            menu.createNewSlot(i, preset.gameObject, holder.transform, Color.white);

        updateSlotInfo();
    }

    public void hideMenu() {
        obj.SetActive(false);
    }

    void generateRandomQuests() {
        quests.Clear();
        int count = Random.Range(1, 9);
        for(int i = 0; i < count; i++) {
            int rand = Random.Range(0, 2);
            if(rand == 0) {
                var temp = new AccumulativeQuest(AccumulativeQuest.type.killEnemies, 25);
                quests.Add(temp);
            }

            else if(rand == 1) {
                List<Weapon> things = new List<Weapon>() {
                    FindObjectOfType<PresetLibrary>().getRandomWeapon()
                };
                var temp = new DeliveryQuest(TownLibrary.getTown(FindObjectOfType<TownInstance>().town.t_index + 1), things);
                quests.Add(temp);
            }
        }
    }

    void updateSlotInfo() {
        List<GameObject> slots = menu.getSlots();
        for(int i = 0; i < menu.getSlots().Count; i++) {
            slots[i].GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
            slots[i].GetComponent<Image>().color = Color.black;
        }
        menu.setSlots(slots);

        if(menu.getSelectedSlot() != null) {
            typeText.text = quests[menu.getSelectedSlotIndex()].q_type.ToString();
            menu.getSelectedSlot().GetComponent<Image>().color = Color.grey;
        }
    }


    //  Buttons
    public void acceptQuest() {
        if(menu.getSelectedSlot() != null) {
            Quest qu = quests[menu.getSelectedSlotIndex()];
            quests.RemoveAt(menu.getSelectedSlotIndex());
            menu.deleteSlotAtIndex(menu.getSelectedSlotIndex());
            switch(qu.q_type) {
                case Quest.questType.accumulative:
                    qu.questInit();
                    ActiveQuests.addQuest(qu.accRef);
                    Debug.Log(qu.q_type);
                    break;
                case Quest.questType.bossFight:
                    qu.questInit();
                    ActiveQuests.addQuest(qu.bossRef);
                    Debug.Log(qu.q_type);
                    break;
                case Quest.questType.delivery:
                    qu.questInit();
                    ActiveQuests.addQuest(qu.delRef);
                    Debug.Log(qu.q_type);
                    break;
                case Quest.questType.equipmentPickup:
                    qu.questInit();
                    ActiveQuests.addQuest(qu.pickupRef);
                    Debug.Log(qu.q_type);
                    break;
            }

            updateSlotInfo();
            Debug.Log(ActiveQuests.getQuestCount());
        }
    }

    public bool showing() {
        return obj.activeInHierarchy;
    }
}

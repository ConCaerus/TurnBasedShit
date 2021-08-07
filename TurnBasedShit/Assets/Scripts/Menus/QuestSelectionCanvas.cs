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
                var temp = new KillQuest(25, FindObjectOfType<PresetLibrary>().getRandomEnemy().GetComponent<EnemyUnitInstance>().enemyType);
                quests.Add(temp);
            }

            else if(rand == 1) {
                List<Weapon> things = new List<Weapon>() {
                    FindObjectOfType<PresetLibrary>().getRandomWeapon()
                };
                var towns = MapLocationHolder.getLocations(new List<MapLocation.locationType>() { MapLocation.locationType.town });
                var useableTowns = new List<TownLocation>();
                foreach(var t in towns) {
                    if(!t.isEqualTo(GameInfo.getCurrentLocationAsTown()))
                        useableTowns.Add((TownLocation)t);
                }
                var temp = new DeliveryQuest(useableTowns[Random.Range(0, useableTowns.Count)].town, things);
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
                case Quest.questType.kill:
                    //  handled in the ActiveQuests thing
                    qu.questInit(false);
                    ActiveQuests.addQuest((KillQuest)qu);
                    break;
                case Quest.questType.bossFight:
                    qu.questInit(false);
                    ActiveQuests.addQuest((BossFightQuest)qu);
                    break;
                case Quest.questType.delivery:
                    qu.questInit(false);
                    ActiveQuests.addQuest((DeliveryQuest)qu);
                    break;
                case Quest.questType.pickup:
                    qu.questInit(false);
                    ActiveQuests.addQuest((PickupQuest)qu);
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

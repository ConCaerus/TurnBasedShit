using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestInfoSlot : MonoBehaviour {
    public TextMeshProUGUI mainText, progressText;
    public Slider progressSlider;


    public void setQuestInfo(Quest q) {
        //  creates the main text for the quest
        switch(q.q_type) {
            case Quest.questType.bossFight:
                mainText.text = "Kill " + ((BossFightQuest)q).bossUnit.u_name;

                break;

            case Quest.questType.kill:
                mainText.text = "Kill " + ((KillQuest)q).howManyToKill.ToString() + " " + ((KillQuest)q).enemyType.ToString();
                break;

            case Quest.questType.pickup:
                mainText.text = "Collect ";
                if(((PickupQuest)q).pickupWeapon != null)
                    mainText.text += ((PickupQuest)q).pickupWeapon.w_name;
                else if(((PickupQuest)q).pickupArmor != null)
                    mainText.text += ((PickupQuest)q).pickupArmor.a_name;
                else if(((PickupQuest)q).pickupConsumable != null)
                    mainText.text += ((PickupQuest)q).pickupConsumable.c_name;
                else if(((PickupQuest)q).pickupItem != null)
                    mainText.text += ((PickupQuest)q).pickupItem.i_name;

                break;

            case Quest.questType.delivery:
                mainText.text = "Deliver " + ((DeliveryQuest)q).type.ToString() + " to " + ((DeliveryQuest)q).deliveryLocation.t_name;
                break;
        }

        //  updates the progress slider and text for the quest
        switch(q.q_type) {
            case Quest.questType.bossFight:
                progressSlider.maxValue = 1.0f;

                if(!q.isCompleted()) {
                    progressSlider.value = 0.0f;
                    progressText.text = "Ongoing";
                } else {
                    progressSlider.value = 1.0f;
                    progressText.text = "Completed";
                }

                break;

            case Quest.questType.kill:
                progressSlider.maxValue = ((KillQuest)q).howManyToKill;
                progressSlider.value = ((KillQuest)q).killCount;

                progressText.text = ((KillQuest)q).killCount.ToString() + " of " + ((KillQuest)q).howManyToKill.ToString();

                if(q.isCompleted())
                    progressText.text = "Completed";
                break;

            case Quest.questType.pickup:
                progressSlider.maxValue = 1.0f;

                if(!q.isCompleted()) {
                    progressSlider.value = 0.0f;
                    progressText.text = "Ongoing";
                }
                else {
                    progressSlider.value = 1.0f;
                    progressText.text = "Completed";
                }
                break;

            case Quest.questType.delivery:
                progressSlider.maxValue = 1.0f;

                if(!q.isCompleted()) {
                    progressSlider.value = 0.0f;
                    progressText.text = "Ongoing";
                }
                else {
                    progressSlider.value = 1.0f;
                    progressText.text = "Completed";
                }
                break;
        }
    }
}

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
                mainText.text = "Kill " + q.bossRef.bossUnit.u_name;

                break;

            case Quest.questType.kill:
                mainText.text = "Kill " + q.killRef.howManyToKill.ToString() + " " + q.killRef.enemyType.ToString();
                break;

            case Quest.questType.equipmentPickup:
                mainText.text = "Collect ";
                if(q.pickupRef.pickupWeapon != null)
                    mainText.text += q.pickupRef.pickupWeapon.w_name;
                else if(q.pickupRef.pickupArmor != null)
                    mainText.text += q.pickupRef.pickupArmor.a_name;
                else if(q.pickupRef.pickupConsumable != null)
                    mainText.text += q.pickupRef.pickupConsumable.c_name;
                else if(q.pickupRef.pickupItem != null)
                    mainText.text += q.pickupRef.pickupItem.i_name;

                break;

            case Quest.questType.delivery:
                mainText.text = "Deliver " + q.delRef.type.ToString() + " to " + q.delRef.deliveryLocation.t_name;
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
                progressSlider.maxValue = q.killRef.howManyToKill;
                progressSlider.value = q.killRef.killCount;

                progressText.text = q.killRef.killCount.ToString() + " of " + q.killRef.howManyToKill.ToString();

                if(q.isCompleted())
                    progressText.text = "Completed";
                break;

            case Quest.questType.equipmentPickup:
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

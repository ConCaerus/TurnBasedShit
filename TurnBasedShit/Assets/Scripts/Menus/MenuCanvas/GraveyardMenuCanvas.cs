using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraveyardMenuCanvas : MonoBehaviour {
    [SerializeField] SlotMenu slots;
    [SerializeField] GameObject slotPreset;
    [SerializeField] TextMeshProUGUI count;



    private void Update() {
        slots.run();
    }

    public void createSlots() {
        count.text = Graveyard.getDeadCount().ToString();
        for(int i = 0; i < Graveyard.getDeadCount(); i++) {
            var obj = slots.createSlot(i, Graveyard.getDeadStats(i).u_sprite.color, Graveyard.getDeadStats(i).u_name);
            obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Graveyard.getDeadStats(i).u_name;

            if(Graveyard.getDeadStats(i).u_deathInfo.causeOfDeath == DeathInfo.killCause.murdered) {
                obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Killed by ";
                if(Graveyard.getDeadStats(i).u_deathInfo.enemyType == (EnemyUnitInstance.type)(-1))
                    obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text += "a teammate named " + Graveyard.getDeadStats(i).u_deathInfo.nameOfKiller;
                else
                    obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text += "a " + Graveyard.getDeadStats(i).u_deathInfo.enemyType.ToString() + " named " + Graveyard.getDeadStats(i).u_deathInfo.nameOfKiller;
            }
        }
    }
}

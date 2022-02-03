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
        count.text = Graveyard.getHolder().getObjectCount<UnitStats>().ToString();
        for(int i = 0; i < Graveyard.getHolder().getObjectCount<UnitStats>(); i++) {
            var obj = slots.createSlot(i, Graveyard.getHolder().getObject<UnitStats>(i).u_sprite.color, Graveyard.getHolder().getObject<UnitStats>(i).u_name);
            obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Graveyard.getHolder().getObject<UnitStats>(i).u_name;

            if(Graveyard.getHolder().getObject<UnitStats>(i).u_deathInfo.causeOfDeath == DeathInfo.killCause.murdered) {
                obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Killed by ";
                if(Graveyard.getHolder().getObject<UnitStats>(i).u_deathInfo.combatType == (GameInfo.combatUnitType)(-1))
                    obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text += "a teammate named " + Graveyard.getHolder().getObject<UnitStats>(i).u_deathInfo.nameOfKiller;
                else
                    obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text += "a " + Graveyard.getHolder().getObject<UnitStats>(i).u_deathInfo.combatType.ToString() + " named " + Graveyard.getHolder().getObject<UnitStats>(i).u_deathInfo.nameOfKiller;
            }
        }
    }
}

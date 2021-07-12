using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyPopulator : MonoBehaviour {
    public void clearParty() {
        Party.clearParty();
    }


    public void addUnit() {
        var thing = FindObjectOfType<PresetLibrary>().getPlayerUnit().GetComponent<UnitClass>().stats;
        Debug.Log("Added " + thing.u_name);
        Party.addNewUnit(thing);
    }
    public void clearPartyEquippment() {
        Party.clearPartyEquipment();
    }
}

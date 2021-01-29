using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsReseter : MonoBehaviour {

    public void resetPlayerPrefs() {
        PlayerPrefs.DeleteAll();
        Debug.Log("Deleted");
        Inventory.clearInventory();
        PlayerPrefs.Save();
    }


    public void resetPlayerUnitEquippment() {
        foreach(var i in FindObjectsOfType<PlayerUnitInstance>()) {
            i.resetSavedEquippment();
        }
    }
}

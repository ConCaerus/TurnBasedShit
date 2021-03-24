using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveSlotInformation : MonoBehaviour {
    public int saveIndex = 0;
    [SerializeField] TextMeshProUGUI saveName, partyCount;

    private void Awake() {
        setInfo();
    }


    public void setInfo() {
        if(SaveData.hasSaveDataForSave(saveIndex))
            saveName.text = SaveData.getStringInSave(saveIndex, "Save Name");
        else
            saveName.text = "No Data";
        partyCount.text = SaveData.getIntInSave(saveIndex, Party.partySizeTag).ToString();
    }


    public void setName(string s) {
        saveName.text = s;
    }
}

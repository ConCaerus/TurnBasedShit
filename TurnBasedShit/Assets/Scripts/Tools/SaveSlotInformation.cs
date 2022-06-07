using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveSlotInformation : MonoBehaviour {
    public int saveIndex = 0;
    [SerializeField] TextMeshProUGUI saveName, partyCount, timeCount;

    private void Awake() {
        setInfo();
    }


    void setInfo() {
        var prevInd = SaveData.getCurrentSaveIndex();
        SaveData.setCurrentSaveIndex(saveIndex);
        if(SaveData.hasSaveDataForSave(saveIndex)) {
            saveName.text = SaveData.getStringInSave(saveIndex, "Save Name");
            partyCount.text = Party.getHolder().getObjectCount<UnitStats>().ToString();
            timeCount.text = TimeInfo.timeToString();
        }
        else {
            saveName.text = "No Data";
            partyCount.text = "0";
            timeCount.text = "0:00";
        }
        SaveData.setCurrentSaveIndex(prevInd);
    }


    public void setName(string s) {
        saveName.text = s;
    }
}

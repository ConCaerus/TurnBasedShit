using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundCounterCanvas : MonoBehaviour {
    [SerializeField] TextMeshProUGUI roundCounter, waveCounter;

    int roundCount = 0, waveCount = 0;


    public void resetCount() {
        roundCount = 0;
        waveCount = 0;
    }

    public void updateInfo() {
        roundCounter.text = "Round\n" + roundCount .ToString();
        waveCounter.text = "Wave\n" + (waveCount + 1).ToString() + " of " + (GameInfo.getCombatDetails().waves.Count).ToString();
    }

    public void incrementAndUpdateRoundCount() {
        roundCount++;
        roundCounter.text = "Round\n" + roundCount.ToString();
    }

    public void incrementAndUpdateWaveCount() {
        waveCount++;
        waveCounter.text = "Wave\n" + (waveCount + 1).ToString() + " of " + (GameInfo.getCombatDetails().waves.Count).ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundCounterCanvas : MonoBehaviour {
    [SerializeField] TextMeshProUGUI roundCounter;

    public int roundCount = 0;


    private void Update() {
        roundCounter.text = "Round\n" + roundCount.ToString();
    }


    public void resetRoundCount() {
        roundCount = 0;
    }
}

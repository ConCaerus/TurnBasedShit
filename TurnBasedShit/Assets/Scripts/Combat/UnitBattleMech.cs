using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBattleMech : MonoBehaviour {

    private void Awake() {
        FindObjectOfType<PartyObject>().instantiatePartyMembers();
    }

    private void Start() {
        resetBattleRound();
    }

    public void resetBattleRound() {
        FindObjectOfType<TurnOrderSorter>().resetList();
        FindObjectOfType<RoundCounterCanvas>().roundCount++;
    }
}

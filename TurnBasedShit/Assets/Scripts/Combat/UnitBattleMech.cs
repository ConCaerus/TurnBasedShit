using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBattleMech : MonoBehaviour {

    private void Awake() {
        FindObjectOfType<Party>().loadParty();
        FindObjectOfType<Party>().instantiateUnitsInParty();
        FindObjectOfType<Party>().saveParty();

        Inventory.loadAllEquippment();
    }

    private void Start() {
        resetBattleRound();
        FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
    }

    public void resetBattleRound() {
        FindObjectOfType<TurnOrderSorter>().resetList();
        FindObjectOfType<RoundCounterCanvas>().roundCount++;
    }
}

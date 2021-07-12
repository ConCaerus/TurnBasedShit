using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSpot : MonoBehaviour {
    




    public bool isPlayerSpot() {
        return transform.position.x < 0.0f;
    }
}

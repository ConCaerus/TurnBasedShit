using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInstance : MonoBehaviour {
    public Building.type b_type;

    private void Start() {
        if(GetComponent<Animator>() != null) {
            GetComponent<InfoBearer>().runOnMouseOver(delegate { GetComponent<Animator>().SetTrigger("mousedOver"); });
            GetComponent<InfoBearer>().runOnMouseExit(delegate { GetComponent<Animator>().ResetTrigger("mousedOver"); });
        }
    }
}

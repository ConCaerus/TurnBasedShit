using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogMask : MonoBehaviour {

    private void Start() {
        transform.localScale = new Vector3(100.0f, 100.0f);
        transform.localPosition = new Vector3(-26.0f, -26.0f);

        transform.GetChild(0).localScale = new Vector3(5.6f, 1.1f);
        transform.GetChild(0).localPosition = new Vector3(1.38f, 0.25f);
    }
}

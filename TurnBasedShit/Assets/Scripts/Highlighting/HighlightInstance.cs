using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightInstance : MonoBehaviour {

    public string originName;

    public void destroy(string oName) {
        if(oName == name)
            Destroy(gameObject);
    }
    public void hardDestroy() {
        Destroy(gameObject);
    }
}

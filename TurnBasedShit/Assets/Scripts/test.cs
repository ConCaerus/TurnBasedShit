using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour {

    private void Start() {
        Debug.Log(JsonUtility.FromJson<Vector2>(JsonUtility.ToJson(new Vector2(10.0f, 2.5f))));
    }
}
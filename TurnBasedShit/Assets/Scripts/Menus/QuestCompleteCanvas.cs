using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestCompleteCanvas : MonoBehaviour {
    [SerializeField] GameObject image;

    public void showCanvas() {
        transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("completed");
    }
}

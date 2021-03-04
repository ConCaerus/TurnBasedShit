using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEnvironment : MonoBehaviour {
    float minDist = 1.5f;

    private void OnMouseEnter() {
        if(GetComponent<Animator>() != null) {
            foreach(var i in FindObjectsOfType<CombatEnvironment>()) {
                if(i.GetComponent<Animator>() != null && Vector2.Distance(transform.position, i.transform.position) < minDist) {
                    i.play();
                }
            }
            play();
        }
    }

    public void play() {
        GetComponent<Animator>().Play("TreeSway");
        FindObjectOfType<AudioManager>().playSwaySound();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEnvironment : MonoBehaviour {
    [SerializeField] GameObject clouds;


    private void Start() {
        foreach(var i in clouds.GetComponentsInChildren<SpriteRenderer>())
            StartCoroutine(cloudMover(i.gameObject, i.transform.localPosition.x));
    }


    IEnumerator cloudMover(GameObject c, float origin) {
        float max = 0.75f;
        var speed = Random.Range(.5f, .75f);
        if(c.tag == "FourthParallax") {
            max /= 2.0f;
            speed /= 2.0f;
        }
        else if(c.tag == "FifthParallax") {
            max /= 4.0f;
            speed /= 4.0f;
        }

        var x = Random.Range(-max, max);
        var xPos = origin + x;

        while(Mathf.Abs(c.transform.localPosition.x - xPos) > 0.05f) {
            c.transform.localPosition = Vector2.Lerp(c.transform.localPosition, new Vector2(xPos, c.transform.localPosition.y), speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(cloudMover(c, origin));
    }
}

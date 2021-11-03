using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class EnounterCanvas : MonoBehaviour {
    [SerializeField] GameObject enemyEncounterImage;

    private void Start() {
        enemyEncounterImage.SetActive(false);
    }

    public void showEnemyEncounterAlert(MapLocation location = null) {
        StartCoroutine(enterMapLocationAnimation(enemyEncounterImage, location));
    }

    IEnumerator enterMapLocationAnimation(GameObject relevantImage, MapLocation loc = null) {
        //  Turns shit off
        FindObjectOfType<MapCameraController>().enabled = false;

        //  animation
        relevantImage.SetActive(true);
        var target = relevantImage.transform.localScale;
        relevantImage.transform.localScale = Vector3.zero;

        relevantImage.transform.DOScale(target, 0.15f);

        yield return new WaitForSeconds(1.0f);

        if(loc == null)
            FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("Combat");
        else
            loc.enterLocation(FindObjectOfType<TransitionCanvas>());
    }
}

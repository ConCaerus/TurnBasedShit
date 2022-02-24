using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TutorialCanvas : MonoBehaviour {
    [System.Serializable]
    public struct tutInfo {
        public Vector2 screenPos;
        public string tut;
    }


    public enum tutState {
        combat, map, town
    };


    [SerializeField] TextMeshProUGUI tutText;


    Coroutine waitor = null;

    [SerializeField] List<tutInfo> combatInfo = new List<tutInfo>();
    [SerializeField] List<tutInfo> mapInfo = new List<tutInfo>();
    [SerializeField] List<tutInfo> townInfo = new List<tutInfo>();

    List<tutInfo> currInfo;



    private void Start() {
        startTutorial(tutState.combat);
    }


    public void startTutorial(tutState state) {
        currInfo = state == tutState.combat ? combatInfo : state == tutState.map ? mapInfo : townInfo;
        transform.DOScale(1.0f, .05f);
        activateTutorial();
    }


    void activateTutorial() {
        if(currInfo.Count == 0) {
            transform.DOScale(0.0f, .15f);
            return;
        }

        var info = currInfo[0];

        transform.DOMove(info.screenPos, .075f, false);
        tutText.text = info.tut;
    }


    //  button
    public void advanceTut() {
        if(currInfo.Count <= 0)
            return;

        currInfo.RemoveAt(0);
        activateTutorial();
    }
}

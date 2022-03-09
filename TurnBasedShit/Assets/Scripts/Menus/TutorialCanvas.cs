using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

public class TutorialCanvas : MonoBehaviour {
    [System.Serializable]
    public class tutInfo {
        public Vector2 screenPos;   //  when tutBut is not null, this is used as the offset form the button's position
        public Button tutBut;
        public UnityAction butFunc;
        public trigger trig = (trigger)(-1);
        public string title;
        public string tut;
        public float timer;

        public void init(UnityAction func) {
            if(tutBut != null) {
                butFunc = func;
                tutBut.onClick.AddListener(butFunc);
            }
        }


        public Vector2 getRealPos() {
            return tutBut != null ? (Vector2)tutBut.transform.position + screenPos : screenPos;
        }
    }


    public enum tutState {
        combat, map, town
    };

    public enum trigger {
        whenPlayersTurn, whenSummonersTurn, whenHealersTurn
    }


    [SerializeField] TextMeshProUGUI tutText, titleText;


    Coroutine moveAnim = null;

    [SerializeField] List<tutInfo> combatInfo = new List<tutInfo>();
    [SerializeField] List<tutInfo> mapInfo = new List<tutInfo>();
    [SerializeField] List<tutInfo> townInfo = new List<tutInfo>();

    List<tutInfo> currInfo;



    private void Start() {
        transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(FindObjectOfType<TransitionCanvas>().runAfterLoading(runChecker));
    }

    private void Update() {
        if(currInfo.Count > 0 && currInfo[0].trig != (trigger)(-1) && isTriggerReady())
            advanceFromChildButton();
    }

    public void runChecker() {
        switch(GameInfo.currentGameState) {
            case GameInfo.state.combat:
                if(!GameInfo.hasCompletedCombatTutorial())
                    StartCoroutine(introAnim(tutState.combat)); //  also needs to set a default combat location 
                break;

            case GameInfo.state.map:
                if(!GameInfo.hasCompletedMapTutorial())
                    StartCoroutine(introAnim(tutState.map));
                break;

            case GameInfo.state.town:
                if(!GameInfo.hasCompletedTownTutorial())
                    StartCoroutine(introAnim(tutState.town));
                break;
        }
    }

    void activateTutorial() {
        if(currInfo.Count == 0) {
            transform.DOKill();
            transform.DOScale(0.0f, .1f);
            return;
        }

        var info = currInfo[0];

        tutText.text = info.tut;
        titleText.text = info.title;

        if(moveAnim != null)
            StopCoroutine(moveAnim);
        moveAnim = StartCoroutine(tutAnim(info));
    }


    //  button
    public void advanceFromTutBut() {
        if(currInfo.Count <= 0)
            return;

        if(currInfo[0].tutBut != null) {
            currInfo[0].tutBut.onClick.RemoveListener(currInfo[0].butFunc);
        }

        currInfo.RemoveAt(0);


        activateTutorial();
    }

    public void advanceFromChildButton() {
        if(currInfo.Count <= 0 || currInfo[0].tutBut != null)
            return;
        currInfo.RemoveAt(0);

        activateTutorial();
    }

    bool isTriggerReady() {
        var pu = FindObjectOfType<TurnOrderSorter>().playingUnit.GetComponent<UnitClass>();
        switch(currInfo[0].trig) {
            case trigger.whenPlayersTurn:
                return pu.stats.u_type == GameInfo.combatUnitType.player && pu.combatStats.isPlayerUnit;
            case trigger.whenSummonersTurn:
                return pu.stats.u_type == GameInfo.combatUnitType.player && pu.stats.weapon != null && pu.stats.weapon.sUsage == Weapon.specialUsage.summoning;
            case trigger.whenHealersTurn:
                return pu.stats.u_type == GameInfo.combatUnitType.player && pu.stats.weapon != null && pu.stats.weapon.sUsage == Weapon.specialUsage.healing;
            default: return false;
        }
    }


    IEnumerator introAnim(tutState state) {
        currInfo = state == tutState.combat ? combatInfo : state == tutState.map ? mapInfo : townInfo;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.position = currInfo[0].getRealPos();

        transform.DOKill();
        transform.DOScale(.25f, .075f);

        //  initializes the button functions
        foreach(var i in currInfo)
            i.init(advanceFromTutBut);

        yield return new WaitForSeconds(.075f);

        activateTutorial();
    }

    IEnumerator tutAnim(tutInfo info) {
        transform.DOKill();
        transform.localScale = new Vector3(.25f, .25f);
        transform.DOPunchScale(new Vector3(1.1f, 1.1f), 0.1f);

        yield return new WaitForSeconds(.1f);
        transform.DOMove(info.getRealPos(), .075f);

        yield return new WaitForSeconds(.075f);

        transform.DOScale(1.0f, .075f);

        yield return new WaitForSeconds(.075f);
        float offset = .25f;
        float t = .5f;
        float elapsedTime = 0.0f;
        while(elapsedTime < info.timer || info.timer == 0.0f) {
            transform.DOMove(info.getRealPos() + new Vector2(0.0f, offset), t);
            yield return new WaitForSeconds(t);
            transform.DOMove(info.getRealPos(), t);
            yield return new WaitForSeconds(t);

            elapsedTime += t * 2;
        }
    }
}

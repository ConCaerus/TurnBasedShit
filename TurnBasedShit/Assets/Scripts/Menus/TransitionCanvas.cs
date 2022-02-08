using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TransitionCanvas : MonoBehaviour {
    [SerializeField] GameObject background;

    Coroutine shower = null, hider = null, sceneLoader = null;

    float transitionTime = 0.5f;

    public bool loaded = false;
    public delegate void func();

    private void Awake() {
        DOTween.Init();
    }

    private void Start() {
        background.SetActive(true);
        hideBackground();
    }


    public void showBackground() {
        if(hider != null)
            StopCoroutine(hider);
        if(shower != null)
            return;
        background.transform.localPosition = new Vector3(0.0f, -1000.0f, 0.0f);
        shower = StartCoroutine(showBackgroundObject());
    }

    public void hideBackground() {
        if(shower != null)
            StopCoroutine(shower);
        if(hider != null)
            return;
        background.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        hider = StartCoroutine(hideBackgroundObject());
    }


    IEnumerator showBackgroundObject() {
        yield return new WaitForEndOfFrame();

        loaded = false;
        background.SetActive(true);
        background.transform.DOKill();
        background.transform.DOLocalMoveY(0.0f, transitionTime);
        shower = null;
    }

    IEnumerator hideBackgroundObject() {
        yield return new WaitForEndOfFrame();

        background.SetActive(true);
        background.transform.DOKill();
        background.transform.DOLocalMoveY(-1000.0f, transitionTime);

        yield return new WaitForSeconds(transitionTime);
        background.SetActive(false);
        loaded = true;
        hider = null;
    }

    IEnumerator loadSceneAfterBackgroundShown(string name, bool autoProgress = true) {
        showBackground();

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadSceneAsync(name);
    }
    IEnumerator runFuncAfterBackgroundShown(func funcToRun) {
        showBackground();
        yield return new WaitForSeconds(transitionTime);

        funcToRun();
    }

    //  FUCKING REMEMBER: when calling this function you need to use StartCoroutine();
    public IEnumerator runAfterLoading(func funcToRun) {
        yield return new WaitForEndOfFrame();

        if(loaded) {
            funcToRun();
        }
        else
            StartCoroutine(runAfterLoading(funcToRun));
    }

    public void loadSceneWithTransition(string name, bool autoProgress = true) {
        if(sceneLoader != null) {
            Debug.LogError("Already loading another scene");
            return;
        }
        sceneLoader = StartCoroutine(loadSceneAfterBackgroundShown(name, autoProgress));
    }

    public void loadSceneWithFunction(func funcToRun) {
        if(sceneLoader != null) {
            Debug.LogError("Already loading another scene");
            return;
        }
        sceneLoader = StartCoroutine(runFuncAfterBackgroundShown(funcToRun));
    }

    public float getTransitionTime() {
        return transitionTime;
    }
}

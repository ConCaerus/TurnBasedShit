using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TransitionCanvas : MonoBehaviour {
    [SerializeField] GameObject background;

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
        StartCoroutine(showBackgroundObject());
    }

    public void hideBackground() {
        StartCoroutine(hideBackgroundObject());
    }


    IEnumerator showBackgroundObject() {
        yield return new WaitForEndOfFrame();

        loaded = false;
        background.SetActive(true);
        background.transform.localPosition = new Vector3(0.0f, -1000.0f, 0.0f);
        background.transform.DOLocalMove(new Vector3(0.0f, 0.0f, 0.0f), transitionTime);
    }

    IEnumerator hideBackgroundObject() {
        yield return new WaitForEndOfFrame();

        background.SetActive(true);
        background.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        background.transform.DOLocalMove(new Vector3(0.0f, -1000.0f, 0.0f), transitionTime);

        yield return new WaitForSeconds(transitionTime);
        background.SetActive(false);
        loaded = true;
    }

    IEnumerator loadSceneAfterBackgroundShown(string name) {
        showBackground();
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(name);
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

    public void loadSceneWithTransition(string name) {
        StartCoroutine(loadSceneAfterBackgroundShown(name));
    }

    public void loadSceneWithFunction(func funcToRun) {
        StartCoroutine(runFuncAfterBackgroundShown(funcToRun));
    }

    public float getTransitionTime() {
        return transitionTime;
    }
}

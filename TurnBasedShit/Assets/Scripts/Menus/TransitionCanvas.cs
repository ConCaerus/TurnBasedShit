using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TransitionCanvas : MonoBehaviour {
    [SerializeField] GameObject background;

    [SerializeField] float transitionSpeed = 0.25f;

    public bool loaded = false;
    public delegate void func();

    private void Awake() {
        background.SetActive(true);
        DOTween.Init();
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
        background.transform.DOLocalMove(new Vector3(0.0f, 0.0f, 0.0f), transitionSpeed);
    }

    IEnumerator hideBackgroundObject() {
        yield return new WaitForEndOfFrame();

        background.SetActive(true);
        background.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        background.transform.DOLocalMove(new Vector3(0.0f, -1000.0f, 0.0f), transitionSpeed);

        yield return new WaitForSeconds(transitionSpeed);
        background.SetActive(false);
        loaded = true;
    }

    IEnumerator loadSceneAfterBackgroundShown(string name) {
        showBackground();
        yield return new WaitForSeconds(transitionSpeed);

        SceneManager.LoadScene(name);
    }
    IEnumerator runFuncAfterBackgroundShown(func funcToRun) {
        showBackground();
        yield return new WaitForSeconds(transitionSpeed);

        funcToRun();
    }


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
}

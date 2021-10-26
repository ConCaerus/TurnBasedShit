using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FishingCanvas : MonoBehaviour {
    public Slider reelSlider, fishSlider, reelTargetSlider, progressSlider;

    public float fishTarget = 0.0f;
    public float reelTarget = 0.0f;
    float reelOffset = 0.0f;

    float points = 0.0f;


    Coroutine fishWaiter = null, randReelWaiter = null;


    private void Awake() {
        DOTween.Init();
    }

    private void Start() {
        reelSlider.value = 0.0f;
        fishSlider.value = 0.0f;
        reelTargetSlider.value = 0.0f;
        progressSlider.value = 0.0f;

        reelTargetSlider.onValueChanged.AddListener(delegate { reelTarget = reelTargetSlider.value; });
    }

    private void Update() {
        fishMovement();

        reelMovement();

        gainPoints();

        if(progressSlider.value == 1.0f) {
            Debug.Log("Done");
            this.enabled = false;
        }
    }


    void gainPoints() {
        float diff = Mathf.Abs(fishSlider.value - reelSlider.value);
        float pointsForDiff = Mathf.Clamp(((0.15f - diff)) * Time.deltaTime, -0.002f, 0.002f);
        progressSlider.value = Mathf.Clamp(progressSlider.value + pointsForDiff, 0.0f, 1.0f);
    }


    //  reel shit

    void reelMovement() {
        if(randReelWaiter == null) {
            randReelWaiter = StartCoroutine(setNewRandReelOffset());
        }

        //  modify target by scroll val
        float scroll = Input.mouseScrollDelta.y;
        if(Mathf.Abs(scroll) > 0.001f)
            reelTargetSlider.value = Mathf.Clamp(reelTargetSlider.value + scroll / 20.0f, reelSlider.minValue, reelSlider.maxValue);
        reelTarget = reelTargetSlider.value;

        reelSlider.value = Mathf.Lerp(reelSlider.value, reelTarget + reelOffset, 0.75f * Time.deltaTime);
    }

    IEnumerator setNewRandReelOffset() {
        float timeToSet = 0.25f;
        float maxChange = 0.25f;
        float maxOffset = reelTargetSlider.maxValue - fishSlider.maxValue;

        //  small change
        yield return new WaitForSeconds(timeToSet / 2.0f);
        reelOffset = Mathf.Clamp(reelOffset + Random.Range(-maxChange / 2.0f, maxChange / 2.0f), -maxOffset, maxOffset);

        //  big change
        yield return new WaitForSeconds(timeToSet / 2.0f);
        reelOffset = Mathf.Clamp(reelOffset + Random.Range(-maxChange * 1.5f, maxChange * 1.5f), -maxOffset, maxOffset);


        randReelWaiter = null;
    }



    //  fish shit

    void fishMovement() {
        if(Mathf.Abs(fishSlider.value - fishTarget) < 0.001f && fishWaiter == null)
            fishWaiter = StartCoroutine(waitForNewFishTarget());

        fishSlider.value = Mathf.Lerp(fishSlider.value, fishTarget, 2.0f * Time.deltaTime);
    }


    IEnumerator waitForNewFishTarget() {
        float timeToReset = 0.75f;

        yield return new WaitForSeconds(timeToReset / 2.0f);    //  smol change

        var newTarget = Random.Range(fishSlider.minValue, fishSlider.maxValue);
        while(Mathf.Abs(newTarget - fishTarget) > 0.35f)
            newTarget = Random.Range(fishSlider.minValue, fishSlider.maxValue);

        fishTarget = newTarget;

        yield return new WaitForSeconds(timeToReset / 2.0f);    //  big change

        newTarget = Random.Range(fishSlider.minValue, fishSlider.maxValue);
        while(Mathf.Abs(newTarget - fishTarget) < 0.35f)
            newTarget = Random.Range(fishSlider.minValue, fishSlider.maxValue);

        fishTarget = newTarget;
        fishWaiter = null;
    }
}

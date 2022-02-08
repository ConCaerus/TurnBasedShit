using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FishingCanvas : MonoBehaviour {
    public Slider reelSlider, fishSlider, reelTargetSlider, progressSlider;
    [SerializeField] GameObject fishSliderImage, reelSliderImage;
    [SerializeField] Vector2 spoilsImageEndPos;

    public float fishTarget = 0.0f;
    public float reelTarget = 0.0f;
    float reelOffset = 0.0f;

    float distToGainPoints = .15f;

    public bool running = false;


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

        stopFishing();
    }

    private void Update() {
        if(running) {
            fishMovement();
            reelMovement();
            gainPoints();
            //  fishSliderImage shit
            if(Mathf.Abs(reelSlider.value - fishSlider.value) < distToGainPoints) {
                float scale = 0.1f + (distToGainPoints - Mathf.Abs(reelSlider.value - fishSlider.value)) * .3f;
                fishSliderImage.transform.localScale = new Vector3(scale, scale);
            }
            else
                fishSliderImage.transform.localScale = new Vector3(.1f, .1f);

            fishSliderImage.transform.GetChild(0).transform.rotation = Quaternion.Euler(0.0f, 0.0f, (-20.0f * (1.0f - progressSlider.value)) + (progressSlider.value * 65.0f));


            //  reelSliderImage shit
            if(Mathf.Abs(reelSlider.value - fishSlider.value) < distToGainPoints) {
                float scale = 0.15f + (distToGainPoints - Mathf.Abs(reelSlider.value - fishSlider.value)) * .1f;
                reelSliderImage.transform.localScale = new Vector3(scale, .15f);
            }
            else
                reelSliderImage.transform.localScale = new Vector3(.15f, .15f);


            if(progressSlider.value == 1.0f) {
                stopFishing();
                FindObjectOfType<RoomMovement>().deinteract();

                addSpoils();
                running = false;
            }
        }
    }



    public void startFishing() {
        FindObjectOfType<RoomMovement>().gameObject.transform.localScale = new Vector3(0.0f, 0.0f);
        FindObjectOfType<FishingUnit>().gameObject.transform.localScale = new Vector3(.5f, .5f);
        running = true;
    }

    public void stopFishing() {
        FindObjectOfType<RoomMovement>().gameObject.transform.localScale = new Vector3(.5f, .5f);
        FindObjectOfType<FishingUnit>().gameObject.transform.localScale = new Vector3(0.0f, 0.0f);
        FindObjectOfType<Bobber>().resetValues();

        progressSlider.value = 0.0f;
        running = false;
    }


    bool gainPoints() {
        float diff = Mathf.Abs(fishSlider.value - reelSlider.value);
        float pointsForDiff = Mathf.Clamp(((distToGainPoints - diff)) * Time.deltaTime, -0.002f, 0.002f);
        progressSlider.value = Mathf.Clamp(progressSlider.value + pointsForDiff, 0.0f, 1.0f);

        return pointsForDiff > 0.0f;
    }


    //  reel shit

    void reelMovement() {
        if(randReelWaiter == null) {
            randReelWaiter = StartCoroutine(setNewRandReelOffset());
        }

        //  modify target by scroll val
        float scroll = Input.mouseScrollDelta.y;
        if(Mathf.Abs(scroll) > 0.001f)
            reelTargetSlider.value = Mathf.Clamp(reelTargetSlider.value + scroll / 15.0f, reelTargetSlider.minValue, reelTargetSlider.maxValue);
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


    void addSpoils() {
        var col = calcCaughtFish();
        Inventory.addSingleCollectable(col, FindObjectOfType<PresetLibrary>(), FindObjectOfType<FullInventoryCanvas>());

        FindObjectOfType<Bobber>().showSpoils(col);
    }



    public Collectable calcCaughtFish() {
        var list = FindObjectOfType<PresetLibrary>().getFishableCollectables(GameInfo.getCurrentRegion());
        int count = list.Count;
        float defaultChancePerUnit = 1.0f / count;
        float total = 0.0f;

        //  randomize the list
        int n = count;
        while(n > 1) {
            n--;
            int k = Random.Range(0, n + 1);
            var value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        for(int i = 0; i < count; i++) {
            total += GameVariables.getChanceToCatchFish(list[i].fishedData.chanceToCatch) * 100.0f;
        }

        float rand = Random.Range(0.0f, total);
        for(int i = 0; i < count; i++) {
            var chance = GameVariables.getChanceToCatchFish(list[i].fishedData.chanceToCatch) * 100.0f;
            if(rand < chance)
                return list[i];
            rand -= chance;
        }

        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class CasinoCanvas : MonoBehaviour {
    [SerializeField] GameObject[] machines;
    [SerializeField] TextMeshProUGUI machineNumberText;
    [SerializeField] CoinCount coinCount;

    Coroutine costManager;
    [SerializeField] Slider costSlider;
    [SerializeField] GameObject costBackground;
    [SerializeField] TextMeshProUGUI costText;

    float showTime = 0.15f;
    float numBuffer;


    //  slot mechs
    int cost = 0;
    System.Random rng = new System.Random();
    int numOf0 = 4, numOf1 = 4, numOf2 = 3, numOf3 = 2, numOf7 = 1;
    [SerializeField] GameObject[] slots;
    [SerializeField] GameObject num0Preset, num1Preset, num2Preset, num3Preset, num7Preset;
    List<List<GameObject>> slotNums = new List<List<GameObject>>();

    [SerializeField] GameObject leverRot;

    int[] outcome = new int[3];

    bool spinning = false;


    private void Start() {
        coinCount.updateCount(false);
        costSlider.onValueChanged.AddListener(delegate { updateCostSlider(); });
        costSlider.value = 0.0f;
        updateCostSlider();
        lightUpLever(false);
    }



    public void spawnNewNums() {
        for(int i = 0; i < slotNums.Count; i++) {
            foreach(var s in slotNums[i])
                Destroy(s.gameObject);
        }

        slotNums.Clear();
        slotNums.Add(new List<GameObject>());
        slotNums.Add(new List<GameObject>());
        slotNums.Add(new List<GameObject>());
        numBuffer = (num7Preset.GetComponent<RectTransform>().rect.yMax - num7Preset.GetComponent<RectTransform>().rect.yMin) / 2.0f;

        for(int n = 0; n < 3; n++) {
            for(int i = 0; i < numOf7; i++)
                slotNums[n].Add(Instantiate(num7Preset.gameObject, slots[n].transform));
            for(int i = 0; i < numOf3; i++)
                slotNums[n].Add(Instantiate(num3Preset.gameObject, slots[n].transform));
            for(int i = 0; i < numOf2; i++)
                slotNums[n].Add(Instantiate(num2Preset.gameObject, slots[n].transform));
            for(int i = 0; i < numOf1; i++)
                slotNums[n].Add(Instantiate(num1Preset.gameObject, slots[n].transform));
            for(int i = 0; i < numOf0; i++)
                slotNums[n].Add(Instantiate(num0Preset.gameObject, slots[n].transform));
        }
        shuffleNums();
    }
    public void shuffleNums() {
        for(int n = 0; n < 3; n++) {
            //  randomizes the list
            int c = slotNums[n].Count;
            while(c > 1) {
                c--;
                int k = rng.Next(c + 1);
                GameObject value = slotNums[n][k];
                slotNums[n][k] = slotNums[n][c];
                slotNums[n][c] = value;
            }

            for(int i = 0; i < slotNums[n].Count; i++) {
                slotNums[n][i].transform.localPosition = new Vector3(0.0f, -numBuffer * i, 0.0f);
            }
        }
    }
    public void calcWinnings() {
        int c0 = 0, c1 = 0, c2 = 0, c3 = 0, c7 = 0;
        foreach(var i in outcome) {
            if(i == 0)
                c0++;
            else if(i == 1)
                c1++;
            else if(i == 2)
                c2++;
            else if(i == 3)
                c3++;
            else if(i == 7)
                c7++;
        }

        //  7x return
        if(c7 == 3) {
            //Debug.Log("(" + outcome[0] + ", " + outcome[1] + ", " + outcome[2] + "):  7");
            Inventory.addCoins(cost * 7, coinCount, true);
        }

        //  5x return
        else if(c7 == 2 || (c7 == 1 && c3 == 2)) {
            //Debug.Log("(" + outcome[0] + ", " + outcome[1] + ", " + outcome[2] + "):  5");
            Inventory.addCoins(cost * 5, coinCount, true);
        }

        //  triple return
        else if(c7 == 1 || c3 == 3) {
            //Debug.Log("(" + outcome[0] + ", " + outcome[1] + ", " + outcome[2] + "):  3");
            Inventory.addCoins(cost * 3, coinCount, true);
        }

        //  double return
        else if(c7 == 1 || c3 == 2 || c2 == 3 || (c1 == 1 && c2 == 1 && c3 == 1)) {
            //Debug.Log("(" + outcome[0] + ", " + outcome[1] + ", " + outcome[2] + "):  2");
            Inventory.addCoins(cost * 2, coinCount, true);
        }

        //  single return
        else if(c1 == 3) {
            //Debug.Log("(" + outcome[0] + ", " + outcome[1] + ", " + outcome[2] + "):  1");
            Inventory.addCoins(cost, coinCount, true);
        }


        float percentage = costSlider.value / costSlider.maxValue;
        costSlider.maxValue = Inventory.getCoinCount();
        costSlider.value = Inventory.getCoinCount() * percentage;
        cost = (int)costSlider.value;
        costText.text = cost.ToString();
        costSlider.interactable = true;

        keepCostAmountShown();

        spinning = false;
    }

    public void updateCostSlider() {
        if(spinning)
            return;
        costSlider.maxValue = Inventory.getCoinCount();
        cost = (int)costSlider.value;
        costText.text = cost.ToString();

        keepCostAmountShown();
    }

    public void exit() {
        if(!spinning) {
            FindObjectOfType<RoomMovement>().deinteract();
        }
    }

    public void showCanvas() {
        costSlider.value = 0.0f;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        transform.GetChild(0).gameObject.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), showTime);
        numOf7 = 1;

        float closest = Mathf.Abs(FindObjectOfType<RoomMovement>().transform.position.x - machines[0].transform.position.x);
        int closestIndex = 0;
        for(int i = 1; i < machines.Length; i++) {
            if(Mathf.Abs(FindObjectOfType<RoomMovement>().transform.position.x - machines[i].transform.position.x) < closest) {
                closest = Mathf.Abs(FindObjectOfType<RoomMovement>().transform.position.x - machines[i].transform.position.x);
                closestIndex = i;
            }
        }


        machineNumberText.text = (closestIndex + 1).ToString();
        if(closestIndex == 1)
            numOf7 = 2;

        keepCostAmountShown();

        spawnNewNums();
    }
    public void hideCanvas() {
        if(!spinning) {
            StartCoroutine(hider());
        }
    }
    public IEnumerator hider() {
        transform.GetChild(0).gameObject.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), showTime);
        yield return new WaitForSeconds(showTime);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    IEnumerator manageCostAmount(bool show) {
        if(show) {
            costBackground.transform.DOScale(new Vector3(0.5f, 0.25f, 1.0f), 0.15f);
            costBackground.transform.DOLocalMoveY(30.0f, 0.15f);
        }

        yield return new WaitForSeconds(2.0f);

        if(!costBackground.activeInHierarchy)
            yield return null;

        costBackground.transform.DOScale(new Vector3(0.0f, 0.0f, 0.15f), 0.25f);
        costBackground.transform.DOLocalMoveY(0.0f, 0.15f);

        yield return new WaitForSeconds(0.25f);
        costManager = null;
    }
    public void keepCostAmountShown() {
        if(costManager != null) {
            StopCoroutine(costManager);
            costManager = StartCoroutine(manageCostAmount(false));
        }
        else {
            costManager = StartCoroutine(manageCostAmount(true));
        }
    }


    public void spin() {
        if(!spinning && Inventory.getCoinCount() > 0 && cost > 0) {
            spinning = true;
            outcome[0] = -1;
            outcome[1] = -1;
            outcome[2] = -1;

            Inventory.addCoins(-cost, coinCount, true);

            costText.text = cost.ToString();
            costSlider.interactable = false;

            StartCoroutine(spinAnim());
        }
    }
    IEnumerator spinAnim() {
        float slotSpinWaiter = 0.25f;
        for(int i = 0; i < 3; i++) {
            yield return new WaitForSeconds(slotSpinWaiter * i);

            StartCoroutine(singleSlotAnim(i));
        }

        //  shuffles the numbers after all slots are spinning
        shuffleNums();
    }
    IEnumerator singleSlotAnim(int slotIndex) {
        float speed = 0.01f;
        float stoppingSpeed = 0.35f;
        float speedMult = 1.05f;
        float speedMultMaxDiffer = 0.05f;

        //  spin while valid
        while(speed < stoppingSpeed) {
            //  move slots
            var rand = Random.Range(-(numBuffer / 10.0f), numBuffer / 10.0f);
            for(int i = 0; i < slotNums[slotIndex].Count; i++) {
                slotNums[slotIndex][i].GetComponent<RectTransform>().DOLocalMoveY(slotNums[slotIndex][i].GetComponent<RectTransform>().localPosition.y + numBuffer + rand, speed);
            }

            yield return new WaitForSeconds(speed);

            //  reposition
            float top = 0.0f, bot = 0.0f;
            int topIndex = 0, botIndex = 0;
            for(int i = 0; i < slotNums[slotIndex].Count; i++) {
                //  num is on top
                if(slotNums[slotIndex][i].GetComponent<RectTransform>().localPosition.y > top || i == 0) {
                    top = slotNums[slotIndex][i].GetComponent<RectTransform>().localPosition.y;
                    topIndex = i;
                }

                if(slotNums[slotIndex][i].GetComponent<RectTransform>().localPosition.y < bot || i == 0) {
                    bot = slotNums[slotIndex][i].GetComponent<RectTransform>().localPosition.y;
                    botIndex = i;
                }
            }

            if(top > numBuffer * 2.0f) {
                float botPos = slotNums[slotIndex][botIndex].GetComponent<RectTransform>().localPosition.y - numBuffer;
                slotNums[slotIndex][topIndex].GetComponent<RectTransform>().localPosition = new Vector2(0.0f, botPos);
            }

            speed *= speedMult + Random.Range(-speedMultMaxDiffer, speedMultMaxDiffer);
        }

        //  wait to cleanup
        yield return new WaitForSeconds(0.5f);

        float offset = 0.0f;
        int winnerIndex = -1;
        for(int i = 0; i < slotNums[slotIndex].Count; i++) {
            if(Mathf.Abs(slotNums[slotIndex][i].GetComponent<RectTransform>().localPosition.y) < Mathf.Abs(offset) || i == 0) {
                winnerIndex = i;
                offset = slotNums[slotIndex][i].GetComponent<RectTransform>().localPosition.y;
            }
        }


        //  save as outcome for slot
        if(slotNums[slotIndex][winnerIndex].gameObject.GetComponent<Image>().sprite == num0Preset.gameObject.GetComponent<Image>().sprite)
            outcome[slotIndex] = 0;
        else if(slotNums[slotIndex][winnerIndex].gameObject.GetComponent<Image>().sprite == num1Preset.gameObject.GetComponent<Image>().sprite)
            outcome[slotIndex] = 1;
        else if(slotNums[slotIndex][winnerIndex].gameObject.GetComponent<Image>().sprite == num2Preset.gameObject.GetComponent<Image>().sprite)
            outcome[slotIndex] = 2;
        else if(slotNums[slotIndex][winnerIndex].gameObject.GetComponent<Image>().sprite == num3Preset.gameObject.GetComponent<Image>().sprite)
            outcome[slotIndex] = 3;
        else if(slotNums[slotIndex][winnerIndex].gameObject.GetComponent<Image>().sprite == num7Preset.gameObject.GetComponent<Image>().sprite)
            outcome[slotIndex] = 7;

        //  move numbers accordingly
        for(int i = 0; i < slotNums[slotIndex].Count; i++) {
            if(i == winnerIndex)
                slotNums[slotIndex][i].GetComponent<RectTransform>().DOLocalMoveY(0.0f, speed);
            else
                slotNums[slotIndex][i].GetComponent<RectTransform>().DOLocalMoveY(slotNums[slotIndex][i].GetComponent<RectTransform>().localPosition.y - offset, speed);
        }

        //  ready for result
        if(outcome[0] > -1 && outcome[1] > -1 && outcome[2] > -1) {
            yield return new WaitForSeconds(0.15f);
            calcWinnings();
        }
    }


    public void pullLever() {
        spin();
        leverRot.transform.DOPunchRotation(new Vector3(0.0f, 0.0f, -80.0f), 0.25f);
    }
    public void lightUpLever(bool state) {
        leverRot.transform.GetChild(1).GetChild(0).gameObject.SetActive(state);
    }
}

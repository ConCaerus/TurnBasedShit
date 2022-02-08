using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InfoCanvas : MonoBehaviour {
    bool infoShown = false, optionsShown = false;
    public bool infoWindowShown = false;

    float waitTimeForInfo = 0.5f;

    GameObject infoObj, optionsObj, infoWindow;
    TextMeshProUGUI infoText;
    public InfoBearer shownInfo { get; private set; } = null;
    [SerializeField] Button viewButton, dropButton;
    [SerializeField] Color disabledColor;

    Coroutine infoShower = null;


    private void Awake() {
        infoObj = transform.GetChild(0).gameObject;
        optionsObj = transform.GetChild(1).gameObject;
        infoWindow = transform.GetChild(2).gameObject;
        GetComponent<Canvas>().worldCamera = Camera.main;
        infoText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        hideInfo();
        hideOptions();
        hideInfoWindow();
    }


    private void Update() {
        if(infoShown) {
            positionInfoBox();
        }

        if(Input.GetMouseButtonDown(1) && shownInfo != null) {
            if(shownInfo.optionsCollectableReference != null)
                showOptions(shownInfo.optionsCollectableReference);
        }
        else if((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && optionsShown && !optionsObj.GetComponent<InfoOptions>().mouseOver)
            hideOptions();
    }

    void positionInfoBox() {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<Canvas>().transform as RectTransform, Input.mousePosition, GetComponent<Canvas>().worldCamera, out pos);
        Vector2 offset;
        if(pos.x < 0.0f)
            offset = (infoObj.GetComponent<RectTransform>().sizeDelta / 2.9f) * new Vector2(1.0f, -1.0f);
        else
            offset = (infoObj.GetComponent<RectTransform>().sizeDelta / 2.9f) * new Vector2(-0.9f, -1.0f);
        infoObj.transform.position = GetComponent<Canvas>().transform.TransformPoint(pos + offset);
    }
    void positionOptionsBox() {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<Canvas>().transform as RectTransform, Input.mousePosition, GetComponent<Canvas>().worldCamera, out pos);
        Vector2 offset;
        if(pos.x < 0.0f)
            offset = (optionsObj.GetComponent<RectTransform>().sizeDelta / 2.9f) * new Vector2(1.0f, -1.0f);
        else
            offset = (optionsObj.GetComponent<RectTransform>().sizeDelta / 2.9f) * new Vector2(-0.9f, -1.0f);
        optionsObj.transform.position = GetComponent<Canvas>().transform.TransformPoint(pos + offset);
    }



    void setAndPositionInfo(string info) {
        positionInfoBox();
        infoText.text = info;
    }

    public void startShowing(InfoBearer ib) {
        if(optionsShown)
            return;
        if(infoShower != null) {
            if(shownInfo != ib)
                StopCoroutine(infoShower);
            else
                return;
        }

        shownInfo = ib;
        infoObj.GetComponent<Image>().DOComplete();
        infoText.GetComponent<TextMeshProUGUI>().DOComplete();

        infoShower = StartCoroutine(waitToShow(ib.getInfo()));
    }
    public void showOptions(Collectable col) {
        optionsShown = true;
        optionsObj.SetActive(true);
        optionsObj.GetComponent<InfoOptions>().referenceCol = col;

        if(!Inventory.hasCollectable(col)) {
            dropButton.interactable = false;
            dropButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = disabledColor;
            viewButton.interactable = false;
            viewButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = disabledColor;
        }
        else {
            dropButton.interactable = true;
            dropButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
            viewButton.interactable = true;
            viewButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        }

        positionOptionsBox();

        hideInfo();
    }
    public void showInfoWindow(Collectable col) {
        infoWindow.transform.DOScale(1f, .075f);
        hideOptions();
        hideInfo();

        infoWindow.GetComponent<InfoWindow>().updateInfo(col);

        infoWindowShown = true;
    }

    public void hideInfo() {
        if(infoShower != null)
            StopCoroutine(infoShower);
        infoShower = null;

        infoObj.GetComponent<Image>().DOComplete();
        infoText.GetComponent<TextMeshProUGUI>().DOComplete();

        infoObj.GetComponent<Image>().DOColor(Color.clear, 0.15f);
        infoText.GetComponent<TextMeshProUGUI>().DOColor(Color.clear, 0.15f);

        infoShown = false;
    }
    public void hideOptions() {
        optionsObj.SetActive(false);
        optionsShown = false;
    }
    public void hideInfoWindow() {
        infoWindow.transform.DOScale(0.0f, .15f);
        infoWindowShown = false;
    }

    public void resetShownInfo() {
        shownInfo = null;
    }



    IEnumerator waitToShow(string info) {
        infoText.GetComponent<LayoutElement>().preferredWidth = 200.0f;
        setAndPositionInfo(info);


        if(!infoShown) {
            yield return new WaitForSeconds(waitTimeForInfo);
        }
        else
            yield return new WaitForEndOfFrame();

        if(infoText.textBounds.size.x < 200.0f && infoText.textInfo.lineCount == 1)
            infoText.GetComponent<LayoutElement>().preferredWidth = infoText.textBounds.size.x;

        if(!infoShown) {
            infoObj.GetComponent<Image>().color = Color.clear;
            infoText.GetComponent<TextMeshProUGUI>().color = Color.clear;
            infoObj.GetComponent<Image>().DOColor(Color.black, 0.1f);
            infoText.GetComponent<TextMeshProUGUI>().DOColor(Color.white, 0.1f);
            infoShown = true;

            yield return new WaitForSeconds(0.1f);
        }

        infoShower = null;
    }


    public bool isOpen() {
        return infoShown || optionsShown;
    }


    //  Buttons
    public void view() {
        FindObjectOfType<MenuCanvas>().show();
        switch(optionsObj.GetComponent<InfoOptions>().referenceCol.type) {
            case Collectable.collectableType.Weapon:
                FindObjectOfType<InventoryCanvas>().state = 0;
                break;
            case Collectable.collectableType.Armor:
                FindObjectOfType<InventoryCanvas>().state = 1;
                break;
            case Collectable.collectableType.Item:
                FindObjectOfType<InventoryCanvas>().state = 2;
                break;
            case Collectable.collectableType.Usable:
                FindObjectOfType<InventoryCanvas>().state = 3;
                break;
            case Collectable.collectableType.Unusable:
                FindObjectOfType<InventoryCanvas>().state = 4;
                break;
        }
        FindObjectOfType<MenuCanvas>().inventoryTab();
        hideOptions();
    }

    public void info() {
        showInfoWindow(optionsObj.GetComponent<InfoOptions>().referenceCol);
        hideOptions();
    }
    public void drop() {
        //  "Are you sure" prompt

        //  drop the thing
        Inventory.removeCollectable(optionsObj.GetComponent<InfoOptions>().referenceCol);
        hideOptions();
    }
}

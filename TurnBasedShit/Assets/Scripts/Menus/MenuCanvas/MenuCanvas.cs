using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour {
    [SerializeField] GameObject menuObj;
    [SerializeField] GameObject unitCanvas, questCanvas, townCanvas, inventoryCanvas, graveyardCanvas, optionsCanvas;

    bool showing = false;

    const string openTag = "Opened Tab Index";


    private void Start() {
        GetComponent<Canvas>().worldCamera = Camera.main;
        hardHide();

        openRecentTab();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape)) {
            if(isOpen()) {
                hide();
            }
            else {
                show();
                if(Input.GetKeyDown(KeyCode.Escape))
                    optionsTab();
            }
        }
    }


    void openRecentTab() {
        switch(SaveData.getInt(openTag)) {
            case 0:
            default:
                unitTab();
                break;
            case 1:
                inventoryTab();
                break;
            case 2:
                questTab();
                break;
            case 3:
                townTab();
                break;
            case 4:
                graveyardTab();
                break;

            case 5:
                optionsTab();
                break;
        }
    }


    public void show() {
        if(FindObjectOfType<UnitCanvas>() != null)
            FindObjectOfType<UnitCanvas>().setup();
        else if(FindObjectOfType<QuestCanvas>() != null)
            FindObjectOfType<QuestCanvas>().updateMenu();
        else if(FindObjectOfType<TownMenuCanvas>() != null)
            FindObjectOfType<TownMenuCanvas>().updateSlots();
        else if(FindObjectOfType<GraveyardMenuCanvas>() != null)
            FindObjectOfType<GraveyardMenuCanvas>().createSlots();

        GetComponent<Animator>().StopPlayback();
        GetComponent<Animator>().Play("MenuOpenAnim");
        showing = true;
    }
    public void hide() {
        GetComponent<Animator>().StopPlayback();
        GetComponent<Animator>().Play("MenuCloseAnim");
        showing = false;
    }
    public void hardHide() {
        GetComponent<Animator>().StopPlayback();
        GetComponent<Animator>().Play("MenuIdle");
        showing = false;
    }



    //  tabs
    public void unitTab() {
        SaveData.setInt(openTag, 0);
        unitCanvas.SetActive(true);
        questCanvas.SetActive(false);
        townCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
        graveyardCanvas.SetActive(false);
        optionsCanvas.SetActive(false);

        FindObjectOfType<UnitCanvas>().setup();
    }
    public void inventoryTab() {
        SaveData.setInt(openTag, 1);
        inventoryCanvas.SetActive(true);
        townCanvas.SetActive(false);
        unitCanvas.SetActive(false);
        questCanvas.SetActive(false);
        graveyardCanvas.SetActive(false);
        optionsCanvas.SetActive(false);

        foreach(var i in FindObjectsOfType<InventorySelectionCanvas>())
            i.populateSlots();
    }
    public void questTab() {
        SaveData.setInt(openTag, 2);
        questCanvas.SetActive(true);
        unitCanvas.SetActive(false);
        townCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
        graveyardCanvas.SetActive(false);
        optionsCanvas.SetActive(false);

        FindObjectOfType<QuestCanvas>().updateMenu();
    }
    public void townTab() {
        SaveData.setInt(openTag, 3);
        townCanvas.SetActive(true);
        unitCanvas.SetActive(false);
        questCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
        graveyardCanvas.SetActive(false);
        optionsCanvas.SetActive(false);

        FindObjectOfType<TownMenuCanvas>().updateSlots();
    }
    public void graveyardTab() {
        SaveData.setInt(openTag, 4);
        graveyardCanvas.SetActive(true);
        inventoryCanvas.SetActive(false);
        questCanvas.SetActive(false);
        townCanvas.SetActive(false);
        unitCanvas.SetActive(false);
        optionsCanvas.SetActive(false);

        FindObjectOfType<GraveyardMenuCanvas>().createSlots();
    }
    public void optionsTab() {
        SaveData.setInt(openTag, 5);
        optionsCanvas.SetActive(true);
        graveyardCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
        questCanvas.SetActive(false);
        townCanvas.SetActive(false);
        unitCanvas.SetActive(false);
    }

    public void showUnitCanvas() {
        unitCanvas.SetActive(true);
    }


    public bool isOpen() {
        return showing;
    }
}

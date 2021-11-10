using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour {
    [SerializeField] GameObject menuObj;
    [SerializeField] GameObject unitCanvas, questCanvas, townCanvas, inventoryCanvas, graveyardCanvas, optionsCanvas;

    bool showing = false;

    const string openTag = "Opened Tab Index";

    public delegate void func();
    List<func> runOnOpen = new List<func>();
    List<func> runOnClose = new List<func>();


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
        showing = true;
        if(FindObjectOfType<UnitCanvas>() != null)
            FindObjectOfType<UnitCanvas>().setup();
        else if(FindObjectOfType<QuestCanvas>() != null)
            FindObjectOfType<QuestCanvas>().updateMenu();
        else if(FindObjectOfType<TownMenuCanvas>() != null)
            FindObjectOfType<TownMenuCanvas>().updateSlots();
        else if(FindObjectOfType<InventoryCanvas>() != null)
            FindObjectOfType<InventoryCanvas>().populateSlots();
        else if(FindObjectOfType<GraveyardMenuCanvas>() != null)
            FindObjectOfType<GraveyardMenuCanvas>().createSlots();

        foreach(var i in runOnOpen)
            i();

        GetComponent<Animator>().StopPlayback();
        GetComponent<Animator>().Play("MenuOpenAnim");

        foreach(var i in FindObjectsOfType<InfoBearer>()) {
            if(i.hideWhenMenuOpen)
                i.GetComponent<Collider2D>().enabled = false;
            else
                i.GetComponent<Collider2D>().enabled = true;
        }
    }
    public void hide() {
        GetComponent<Animator>().StopPlayback();
        GetComponent<Animator>().Play("MenuCloseAnim");

        foreach(var i in runOnClose)
            i();
        
        foreach(var i in FindObjectsOfType<InfoBearer>()) {
            if(i.hideWhenMenuOpen)
                i.GetComponent<Collider2D>().enabled = true;
            else
                i.GetComponent<Collider2D>().enabled = false;
        }
        showing = false;
    }
    public void hardHide() {
        foreach(var i in FindObjectsOfType<InfoBearer>()) {
            if(i.GetComponent<Collider2D>() == null) {
                Debug.Log(i.name);
                continue;
            }
            if(i.hideWhenMenuOpen)
                i.GetComponent<Collider2D>().enabled = true;
            else
                i.GetComponent<Collider2D>().enabled = false;
        }
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

        FindObjectOfType<InventoryCanvas>().populateSlots();
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

    public void addNewRunOnOpen(func f) {
        runOnOpen.Add(f);
    }
    public void addNewRunOnClose(func f) {
        runOnClose.Add(f);
    }


    public bool isOpen() {
        return showing;
    }
}

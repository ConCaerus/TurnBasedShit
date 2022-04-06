using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour {
    [SerializeField] GameObject menuObj;
    [SerializeField] GameObject unitCanvas, questCanvas, townCanvas, inventoryCanvas, graveyardCanvas, optionsCanvas, collectionCanvas;

    bool showing = false;

    const string openTag = "Opened Tab Index";

    public delegate void func();
    List<func> runOnOpen = new List<func>();
    List<func> runOnClose = new List<func>();


    private void Start() {
        GetComponent<Canvas>().worldCamera = Camera.main;
        hardHide();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape)) {
            if(isOpen()) {
                hide();
            }
            else {
                openRecentTab();
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
                collectionTab();
                break;
            case 3:
                questTab();
                break;
            case 4:
                townTab();
                break;
            case 5:
                graveyardTab();
                break;
            case 6:
                optionsTab();
                break;
        }
    }


    public void show() {
        //  player wanted to close other menus, not up this one
        if(FindObjectOfType<MapMerchantCanvas>() != null && FindObjectOfType<MapMerchantCanvas>().isOpen()) {
            FindObjectOfType<MapMerchantCanvas>().hide();
            return;
        }
        else if(FindObjectOfType<FullInventoryCanvas>() != null && FindObjectOfType<FullInventoryCanvas>().isOpen()) {
            FindObjectOfType<FullInventoryCanvas>().done();
            return;
        }
        //  add all of the building canvases here, also any thing's that I missed
        showing = true;

        if(FindObjectOfType<UnitCanvas>() != null)
            FindObjectOfType<UnitCanvas>().setup();
        else if(FindObjectOfType<QuestCanvas>() != null)
            FindObjectOfType<QuestCanvas>().updateMenu();
        else if(FindObjectOfType<TownMenuCanvas>() != null)
            FindObjectOfType<TownMenuCanvas>().updateSlots();
        else if(FindObjectOfType<InventoryCanvas>() != null)
            FindObjectOfType<InventoryCanvas>().populateSlots();
        else if(FindObjectOfType<CollectionCanvas>() != null)
            FindObjectOfType<CollectionCanvas>().populateSlots();
        else if(FindObjectOfType<GraveyardMenuCanvas>() != null)
            FindObjectOfType<GraveyardMenuCanvas>().createSlots();

        foreach(var i in runOnOpen)
            i();

        GetComponent<Animator>().StopPlayback();
        GetComponent<Animator>().Play("MenuOpenAnim");

        foreach(var i in FindObjectsOfType<InfoBearer>()) {
            if(!i.showNoMatterMenuState) {
                i.GetComponent<Collider2D>().enabled = !i.hideWhenMenuOpen;
            }
        }
    }
    public void hide() {
        GetComponent<Animator>().StopPlayback();
        GetComponent<Animator>().Play("MenuCloseAnim");

        foreach(var i in runOnClose)
            i();

        foreach(var i in FindObjectsOfType<InfoBearer>()) {
            if(!i.showNoMatterMenuState) {
                i.GetComponent<Collider2D>().enabled = i.hideWhenMenuOpen;
            }
        }
        showing = false;
    }
    public void hardHide() {
        foreach(var i in FindObjectsOfType<InfoBearer>()) {
            if(!i.showNoMatterMenuState) {
                if(i.GetComponent<Collider2D>() == null) {
                    continue;
                }
                i.GetComponent<Collider2D>().enabled = i.hideWhenMenuOpen;
            }
        }
        GetComponent<Animator>().StopPlayback();
        GetComponent<Animator>().Play("MenuIdle");
        showing = false;
    }



    //  tabs
    void closeAllTabs() {
        unitCanvas.SetActive(false);
        questCanvas.SetActive(false);
        townCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
        graveyardCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        collectionCanvas.SetActive(false);
    }
    public void unitTab() {
        SaveData.setInt(openTag, 0);
        closeAllTabs();
        unitCanvas.SetActive(true);

        FindObjectOfType<UnitCanvas>().setup();
    }
    public void inventoryTab() {
        SaveData.setInt(openTag, 1);
        closeAllTabs();
        inventoryCanvas.SetActive(true);

        FindObjectOfType<InventoryCanvas>().populateSlots();
    }
    public void collectionTab() {
        SaveData.setInt(openTag, 2);
        closeAllTabs();
        collectionCanvas.SetActive(true);

        FindObjectOfType<CollectionCanvas>().populateSlots();
    }
    public void questTab() {
        SaveData.setInt(openTag, 3);
        closeAllTabs();
        questCanvas.SetActive(true);

        FindObjectOfType<QuestCanvas>().updateMenu();
    }
    public void townTab() {
        SaveData.setInt(openTag, 4);
        closeAllTabs();
        townCanvas.SetActive(true);

        FindObjectOfType<TownMenuCanvas>().updateSlots();
    }
    public void graveyardTab() {
        SaveData.setInt(openTag, 5);
        closeAllTabs();
        graveyardCanvas.SetActive(true);

        FindObjectOfType<GraveyardMenuCanvas>().createSlots();
    }
    public void optionsTab() {
        SaveData.setInt(openTag, 6);
        closeAllTabs();
        optionsCanvas.SetActive(true);
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

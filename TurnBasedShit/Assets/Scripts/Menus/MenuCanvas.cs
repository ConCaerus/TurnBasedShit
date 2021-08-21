using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour {
    [SerializeField] GameObject menuObj;
    [SerializeField] GameObject unitCanvas, questCanvas, townCanvas;

    bool showing = false;


    private void Start() {
        hardHide();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Tab)) {
            if(isOpen()) {
                hide();
            }
            else 
                show();
        }
    }


    public void show() {
        if(FindObjectOfType<UnitCanvas>() != null)
            FindObjectOfType<UnitCanvas>().setup();
        else if(FindObjectOfType<QuestCanvas>() != null)
            FindObjectOfType<QuestCanvas>().updateMenu();
        else if(FindObjectOfType<TownMenuCanvas>() != null)
            FindObjectOfType<TownMenuCanvas>().updateSlots();

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
        unitCanvas.SetActive(true);
        questCanvas.SetActive(false);
        townCanvas.SetActive(false);

        FindObjectOfType<UnitCanvas>().setup();
    }
    public void inventoryTab() {

    }
    public void questTab() {
        questCanvas.SetActive(true);
        unitCanvas.SetActive(false);
        townCanvas.SetActive(false);

        FindObjectOfType<QuestCanvas>().updateMenu();
    }
    public void townTab() {
        townCanvas.SetActive(true);
        unitCanvas.SetActive(false);
        questCanvas.SetActive(false);

        FindObjectOfType<TownMenuCanvas>().updateSlots();
    }


    public void showUnitCanvas() {
        unitCanvas.SetActive(true);
    }


    public bool isOpen() {
        return showing;
    }
}

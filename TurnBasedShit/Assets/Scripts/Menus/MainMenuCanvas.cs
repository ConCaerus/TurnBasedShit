﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour {
    [SerializeField] GameObject menuCanvas, savesCanvas;
    [SerializeField] GameObject nameInputCanvas;


    public bool createNewSave { get; set; }

    private void Awake() {
        menuCanvas.SetActive(true);
        savesCanvas.SetActive(false);
        nameInputCanvas.SetActive(false);
    }

    public void quit() {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void updateNameText(string s) {
        nameInputCanvas.GetComponentInChildren<TextMeshProUGUI>().text = s;
        foreach(var i in FindObjectsOfType<SaveSlotInformation>()) {
            if(i.saveIndex == SaveData.saveIndex) {
                i.setName(s);
                break;
            }
        }
    }
    public void setNameText(string s) {
        nameInputCanvas.SetActive(false);
        SaveData.setString("Save Name", s);
        SaveData.deleteCurrentSave();
        loadCurrentState();
    }

    void loadCurrentState() {
        if(!SaveData.hasSaveDataForCurrentSave())
            SaveData.createSaveDataForCurrentSave();

        switch(GameState.currentGameState) {
            case GameState.state.combat:
                SceneManager.LoadScene("Combat");
                break;
            case GameState.state.map:
                SceneManager.LoadScene("Map");
                break;
            case GameState.state.town:
                SceneManager.LoadScene("Town");
                break;
        }
    }


    //  games buttons
    public void loadGame(int index) {
        SaveData.saveIndex = index;

        if(createNewSave) {
            nameInputCanvas.SetActive(true);
            StartCoroutine(FindObjectOfType<TextInputReader>().reader(updateNameText, setNameText));
        }
        else {
            loadCurrentState();
        }
    }
}

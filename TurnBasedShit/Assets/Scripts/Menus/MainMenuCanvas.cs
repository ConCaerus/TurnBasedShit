using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour {
    [SerializeField] GameObject menuCanvas, savesCanvas;
    [SerializeField] GameObject nameInputCanvas;
    [SerializeField] AudioClip music;


    public bool createNewSave { get; set; }

    private void Awake() {
        menuCanvas.SetActive(true);
        savesCanvas.SetActive(false);
        nameInputCanvas.SetActive(false);
    }

    private void Start() {
        FindObjectOfType<AudioManager>().playMusic(music, true);
    }

    public void quit() {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void updateNameText(string s) {
        nameInputCanvas.GetComponentInChildren<TextMeshProUGUI>().text = s;
        foreach(var i in FindObjectsOfType<SaveSlotInformation>()) {
            if(i.saveIndex == SaveData.getCurrentSaveIndex()) {
                i.setName(s);
                break;
            }
        }
    }
    public void setNameText(string s) {
        nameInputCanvas.SetActive(false);
        SaveData.setString("Save Name", s);
        StartCoroutine(loadCurrentState());
    }

    IEnumerator loadCurrentState() {
        FindObjectOfType<TransitionCanvas>().showBackground();

        yield return new WaitForSeconds(FindObjectOfType<TransitionCanvas>().getTransitionTime());

        if(createNewSave) {
            SaveData.deleteCurrentSave();
            SaveData.createSaveDataForCurrentSave(FindObjectOfType<PresetLibrary>(), FindObjectOfType<TransitionCanvas>());
        }

        switch(GameInfo.currentGameState) {
            case GameInfo.state.combat:
                SceneManager.LoadSceneAsync("Combat");
                break;
            case GameInfo.state.map:
                SceneManager.LoadSceneAsync("Map");
                break;
            case GameInfo.state.town:
                SceneManager.LoadSceneAsync("Town");
                break;
        }
    }


    //  games buttons
    public void loadGame(int index) {
        SaveData.setCurrentSaveIndex(index);

        if(createNewSave) {
            nameInputCanvas.SetActive(true);
            FindObjectOfType<TextInputReader>().startReading(GameVariables.getMaxPlayerUnitNameLength(), updateNameText, setNameText);
        }
        else {
            StartCoroutine(loadCurrentState());
        }
    }
}

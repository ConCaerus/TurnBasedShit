using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour {
    [SerializeField] GameObject menuCanvas, savesCanvas;

    //  0 - new game, 1 - continue game
    int loadState = 0;

    private void Awake() {
        menuCanvas.SetActive(true);
        savesCanvas.SetActive(false);
    }

    //  Buttons
    public void setLoadState(int i) {
        loadState = i;
    }

    public void quit() {
        Debug.Log("Quit");
        Application.Quit();
    }

    //  games buttons
    public void loadGame(int index) {
        SaveData.saveIndex = index;
        SceneManager.LoadScene("Map");
    }
}

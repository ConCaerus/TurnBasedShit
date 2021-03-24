using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    private void Awake() {
        setPause(false);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape))
            setPause(!transform.GetChild(0).gameObject.activeInHierarchy);
    }


    public void setPause(bool b) {
        transform.GetChild(0).gameObject.SetActive(b);
        if(b) 
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;
    }

    //  Buttons
    public void mainMenu() {
        setPause(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void quit() {
        Debug.Log("Quit");
        Application.Quit();
    }
}

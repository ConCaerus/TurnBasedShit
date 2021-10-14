using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsCanvas : MonoBehaviour {


    public void mainMenu() {
        FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("MainMenu");
    }

    public void quitToDesktop() {
        FindObjectOfType<TransitionCanvas>().loadSceneWithFunction(delegate { Debug.Log("Quit"); Application.Quit(); });
    }
}

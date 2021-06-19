using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour {
    [SerializeField] GameObject menuObj;

    bool showing = false;


    private void Start() {
        hardHide();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Tab)) {
            if(isShowing()) {
                hide();
            }
            else 
                show();
        }
    }


    public void show() {
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

    public bool isShowing() {
        return showing;
    }
}

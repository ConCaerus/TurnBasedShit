using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBearer : MonoBehaviour {
    [SerializeField] string info = "<b><u>Information";

    public bool hideWhenMenuOpen = true;


    public delegate void func();
    func mouseOverFunc = null, mouseExitFunc = null;


    private void OnMouseEnter() {
        if(hideWhenMenuOpen && FindObjectOfType<MenuCanvas>().isOpen())
            return;
        if(string.IsNullOrEmpty(info))
            return;
        FindObjectOfType<InfoCanvas>().startShowing(this);

        if(mouseOverFunc != null)
            mouseOverFunc();
    }

    private void OnMouseExit() {
        FindObjectOfType<InfoCanvas>().startHiding();

        if(mouseExitFunc != null)
            mouseExitFunc();
    }


    public void setInfo(string st, bool hasUnderlinedBold = true) {

        if(hasUnderlinedBold)
            info = "<b><u>" + st;
        else
            info = st;

        if(FindObjectOfType<InfoCanvas>().shownInfo == this)
            FindObjectOfType<InfoCanvas>().startShowing(this);
    }
    public string getInfo() {
        return info;
    }


    public void runOnMouseOver(func f) {
        mouseOverFunc = f;
    }
    public void runOnMouseExit(func f) {
        mouseExitFunc = f;
    }
}

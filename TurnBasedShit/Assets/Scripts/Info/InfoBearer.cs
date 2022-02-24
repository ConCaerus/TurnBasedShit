using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBearer : MonoBehaviour {
    [SerializeField] string info = "<b><u>Information";

    public bool showNoMatterMenuState = false;
    public bool hideWhenMenuOpen = true;
    public bool hideWhenCoveredByMapFog = false;
    public Collectable optionsCollectableReference = null;


    public delegate void func();
    func mouseOverFunc = null, mouseExitFunc = null;


    private void OnMouseEnter() {
        if(hideWhenMenuOpen && !showNoMatterMenuState && FindObjectOfType<MenuCanvas>().isOpen())
            return;
        if(hideWhenCoveredByMapFog && !FindObjectOfType<MapFogTexture>().isPositionCleared(transform.position))
            return;
        if(string.IsNullOrEmpty(info))
            return;
        FindObjectOfType<InfoCanvas>().showInfo(this);

        if(mouseOverFunc != null)
            mouseOverFunc();
    }

    private void OnMouseExit() {
        FindObjectOfType<InfoCanvas>().hideInfo();
        FindObjectOfType<InfoCanvas>().resetShownInfo();

        if(mouseExitFunc != null)
            mouseExitFunc();
    }


    public void setInfo(string st, bool hasUnderlinedBold = true) {
        if(hasUnderlinedBold)
            info = "<b><u>" + st;
        else
            info = st;

        if(FindObjectOfType<InfoCanvas>().shownInfo == this)
            FindObjectOfType<InfoCanvas>().showInfo(this);
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

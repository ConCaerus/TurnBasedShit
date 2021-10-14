using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBearer : MonoBehaviour {
    public List<string> infos = new List<string>() { "Information" };

    public bool hideWhenMenuOpen = true;

    private void OnMouseEnter() {
        if(hideWhenMenuOpen && FindObjectOfType<MenuCanvas>().isOpen())
            return;
        FindObjectOfType<InfoCanvas>().startShowing(infos[0]);
    }

    private void OnMouseExit() {
        FindObjectOfType<InfoCanvas>().startHiding();
    }


    public void show() {
        if(hideWhenMenuOpen && FindObjectOfType<MenuCanvas>().isOpen())
            return;
        if(infos.Count == 0)
            return;
        FindObjectOfType<InfoCanvas>().startShowing(infos[0]);
    }
    public void hide() {
        FindObjectOfType<InfoCanvas>().startHiding();
    }
}

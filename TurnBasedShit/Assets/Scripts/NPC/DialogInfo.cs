using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogInfo {
    public string mainText = "", firstOption = "", secondOption = "";

    public DialogInfo firstResponseDialog;
    public DialogInfo secondResponseDialog;

    public delegate void func();
    public func firstAction = null, secondAction = null;

    public DialogInfo() {
        firstResponseDialog = null;
        secondResponseDialog = null;
    }
    public DialogInfo(string m) {
        mainText = m;
        firstResponseDialog = null;
        secondResponseDialog = null;
    }
    public DialogInfo(string m, string f) {
        mainText = m;
        firstOption = f;
        firstResponseDialog = null;
        secondResponseDialog = null;
    }
    public DialogInfo(string m, string f, string s) {
        mainText = m;
        firstOption = f;
        secondOption = s;
        firstResponseDialog = null;
        secondResponseDialog = null;
    }
    public DialogInfo(string m, string f, string s, DialogInfo firstNext, DialogInfo secondNext) {
        mainText = m;
        firstOption = f;
        secondOption = s;
        firstResponseDialog = firstNext;
        secondResponseDialog = secondNext;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour {

    DialogInfo dialog = null;
    DialogBox box;

    private void Start() {
        box = FindObjectOfType<DialogBox>();
        box.setName("Tobster");

        dialog = new DialogInfo("Hey [name], wanna go fucking " + FindObjectOfType<TextCreator>().createQuestText("Fishing") + " with me?");
        dialog.firstOption = "Yes";
        dialog.secondOption = "No";
        dialog.firstResponseDialog = dialog;
        dialog.secondResponseDialog = dialog;

        box.setDialog(dialog);
        box.showDialog();
    }
}
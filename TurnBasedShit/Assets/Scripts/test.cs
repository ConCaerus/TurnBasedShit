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

        dialog = DialogLibrary.getTobyDialog();

        box.setDialog(dialog);
        box.showDialog();
    }
}
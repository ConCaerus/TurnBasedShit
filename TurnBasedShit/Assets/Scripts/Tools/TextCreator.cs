using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextCreator : MonoBehaviour {
    [SerializeField] List<Color> colors = new List<Color>();


    public string createColoredText(string s, int cIndex = 0) {
        var hex = ColorUtility.ToHtmlStringRGBA(colors[cIndex]);
        return "<color=#" + hex + ">" + s + "</color>";
    }
}

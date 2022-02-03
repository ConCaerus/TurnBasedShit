using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotObject : MonoBehaviour {
    public Image[] images;
    public TextMeshProUGUI[] texts;
    public InfoBearer info;

    public Button button;


    public void setImage(int index, Sprite sp) {
        images[index].sprite = sp;
    }
    public void setImageColor(int index, Color color) {
        images[index].color = color;
    }

    public void setText(int index, string t) {
        texts[index].text = t;
    }

    public void setInfo(string t) {
        info.setInfo(t);
    }
}

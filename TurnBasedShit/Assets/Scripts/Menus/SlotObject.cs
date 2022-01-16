using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotObject : MonoBehaviour {
    public Image mainImage, secondImage;
    public TextMeshProUGUI text;
    public InfoBearer info;


    public void setMainImage(Sprite sp) {
        mainImage.sprite = sp;
    }
    public void setSecondImage(Sprite sp) {
        secondImage.sprite = sp;
    }
    public void setMainImageColor(Color c) {
        mainImage.color = c;
    }
    public void setSecondImageColor(Color c) {
        secondImage.color = c;
    }

    public void setText(string t) {
        text.text = t;
    }

    public void setInfo(string t) {
        info.setInfo(t);
    }
}

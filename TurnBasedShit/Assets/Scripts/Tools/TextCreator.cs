using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


public class TextCreator : MonoBehaviour {
    public Color[] colors;
    public Color questColor;


    public string createColoredText(string s, int index = 0) {
        var hex = ColorUtility.ToHtmlStringRGBA(colors[index]);
        return "<color=#" + hex + ">" + s + "</color>";
    }


    public string createQuestText(string s) {
        var hex = ColorUtility.ToHtmlStringRGBA(questColor);
        return "<color=#" + hex + "><b><size=115%>" + s + "</color></b><size=100%>";
    }

    public string createUnitNameText(UnitStats stats) {
        float modifier = 0.15f;
        var color = stats.u_sprite.color;
        if(color.r < modifier && color.g < modifier && color.b < modifier)
            color += new Color(modifier, modifier, modifier);
        var hex = ColorUtility.ToHtmlStringRGBA(color);
        return "<color=#" + hex + "><b><size=115%>" + stats.u_name + "</color></b><size=100%>";
    }



    public void animateText(string s, TextMeshProUGUI text) {
        StopAllCoroutines();
        StartCoroutine(animateWords(applyCustomCommands(s), text));
    }


    string applyCustomCommands(string s) {
        bool reading = false;
        string command = "";

        var temp = "";

        foreach(var i in s) {
            if(i == '[') {
                reading = true;
                command = "";
                command += i;
                continue;
            }

            if(reading) {
                command += i;

                if(i == ']') {
                    temp += getStringForCommand(command);
                    reading = false;
                }
                continue;
            }

            temp += i;
        }

        return temp;
    }

    string getStringForCommand(string command) {
        //  uses to lower to decrease oopsies
        switch(command.ToLower()) {
            case "[name]": return  createUnitNameText(Party.getLeaderStats());
            case "[partysize]": return Party.getMemberCount().ToString();
            case "[region]": return GameInfo.getCurrentRegion().ToString();
            case "[money]": return Inventory.getCoinCount().ToString();

            default: return "";
        }
    }

    IEnumerator animateWords(string s, TextMeshProUGUI t, int curIndex = 0, string curString = "") {
        //  adds commands without delay
        while(s[curIndex] == '<') {
            while(s[curIndex] != '>') {
                curString += s[curIndex];
                curIndex++;
            }
            curString += s[curIndex];
            curIndex++;
        }

        //  stirng to be sent to the actual tmPro
        var temp = curString + "<size=50%>" + s[curIndex];

        //  string to be referenced to later
        curString += s[curIndex];


        curIndex++;
        t.text = temp;

        //  doesn't wait for spaces
        if(curIndex < s.Length && s[curIndex] != ' ')
            yield return new WaitForSeconds(0.005f);

        if(curIndex < s.Length)
            StartCoroutine(animateWords(s, t, curIndex, curString));
        else
            t.text = s;
    }
}

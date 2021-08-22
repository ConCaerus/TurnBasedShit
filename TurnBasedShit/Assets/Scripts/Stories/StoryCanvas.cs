using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryCanvas : MonoBehaviour {
    [SerializeField] GameObject storyObj;
    [SerializeField] TextMeshProUGUI story, cause, cause2;
    [SerializeField] List<TextMeshProUGUI> choices;
    public Story currentStory;

    private void Start() {
        updateText();
    }


    private void Update() {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            updateStory(0);
        else if(Input.GetKeyDown(KeyCode.Alpha2))
            updateStory(1);
        else if(Input.GetKeyDown(KeyCode.Alpha3))
            updateStory(2);
        else if(Input.GetKeyDown(KeyCode.Alpha4))
            updateStory(3);
        else if(Input.GetKeyDown(KeyCode.Space))
            updateText();
    }

    public void updateStory(int choice) {
        //  ends the story if the current story is the end
        if(currentStory.isEnd) {
            endStory();
            return;
        }

        currentStory = currentStory.getNextStory(choice);
        updateText();
    }

    public void endStory() {
        storyObj.SetActive(false);
    }

    public void playStory(Story s) {
        currentStory = s;
        storyObj.SetActive(true);
        updateText();
    }

    void updateText() { 
        story.text = currentStory.mainText;

        //  choice text
        foreach(var i in choices)
            i.text = "";
        for(int i = 0; i < currentStory.getChoiceCount(); i++) {
            choices[i].text = (i+1).ToString() + ". " + currentStory.getChoiceTitle(i);
        }

        //  cause text
        cause.text = "";
        cause2.text = "";
        if(currentStory.affectsParty)
            currentStory.affectParty(cause, cause2);
    }
}

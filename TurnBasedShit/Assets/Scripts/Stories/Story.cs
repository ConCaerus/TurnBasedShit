using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Story", menuName = "Presets/Story")]
public class Story : ScriptableObject {
    [TextArea]
    public string story;

    public List<StoryChoice> choices;


    public Story getNextStory(int choice) {
        return choices[choice].choice;
    }
    public string getChoiceText(int choice) {
        return choices[choice].story;
    }
}


[System.Serializable]
public class StoryChoice {
    [TextArea]
    public string story;
    public Story choice;
}


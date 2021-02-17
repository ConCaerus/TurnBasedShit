using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestStorySystem : MonoBehaviour {
    [SerializeField] TextMeshProUGUI story;
    [SerializeField] List<TextMeshProUGUI> choices;
    public Story currentStory;

    public float offset = 2.5f;

    private void Awake() {
        choices.Clear();
    }


    public void advanceStory(int choice) {
        currentStory = currentStory.getNextStory(choice);

        if(choices.Count > 0) {
            foreach(var i in choices)
                Destroy(i.gameObject);
        }
        choices.Clear();

        story.text = currentStory.story;
        for(int i = 0; i < currentStory.choices.Count; i++) {
            var temp = Instantiate(story);
            temp.transform.position = story.transform.position - new Vector3(0.0f, offset * (i + 1), 0.0f);
            temp.text = currentStory.getChoiceText(i);
            temp.GetComponent<RectTransform>().SetParent(story.transform.parent.transform);

            choices.Add(temp);
        }
    }

    public void resetStory() {
        if(choices.Count > 0) {
            foreach(var i in choices)
                Destroy(i.gameObject);
        }
        choices.Clear();

        story.text = currentStory.story;
        for(int i = 0; i < currentStory.choices.Count; i++) {
            var temp = Instantiate(story);
            temp.transform.position = story.transform.position - new Vector3(0.0f, offset * (i + 1), 0.0f);
            temp.text = currentStory.getChoiceText(i);
            temp.GetComponent<RectTransform>().SetParent(story.transform.parent.transform);

            choices.Add(temp);
        }
    }
}

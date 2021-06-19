using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building {
    public const int buildingTypeCount = 8;
    public enum type {
        Empty, Hospital, Shop, Brothel, Church, House
    }

    public type b_type;
    public bool isOnlyOne;
    public bool canBeInteractedWith;

    public SpriteLoader b_sprite;

    public List<Story> b_storyBeginnings;
    public List<Quest> b_quests = new List<Quest>();


    public void setup(PresetLibrary lib) {
        if(b_quests == null) {
            b_quests = new List<Quest>();
            b_quests.Clear();
            if(b_type == type.House) {
                int rand = Random.Range(0, 101);

                //  house has quests available
                if(rand > 25) {
                    canBeInteractedWith = true;

                    int count = Random.Range(1, 3);
                    for(int i = 0; i < count; i++) {
                        b_quests.Add(lib.createRandomQuest());
                    }
                }
                else
                    canBeInteractedWith = false;
            }
        }
    }

    public Building(type t = type.Empty) {
        b_type = t;
    }
    public Building(Building b) {
        b_type = b.b_type;
        isOnlyOne = b.isOnlyOne;
        canBeInteractedWith = b.canBeInteractedWith;
    }

    public Story getRandStory() {
        int rand = Random.Range(0, b_storyBeginnings.Count);
        return b_storyBeginnings[rand];
    }
}
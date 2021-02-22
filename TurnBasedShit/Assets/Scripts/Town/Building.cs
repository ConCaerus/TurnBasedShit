using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building {
    public const int buildingTypeCount = 8;
    public enum type {
        Empty, Hospital, WeaponShop, ArmorShop, ItemShop, Brothel, Church, House
    }

    public type b_type;
    public bool isOnlyOne;
    public bool canBeInteractedWith;

    public SpriteLocation b_sprite;

    public List<Story> buildingStoryBeginnings;


    public Building(type t = type.Empty) {
        b_type = t;
    }
    public Building(Building b) {
        b_type = b.b_type;
        isOnlyOne = b.isOnlyOne;
        canBeInteractedWith = b.canBeInteractedWith;
    }

    public Story getRandStory() {
        int rand = Random.Range(0, buildingStoryBeginnings.Count);
        return buildingStoryBeginnings[rand];
    }
}
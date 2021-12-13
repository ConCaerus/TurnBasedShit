using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingQuest : Quest {
    public Collectable fish;


    public FishingQuest(Collectable f) {
        fish = f;
        if(!fish.canBeFished)
            fish = null;
    }

    public override questType getType() {
        return questType.fishing;
    }
}

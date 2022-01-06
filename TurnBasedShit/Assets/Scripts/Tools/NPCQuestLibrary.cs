using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCQuestLibrary : MonoBehaviour {
    [SerializeField] UnusablePreset tobysFishable;

    public Quest getQuestForNPC(TownMember npc) {
        if(!npc.isNPC) return null;
        switch(npc.name) {
            case "Toby": return getTobyQuest();

            default: return null;
        }
    }


    public FishingQuest getTobyQuest() {
        var loc = FindObjectOfType<PresetLibrary>().createFishingLocation(GameInfo.getCurrentRegion());
        loc.fish = FindObjectOfType<PresetLibrary>().getUnusable(tobysFishable.preset);
        FishingQuest qu = new FishingQuest(loc, true);
        return qu;
    }
}

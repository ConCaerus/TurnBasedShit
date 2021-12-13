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


    public Quest getTobyQuest() {
        FishingQuest qu = new FishingQuest(tobysFishable.preset);
        return qu;
    }
}

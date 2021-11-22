using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogLibrary {

    public static DialogInfo getDialogForTownMember(TownMember mem) {
        if(mem.isNPC)
            return getDialogForNPC(mem);
        if(mem.hasQuest)
            return getGenericDialogForQuest(mem.questType);
        return null;
    }

    public static DialogInfo getDialogForNPC(TownMember npc) {
        if(npc.name == "Toby")
            return getTobyDialog();
        return null;
    }

    public static DialogInfo getGenericDialogForQuest(GameInfo.questType type) {
        switch(type) {
            case GameInfo.questType.kill:
                return new DialogInfo("Kill", "Ok", "No");

            case GameInfo.questType.rescue:
                return new DialogInfo("Rescue", "Ok", "No");

            case GameInfo.questType.pickup:
                return new DialogInfo("Pickup", "Ok", "No");

            case GameInfo.questType.delivery:
                return new DialogInfo("Deliver", "Ok", "No");

            case GameInfo.questType.bossFight:
                return new DialogInfo("Boss", "Ok", "No");
        }

        return null;
    }


    public static DialogInfo getTobyDialog() {
        var main = new DialogInfo("Up for a good game of fishing, old sport?", "Sure thing, champ", "Fish creep me out");
        var f = new DialogInfo("Well said, slugger. Grab your nearest rod and head to the lake", "Hell yeah, brother!", "lakes creep me out");
        var s = new DialogInfo("I understand completely. Fishing are monsters of the deep that need to be removed from modern society. I myself am deeply afraid of the creatures, but someone has to " +
            "delve their line into the murky depths to rid the world of the monsters. I hope one day you find the courage to aid my in my noble quest. Good day my friend", "Neat");
        
        main.firstResponseDialog = f;
        main.secondResponseDialog = s;
        f.firstResponseDialog= main;
        f.secondResponseDialog = s;
        s.firstResponseDialog = main;

        return main;
    }
}

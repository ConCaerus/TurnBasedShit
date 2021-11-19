using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogLibrary {


    public static DialogInfo getDialog() {
        var temp = new DialogInfo("Fuck Bithces?", "Hell Yeah Brobther!", "Not today, sorry");
        var f = new DialogInfo("HELL YEAH BROBTHER!!!", "Next");
        var s = new DialogInfo("Coward", "Next");

        temp.firstResponseDialog = f;
        temp.secondResponseDialog = s;

        f.firstResponseDialog = temp;
        s.firstResponseDialog = temp;

        return temp;
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

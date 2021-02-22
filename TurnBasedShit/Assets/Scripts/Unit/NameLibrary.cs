using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NameLibrary {
    static List<string> names = new List<string>() {
        "Carl", "Victor", "Harold", "Felix", "Vermont", "Lars", "Tars", "Mars", "Lucky"
    };


    public static string getRandomName() {
        int rand = Random.Range(0, names.Count);
        return names[rand];
    }

    public static string getRandomUsableName() {
        List<string> temp = new List<string>();

        foreach(var i in names)
            temp.Add(i);

        for(int i = 0; i < Party.getPartySize(); i++) {
            foreach(var n in temp) {
                if(n == Party.getMemberStats(i).u_name) {
                    temp.Remove(n);
                    break;
                }
            }
        }

        return temp[Random.Range(0, temp.Count)];
    }
}

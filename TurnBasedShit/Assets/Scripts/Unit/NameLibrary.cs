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
}

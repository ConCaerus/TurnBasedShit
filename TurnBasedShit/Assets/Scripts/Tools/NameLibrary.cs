using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NameLibrary {
    static List<string> playerNames = new List<string>() {
        "Carl", "Victor", "Harold", "Felix", "Vermont", "Lars", "Tars", "Mars", "Lucky", "Courage"
    };


    static List<string> enemyFirstNames = new List<string>() {
        "Tim", "Todd", "Marlin", "Rupert", "Larry"
    };
    static List<string> enemyLastNames = new List<string>() {
        "the Terrible", "the Fearful", "the Handsome", "the Miserable", "the Merriful", "of the Forrest", "of the Wild", "the Killer"
    };

    static List<string> townNames = new List<string>() {
        "Cuntsville", "Fucksville", "Bitchville", "Whoreville", "Yucksville",
        "Ballsmouth", "Greensville", "Mornse", "Flornse", "Yorkshire", "Forkshire",
        "Mas", "Kuor", "Moorse", "Torense", "Portur", "Ooren", "Boore", "Maor",
        "Gheast", "Relment", "Quendid", "Marense", "Larense", "Harense", "Basmerd", 
        "Jorvin", "Kilmed", "Vulpin", "Corense", "Whilbed"
    };


    public static string getRandomPlayerName() {
        int rand = Random.Range(0, playerNames.Count);
        return playerNames[rand];
    }

    public static string getRandomUsablePlayerName() {
        List<string> temp = new List<string>();

        foreach(var i in playerNames)
            temp.Add(i);

        for(int i = 0; i < Party.getMemberCount(); i++) {
            foreach(var n in temp) {
                if(Party.getMemberStats(i) != null && !Party.getMemberStats(i).isEmpty() && n == Party.getMemberStats(i).u_name) {
                    temp.Remove(n);
                    break;
                }
            }
        }

        return temp[Random.Range(0, temp.Count)];
    }

    public static string getRandomEnemyName() {
        string first = enemyFirstNames[Random.Range(0, enemyFirstNames.Count)];
        string last = enemyLastNames[Random.Range(0, enemyLastNames.Count)];
        return first + " " + last;
    }
    public static string getRandomUsableTownName() {
        if(MapLocationHolder.getTownCount() >= townNames.Count)
            return townNames[Random.Range(0, townNames.Count)];

        var useables = townNames;
        for(int i = 0; i < MapLocationHolder.getTownCount(); i++) {
            foreach(var u in useables) {
                if(u == MapLocationHolder.getTownLocation(i).town.t_name) {
                    useables.Remove(u);
                    break;
                }
            }
        }
        if(useables.Count == 0) {
            return townNames[Random.Range(0, townNames.Count)];
        }
        return useables[Random.Range(0, useables.Count)];
    }
}

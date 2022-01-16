using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NameLibrary {
    static List<string> playerNames = new List<string>() {
        "Vermont", "Lucky", "Mars", "Aldous", "Gayla", "Kilie", "Phiobe", "Kayson", "Bilha", "Saule",
        "Flynn", "Elmo", "Gloria", "Ansley", "Rita", "Natalius", "Kaley", "Rufus", "Japheth", "Reuven",
        "Chesley", "Celsus", "Maddy", "Dayna", "Waldo", "Ivan", "Rilla", "Rhode", "Burke", "Ester",
        "Anzo", "Yama", "Modred", "Kelley", "Shane", "Audra", "Connor", "Dina", "Glenn", "Colbert",
        "Deana", "Abe", "Pax", "Tabetha", "Tara", "Gayle", "Mandy", "Kosmas", "Tane", "Everlee",
        "Luna", "Medrod", "Davy", "Jeptha", "Cealia", "Hester", "Claude", "Peony", "Lana", "Anil"
    };


    static List<string> enemyFirstNames = new List<string>() {
        "Tim", "Todd", "Marlin", "Rupert", "Magnolia", "Oddbjorn", "Marcellus", "Osee", "Mnemosyne", "Veles",
        "Dennis", "Ukko", "Clemens", "Yair", "Mary", "Pearle", "Lynn", "Myra", "Terminus", "Deven",
        "Celine", "Tristan", "Zoey", "Pele", "Stigr", "Gavin", "Euphemia", "Lycus", "Zosime", "Rylee"
    };
    static List<string> enemyLastNames = new List<string>() {
        "the Terrible", "the Fearful", "the Handsome", "the Miserable", "the Merriful", 
        "of the Forrest", "of the Wild", "the Killer", "the Harmful", "the Negative",
        "the Ruinous", "the Sinister", "the Menacing", "the Bad", "the Noxious",
        "The Fluffy", "the Obese", "the Unsafe", "the Fragile", "the Breaker"
    };

    static List<string> townNames = new List<string>() {
        "Ballsmouth", "Mornse", "Flornse", "Boore", "Ooren", "Quendid", "Jorvin", "Gheast", "Portur", "Yorkshire",
        "Lustinus", "Yeshua", "Hattie", "Solon", "Damocles", "Pomona", "Walt", "Dunstan", "Melitta", "Petru",
        "Magni", "Bronte", "Kacie", "Lucilla", "Edith", "Euandros", "Flaviana", "Romilly", "Lewi", "Laila",
        "Dex", "Westley", "Rowland", "Kurt", "Liv", "Paulina", "Elwood", "Wayne", "Meridith", "Kian",
        "Daphne", "Martina", "Phodopis", "Romaine", "Amos", "Humphry", "Alodia", "Ouranos", "Coronis", "Diodoros"
    };


    public static string getRandomPlayerName() {
        int rand = Random.Range(0, playerNames.Count);
        return playerNames[rand];
    }

    public static string getRandomUsablePlayerName() {
        List<string> temp = new List<string>();

        foreach(var i in playerNames)
            temp.Add(i);

        for(int i = 0; i < Party.getHolder().getObjectCount<UnitStats>(); i++) {
            foreach(var n in temp) {
                if(Party.getHolder().getObject<UnitStats>(i) != null && !Party.getHolder().getObject<UnitStats>(i).isEmpty() && n == Party.getHolder().getObject<UnitStats>(i).u_name) {
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
        if(MapLocationHolder.getHolder().getObjectCount<TownLocation>() >= townNames.Count)
            return townNames[Random.Range(0, townNames.Count)];

        var useables = townNames;
        for(int i = 0; i < MapLocationHolder.getHolder().getObjectCount<TownLocation>(); i++) {
            foreach(var u in useables) {
                if(u == MapLocationHolder.getHolder().getObject<TownLocation>(i).town.t_name) {
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

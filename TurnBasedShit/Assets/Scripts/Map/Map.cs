using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Map {
    public static float leftBound = -18.0f, rightBound = 18.0f;
    public static float botBound = -18.0f, topBound = 18.0f;


    public static Vector2 getRandPos() {
        return new Vector2(Random.Range(leftBound, rightBound), Random.Range(botBound, topBound));
    }
}

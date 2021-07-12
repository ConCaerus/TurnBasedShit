using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoadCreator {

    static bool flipped = true;

    public static List<Vector2> calcRoadPoints(Vector2 startingPoint, Vector2 endingPoint, bool switchFlippedWhenDone, int pointCount = 25) {
        List<Vector2> points = new List<Vector2>();


        var x = endingPoint.x - startingPoint.x;
        var y = endingPoint.y - startingPoint.y;
        var theta = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

        const float degToFlip = 2.5f;

        if(!flipped && Mathf.Abs(theta) > 90.0f - degToFlip && Mathf.Abs(theta) < 90.0f + degToFlip) {
            flipped = true;
        }
        else if(flipped && Mathf.Abs(theta) < 0.0f + degToFlip || Mathf.Abs(theta) > 180.0f - degToFlip) {
            flipped = false;
        }

        if(flipped) {
            var temp = startingPoint;
            startingPoint = endingPoint;
            endingPoint = temp;
        }

        endingPoint = new Vector2(endingPoint.x - startingPoint.x, endingPoint.y - startingPoint.y);


        for(int i = 0; i <= pointCount; i++) {
            float xVal = endingPoint.x * (i / (float)pointCount);

            //  x^e
            float modY = Mathf.Abs(endingPoint.y);
            if(modY == 0.0f)
                modY = 0.001f;

            var target = modY * (i / (float)pointCount);
            float b;
            float yVal = 0.0f;

            b = Mathf.Pow((float)System.Math.E, (Mathf.Log(target) / (float)System.Math.E)) * (i / (float)pointCount);
            yVal = Mathf.Pow(b, (float)System.Math.E);

            if(endingPoint.y < 0.0f)
                yVal *= -1.0f;

            xVal += startingPoint.x;
            yVal += startingPoint.y;


            points.Add(new Vector2(xVal, yVal));
        }

        //  for nice visual effect
        if(switchFlippedWhenDone)
            flipped = !flipped;

        if(!flipped)
            points.Reverse();
        return points;
    }


    public static void setFlipped(bool b) {
        flipped = b;
    }
    public static bool getFlipped() {
        return flipped;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CurvedLine : MonoBehaviour {
    LineRenderer ln;

    public float det;


    private void Awake() {
        ln = GetComponent<LineRenderer>();
    }


    private void Update() {
        createLine();
    }



    void createLine() {
        var things = calcPoints(new Vector2(2.0f, 2.0f), GameInfo.getMousePos());

        ln.positionCount = things.Count;
        for(int i = 0; i < things.Count; i++) {
            ln.SetPosition(i, things[i]);
        }
    }


    List<Vector2> calcPoints(Vector2 startingPoint, Vector2 endingPoint, int pointCount = 25) {
        List<Vector2> points = new List<Vector2>();

        endingPoint = new Vector2(endingPoint.x - startingPoint.x, endingPoint.y - startingPoint.y);

        for(int i = 0; i <= pointCount; i++) {
            float xVal = startingPoint.x + endingPoint.x * (i / (float)pointCount);

            //  x^e
            float modY = Mathf.Abs(endingPoint.y);
            if(modY == 0.0f)
                modY = 0.001f;

            var target = modY * (i / (float)pointCount);
            var b = Mathf.Pow((float)System.Math.E, (Mathf.Log(target) / (float)System.Math.E)) * (i / (float)pointCount);
            var yVal = Mathf.Pow(b, (float)System.Math.E);
            if(endingPoint.y < 0.0f)
                yVal *= -1.0f;

            yVal += startingPoint.y;


            points.Add(new Vector2(xVal, yVal));
        }

        return points;
    }
}

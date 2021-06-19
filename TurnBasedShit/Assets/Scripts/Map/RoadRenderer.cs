using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoadRenderer : MonoBehaviour {
    LineRenderer ln;

    [SerializeField] GameObject roadSegmentPreset, roadPointPreset;

    public int roadPointCount = 10;
    public float roadSegmentXLength = 5.0f, roadSegmentXLengthError = 2.0f;
    public float roadSegmentYLengthError = 12.0f;
    public List<RoadSegmentInfo> roadPoints = new List<RoadSegmentInfo>();
    public List<GameObject> roadSegments = new List<GameObject>();

    public List<GameObject> roadPointObjects = new List<GameObject>();

    public GameObject partyObjectPreset;
    GameObject partyObject;

    float maxCustomDist = 5.0f;
    GameObject customPoint;
    GameObject customSegment;
    Vector2 customPointReferencePoint;


    private void Awake() {
        //MapAnchorPositionSaver.clearPositions();
        ln = GetComponent<LineRenderer>();
        DOTween.Init();
    }

    private void Start() {
        partyObject = Instantiate(partyObjectPreset, Vector2.zero, Quaternion.identity, transform);

        loadSavedPoints();
        renderRoad();
    }


    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            //  create new
            if(getMousedOverPoint() >= 0 && customPoint == null) {
                customPoint = Instantiate(roadPointPreset, transform);
                customSegment = Instantiate(roadSegmentPreset, transform);
                customPointReferencePoint = roadPointObjects[getMousedOverPoint()].transform.position;
            }

            //  drop and save
            else if(customPoint != null) {
                addCustomPointToRoad();
            }
        }

        if(customPoint != null && customSegment != null)
            moveCustomPoint();
    }


    int getMousedOverPoint() {
        for(int i = 0; i < roadPointObjects.Count; i++) {
            if(roadPointObjects[i].GetComponent<RoadPoint>().mouseOver)
                return i;
        }

        return -1;
    }


    public void clearRoad() {
        foreach(var i in roadSegments)
            Destroy(i.gameObject);
        foreach(var i in roadPointObjects)
            Destroy(i.gameObject);

        roadPointObjects.Clear();
        roadSegments.Clear();
    }
    void loadSavedPoints() {
        clearRoad();

        //  normal anchor poses
        if(MapAnchorPositionSaver.getAnchorCount() == 0) {
            createNewRoad();
        }
        else {
            for(int i = 0; i < MapAnchorPositionSaver.getAnchorCount(); i++) {
                roadPoints.Add(MapAnchorPositionSaver.getAnchorPos(i));
            }
        }


        //  party anchor pos
        partyObject.transform.position = roadPoints[MapAnchorPositionSaver.getPartyPointLocationIndex()].start;
    }


    void createNewRoad() {
        MapAnchorPositionSaver.clearPositions();

        Vector2 prev = Vector2.zero;

        roadPoints.Clear();
        for(int i = 0; i < roadPointCount; i++) {
            float x = 0.0f, y = 0.0f;


            if(i > 0) {
                x = prev.x + Random.Range(roadSegmentXLength - roadSegmentXLengthError, roadSegmentXLength + roadSegmentXLengthError);
                y = prev.y + Random.Range(-roadSegmentYLengthError, roadSegmentYLengthError);
            }
            else {
                x = Map.leftBound;
                y = (Map.topBound + Map.botBound) / 2.0f;
            }

            if(i > 0) {
                roadPoints.Add(new RoadSegmentInfo(prev, new Vector2(x, y)));
                MapAnchorPositionSaver.addNewMapAnchor(new RoadSegmentInfo(prev, new Vector2(x, y)));
            }

            prev = new Vector2(x, y);
        }
    }
    void renderRoad() {
        clearRoad();

        for(int i = 0; i < roadPoints.Count; i++) {
            //  create a point obj
            if(getPointAtPos(roadPoints[i].start) == null) {
                var pnt = Instantiate(roadPointPreset, transform);
                pnt.transform.localPosition = roadPoints[i].start;
                roadPointObjects.Add(pnt.gameObject);
            }
            if(getPointAtPos(roadPoints[i].end) == null) {
                var pnt = Instantiate(roadPointPreset, transform);
                pnt.transform.localPosition = roadPoints[i].end;
                roadPointObjects.Add(pnt.gameObject);
            }



            //  create a road segment and set its line
            var seg = Instantiate(roadSegmentPreset, transform);
            if(i == 0)
                renderRoadSegment(seg, calcPoints(roadPoints[i].start, roadPoints[i].end));
            else
                renderRoadSegment(seg, calcPoints(roadPoints[i].start, roadPoints[i].end), roadSegments[i - 1]);


            roadSegments.Add(seg.gameObject);
        }
    }

    GameObject getPointAtPos(Vector2 pos) {
        foreach(var i in roadPointObjects) {
            if(pos == (Vector2)i.transform.position)
                return i;
        }
        return null;
    }
    RoadSegmentInfo getSegmentWithEndingPos(Vector2 end) {
        foreach(var i in roadPoints) {
            if(i.end == end)
                return i;
        }

        return null;
    }
    RoadSegmentInfo getSegmentWithStartingPos(Vector2 start) {
        foreach(var i in roadPoints) {
            if(i.start == start)
                return i;
        }

        return null;
    }


    void moveCustomPoint() {
        var target = GameInfo.getMousePos();

        if(Vector2.Distance(target, customPointReferencePoint) >= maxCustomDist) {
            var center = customPointReferencePoint;
            var newVar = GameInfo.getMousePos() - center;
            var theta = Mathf.Atan2(newVar.y, newVar.x);

            var y = center.y + maxCustomDist * Mathf.Sin(theta);
            var x = center.x + maxCustomDist * Mathf.Cos(theta);
            target = new Vector2(x, y);
        }
        customPoint.transform.position = target;
        renderRoadSegment(customSegment, calcPoints(customPointReferencePoint, target));
    }
    void addCustomPointToRoad() {
        MapAnchorPositionSaver.addNewMapAnchor(new RoadSegmentInfo(customPointReferencePoint, customPoint.transform.position));
        roadSegments.Add(customSegment.gameObject);
        roadPointObjects.Add(customPoint.gameObject);

        customPoint = null;
        customSegment = null;
        customPointReferencePoint = Vector2.zero;
    }



    void renderRoadSegment(GameObject segment, List<Vector2> points, GameObject prevSegment = null) {
        var ln = segment.GetComponent<LineRenderer>();
        ln.positionCount = points.Count;

        for(int i = 0; i < points.Count; i++)
            ln.SetPosition(i, points[i]);

    }


    List<Vector2> calcPoints(Vector2 startingPoint, Vector2 endingPoint, int pointCount = 25) {
        List<Vector2> points = new List<Vector2>();

        var x = endingPoint.x - startingPoint.x;
        var y = endingPoint.y - startingPoint.y;
        var theta = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

        if(Mathf.Abs(theta) > 45.0f && Mathf.Abs(theta) < 135.0f) {
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

        return points;
    }



    public Vector2 getPartyObjectPos() {
        return partyObject.transform.position;
    }
}


[System.Serializable]
public class RoadSegmentInfo {
    public Vector2 start, end;

    public RoadSegmentInfo(Vector2 s, Vector2 e) {
        start = s;
        end = e;
    }
}

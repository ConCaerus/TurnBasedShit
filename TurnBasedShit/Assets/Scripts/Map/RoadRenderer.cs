using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoadRenderer : MonoBehaviour {
    [SerializeField] GameObject roadSegmentPreset, roadPointPreset;

    public int roadPointCount = 10;
    public float roadSegmentXLength = 5.0f, roadSegmentXLengthError = 2.0f;
    public float roadSegmentYLengthError = 12.0f;
    public List<RoadSegmentInfo> roadSegments = new List<RoadSegmentInfo>();

    public List<GameObject> roadPointObjects = new List<GameObject>();
    public List<GameObject> roadSegmentObjects = new List<GameObject>();

    public float partySpeed = 0.25f, pauseAtPointTime = 0.0f, partyGrowSpeed = 0.25f;
    public GameObject partyObjectPreset;
    GameObject partyObject;

    float maxCustomDist = 5.0f, snappingDist = 0.25f;
    GameObject customPoint;
    GameObject customSegment;
    Vector2 customPointReferencePoint;

    int partyTargetIndex = 0;
    public bool canMove = true;
    Coroutine partyMover = null;


    private void Awake() {
        //MapAnchorPositionSaver.clearPositions();
        DOTween.Init();
    }

    private void Start() {
        resetPartyObject();
        partyObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        StartCoroutine(FindObjectOfType<TransitionCanvas>().runAfterLoading(growPartyObj));
        loadAndRenderSavedPoints();
        canMove = true;
    }


    private void Update() {
        //  creating new points
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


        //  move party Anchor
        if(Input.GetKey(KeyCode.Space) && partyMover == null) {
            partyMover = StartCoroutine(movePartyObject());
        }
        else if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)) && partyMover == null) {
            changePartyTarget(Input.GetKeyDown(KeyCode.W), Input.GetKeyDown(KeyCode.S));
        }

        //  enter the location that the party object is over
        /*
        if(Input.GetKeyDown(KeyCode.S) && MapLocationHolder.locationAtPosition(partyObject.transform.position)) {
            shrinkPartyObj();
            canMove = false;
            FindObjectOfType<EnounterCanvas>().showEnemyEncounterAlert();
        }*/


        if(Input.GetKeyDown(KeyCode.Alpha0)) {
            MapAnchorPositionSaver.clearPositions();
            loadAndRenderSavedPoints();
        }
    }



    int getMousedOverPoint() {
        for(int i = 0; i < roadPointObjects.Count; i++) {
            if(roadPointObjects[i].GetComponent<RoadPoint>().mouseOver)
                return i;
        }

        return -1;
    }


    public void clearRoadObjects() {
        foreach(var i in roadSegmentObjects)
            Destroy(i.gameObject);
        foreach(var i in roadPointObjects)
            Destroy(i.gameObject);

        roadPointObjects.Clear();
        roadSegmentObjects.Clear();
    }
    void loadAndRenderSavedPoints() {
        clearRoadObjects();

        //  normal anchor poses
        if(MapAnchorPositionSaver.getAnchorCount() == 0) {
            createNewRoad();
        }
        else {
            for(int i = 0; i < MapAnchorPositionSaver.getAnchorCount(); i++) {
                roadSegments.Add(MapAnchorPositionSaver.getAnchorPos(i));
            }
        }

        renderRoad();

        //  party anchor pos
        partyObject.transform.position = roadPointObjects[MapAnchorPositionSaver.getPartyPointLocationIndex()].transform.position;
    }


    void createNewRoad() {
        MapAnchorPositionSaver.clearPositions();

        Vector2 startingPoint = new Vector2(Map.leftBound(), 0.0f);
        Vector2 endingPoint = new Vector2(Map.rightBound(), Map.getRandPos().y);

        roadSegments.Clear();
        createSegmentsBtwPoints(startingPoint, endingPoint);
    }
    void renderRoad() {
        clearRoadObjects();

        for(int i = 1; i < roadSegments.Count; i++) {
            //  create a point obj
            if(getPointAtPos(roadSegments[i].start) == null) {
                var pnt = Instantiate(roadPointPreset, transform);
                pnt.transform.localPosition = roadSegments[i].start;
                roadPointObjects.Add(pnt.gameObject);
            }
            if(getPointAtPos(roadSegments[i].end) == null) {
                var pnt = Instantiate(roadPointPreset, transform);
                pnt.transform.localPosition = roadSegments[i].end;
                roadPointObjects.Add(pnt.gameObject);
            }



            //  create a road segment and set its line
            var segment = Instantiate(roadSegmentPreset, transform);
            var ln = segment.GetComponent<LineRenderer>();


            RoadCreator.setFlipped(roadSegments[i].flipped);
            var points = RoadCreator.calcRoadPoints(roadSegments[i].start, roadSegments[i].end, false);
            ln.positionCount = points.Count;

            for(int l = 0; l < points.Count; l++) {
                ln.SetPosition(l, points[l]);
            }

            roadSegmentObjects.Add(segment.gameObject);
        }
    }

    GameObject getPointAtPos(Vector2 pos) {
        foreach(var i in roadPointObjects) {
            if(Vector2.Distance(pos, i.transform.localPosition) < snappingDist)
                return i;
        }
        return null;
    }
    RoadSegmentInfo getSegmentWithEndingPos(Vector2 end) {
        foreach(var i in roadSegments) {
            if(i.end == end)
                return i;
        }

        return null;
    }
    List<RoadSegmentInfo> getSegmentsWithEndingPos(Vector2 end) {
        List<RoadSegmentInfo> temp = new List<RoadSegmentInfo>();

        foreach(var i in roadSegments) {
            if(i.end == end)
                temp.Add(i);
        }

        return temp;
    }
    RoadSegmentInfo getSegmentWithStartingPos(Vector2 start) {
        foreach(var i in roadSegments) {
            if(i.start == start)
                return i;
        }

        return null;
    }
    List<RoadSegmentInfo> getSegmentsWithStartingPos(Vector2 start) {
        List<RoadSegmentInfo> temp = new List<RoadSegmentInfo>();

        foreach(var i in roadSegments) {
            if(i.start == start)
                temp.Add(i);
        }

        return temp;
    }
    RoadSegmentInfo getSegmentWithBounds(Vector2 first, Vector2 second) {
        foreach(var i in getSegmentsWithStartingPos(first)) {
            if(i.end == second)
                return i;
        }
        foreach(var i in getSegmentsWithEndingPos(first)) {
            if(i.start == second) {
                RoadSegmentInfo temp = new RoadSegmentInfo(i.end, i.start, i.flipped);
                return temp;
            }
        }
        return null;
    }
    GameObject getSegmentObject(RoadSegmentInfo info) {
        foreach(var i in roadSegmentObjects) {
            var ln = i.GetComponent<LineRenderer>();
            if((Vector2)ln.GetPosition(0) == info.start && (Vector2)ln.GetPosition(ln.positionCount - 1) == info.end)
                return i.gameObject;
            if((Vector2)ln.GetPosition(0) == info.end && (Vector2)ln.GetPosition(ln.positionCount - 1) == info.start)
                return i.gameObject;
        }

        return null;
    }

    int getSegmentIndex(RoadSegmentInfo seg) {
        for(int i = 0; i < roadSegments.Count; i++) {
            if(roadSegments[i] == seg)
                return i;
        }
        return -1;
    }
    int getPointIndex(GameObject point) {
        for(int i = 0; i < roadPointObjects.Count; i++) {
            if(point == roadPointObjects[i].gameObject)
                return i;
        }

        return -1;
    }


    void moveCustomPoint() {
        var target = GameInfo.getMousePos();

        //  greater than max
        if(Vector2.Distance(target, customPointReferencePoint) >= maxCustomDist) {
            var center = customPointReferencePoint;
            var newVar = GameInfo.getMousePos() - center;
            var theta = Mathf.Atan2(newVar.y, newVar.x);

            var y = center.y + maxCustomDist * Mathf.Sin(theta);
            var x = center.x + maxCustomDist * Mathf.Cos(theta);
            target = new Vector2(x, y);
        }


        //  snapping to another point
        foreach(var i in roadPointObjects) {
            if(Vector2.Distance(i.transform.position, target) < snappingDist) {
                target = i.transform.position;
                break;
            }
        }

        //  snapping to map location
        /*
        foreach(var i in MapLocationHolder.getLocations()) {
            if(Vector2.Distance(i.pos, target) < snappingDist) {
                target = i.pos;
                break;
            }
        }*/

        customPoint.transform.position = target;
        var points = RoadCreator.calcRoadPoints(customPointReferencePoint, target, false);
        customSegment.GetComponent<LineRenderer>().positionCount = points.Count;
        for(int i = 0; i < points.Count; i++) {
            customSegment.GetComponent<LineRenderer>().SetPosition(i, points[i]);
        }
    }
    void addCustomPointToRoad() {
        if(getSegmentWithBounds(customPointReferencePoint, customPoint.transform.position) == null) {
            var seg = new RoadSegmentInfo(customPointReferencePoint, customPoint.transform.position, RoadCreator.getFlipped());
            MapAnchorPositionSaver.addNewMapAnchor(seg);
            roadSegments.Add(seg);
            var temp = customSegment;
            roadSegmentObjects.Add(temp);
            roadPointObjects.Add(customPoint.gameObject);
        }
        else
            Destroy(customSegment.gameObject);

        customPoint = null;
        customSegment = null;
        customPointReferencePoint = Vector2.zero;
    }


    void changePartyTarget(bool up, bool down) {
        up = up && !down;
        down = down && !up;
        if(up) {
            partyTargetIndex++;

            if(partyTargetIndex > getPossiblePartyTargets().Count - 1)
                partyTargetIndex = 0;
        }
        else if(down) {
            partyTargetIndex--;

            if(partyTargetIndex < 0)
                partyTargetIndex = getPossiblePartyTargets().Count - 1;
        }

        updatePartyTarget();
    }
    void updatePartyTarget() {
        foreach(var i in getPossiblePartyTargets()) {
            if(getPointAtPos(i) != null)
                getPointAtPos(i).GetComponent<RoadPoint>().shrinkToNormal();
        }

        GameObject target = getPointAtPos(getPossiblePartyTargets()[partyTargetIndex]);
        target.GetComponent<RoadPoint>().enlarge();
    }

    List<Vector2> getPossiblePartyTargets() {
        List<Vector2> possibleTargetPoints = new List<Vector2>();
        var partyPoint = roadPointObjects[MapAnchorPositionSaver.getPartyPointLocationIndex()].transform.position;

        foreach(var i in roadSegmentObjects) {
            if(i.GetComponent<LineRenderer>().GetPosition(0) == partyPoint)
                possibleTargetPoints.Add(i.GetComponent<LineRenderer>().GetPosition(i.GetComponent<LineRenderer>().positionCount - 1));
            else if(i.GetComponent<LineRenderer>().GetPosition(i.GetComponent<LineRenderer>().positionCount - 1) == partyPoint)
                possibleTargetPoints.Add(i.GetComponent<LineRenderer>().GetPosition(0));
        }

        return possibleTargetPoints;
    }



    IEnumerator movePartyObject() {
        Vector2 partyPoint = roadPointObjects[MapAnchorPositionSaver.getPartyPointLocationIndex()].transform.position;
        var prevPoint = partyPoint;
        if(getPossiblePartyTargets().Count == 1) {
            partyTargetIndex = 0;
        }
        var target = getPossiblePartyTargets()[partyTargetIndex];


        var segment = getSegmentWithBounds(partyPoint, target);
        var obj = getSegmentObject(segment);
        if(obj == null || obj.GetComponent<LineRenderer>() == null) {

            partyMover = null;
            yield return 0;
        }
        RoadCreator.setFlipped(segment.flipped);
        var points = new List<Vector2>();
        for(int i = 0; i < obj.GetComponent<LineRenderer>().positionCount; i++)
            points.Add(obj.GetComponent<LineRenderer>().GetPosition(i));
        if(partyPoint != points[0])
            points.Reverse();

        List<Vector2> unusedPoints = new List<Vector2>();
        List<Vector2> usedPoints = new List<Vector2>();

        foreach(var i in points)
            unusedPoints.Add(i);


        bool canEnd = false;
        bool reachedDest = false;

        while(canMove && FindObjectOfType<TransitionCanvas>().loaded) {
            //  advance
            if(Input.GetKey(KeyCode.Space)) {
                partyObject.transform.DOMove(unusedPoints[0], partySpeed / points.Count);
                yield return new WaitForSeconds(partySpeed / points.Count);

                //  moves the prev point to the used points list
                var temp = unusedPoints[0];
                unusedPoints.RemoveAt(0);
                usedPoints.Add(temp);

                if(unusedPoints.Count == 0) {
                    MapAnchorPositionSaver.setPartyPointIndex(getPointIndex(getPointAtPos(target)));
                    reachedDest = true;
                    break;
                }
            }

            //  go back
            else {
                partyObject.transform.DOMove(usedPoints[usedPoints.Count - 1], (partySpeed * 2.0f) / points.Count);
                yield return new WaitForSeconds((partySpeed * 2.0f) / points.Count);

                //  moves the prev point to the used points list
                var temp = usedPoints[usedPoints.Count - 1];
                usedPoints.RemoveAt(usedPoints.Count - 1);
                unusedPoints.Insert(0, temp);

                if(usedPoints.Count == 0)
                    break;
            }

            if(unusedPoints.Count == 0) {
                MapAnchorPositionSaver.setPartyPointIndex(getPointIndex(getPointAtPos(target)));
                break;
            }
            else if(canEnd && usedPoints.Count == 0) {
                break;
            }

            canEnd = true;
        }


        //  cleanup
        if(reachedDest) {
            if(pauseAtPointTime > 0.0f)
                yield return new WaitForSeconds(pauseAtPointTime);

            //  change index to match direction
            bool hitEnd = true;
            for(int i = 0; i < getPossiblePartyTargets().Count; i++) {
                if(getPossiblePartyTargets()[i] != prevPoint) {
                    partyTargetIndex = i;
                    updatePartyTarget();
                    hitEnd = false;
                    break;
                }
            }
            if(hitEnd) {
                if(partyTargetIndex > getPossiblePartyTargets().Count - 1)
                    partyTargetIndex = 0;
                if(partyTargetIndex < 0)
                    partyTargetIndex = getPossiblePartyTargets().Count - 1;
            }


            //  decide what happens at this location
            MapAnchorPositionSaver.setPartyPointIndex(getPointIndex(getPointAtPos(target)));
            FindObjectOfType<MapEventsHandler>().triggerAnchorEvents(roadPointObjects[MapAnchorPositionSaver.getPartyPointLocationIndex()].transform.position);

            while(hitEnd && Input.GetKey(KeyCode.Space))
                yield return new WaitForEndOfFrame();
        }

        partyMover = null;
    }

    public void shrinkPartyObj() {
        partyObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.0f);
        partyObject.transform.DOScale(0.0f, partyGrowSpeed);
        partyObject.transform.DORotate(new Vector3(0.0f, 0.0f, -360.0f), partyGrowSpeed, RotateMode.FastBeyond360);
    }
    public void growPartyObj() {
        partyObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        partyObject.transform.DOScale(1f, partyGrowSpeed);
        partyObject.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, 360.0f), partyGrowSpeed, RotateMode.FastBeyond360);
    }


    RoadSegmentInfo createSegment(Vector2 start, Vector2 end, bool fp) {
        var segment = Instantiate(roadSegmentPreset, transform);
        var ln = segment.GetComponent<LineRenderer>();

        bool f = RoadCreator.getFlipped();
        var points = RoadCreator.calcRoadPoints(start, end, fp);
        ln.positionCount = points.Count;

        for(int i = 0; i < points.Count; i++) {
            ln.SetPosition(i, points[i]);
        }

        var seg = new RoadSegmentInfo(points[0], points[points.Count - 1], f);
        roadSegmentObjects.Add(segment.gameObject);
        return seg;
    }


    List<Vector2> getAllRoadPoints() {
        var temp = new List<Vector2>();
        temp.Clear();

        foreach(var i in roadSegmentObjects) {
            for(int p = 0; p < i.GetComponent<LineRenderer>().positionCount; p++) {
                temp.Add(i.GetComponent<LineRenderer>().GetPosition(p));
            }
        }

        return temp;
    }

    void createSegmentsBtwPoints(Vector2 startingPoint, Vector2 endingPoint) {
        float x = endingPoint.x - startingPoint.x;
        float y = endingPoint.y - startingPoint.y;
        float theta = Mathf.Atan2(y, x);

        int count = (int)(Vector2.Distance(startingPoint, endingPoint) / maxCustomDist) + 1;
        var incDist = Vector2.Distance(startingPoint, endingPoint) / (float)count;

        float xInc = incDist * Mathf.Cos(theta);
        float yInc = incDist * Mathf.Sin(theta);

        Vector2 prev = startingPoint;


        for(int i = 0; i < count + 1; i++) {
            var newPoint = startingPoint + (new Vector2(xInc, yInc) * i);

            if(i == count)
                newPoint = endingPoint;

            var seg = createSegment(prev, newPoint, true);
            roadSegments.Add(seg);
            MapAnchorPositionSaver.addNewMapAnchor(seg);

            createPointAtPos(prev);
            prev = newPoint;
        }

        createPointAtPos(prev);
    }
    void createPointAtPos(Vector2 pos) {
        foreach(var i in roadPointObjects) {
            if((Vector2)i.transform.localPosition == pos)
                return;
        }

        var point = Instantiate(roadPointPreset, transform);
        point.transform.position = pos;
        roadPointObjects.Add(point.gameObject);
    }



    public Vector2 getPartyObjectPos() {
        return partyObject.transform.position;
    }
    public GameObject getPartyObject() {
        return partyObject;
    }
    public void resetPartyObject() {
        Vector2 pos = Vector2.zero;
        if(partyObject != null) {
            pos = partyObject.transform.position;
            Destroy(partyObject.gameObject);
        }

        int headIndex = Party.getLeaderStats().u_sprite.headIndex;
        int faceIndex = Party.getLeaderStats().u_sprite.faceIndex;
        partyObject = Instantiate(FindObjectOfType<PresetLibrary>().getUnitHead(headIndex), pos, Quaternion.identity, transform);
        partyObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getUnitFace(faceIndex);
        partyObject.transform.GetChild(0).transform.localPosition = Vector3.zero;
        partyObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        partyObject.transform.GetChild(0).GetComponent<Animator>().enabled = false;
        partyObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Party.getLeaderStats().u_sprite.color;
    }
}


[System.Serializable]
public class RoadSegmentInfo {
    public Vector2 start, end;
    public bool flipped;

    public RoadSegmentInfo(Vector2 s, Vector2 e, bool f) {
        start = s;
        end = e;
        flipped = f;
    }
}

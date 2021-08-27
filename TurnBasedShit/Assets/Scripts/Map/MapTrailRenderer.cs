using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapTrailRenderer : MonoBehaviour {
    [SerializeField] GameObject anchor;
    List<GameObject> anchors = new List<GameObject>();
    GameObject movingAnchor = null;

    [SerializeField] GameObject partyAnchorPrefab;
    GameObject partyAnchor;

    [SerializeField] LineRenderer line;

    const float minAnchorDist = 0.25f, maxAnchorDist = 5.0f, snappingDist = 0.25f;
    Vector2 startingPos = new Vector2(-8.0f, -1.0f);

    const int foodForMaxLine = 2;

    [SerializeField] float partySpeed = 10.0f;


    private void Awake() {
        //MapAnchorPositionSaver.clearPositions();
        partyAnchor = Instantiate(partyAnchorPrefab, startingPos, Quaternion.identity, transform);
        loadSavedAnchors();

        if(anchors.Count == 0)
            createNewAnchor();
        drawLine();

        if(Party.getLeaderID() != -1)
            partyAnchor.GetComponent<SpriteRenderer>().color = Party.getLeaderStats().u_sprite.color;
    }

    private void Update() {
        //  set down moving anchor or move an existing anchor
        if(Input.GetMouseButtonDown(0)) {
            //  move existing anchor
            if(movingAnchor == null && getMousedOverAnchor() != null)
                movingAnchor = getMousedOverAnchor();

            //  put down moving anchor
            else if(movingAnchor != null)
                putDownMovingAnchor();

            //  create new anchor if not trying to modify existing anchor
            else if(movingAnchor == null)
                createNewAnchor();
        }

        //  move party anchor along trail
        else if(Input.GetKey(KeyCode.D) && anchors.Count > 0)
            movePartyAnchor();


        //  update visuals and moving anchor position
        if(movingAnchor != null) {
            moveMovingAnchor();
            drawLine();
        }
    }


    //  regular anchor shit
    GameObject getMousedOverAnchor() {
        //  return if their are no active slots
        if(anchors.Count == 0)
            return null;

        Ray ray;
        RaycastHit2D hit;

        List<Collider2D> unwantedHits = new List<Collider2D>();

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        //  return if the ray did not hit anything
        if(hit.collider == null)
            return null;

        while(true) {
            //  if the hit is hitting an inventory slot
            foreach(var i in anchors) {
                if(hit.collider == i.GetComponent<Collider2D>()) {
                    foreach(var u in unwantedHits)
                        u.enabled = true;
                    return i.gameObject;
                }
            }

            //  hit is not a wanted object
            hit.collider.enabled = false;
            unwantedHits.Add(hit.collider);

            hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            //  hit has run out of hit objects
            if(hit.collider == null) {
                foreach(var i in unwantedHits)
                    i.enabled = true;
                return null;
            }
        }
    }
    int getAnchorIndex(GameObject an) {
        for(int i = 0; i < anchors.Count; i++) {
            if(an == anchors[i])
                return i;
        }
        return -1;
    }

    public void createNewAnchor() {
        var temp = Instantiate(anchor);
        temp.transform.SetParent(transform);
        movingAnchor = temp;

        anchors.Add(temp.gameObject);
    }
    public void createNewAnchorAtPos(Vector2 pos) {
        var temp = Instantiate(anchor);
        temp.transform.position = pos;
        temp.transform.SetParent(transform);

        anchors.Add(temp.gameObject);
    }

    void drawLine() {
        if(anchors.Count > 0) {
            line.positionCount = anchors.Count + 1;
            for(int i = 0; i < anchors.Count + 1; i++) {
                if(i == 0) {
                    line.SetPosition(line.positionCount - 1, partyAnchor.transform.position);
                    continue;
                }
                line.SetPosition(line.positionCount - 1 - i, anchors[i - 1].transform.position);
            }
        }
    }

    void moveMovingAnchor() {
        var target = GameInfo.getMousePos();

        bool canMoveToTarget = true;

        if(anchors.Count > 0) {
            //  moving anchor is too close to the center
            int movingAnchorIndex = getAnchorIndex(movingAnchor);
            GameObject prevAnchor = null, nextAnchor = null;
            GameObject closestAnchor = null;

            if(anchors.Count > 1) {
                //  index is the last in the list
                if(movingAnchorIndex == anchors.Count - 1) {
                    nextAnchor = null;
                    prevAnchor = anchors[movingAnchorIndex - 1];
                    closestAnchor = nextAnchor;
                }

                //  index is the first in the list
                else if(movingAnchorIndex == 0) {
                    nextAnchor = anchors[movingAnchorIndex + 1];
                    prevAnchor = null;
                    closestAnchor = prevAnchor;
                }

                //  index is in the middle of the list
                else {
                    nextAnchor = anchors[movingAnchorIndex + 1];
                    prevAnchor = anchors[movingAnchorIndex - 1];

                    if(Vector2.Distance(target, nextAnchor.transform.position) > Vector2.Distance(target, prevAnchor.transform.position))
                        closestAnchor = prevAnchor;
                    else
                        closestAnchor = nextAnchor;
                }
            }
            else {
                prevAnchor = partyAnchor;
                nextAnchor = null;
                closestAnchor = null;
            }


            //  check dists for next anchor in list
            if(nextAnchor != null) {
                //  too close
                if(Vector2.Distance(target, nextAnchor.transform.position) <= minAnchorDist) {
                    var center = nextAnchor.transform.position;
                    var newVar = GameInfo.getMousePos() - (Vector2)center;
                    var theta = Mathf.Atan2(newVar.y, newVar.x);

                    var y = center.y + minAnchorDist * Mathf.Sin(theta);
                    var x = center.x + minAnchorDist * Mathf.Cos(theta);
                    movingAnchor.transform.position = new Vector2(x, y);
                    canMoveToTarget = false;
                }

                //  too far
                else if(Vector2.Distance(target, nextAnchor.transform.position) >= maxAnchorDist && closestAnchor != nextAnchor) {
                    var center = nextAnchor.transform.position;
                    var newVar = GameInfo.getMousePos() - (Vector2)center;
                    var theta = Mathf.Atan2(newVar.y, newVar.x);

                    var y = center.y + maxAnchorDist * Mathf.Sin(theta);
                    var x = center.x + maxAnchorDist * Mathf.Cos(theta);
                    movingAnchor.transform.position = new Vector2(x, y);
                    canMoveToTarget = false;
                }
            }

            //  check dists for prev anchor in list
            if(prevAnchor != null) {
                //  too close
                if(Vector2.Distance(target, prevAnchor.transform.position) <= minAnchorDist) {
                    var center = prevAnchor.transform.position;
                    var newVar = GameInfo.getMousePos() - (Vector2)center;
                    var theta = Mathf.Atan2(newVar.y, newVar.x);

                    var y = center.y + minAnchorDist * Mathf.Sin(theta);
                    var x = center.x + minAnchorDist * Mathf.Cos(theta);
                    movingAnchor.transform.position = new Vector2(x, y);
                    canMoveToTarget = false;
                }

                //  too far
                else if(Vector2.Distance(target, prevAnchor.transform.position) >= maxAnchorDist && closestAnchor != prevAnchor) {
                    var center = prevAnchor.transform.position;
                    var newVar = GameInfo.getMousePos() - (Vector2)center;
                    var theta = Mathf.Atan2(newVar.y, newVar.x);

                    var y = center.y + maxAnchorDist * Mathf.Sin(theta);
                    var x = center.x + maxAnchorDist * Mathf.Cos(theta);
                    movingAnchor.transform.position = new Vector2(x, y);
                    canMoveToTarget = false;
                }
            }
        }

        if(canMoveToTarget)
            movingAnchor.transform.position = target;
    }
    void putDownMovingAnchor() {
        bool snapped = false;

        //  checks for map locations
        foreach(var i in MapLocationHolder.getLocations()) {
            if(Vector2.Distance(movingAnchor.transform.position, i.pos) < snappingDist * 2.0f) {
                movingAnchor.transform.position = i.pos;
                snapped = true;
                break;
            }
        }

        //  checks for other anchors if not snapped yet
        if(!snapped) {
            foreach(var i in anchors) {
                if(movingAnchor != i && Vector2.Distance(movingAnchor.transform.position, i.transform.position) < snappingDist) {
                    movingAnchor.transform.position = i.transform.position;
                    snapped = true;
                    break;
                }
            }
        }

       // MapAnchorPositionSaver.overrideAnchor(getAnchorIndex(movingAnchor), movingAnchor);
       // MapAnchorPositionSaver.setPartyAnchor(partyAnchor);
        movingAnchor = null;
        drawLine();
    }

    void loadSavedAnchors() {
        anchors.Clear();
        //  party anchor pos
        //if(MapAnchorPositionSaver.getPartyPos() == Vector2.zero) {
            partyAnchor.transform.position = startingPos;
           // MapAnchorPositionSaver.setPartyAnchor(partyAnchor);
      //  }
      //  else
          //  partyAnchor.transform.position = MapAnchorPositionSaver.getPartyPos();

        //  normal anchor poses
        if(MapAnchorPositionSaver.getAnchorCount() == 0) {
          //  createNewAnchorAtPos(MapAnchorPositionSaver.getPartyPos());
        }
        else {
            for(int i = 0; i < MapAnchorPositionSaver.getAnchorCount(); i++) {
                //createNewAnchorAtPos(MapAnchorPositionSaver.getAnchorPos(i));
            }
        }
    }


    //  party anchor shit
    //  returns if the party passed an anchor
    void movePartyAnchor() {
        if(anchors.Count > 0) {
            //  party reached the last anchor
            if(Vector2.Distance(partyAnchor.transform.position, anchors[0].transform.position) < 0.01f) {
                GameObject temp = anchors[0];
                anchors.RemoveAt(0);
                Destroy(temp.gameObject);

                MapAnchorPositionSaver.removeAnchor(0);
               // MapAnchorPositionSaver.setPartyAnchor(partyAnchor);
                FindObjectOfType<MapEventsHandler>().triggerAnchorEvents(partyAnchor.transform.position);


                if(anchors.Count == 0)
                    return;
            }

            partyAnchor.transform.position = Vector2.MoveTowards(partyAnchor.transform.position, anchors[0].transform.position, partySpeed * Time.deltaTime);
            drawLine();
        }
    }




    //  getters
    public int getFoodCostForTrail() {
        int food = 0;
        for(int i = 0; i < anchors.Count; i++) {
            if(i == 0)
                continue;

            float distBtw = Vector2.Distance(anchors[i].transform.position, anchors[i - 1].transform.position);
            float percentageOfFull = distBtw / maxAnchorDist;
            int foodCostForDist = (int)(percentageOfFull * foodForMaxLine) + 1;

            if(foodCostForDist > foodForMaxLine)
                foodCostForDist = foodForMaxLine;

            food += foodCostForDist;
        }

        return food;
    }

    public Vector2 getPartyAnchorPos() {
        return partyAnchor.transform.position;
    }
    public GameInfo.diffLvl getDiffLevelForPartyAnchor() {
        return FindObjectOfType<RegionDivider>().getRelevantDifficultyLevel(partyAnchor.transform.position.x);
    }
}

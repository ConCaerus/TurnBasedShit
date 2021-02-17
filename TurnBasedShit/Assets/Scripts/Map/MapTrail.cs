using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapTrail : MonoBehaviour {
    [SerializeField] List<Vector2> anchorPoses = new List<Vector2>();
    List<GameObject> anchors = new List<GameObject>();
    [SerializeField] GameObject anchor;
    GameObject movingAnchor = null;

    const float minAnchorDist = 0.25f, maxAnchorDist = 5.0f, snappingDist = 0.25f;

    Vector2 startingPos = new Vector2(-8.0f, -1.0f);

    class posHolder {
        public List<float> xs = new List<float>();
        public List<float> ys = new List<float>();
    }


    private void Awake() {
        DOTween.Init();
    }

    private void Start() {
        loadAnchorPoses();
        createAnchors();
        createLine();

        movingAnchor = anchors[anchors.Count - 1];
        Camera.main.transform.position = new Vector3(movingAnchor.transform.position.x, movingAnchor.transform.position.y, Camera.main.transform.position.z);
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0))
            createNewAnchor();
        createLine();
        moveMovingAnchor();
    }


    public Vector2 getMousePos() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    void saveAnchorPoses() {
        posHolder holder = new posHolder();
        foreach(var i in anchorPoses) {
            holder.xs.Add(i.x);
            holder.ys.Add(i.y);
        }
        
        var data = JsonUtility.ToJson(holder);
        PlayerPrefs.SetString("Trail Anchors", data);
        PlayerPrefs.Save();
    }
    void loadAnchorPoses() {
        var data = PlayerPrefs.GetString("Trail Anchors");
        var temp = JsonUtility.FromJson<posHolder>(data);

        anchorPoses.Clear();

        if(temp == null) {
            anchorPoses = new List<Vector2>();
            anchorPoses.Add(startingPos);
            anchorPoses.Add(Vector2.zero);
        }

        else {
            for(int i = 0; i < temp.xs.Count; i++) {
                anchorPoses.Add(new Vector2(temp.xs[i], temp.ys[i]));
            }
        }
    }

    void createAnchors() {
        anchors.Clear();
        foreach(var i in anchorPoses) { 
            var temp = Instantiate(anchor);
            temp.transform.position = i;
            temp.transform.SetParent(transform);

            anchors.Add(temp.gameObject);
        }

        movingAnchor = anchors[anchors.Count - 1];
        saveAnchorPoses();

        moveCamera();
    }

    void createNewAnchor() {
        moveCamera();
        var temp = Instantiate(anchor);
        temp.transform.SetParent(transform);
        movingAnchor = temp;

        anchorPoses.Add(anchors[anchors.Count - 1].transform.position);
        anchors.Add(temp.gameObject);
        saveAnchorPoses();
    }

    void createLine() {
        var line = GetComponent<LineRenderer>();
        line.positionCount = anchorPoses.Count;
        for(int i = 0; i < line.positionCount; i++) {
            line.SetPosition(i, anchorPoses[i]);
        }
    }

    void setAnchorPosesToAnchorTransforms() {
        for(int i = 0; i < anchors.Count; i++) {
            anchorPoses[i] = anchors[i].transform.position;
        }
    }

    void moveCamera() {
        var target = movingAnchor.transform.position;
        Camera.main.transform.DOMove(new Vector3(target.x, target.y, Camera.main.transform.position.z), 0.15f);
    }

    void moveMovingAnchor() {
        movingAnchor.transform.position = getMousePos();

        Vector2 target = new Vector2(snappingDist + 1.0f, snappingDist + 1.0f);
        //  snaps to other anchors
        foreach(var i in anchors) {
            if(i != movingAnchor && Vector2.Distance(i.transform.position, movingAnchor.transform.position) < snappingDist) {
                if(Vector2.Distance(movingAnchor.transform.position, i.transform.position) <
                    Vector2.Distance(movingAnchor.transform.position, target))
                    target = i.gameObject.transform.position;

            }
        }
        foreach(var i in FindObjectOfType<MapLocationSpawner>().holder.locations) {
            var dist = Vector2.Distance(movingAnchor.transform.position, i.pos);
            if(dist < snappingDist * 2.0f && dist < Vector2.Distance(movingAnchor.transform.position, target))
                target = i.pos;
        }
        if(target != new Vector2(snappingDist + 1.0f, snappingDist + 1.0f))
            movingAnchor.transform.position = target;

        //  moving anchor is too close to the center
        if(Vector2.Distance(movingAnchor.transform.position, anchors[anchors.Count - 2].transform.position) < minAnchorDist) {
            var center = anchors[anchors.Count - 2].transform.position;
            var newVar = getMousePos() - (Vector2)center;
            var theta = Mathf.Atan2(newVar.y, newVar.x);

            var y = center.y + minAnchorDist * Mathf.Sin(theta);
            var x = center.x + minAnchorDist * Mathf.Cos(theta);
            movingAnchor.transform.position = new Vector2(x, y);
        }

        //  moving anchor is too far from the center
        else if(Vector2.Distance(movingAnchor.transform.position, anchors[anchors.Count - 2].transform.position) > maxAnchorDist) {
            var center = anchors[anchors.Count - 2].transform.position;
            var newVar = getMousePos() - (Vector2)center;
            var theta = Mathf.Atan2(newVar.y, newVar.x);

            var y = center.y + maxAnchorDist * Mathf.Sin(theta);
            var x = center.x + maxAnchorDist * Mathf.Cos(theta);
            movingAnchor.transform.position = new Vector2(x, y);
        }

        anchorPoses[anchorPoses.Count - 1] = movingAnchor.transform.position;
    }
}

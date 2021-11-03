using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FogMachine : MonoBehaviour {
    [SerializeField] Sprite fog;

    List<List<GameObject>> segments = new List<List<GameObject>>();
    int numOfXSegments = 25, numOfYSegments = 5;


    private void Start() {
        DOTween.Init();
    }

    public void createFog(Texture2D map) {
        for(int i = 0; i < numOfXSegments * numOfYSegments; i++)
            segments.Add(new List<GameObject>());
        for(float y = Map.botBound; y < Map.topBound; y++) {
            for(float x = Map.leftBound; x < Map.rightBound; x++) {
                var p = FindObjectOfType<MapFogTexture>().getTexPosForWorldPoint(new Vector2(x, y));
                if(map.GetPixel(p.x, p.y) == Color.black) {
                    var obj = new GameObject("Fog");
                    obj.transform.parent = transform;
                    obj.transform.localScale = new Vector3(0.5f, 0.5f);
                    obj.transform.localPosition = new Vector3(x, y);
                    var sr = obj.AddComponent<SpriteRenderer>();
                    sr.color = Color.white;
                    sr.sprite = fog;

                    int index = getSegmentIndex(x, y);
                    segments[index].Add(obj);
                }
            }
        }
    }


    public void moveFogAwayFromPosition(Vector2 pos, float distToDie) {
        if(segments.Count == 0 || segments[0].Count == 0)
            return;

        List<int> relevantIndexes = new List<int>();

        relevantIndexes.Add(getSegmentIndex(pos.x, pos.y));

        if(pos.x + distToDie < Map.rightBound) {
            relevantIndexes.Add(getSegmentIndex(pos.x + distToDie, pos.y));
            
            if(pos.y + distToDie < Map.topBound)
                relevantIndexes.Add(getSegmentIndex(pos.x + distToDie, pos.y + distToDie));
            if(pos.y - distToDie > Map.botBound)
                relevantIndexes.Add(getSegmentIndex(pos.x + distToDie, pos.y - distToDie));
        }
        
        if(pos.x - distToDie > Map.leftBound) {
            relevantIndexes.Add(getSegmentIndex(pos.x - distToDie, pos.y));

            if(pos.y + distToDie < Map.topBound)
                relevantIndexes.Add(getSegmentIndex(pos.x - distToDie, pos.y + distToDie));
            if(pos.y - distToDie > Map.botBound)
                relevantIndexes.Add(getSegmentIndex(pos.x - distToDie, pos.y - distToDie));
        }

        if(pos.y + distToDie < Map.topBound)
            relevantIndexes.Add(getSegmentIndex(pos.x, pos.y + distToDie));
        if(pos.y - distToDie > Map.botBound)
            relevantIndexes.Add(getSegmentIndex(pos.x, pos.y - distToDie));

        List<int> temp = new List<int>();
        foreach(var i in relevantIndexes) {
            bool seen = false;
            foreach(var j in temp) {
                if(i == j) {
                    seen = true;
                    break;
                }
            }
            if(!seen)
                temp.Add(i);
        }

        foreach(var i in temp)
            destroyFogInSegment(i, pos, distToDie);
    }

    void destroyFogInSegment(int index, Vector2 pos, float distToDie) {
        for(int i = segments[index].Count - 1; i >= 0; i--) {
            if(Vector2.Distance(pos, segments[index][i].transform.position) < distToDie) {
                segments[index][i].transform.DOScale(0.0f, 0.15f);
                Destroy(segments[index][i].gameObject, 0.2f);
                segments[index].RemoveAt(i);
            }
        }
    }


    public int getSegmentIndex(float x, float y) {
        float xBuffer = Map.width() / numOfXSegments;
        float yBuffer = Map.height() / numOfYSegments;

        int xIndex = Mathf.FloorToInt((x - Map.leftBound) / xBuffer);
        int yIndex = Mathf.FloorToInt((y - Map.botBound) / yBuffer);

        int rowIndex = numOfXSegments * yIndex;

        int index = rowIndex + xIndex;
        index = Mathf.Clamp(index, 0, numOfXSegments * numOfYSegments);
        return index;
    }
}

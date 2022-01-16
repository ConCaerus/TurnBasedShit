using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapFogTexture : MonoBehaviour {
    [SerializeField] SpriteMask fogMask;
    [SerializeField] GameObject fogBackground;
    public Texture2D fow { get; private set; }
    float seeingDist = 4.0f;

    List<fogPointInfo> clearingPoints = new List<fogPointInfo>();
    Coroutine clearer = null;

    GameInfo.region reg;

    Vector2 lastUpdatedPos;


    struct fogPointInfo {
        public Vector2Int pos;
        public float distPercentage;
    }

    private void Start() {
        fow = Map.getFogTexture(GameInfo.getCurrentRegion());
        reg = GameInfo.getCurrentRegion();
        StartCoroutine(FindObjectOfType<TransitionCanvas>().runAfterLoading(startingClear));
        Debug.Log(GameInfo.getCurrentRegion());

        //  set the sprite of the frog mask
        var r = new Rect(0, 0, fow.width, fow.height);
        var sp = Sprite.Create(fow, r, Vector2.zero);
        sp.name = "Dick";
        fogMask.sprite = sp;

        fogBackground.gameObject.SetActive(true);

        //  reset the position of the frog mask because changing the sprite also shifts it's position
        fogMask.transform.position = new Vector3(-Map.width / 2.0f, -Map.height / 2.0f);
        fogMask.transform.localScale = new Vector3(100.0f, 100.0f);


        setFogBackgroundSprite();
        fogBackground.transform.localScale = new Vector3(fogBackground.transform.localScale.x, fogBackground.transform.localScale.y);
        lastUpdatedPos = transform.position;

        fow.Apply(false);
    }


    public void startingClear() {
        clearFogAroundPos(transform.position, seeingDist * 2.0f, true);
    }


    private void FixedUpdate() {
        if(Vector2.Distance(lastUpdatedPos, transform.position) > 1.0f) {
            clearFogAroundPos(transform.position, seeingDist, false);
            lastUpdatedPos = transform.position;
        }
    }


    public void clearFogAroundPos(Vector2 pos, float area, bool clearingArea) {
        var highlightDist = area * 1.5f;

        //  sets the pos to a good position to map to the texture
        if(clearingArea) 
            pos = getWorldPosForTexPoint(getTexPosForWorldPoint(pos));

        Vector2Int feild = new Vector2Int((int)highlightDist + 1, (int)highlightDist + 1);
        var startingPoint = getTexPosForWorldPoint(pos) - feild;
        var endingPoint = getTexPosForWorldPoint(pos) + feild;

        startingPoint.x = Mathf.Clamp(startingPoint.x, 0, fow.width - 1);
        startingPoint.y = Mathf.Clamp(startingPoint.y, 0, fow.height - 1);
        endingPoint.x = Mathf.Clamp(endingPoint.x, 0, fow.width - 1);
        endingPoint.y = Mathf.Clamp(endingPoint.y, 0, fow.height - 1);

        List<fogPointInfo> clearingAreaPoints = new List<fogPointInfo>();

        for(int x = startingPoint.x - 1; x < endingPoint.x + 1; x++) {
            for(int y = startingPoint.y - 1; y < endingPoint.y + 1; y++) {
                if(fow.GetPixel(x, y).a == 0.0f)
                    continue;
                var p = getWorldPosForTexPoint(new Vector2Int(x, y));
                if(x < fow.width - 1 && y < fow.height - 1) {
                    p += getWorldPosForTexPoint(new Vector2Int(x + 1, y + 1));
                    p /= 2.0f;
                }
                if(Vector2.Distance(pos, p) < area) {
                    var info = new fogPointInfo();
                    info.pos = new Vector2Int(x, y);
                    info.distPercentage = Vector2.Distance(p, pos) / area;
                    if(!clearingArea)
                        clearingPoints.Add(info);
                    else
                        clearingAreaPoints.Add(info);
                }

                else if(Vector2.Distance(pos, p) < highlightDist && fow.GetPixel(x, y) == Color.white) {
                    fow.SetPixel(x, y, new Color(0.0f, 0.0f, 0.0f, 0.1f));
                }
            }
        }

        if(clearer == null && !clearingArea)
            clearer = StartCoroutine(animateFogClearing());
        if(clearingArea)
            StartCoroutine(animateFogAreaClearing(clearingAreaPoints, highlightDist));

        fow.Apply(false);
    }
    public void saveTexture() {
        Map.saveFogTexture(fow, reg);
    }

    public void setFogBackgroundSprite() {
        Vector2 randOffset = new Vector2(Random.Range(-10000.0f, 100000.0f), Random.Range(-100000.0f, 100000.0f));

        int res = 30;
        var tex = new Texture2D(fow.width * res, fow.height * res);
        tex.anisoLevel = 16;
        float mag = 0.05f / res;
        float noiseStretchFactor = 0.5f;

        for(int x = 0; x < tex.width; x++) {
            for(int y = 0; y < tex.height; y++) {
                var value = Mathf.PerlinNoise((x * mag + randOffset.x) / noiseStretchFactor, y * mag + randOffset.y);

                float min = .75f, max = .9f;

                value = Mathf.Clamp(value + 0.35f, min, max);

                if(value != min && value != max) {
                    value = 0.0f;
                }

                tex.SetPixel(x, y, new Color(value, value, value, 1.0f));
            }
        }

        tex.Apply(false);

        var r = new Rect(0, 0, tex.width, tex.height);
        var sp = Sprite.Create(tex, r, Vector2.zero);
        sp.name = "Fog";

        fogBackground.GetComponent<SpriteRenderer>().sprite = sp;
        fogBackground.transform.localPosition = Vector3.zero;
        fogBackground.transform.localScale = (Vector3.one / res) + ((Vector3.one / res) / 10.0f);
    }


    public Vector2 getWorldPosForTexPoint(Vector2Int texPoint) {
        var temp = new Vector2(0.0f, 0.0f);

        float texXPercentage = ((float)texPoint.x / (float)fow.width);
        float texYPercentage = ((float)texPoint.y / (float)fow.height);

        temp.x = Map.width * texXPercentage;
        temp.y = Map.height * texYPercentage;

        temp.x += Map.leftBound();
        temp.y += Map.botBound();

        return temp;
    }
    public Vector2Int getTexPosForWorldPoint(Vector2 pos) {
        var temp = new Vector2Int(0, 0);

        float wXPerc = (pos.x - Map.leftBound()) / (Map.width);
        float wYPerc = (pos.y - Map.botBound()) / (Map.height);

        temp.x = Mathf.FloorToInt((fow.width * wXPerc) + 0.5f);
        temp.y = Mathf.FloorToInt((fow.height * wYPerc) + 0.5f);

        return temp;
    }

    public float getPercentageOfClearedFog() {
        var pixels = fow.GetPixels();
        int clearedCount = 0;

        foreach(var i in pixels) {
            if(i == Color.white)
                continue;
            clearedCount++;
        }

        return (float)clearedCount / pixels.Length;
    }
    public bool isPositionCleared(Vector2 pos) {
        var cord = getTexPosForWorldPoint(pos);
        return fow.GetPixel(cord.x, cord.y).a == 0.0f;
    }

    IEnumerator animateFogClearing() {
        while(clearingPoints.Count > 0) {
            for(int i = clearingPoints.Count - 1; i >= 0; i--) {
                var p = clearingPoints[i];

                if(fow.GetPixel(p.pos.x, p.pos.y).a == 0.0f) {
                    clearingPoints.RemoveAt(i);
                    continue;
                }

                var c = fow.GetPixel(p.pos.x, p.pos.y);
                float negate = 0.5f * Time.deltaTime;
                fow.SetPixel(p.pos.x, p.pos.y, new Color(c.r, c.g, c.b, c.a - negate));
            }
            fow.Apply(false);

            yield return new WaitForEndOfFrame();
        }

        clearer = null;
    }

    IEnumerator animateFogAreaClearing(List<fogPointInfo> clearingAreaPoints, float highlightAmt) {
        List<int> runningIndexes = new List<int>();
        List<int> highlightingIndexes = new List<int>();

        while(clearingAreaPoints.Count > 0) {
            //  disappearing
            for(int i = runningIndexes.Count - 1; i >= 0; i--) {
                var p = clearingAreaPoints[runningIndexes[i]];

                if(fow.GetPixel(p.pos.x, p.pos.y).a == 0.0f) {
                    runningIndexes.RemoveAt(i);
                    continue;
                }

                var c = fow.GetPixel(p.pos.x, p.pos.y);
                float negate = 5.0f * Time.deltaTime * (50.0f * (p.distPercentage + 1.0f));
                fow.SetPixel(p.pos.x, p.pos.y, new Color(c.r, c.g, c.b, c.a - negate));
            }

            //  highlighting
            if(highlightingIndexes.Count > 0) {
                for(int i = highlightingIndexes.Count - 1; i >= 0; i--) {
                    var p = clearingAreaPoints[highlightingIndexes[i]];
                    fow.SetPixel(p.pos.x, p.pos.y, new Color(0.0f, 0.0f, 0.0f, 0.1f));
                    highlightingIndexes.RemoveAt(i);
                }
            }


            fow.Apply(false);

            //  updating lists
            if(clearingAreaPoints.Count > 0) {
                runningIndexes = new List<int>();
                float nextIndex = Mathf.Infinity;

                for(int i = clearingAreaPoints.Count - 1; i >= 0; i--) {
                    if(fow.GetPixel(clearingAreaPoints[i].pos.x, clearingAreaPoints[i].pos.y).a == 0.0f) {
                        clearingAreaPoints.RemoveAt(i);
                        continue;
                    }

                    if(clearingAreaPoints[i].distPercentage < nextIndex) {
                        nextIndex = clearingAreaPoints[i].distPercentage;
                    }
                }

                float highlightIndex = nextIndex + highlightAmt;
                for(int i = 0; i < clearingAreaPoints.Count; i++) {
                    if(clearingAreaPoints[i].distPercentage <= nextIndex)
                        runningIndexes.Add(i);
                    else if(clearingAreaPoints[i].distPercentage <= highlightIndex)
                        highlightingIndexes.Add(i);
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
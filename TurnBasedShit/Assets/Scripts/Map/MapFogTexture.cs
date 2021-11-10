using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFogTexture : MonoBehaviour {
    [SerializeField] SpriteMask fogMask;
    [SerializeField] GameObject fogBackground;
    public Texture2D fow { get; private set; }
    float seeingDist = 4.0f;
    float stretchFactor = 0.1f;

    Vector2 lastUpdatedPos;

    private void Start() {
        fow = Map.getFogTexture();

        if(!Map.hasSavedFogTexture()) {
            for(int x = 0; x < fow.width; x++) {
                for(int y = 0; y < fow.height; y++) {
                    fow.SetPixel(x, y, Color.white);
                }
            }
        }
        updateTex();



        //  set the sprite of the frog mask
        var r = new Rect(0, 0, fow.width, fow.height);
        var sp = Sprite.Create(fow, r, Vector2.zero);
        sp.name = "Dick";
        fogMask.sprite = sp;

        fogBackground.gameObject.SetActive(true);

        //  reset the position of the frog mask because changing the sprite also shifts it's position
        fogMask.transform.position = new Vector3(Map.leftBound, Map.botBound);
        fogMask.transform.localScale = new Vector3(100.0f, 100.0f);


        setFogBackgroundSprite();
        fogBackground.transform.localScale = new Vector3(fogBackground.transform.localScale.x + stretchFactor, fogBackground.transform.localScale.y);
        StartCoroutine(moveFog(stretchFactor / 2.0f));
        lastUpdatedPos = transform.position;

        fow.Apply(false);
    }



    private void FixedUpdate() {
        if(Vector2.Distance(lastUpdatedPos, transform.position) > 1.0f) {
            updateTex();
            lastUpdatedPos = transform.position;
        }
    }


    void updateTex() {
        var highlightDist = seeingDist + 2.0f;
        Vector2Int feild = new Vector2Int((int)highlightDist + 1, (int)highlightDist + 1);
        var startingPoint = getTexPosForWorldPoint(transform.position) - feild;
        var endingPoint = getTexPosForWorldPoint(transform.position) + feild;

        startingPoint.x = Mathf.Clamp(startingPoint.x, 0, fow.width - 1);
        startingPoint.y = Mathf.Clamp(startingPoint.y, 0, fow.height - 1);
        endingPoint.x = Mathf.Clamp(endingPoint.x, 0, fow.width - 1);
        endingPoint.y = Mathf.Clamp(endingPoint.y, 0, fow.height - 1);


        for(int x = startingPoint.x - 1; x < endingPoint.x + 1; x++) {
            for(int y = startingPoint.y - 1; y < endingPoint.y + 1; y++) {
                if(fow.GetPixel(x, y) == Color.clear)
                    continue;
                var p = getWorldPosForTexPoint(new Vector2Int(x, y));
                if(x < fow.width - 1 && y < fow.height - 1) {
                    p += getWorldPosForTexPoint(new Vector2Int(x + 1, y + 1));
                    p /= 2.0f;
                }
                if(Vector2.Distance(transform.position, p) < seeingDist)
                    StartCoroutine(animateFogClearing(x, y));

                else if(Vector2.Distance(transform.position, p) < highlightDist && fow.GetPixel(x, y) == Color.white) {
                    fow.SetPixel(x, y, new Color(0.0f, 0.0f, 0.0f, 0.1f));
                }
            }
        }

        fow.Apply(false);
    }
    public void saveTexture() {
        Map.saveFogTexture(fow);
    }

    public void setFogBackgroundSprite() {
        Vector2 randOffset = new Vector2(Random.Range(-10000.0f, 100000.0f), Random.Range(-100000.0f, 100000.0f));

        int res = 30;
        var tex = new Texture2D(fow.width * res, fow.height * res);
        tex.anisoLevel = 16;
        float mag = 0.05f / res;


        for(int x = 0; x < tex.width; x++) {
            for(int y = 0; y < tex.height; y++) {
                var value = Mathf.PerlinNoise((x * mag + randOffset.x) / (stretchFactor * 2.0f), y * mag + randOffset.y);

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

        temp.x = Map.width() * texXPercentage;
        temp.y = Map.height() * texYPercentage;

        temp.x += Map.leftBound;
        temp.y += Map.botBound;

        return temp;
    }
    public Vector2Int getTexPosForWorldPoint(Vector2 pos) {
        var temp = new Vector2Int(0, 0);

        float wXPerc = (pos.x - Map.leftBound) / Map.width();
        float wYPerc = (pos.y - Map.botBound) / Map.height();

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

    IEnumerator animateFogClearing(int x, int y) {
        while(fow.GetPixel(x, y) != Color.clear) {
            var c = fow.GetPixel(x, y);
            float negate = 0.005f;
            fow.SetPixel(x, y, new Color(c.r - negate, c.g - negate, c.b - negate, c.a - negate));

            fow.Apply(false);

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator moveFog(float dist, bool movingRight = true) {
        float speed = .01f;

        Vector2 target = new Vector2(dist, 0.0f);
        if(!movingRight)
            target *= -1.0f;

        fogBackground.transform.localPosition = Vector2.MoveTowards(fogBackground.transform.localPosition, target, speed * Time.deltaTime);

        yield return new WaitForEndOfFrame();

        if((Vector2)fogBackground.transform.localPosition == target) {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(moveFog(dist, !movingRight));
        }
        else
            StartCoroutine(moveFog(dist, movingRight));
    }
}
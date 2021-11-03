using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFogTexture : MonoBehaviour {
    Texture2D fow;
    float seeingDist = 8.0f;


    Vector2 lastUpdatedPos;

    private void Start() {
        fow = new Texture2D((int)(Map.width() + 1), (int)(Map.height() + 1));

        if(Map.getFogTexture() != null) {
            fow = Map.getFogTexture();
        }
        else {
            for(int x = 0; x < fow.width; x++) {
                for(int y = 0; y < fow.height; y++) {
                    fow.SetPixel(x, y, Color.black);
                }
            }
        }
        updateTex();

        if(FindObjectOfType<FogMachine>() != null)
            FindObjectOfType<FogMachine>().createFog(fow);


        var r = new Rect(0, 0, fow.width, fow.height);
        var sp = Sprite.Create(fow, r, Vector2.zero);
        sp.name = "Dick";
        FindObjectOfType<SpriteMask>().sprite = sp;
        if(FindObjectOfType<FogMachine>() != null)
            FindObjectOfType<FogMachine>().moveFogAwayFromPosition(transform.position, seeingDist);
        lastUpdatedPos = transform.position;
    }



    private void FixedUpdate() {
        if(Vector2.Distance(lastUpdatedPos, transform.position) > 1.0f) {
            updateTex();
            if(FindObjectOfType<FogMachine>() != null)
                FindObjectOfType<FogMachine>().moveFogAwayFromPosition(transform.position, seeingDist);
            lastUpdatedPos = transform.position;
        }
    }


    void updateTex() {
        for(int x = 0; x < fow.width; x++) {
            for(int y = 0; y < fow.height; y++) {
                var wPos = getWorldPosForTexPoint(new Vector2Int(x, y));

                if(Vector2.Distance(wPos, transform.position) < seeingDist)
                    StartCoroutine(animateFogClearing(x, y));
            }
        }

        fow.Apply();
    }

    public void saveTexture() {
        Map.saveFogTexture(fow);
    }

    public Vector2 getWorldPosForTexPoint(Vector2Int texPoint) {
        var temp = new Vector2(0.0f, 0.0f);

        float texXPercentage = (float)texPoint.x / (float)fow.width;
        float texYPercentage = (float)texPoint.y / (float)fow.height;

        temp.x = Map.width() * texXPercentage;
        temp.y = Map.height() * texYPercentage;

        temp.x += Map.leftBound;
        temp.y += Map.botBound;

        return temp;
    }
    public Vector2Int getTexPosForWorldPoint(Vector2 pos) {
        var temp = new Vector2Int(0, 0);

        float wXPerc = pos.x / Map.width();
        float wYPerc = pos.y / Map.height();

        temp.x = Mathf.FloorToInt(fow.width * (wXPerc + 0.5f));
        temp.y = Mathf.FloorToInt(fow.height * (wYPerc + 0.5f));

        return temp;
    }

    IEnumerator animateFogClearing(int x, int y) {
        while(fow.GetPixel(x, y) != Color.clear) {
            var c = fow.GetPixel(x, y);
            float negate = 0.05f;
            fow.SetPixel(x, y, new Color(c.r - negate, c.g - negate, c.b - negate, c.a - negate));

            fow.Apply();

            yield return new WaitForEndOfFrame();
        }
    }
}
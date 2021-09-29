using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpriteHandler : MonoBehaviour {
    public GameObject head, face, body;
    bool showingFace = true;
    int faceIndex = -1, headIndex, bodyIndex = -1;
    int animState = 0;
    int orderOffset = 0;
    Color col;

    Coroutine idler = null;

    public void setEverything(UnitStats stats) {
        setEverything(stats.u_sprite, stats.equippedWeapon, stats.equippedArmor);;
    }
    public void setEverything(int h, int f, int b, Color c, Weapon w, Armor a) {
        setHead(h, a);
        setFace(f, a);
        setBody(b, w, a); 
        setColor(c);
        offsetLayer();

        if(FindObjectOfType<PartyObject>() != null)
            FindObjectOfType<PartyObject>().repositionUnit(FindObjectOfType<PartyObject>().getInstantiatedMember(GetComponentInParent<UnitClass>().stats));
    }
    public void setEverything(UnitSpriteInfo info, Weapon w, Armor a) {
        setEverything(info.headIndex, info.faceIndex, info.bodyIndex, info.color, w, a);
        setColor(info.color);
    }
    public void setHead(int index, Armor a) {
        if(index == -1)
            index = Random.Range(0, FindObjectOfType<PresetLibrary>().getHeadCount());
        Destroy(head.gameObject);
        GameObject obj = FindObjectOfType<PresetLibrary>().getUnitHead(index);

        head = Instantiate(obj, body.transform).transform.GetChild(0).gameObject;
        face = head.transform.GetChild(0).gameObject;

        setFace(faceIndex, a);
        setColor(col);
        headIndex = index;
        offsetLayer();
    }
    public void setFace(int index, Armor a) {
        if(index == -1)
            index = Random.Range(0, FindObjectOfType<PresetLibrary>().getFaceCount());
        face.SetActive(showingFace);
        face.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getUnitFace(index);
        faceIndex = index;
        setArmor(a);
        offsetLayer();
    }
    public void setBody(int index, Weapon w, Armor a) {
        if(index == -1)
            index = Random.Range(0, FindObjectOfType<PresetLibrary>().getBodyCount());
        Destroy(body.gameObject);
        body = Instantiate(FindObjectOfType<PresetLibrary>().getUnitBody(index), transform);
        setColor(col);
        setHead(headIndex, a);

        bodyIndex = index;

        setWeapon(w);
        setArmor(a);
        offsetLayer();
    }

    public void showFace() {
        showingFace = true;
        if(faceIndex == -1)
            return;

        face.SetActive(true);
        face.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getUnitFace(faceIndex);
    }
    public void hideFace() {
        showingFace = false;
        if(faceIndex == -1 || face == null)
            return;
        face.SetActive(false);
    }
    public bool isFaceShown() {
        return showingFace;
    }

    public void setColor(Color c) {
        //  body
        body.GetComponent<SpriteRenderer>().color = c;

        //  arms
        body.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = c;
        body.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().color = c;

        //  head
        head.GetComponent<SpriteRenderer>().color = c;
        col = c;
    }
    public Color getColor() {
        return col;
    }

    public void setWeapon(Weapon w) {
        var weapon = body.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;

        if(w == null || w.isEmpty()) {
            weapon.GetComponent<SpriteRenderer>().sprite = null;
            weapon.GetComponent<SpriteRenderer>().color = Color.white;
            return;
        }

        weapon.transform.localPosition = new Vector2(FindObjectOfType<PresetLibrary>().getWeaponSprite(w).equippedX, FindObjectOfType<PresetLibrary>().getWeaponSprite(w).equippedY);
        weapon.transform.localScale = new Vector3(FindObjectOfType<PresetLibrary>().getWeaponSprite(w).equippedXSize, FindObjectOfType<PresetLibrary>().getWeaponSprite(w).equippedYSize, 0.0f);
        weapon.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, FindObjectOfType<PresetLibrary>().getWeaponSprite(w).equippedRot);
        weapon.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(w).sprite;
    }
    public void setArmor(Armor a) {
        var armor = body.transform.GetChild(2).gameObject;
        var rightShoulder = body.transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        var leftShoulder = body.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        var hat = head.transform.GetChild(1).gameObject;

        if(a == null || a.isEmpty()) {
            armor.GetComponent<SpriteRenderer>().sprite = null;
            rightShoulder.GetComponent<SpriteRenderer>().sprite = null;
            leftShoulder.GetComponent<SpriteRenderer>().sprite = null;
            hat.GetComponent<SpriteRenderer>().sprite = null;
            return;
        }

        var aSprite = FindObjectOfType<PresetLibrary>().getArmorSprite(a);

        if(aSprite.equippedSprite != null) {
            armor.GetComponent<SpriteRenderer>().sprite = aSprite.equippedSprite;
            armor.transform.localPosition = aSprite.getRelevantPos(bodyIndex);
            armor.transform.localScale = aSprite.getRelevantSize(bodyIndex);
        }

        if(aSprite.equippedShoulder != null) {
            rightShoulder.GetComponent<SpriteRenderer>().sprite = aSprite.equippedShoulder;
            rightShoulder.transform.localPosition = aSprite.getRelevantShoulderPos(bodyIndex);
            rightShoulder.transform.localScale = aSprite.getRelevantShoulderSize(bodyIndex);
            rightShoulder.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, aSprite.shoulderRot);

            leftShoulder.GetComponent<SpriteRenderer>().sprite = aSprite.equippedShoulder;
            leftShoulder.transform.localPosition = aSprite.getRelevantShoulderPos(bodyIndex);
            leftShoulder.transform.localScale = aSprite.getRelevantShoulderSize(bodyIndex);
            leftShoulder.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, aSprite.shoulderRot);
        }

        if(aSprite.equippedHat != null) {
            hat.GetComponent<SpriteRenderer>().sprite = aSprite.equippedHat;
            hat.transform.localPosition = aSprite.getRelevantHatPos(headIndex);
            hat.transform.localScale = aSprite.getRelevantHatSize(headIndex);
            hat.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, aSprite.hatRot);
        }
    }

    public Sprite getWeapon() {
        return body.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
    }


    public void setAnimState(int i) {
        body.GetComponent<Animator>().SetInteger("state", i);
        head.GetComponent<Animator>().SetInteger("state", i);
        animState = i;

        if(idler != null)
            StopCoroutine(idler);
        if(i > 0) {
            idler = StartCoroutine(returnToIdle());
        }
    }
    public int getAnimState() {
        return animState;
    }
    public bool isAnimDone() {
        if(body.GetComponent<Animator>().GetInteger("state") == 0 || head.GetComponent<Animator>().GetInteger("state") == 0) {
            return false;
        }

        if(body.GetComponent<Animator>().GetInteger("state") != head.GetComponent<Animator>().GetInteger("state"))
            return false;
        bool h = head.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !head.GetComponent<Animator>().IsInTransition(0);
        bool b = body.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !body.GetComponent<Animator>().IsInTransition(0);

        return h && b;
    }

    public void setAnimSpeed(float sp) {
        body.GetComponent<Animator>().speed = sp;
        head.GetComponent<Animator>().speed = sp;
    }


    public void setLayerOffset(int offset) {
        orderOffset = offset;
    }

    void offsetLayer() {
        //  body
        body.GetComponent<SpriteRenderer>().sortingOrder = FindObjectOfType<PresetLibrary>().getRandomUnitBody().GetComponent<SpriteRenderer>().sortingOrder + orderOffset;

        //  arms
        body.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = body.GetComponent<SpriteRenderer>().sortingOrder - 2;
        body.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = body.GetComponent<SpriteRenderer>().sortingOrder + 4;

        //  arm armor
        body.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = body.GetComponent<SpriteRenderer>().sortingOrder - 1;
        body.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = body.GetComponent<SpriteRenderer>().sortingOrder + 5;


        //  weapon / armor
        body.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = body.GetComponent<SpriteRenderer>().sortingOrder - 3;
        body.transform.GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = body.GetComponent<SpriteRenderer>().sortingOrder + 1;


        //  head / face
        head.GetComponent<SpriteRenderer>().sortingOrder = body.GetComponent<SpriteRenderer>().sortingOrder + 3;
        head.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = body.GetComponent<SpriteRenderer>().sortingOrder + 4;
        head.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = body.GetComponent<SpriteRenderer>().sortingOrder + 5;
    }


    public float getHeight() {
        float thing = body.GetComponent<SpriteRenderer>().bounds.size.y / 1.0f;
        return thing;
    }



    IEnumerator returnToIdle() {
        yield return new WaitForEndOfFrame();

        if(isAnimDone()) {
            setAnimState(0);
            idler = null;
        }
        else
            idler = StartCoroutine(returnToIdle());
    }
}
